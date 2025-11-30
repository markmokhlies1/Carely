namespace Carely.Dtos.Responses.Admin
{
    public class AdminResponse
    {
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public static AdminResponse FromEnitity(Models.Admin admin)
        {
            return new AdminResponse
            {
                Id = admin.Id,
                FirstName = admin.FirstName,
                LastName = admin.LastName,
                Email = admin.Email,
                PhoneNumber = admin.PhoneNumber,
            };
        }
    }
}
