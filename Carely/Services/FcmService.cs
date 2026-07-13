using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;
using System.Collections.Generic;

namespace Carely.Services
{
    public interface IFcmService
    {
        Task SendNotificationAsync(string deviceToken);
        Task SendVaccinationReminderAsync( 
           string deviceToken,
           string vaccinationName,
           string babyName,
           int daysUntilDue,
           string message);
    }
    public class FcmService : IFcmService
    {
        public FcmService(IConfiguration config)
        {

            if (FirebaseApp.DefaultInstance == null)
            {
                var credentialsPath = config["Fcm:CredentialsPath"];

                FirebaseApp.Create(new AppOptions
                {
                    Credential = GoogleCredential
                    .FromFile(credentialsPath)
                   
                   

                });
            }
        }


        //send the data to flutter 
        public async Task SendNotificationAsync(string deviceToken )
        {
            var message = new Message
            {
                Token = deviceToken,
               Data =new Dictionary<string, string> {
                   { "event_type", "CRY_DETECTED" },
                   { "is_crying", "true" },
                   //{ "timestamp", DateTime.UtcNow.ToString("o") }

               },
               Android =new AndroidConfig
               {
                   Priority = Priority.High
               }


            };

            await FirebaseMessaging.DefaultInstance.SendAsync(message);
        }

        public async Task SendVaccinationReminderAsync(string deviceToken,
            string vaccinationName,
            string babyName,
            int daysUntilDue,
            string message)
        {
            var reminderType = daysUntilDue == 0 ? "TODAY"
                : daysUntilDue == 2 ? "TWO_DAYS"
                : "ONE_WEEK";

            var fullMessage = $"{babyName}: {message}";

            var fcmMessage = new Message
            {
                Token = deviceToken,
                Data = new Dictionary<string, string>
                {
                      { "event_type", "VACCINATION_REMINDER" },
                    { "vaccination_name", vaccinationName },
                    { "baby_name", babyName },
                    { "days_until_due", daysUntilDue.ToString() },
                    { "reminder_type", reminderType },
                    { "message", fullMessage  }

                 },
                Android = new AndroidConfig { Priority = Priority.High }
            };
            await FirebaseMessaging.DefaultInstance.SendAsync(fcmMessage);
        
        }
    }
}
