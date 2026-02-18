namespace Carely.Dtos.Responses.Meeting
{
    public class MotherMeetingResponse
    {
        public int Id { get; set; }
        public string? Description { get; set; }
        public string MeetingType { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public string? Address { get; set; }
        public int DoctorId { get; set; }
        public string DoctorName { get; set; } = string.Empty;

       
        public bool IsRegistered { get; set; }

        public static MotherMeetingResponse FromEntity(
            Models.Meeting meeting,
            bool isRegistered)
        {
            return new MotherMeetingResponse
            {
                Id = meeting.Id,
                Description = meeting.Description,
                MeetingType = meeting.MeetingType.ToString(),
                Date = meeting.Date,
                Address = meeting.Address,
                DoctorId = meeting.DoctorId,
                DoctorName = $"{meeting.Doctor?.FirstName} {meeting.Doctor?.LastName}".Trim(),
                IsRegistered = isRegistered
            };
        }
    }

}
