namespace Carely.Dtos.Requests.Mother
{
    public class CreateMotherRequest
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? PhoneNumber { get; set; } 
        public DateTime BirthDate { get; set; }
        public int Hight { get; set; }
        public int Weight { get; set; }
    }
}
