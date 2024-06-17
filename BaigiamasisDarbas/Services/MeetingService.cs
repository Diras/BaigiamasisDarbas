using BaigiamasisDarbas.Contracts;
using BaigiamasisDarbas.Models;
using MongoDB.Driver;
using Serilog;

namespace BaigiamasisDarbas.Services
{
    public class MeetingService : IMeetingService
    {
        private readonly IMeetingRepository _repository;
        private readonly IMeetingRepository _cacheRepository;

        public MeetingService(IMeetingRepository repository, IMeetingRepository cacheRepository)
        {
            _repository = repository;
            _cacheRepository = cacheRepository;
        }

        public async Task AddMeetingAsync(Meeting meeting)
        {
            try
            {
                await _repository.AddMeetingAsync(meeting);
                await _cacheRepository.AddMeetingAsync(meeting);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred while adding meeting");
                throw;
            }
        }

        public async Task AddParticipantsAsync(MeetingParticipant participant)
        {
            try
            {
                await _repository.AddParticipantsAsync(participant);
                await _cacheRepository.AddParticipantsAsync(participant);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred while adding participant");
                throw;
            }
        }

        public async Task DeleteMeetingAsync(int id)
        {
            try
            {
                await _repository.DeleteMeetingAsync(id);
                await _cacheRepository.DeleteMeetingAsync(id);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred while deleting meeting with ID: {Id}", id);
                throw;
            }
        }

        public async Task<IEnumerable<Meeting>> GetAllMeetingsAsync()
        {
            try
            {
                var meetings = await _cacheRepository.GetAllMeetingsAsync();
                if (!meetings.Any())
                {
                    meetings = await _repository.GetAllMeetingsAsync();
                    foreach (var meeting in meetings)
                    {
                        await _cacheRepository.AddMeetingAsync(meeting);
                    }
                }
                return meetings;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred while getting all meetings");
                throw;
            }
        }

        public async Task<Meeting> GetMeetingByIdAsync(int id)
        {
            try
            {
                var meeting = await _cacheRepository.GetMeetingByIdAsync(id);
                if (meeting == null)
                {
                    meeting = await _repository.GetMeetingByIdAsync(id);
                    if (meeting != null)
                    {
                        await _cacheRepository.AddMeetingAsync(meeting);
                    }
                }
                return meeting;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred while getting meeting by ID: {Id}", id);
                throw;
            }
        }

        public async Task RemoveParticipantAsync(int meetingId, int participantId)
        {
            try
            {
                await _repository.RemoveParticipantAsync(meetingId, participantId);
                var meeting = await _cacheRepository.GetMeetingByIdAsync(meetingId);
                if (meeting != null)
                {
                    meeting.Participants.RemoveAll(p => p.ParticipantId == participantId);
                    await _cacheRepository.UpdateMeetingAsync(meeting, meetingId);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred while removing participant with ID: {ParticipantId} from meeting with ID: {MeetingId}", participantId, meetingId);
                throw;
            }
        }

        public async Task UpdateMeetingAsync(Meeting meeting, int id)
        {
            try
            {
                await _repository.UpdateMeetingAsync(meeting, id);
                await _cacheRepository.UpdateMeetingAsync(meeting, id);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred while updating meeting with ID: {Id}", id);
                throw;
            }
        }
    }
}
