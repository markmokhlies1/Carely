using Carely.Dtos.Requests.Feedback;
using Carely.Dtos.Requests.Meeting;
using Carely.Dtos.Responses.Feedback;
using Carely.Dtos.Responses.Meeting;
using Carely.Dtos.Responses.Mother;
using Carely.Models;
using Carely.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Carely.Controllers
{
    [Route("api/meetings")]  
    [ApiController]
    public class MeetingsController : ControllerBase
    {
        private readonly IMeetingRepository _meetingRepo;
        private readonly IFeedbackRepository _feedbackRepo;


        public MeetingsController(IMeetingRepository repo, IFeedbackRepository feedbackRepository)
        {
            _meetingRepo = repo;
            _feedbackRepo = feedbackRepository;

        }

        #region Add Meeting 
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddMeeting(CreateMeetingRequest dto)
        {
            var meeting = new Meeting
            {
                Description = dto.Description,
                MeetingType = dto.MeetingType,
                Date = dto.Date,
                Address = dto.Address,
                DoctorId = dto.DoctorId
            };

            await _meetingRepo.AddAsync(meeting);

            return Ok(new { message = "Meeting created successfully" });
        }
        #endregion

        #region Update Meeting
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateMeeting(int id, UpdateMeetingRequest dto)
        {
            var meeting = await _meetingRepo.GetByIdAsync(id);
            if (meeting == null)
                return NotFound(new { message = "Meeting not found" });

            if (meeting.Date <= DateTime.Now)
                return BadRequest(new { message = "Cannot update an ended meeting" });

            meeting.Description = dto.Description;
            meeting.MeetingType = (Models.Enums.Meeting.MeetingType)dto.MeetingType!;
            meeting.Date = (DateTime)dto.Date!;
            meeting.Address = dto.Address;
            meeting.DoctorId = (int)dto.DoctorId!;

            await _meetingRepo.UpdateAsync(meeting);

            return Ok(new { message = "Meeting updated successfully" });
        }
        #endregion

        #region Delete Meeting
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteMeeting(int id)
        {
            var meeting = await _meetingRepo.GetByIdAsync(id);
            if (meeting == null)
                return NotFound(new { message = "Meeting not found" });

            if (meeting.Date <= DateTime.Now)
                return BadRequest(new { message = "Cannot delete an ended meeting" });

            await _meetingRepo.DeleteAsync(meeting);

            return Ok(new { message = "Meeting deleted successfully" });
        }
        #endregion

        #region Get Meeting By Id
        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetById(int id)
        {
            var meeting = await _meetingRepo.GetByIdAsync(id);
            if (meeting == null)
                return NotFound(new { message = "Meeting not found" });

            return Ok(MeetingResponse.FromEntity(meeting));
        }
        #endregion

        #region Get Upcoming Meeting in all system by Admin
        [HttpGet("upcoming")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetUpcoming()
        {
            var meetings = await _meetingRepo.GetUpcomingAsync();
            

            return Ok(meetings.Select(MeetingResponse.FromEntity));
        }
        #endregion

        #region Get Ended Meeting in all system by Admin
        [HttpGet("ended")]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> GetEnded()
        {
            var meetings = await _meetingRepo.GetEndedAsync();

            return Ok(meetings.Select(MeetingResponse.FromEntity));
        }
        #endregion

        #region Get upcoming Meeting in all system by Mother
        [HttpGet("upcomingmother")]
        [Authorize(Roles = "Mother")]
        public async Task<IActionResult> GetUpcomingForMother()
        {
            var meetings = await _meetingRepo.GetUpcomingAsync();
            var motherIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier)
             ?? User.FindFirstValue(JwtRegisteredClaimNames.Sub);

            if (motherIdClaim == null)
                return Unauthorized(new { message = "Invalid token." });

            if (!int.TryParse(motherIdClaim, out int motherId))
                return BadRequest(new { message = "Invalid mother ID in token." });

            var registeredIds =
                await _meetingRepo.GetRegisteredMeetingIdsAsync(motherId);

            var result = meetings.Select(m =>
                MotherMeetingResponse.FromEntity(
                    m,
                    registeredIds.Contains(m.Id)));

            return Ok(result);
        }
        #endregion

        #region Get Ended Meeting in all system by Mother
        [HttpGet("endedmother")]
        [Authorize(Roles = "Mother")]
        public async Task<IActionResult> GetEndedForMother()
        {
            var meetings = await _meetingRepo.GetEndedAsync();
            var motherIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier)
             ?? User.FindFirstValue(JwtRegisteredClaimNames.Sub);

            if (motherIdClaim == null)
                return Unauthorized(new { message = "Invalid token." });

            if (!int.TryParse(motherIdClaim, out int motherId))
                return BadRequest(new { message = "Invalid mother ID in token." });

            var registeredIds =
                await _meetingRepo.GetRegisteredMeetingIdsAsync(motherId);

            var result = meetings.Select(m =>
                MotherMeetingResponse.FromEntity(
                    m,
                    registeredIds.Contains(m.Id)));

            return Ok(result);
        }
        #endregion

        #region Get All meeting count
        [HttpGet("count")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetTotalMeetingsCount()
        {
            var count = await _meetingRepo.GetTotalMeetingsCountAsync();
            return Ok(new CountResponse { Count = count });
        }
        #endregion

        #region Get Upcoming Meeting Count
        [HttpGet("count/upcoming")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetUpcomingMeetingsCount()
        {
            var count = await _meetingRepo.GetUpcomingMeetingsCountAsync();
            return Ok(new CountResponse { Count = count });
        }
        #endregion

        #region Get Ended Meeting Count
        [HttpGet("count/ended")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetEndedMeetingsCount()
        {
            var count = await _meetingRepo.GetEndedMeetingsCountAsync();
            return Ok(new CountResponse { Count = count });
        }
        #endregion

        #region Register Mother to a Meeting
        [HttpPost("{meetingId}/register")]
        [Authorize(Roles = "Mother")] 
        public async Task<IActionResult> Register(int meetingId)
        {

            var motherIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier)
             ?? User.FindFirstValue(JwtRegisteredClaimNames.Sub);

            if (motherIdClaim == null)
                return Unauthorized(new { message = "Invalid token." });

            if (!int.TryParse(motherIdClaim, out int motherId))
                return BadRequest(new { message = "Invalid mother ID in token." });

            var result = await _meetingRepo.AddMotherToMeetingAsync(meetingId, motherId);

            if (!result)
                return BadRequest(new { message = "Cannot register. Meeting may be ended or already registered." });

            return Ok(new { message = "Registered successfully." });
        }
        #endregion

        #region Unregister Mother from a Meeting
        [HttpDelete("{meetingId}/unregister")]
        [Authorize(Roles = "Mother")]
        public async Task<IActionResult> Unregister(int meetingId)
        {
            var motherIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier)
             ?? User.FindFirstValue(JwtRegisteredClaimNames.Sub);

            if (motherIdClaim == null)
                return Unauthorized(new { message = "Invalid token." });

            if (!int.TryParse(motherIdClaim, out int motherId))
                return BadRequest(new { message = "Invalid mother ID in token." });

            var result = await _meetingRepo.RemoveMotherFromMeetingAsync(meetingId, motherId);

            if (!result)
                return BadRequest(new { message = "Cannot unregister. Meeting may be ended or you are not registered." });

            return Ok(new { message = "Unregistered successfully." });
        }
        #endregion

        #region Add Feedback To Meeting
        [HttpPost("{meetingId}/feedback")]
        [Authorize(Roles = "Mother")]
        public async Task<IActionResult> AddFeedback(int meetingId, [FromBody] CreateFeedbackRequest request)
        {
            var motherIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier)
             ?? User.FindFirstValue(JwtRegisteredClaimNames.Sub);

            if (motherIdClaim == null)
                return Unauthorized(new { message = "Invalid token." });

            if (!int.TryParse(motherIdClaim, out int motherId))
                return BadRequest(new { message = "Invalid mother ID in token." });

            var meeting = await _meetingRepo.GetByIdAsync(meetingId);
            if (meeting == null)
                return NotFound(new { message = "Meeting not found" });

            if (meeting.Date > DateTime.Now)
                return BadRequest(new { message = "Meeting is not ended yet" });

            bool isRegistered = meeting.Mothers.Any(m => m.Id == motherId);
            if (!isRegistered)
                return BadRequest(new { message = "You are not registered in this meeting" });

            var existingFb = await _feedbackRepo.GetFeedbackForMotherAsync(meetingId, motherId);
            if (existingFb != null)
                return BadRequest(new { message = "You already wrote feedback for this meeting" });

            var feedback = new Feedback
            {
                Stars = request.Stars,
                Comment = request.Comment,
                MeetingId = meetingId,
                MotherId = motherId
            };

            await _feedbackRepo.AddAsync(feedback);

            return Ok(FeedbackResponse.FromEntity(feedback));
        }

        #endregion

        #region Update feedback
        [HttpPut("feedback/{feedbackId}")]
        [Authorize(Roles = "Mother")]
        public async Task<IActionResult> UpdateFeedbackById(int feedbackId, [FromBody] UpdateFeedbackRequest request)
        {
            var motherIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier)
             ?? User.FindFirstValue(JwtRegisteredClaimNames.Sub);

            if (motherIdClaim == null)
                return Unauthorized(new { message = "Invalid token." });

            if (!int.TryParse(motherIdClaim, out int motherId))
                return BadRequest(new { message = "Invalid mother ID in token." });

            var feedback = await _feedbackRepo.GetByIdAsync(feedbackId);
            if (feedback == null)
                return NotFound(new { message = "Feedback not found." });

            if (feedback.MotherId != motherId)
                return Forbid(); 

            feedback.Stars = request.Stars ?? feedback.Stars;
            feedback.Comment = request.Comment ?? feedback.Comment;

            await _feedbackRepo.UpdateAsync(feedback);
            return Ok(FeedbackResponse.FromEntity(feedback));
        }

        #endregion

        #region Delete Feedback
        [HttpDelete("feedback/{feedbackId}")]
        [Authorize(Roles = "Mother")]
        public async Task<IActionResult> DeleteFeedbackById(int feedbackId)
        {
            var motherIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier)
             ?? User.FindFirstValue(JwtRegisteredClaimNames.Sub);

            if (motherIdClaim == null)
                return Unauthorized(new { message = "Invalid token." });

            if (!int.TryParse(motherIdClaim, out int motherId))
                return BadRequest(new { message = "Invalid mother ID in token." });

            var feedback = await _feedbackRepo.GetByIdAsync(feedbackId);
            if (feedback == null)
                return NotFound(new { message = "Feedback not found." });

            if (feedback.MotherId != motherId)
                return Forbid(); 

            await _feedbackRepo.DeleteAsync(feedback);
            return Ok(new { message = "Feedback deleted successfully." });
        }

        #endregion

        #region Get all feedback for meeting 
        [HttpGet("{meetingId}/feedbacks")]
        [Authorize] 
        public async Task<IActionResult> GetMeetingFeedbacks(int meetingId)
        {
            var meeting = await _meetingRepo.GetByIdAsync(meetingId);
            if (meeting == null)
                return NotFound(new { message = "Meeting not found" });

            var feedbacks = await _feedbackRepo.GetFeedbacksForMeetingAsync(meetingId);

            return Ok(feedbacks.Select(FeedbackResponse.FromEntity));
        }
        #endregion

        #region Get all mothers registered in a particular meeting

        [HttpGet("{meetingId}/mothers")]
        [Authorize]
        public async Task<IActionResult> GetRegisteredMothers(int meetingId)
        {
            var mothers = await _meetingRepo.GetRegisteredMothersAsync(meetingId);

            if (mothers == null)
                return NotFound(new { message = "Meeting not found." });

            return Ok(mothers.Select(m => MotherBasicResponse.FromEntity(m)));
        }

        #endregion

        #region Get mother count in a particular meeting

        [HttpGet("{meetingId}/mothers/count")]
        [Authorize]
        public async Task<IActionResult> GetRegisteredMotherCount(int meetingId)
        {
            var count = await _meetingRepo.GetRegisteredMotherCountAsync(meetingId);

            return Ok(count);
        }

        #endregion

        #region Get feedback count for a particular meeting

        [HttpGet("{meetingId}/feedbacks/count")]
        [Authorize]
        public async Task<IActionResult> GetFeedbackCount(int meetingId)
        {
            var count = await _meetingRepo.GetFeedbackCountAsync(meetingId);

            return Ok(count);
        }

        #endregion

        #region Get Upcomming meeting for loged in mother 

        [HttpGet("mother/my-meetings/upcoming")]
        [Authorize(Roles = "Mother")]
        public async Task<IActionResult> GetMyUpcomingMeetings()
        {
            var motherIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier)
             ?? User.FindFirstValue(JwtRegisteredClaimNames.Sub);

            if (motherIdClaim == null)
                return Unauthorized(new { message = "Invalid token." });

            if (!int.TryParse(motherIdClaim, out int motherId))
                return BadRequest(new { message = "Invalid mother ID in token." });

            var meetings = await _meetingRepo.GetMotherUpcomingMeetingsAsync(motherId);

            return Ok(meetings.Select(MeetingResponse.FromEntity));
        }
        #endregion

        #region Get ended meeting for loged in mother 
        [HttpGet("mother/my-meetings/ended")]
        [Authorize(Roles = "Mother")]
        public async Task<IActionResult> GetMyEndedMeetings()
        {
            var motherIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier)
             ?? User.FindFirstValue(JwtRegisteredClaimNames.Sub);

            if (motherIdClaim == null)
                return Unauthorized(new { message = "Invalid token." });

            if (!int.TryParse(motherIdClaim, out int motherId))
                return BadRequest(new { message = "Invalid mother ID in token." });

            var meetings = await _meetingRepo.GetMotherEndedMeetingsAsync(motherId);

            return Ok(meetings.Select(MeetingResponse.FromEntity));
        }
        #endregion

        #region Get Upcomming meeting count for loged in mother 

        [HttpGet("mother/my-meetings/upcoming/count")]
        [Authorize(Roles = "Mother")]
        public async Task<IActionResult> GetMyMeetingsUpcommingCount()
        {
            var motherIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier)
             ?? User.FindFirstValue(JwtRegisteredClaimNames.Sub);

            if (motherIdClaim == null)
                return Unauthorized(new { message = "Invalid token." });

            if (!int.TryParse(motherIdClaim, out int motherId))
                return BadRequest(new { message = "Invalid mother ID in token." });

            var upcoming = await _meetingRepo.GetMotherUpcomingCountAsync(motherId);

            return Ok(upcoming);
        }

        #endregion

        #region Get ended meeting count for loged in mother 

        [HttpGet("mother/my-meetings/ended/count")]
        [Authorize(Roles = "Mother")]
        public async Task<IActionResult> GetMyMeetingsEndedCount()
        {
            var motherIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier)
             ?? User.FindFirstValue(JwtRegisteredClaimNames.Sub);

            if (motherIdClaim == null)
                return Unauthorized(new { message = "Invalid token." });

            if (!int.TryParse(motherIdClaim, out int motherId))
                return BadRequest(new { message = "Invalid mother ID in token." });

            var ended = await _meetingRepo.GetMotherEndedCountAsync(motherId);

            return Ok(ended);
        }

        #endregion

        #region Get upcomming meeting for loged in doctor 
        [HttpGet("doctor/my-meetings/upcoming")]
        [Authorize(Roles = "Doctor")]
        public async Task<IActionResult> GetDoctorUpcomingMeetings()
        {
            var doctorIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier)
             ?? User.FindFirstValue(JwtRegisteredClaimNames.Sub);

            if (doctorIdClaim == null)
                return Unauthorized(new { message = "Invalid token." });

            if (!int.TryParse(doctorIdClaim, out int doctorId))
                return BadRequest(new { message = "Invalid doctor ID in token." });

            var meetings = await _meetingRepo.GetDoctorUpcomingMeetingsAsync(doctorId);

            return Ok(meetings.Select(MeetingResponse.FromEntity));
        }

        #endregion

        #region Get ended meeting  for loged in doctor 

        [HttpGet("doctor/my-meetings/ended")]
        [Authorize(Roles = "Doctor")]
        public async Task<IActionResult> GetDoctorEndedMeetings()
        {
            var doctorIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier)
             ?? User.FindFirstValue(JwtRegisteredClaimNames.Sub);

            if (doctorIdClaim == null)
                return Unauthorized(new { message = "Invalid token." });

            if (!int.TryParse(doctorIdClaim, out int doctorId))
                return BadRequest(new { message = "Invalid doctor ID in token." });

            var meetings = await _meetingRepo.GetDoctorEndedMeetingsAsync(doctorId);

            return Ok(meetings.Select(MeetingResponse.FromEntity));
        }

        #endregion

        #region Get upcomming meeting count for loged in doctor 
        [HttpGet("doctor/my-meetings/upcoming/count")]
        [Authorize(Roles = "Doctor")]
        public async Task<IActionResult> GetDoctorMeetingsUpcomingCount()
        {
            var doctorIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier)
             ?? User.FindFirstValue(JwtRegisteredClaimNames.Sub);

            if (doctorIdClaim == null)
                return Unauthorized(new { message = "Invalid token." });

            if (!int.TryParse(doctorIdClaim, out int doctorId))
                return BadRequest(new { message = "Invalid doctor ID in token." });

            var upcoming = await _meetingRepo.GetDoctorUpcomingCountAsync(doctorId);

            return Ok( upcoming);
        }

        #endregion

        #region Get ended meeting count for loged in doctor 
        [HttpGet("doctor/my-meetings/ended/count")]
        [Authorize(Roles = "Doctor")]
        public async Task<IActionResult> GetDoctorMeetingsEndedCount()
        {
            var doctorIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier)
             ?? User.FindFirstValue(JwtRegisteredClaimNames.Sub);

            if (doctorIdClaim == null)
                return Unauthorized(new { message = "Invalid token." });

            if (!int.TryParse(doctorIdClaim, out int doctorId))
                return BadRequest(new { message = "Invalid doctor ID in token." });

            var ended = await _meetingRepo.GetDoctorEndedCountAsync(doctorId);

            return Ok( ended );
        }
        #endregion

    }
}
