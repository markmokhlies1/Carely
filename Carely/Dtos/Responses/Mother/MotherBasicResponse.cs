namespace Carely.Dtos.Responses.Mother
{
    public class MotherBasicResponse
    {
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }

        public static MotherBasicResponse FromEntity(Models.Mother m)
        {
            return new MotherBasicResponse
            {
                Id = m.Id,
                FirstName = m.FirstName,
                LastName = m.LastName
            };
        }
    }
}
