using Carely.Models.Enums.User;
namespace Carely.Models.Base
{
    public abstract class User : Entity
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? PasswordHash {  get; set; }
        public string? PhoneNumber {  get; set; }
        public UserRole Role { get; set; }
    }
}
