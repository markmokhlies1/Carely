using Carely.Data;
using Carely.Models;
using Microsoft.EntityFrameworkCore;

namespace Carely.Services
{
    public class VaccinationReminderService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<VaccinationReminderService> _logger;

        public VaccinationReminderService(
            IServiceScopeFactory scopeFactory,
            ILogger<VaccinationReminderService> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    //real code
                    await CheckAndSendReminders();

                    while (!stoppingToken.IsCancellationRequested)
                    {
                        var now = DateTime.Now;
                        var next8Am = DateTime.Today.AddHours(8);
                        if (now > next8Am)
                            next8Am = next8Am.AddDays(1);

                        var delay = next8Am - now;
                        _logger.LogInformation($"Next vaccination check scheduled at: {next8Am}");

                        await Task.Delay(delay, stoppingToken);
                        await CheckAndSendReminders();
                    }

                    ////testing
                    //_logger.LogInformation($"Vaccination check running at: {DateTime.Now}");
                    //await CheckAndSendReminders(); // ← run first
                    //await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);




                }
                catch (TaskCanceledException)
                {
                    break;

                }
                catch (Exception ex)
                {
                    _logger.LogError($"Vaccination remider failed :{ex.Message}");

                    await Task.Delay(TimeSpan.FromHours(1), stoppingToken);
                }
            }
        }

        private async Task CheckAndSendReminders()
        {
            using var scope = _scopeFactory.CreateScope();

            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var fcmService = scope.ServiceProvider.GetRequiredService<IFcmService>();

            var today = DateTime.Today;

            var babies = await context.Babies
                .Include(b => b.Mother)
                .Include(b => b.BabyUsage)
                .ThenInclude(bv => bv.Vaccination)
                .ToListAsync();

            var allVaccinations = await context.Vaccinations.ToListAsync();
            foreach (var baby in babies)
            {
                var deviceToken = baby.Mother?.DeviceToken;
                if (string.IsNullOrEmpty(deviceToken)) continue;

                var checkedIds = baby.BabyUsage
                    .Select(bv => bv.VaccinationId)
                    .ToHashSet();

                var uncheckedVaccinations = allVaccinations
                   .Where(v => !checkedIds.Contains(v.Id))
                   .ToList();
                foreach (var vaccination in uncheckedVaccinations)
                {

                    var dueDate = baby.DateOfBirth.AddMonths((int)vaccination.Age);
                    var daysUntilDue = (dueDate - today).Days;

                    if (daysUntilDue == 7 || daysUntilDue == 2 || daysUntilDue == 0)
                    {
                        var message = daysUntilDue == 0
                            ? $"Today is the day for {vaccination.Name} vaccination!"
                            : $"{vaccination.Name} vaccination is due in {daysUntilDue} days.";

                        await fcmService.SendVaccinationReminderAsync(
                            deviceToken: deviceToken,
                            vaccinationName: vaccination.Name ?? "Vaccination",
                            babyName: baby.FirstName ?? "Your baby",
                            daysUntilDue: daysUntilDue,
                            message: message
                        );

                        _logger.LogInformation(
                            $"Reminder sent → baby {baby.Id} — {vaccination.Name} in {daysUntilDue} days"
                        );
                    }
                }
            }
        }
    }
}