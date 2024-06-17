using BaigiamasisDarbas.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaigiamasisDarbas.Contracts
{
    public interface IMeetingService
    {
        Task<IEnumerable<Meeting>> GetAllMeetingsAsync();
        Task<Meeting> GetMeetingByIdAsync(int id);
        Task AddMeetingAsync(Meeting meeting);
        Task UpdateMeetingAsync(Meeting meeting, int id);
        Task DeleteMeetingAsync(int id);
        Task AddParticipantsAsync(MeetingParticipant participant);
        Task RemoveParticipantAsync(int meetingId, int participantId);
    }
}
