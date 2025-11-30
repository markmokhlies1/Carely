namespace Carely.Dtos.Responses.Meeting
{
    public class MeetingResponse
    {
        public int Id { get; set; }
        public string? Description { get; set; }
        public string MeetingType { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public string? Address { get; set; }
        public int DoctorId { get; set; }

        public static MeetingResponse FromEntity(Models.Meeting meeting)
        {
            return new MeetingResponse
            {
                Id = meeting.Id,
                Description = meeting.Description,
                MeetingType = meeting.MeetingType.ToString(),
                Date = meeting.Date,
                Address = meeting.Address,
                DoctorId = meeting.DoctorId
            };
        }
    }

}
