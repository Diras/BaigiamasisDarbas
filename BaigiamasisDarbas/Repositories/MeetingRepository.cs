using BaigiamasisDarbas.Contracts;
using BaigiamasisDarbas.Database;
using BaigiamasisDarbas.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaigiamasisDarbas.Repositories
{
    public class MeetingRepository : IMeetingRepository
    {
        private MeetingDbContext _dbContext;

        public MeetingRepository()
        {
            _dbContext = new MeetingDbContext();
        }
        public async Task AddMeetingAsync(Meeting meeting)
        {
            await _dbContext.Meetings.AddAsync(meeting);
            await _dbContext.Admins.AddAsync(meeting.ResponsiblePerson);
            await _dbContext.SaveChangesAsync();
        }

        public async Task AddParticipantsAsync(MeetingParticipant participant)
        {
            await _dbContext.Participants.AddAsync(participant);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteMeetingAsync(int id)
        {
            Meeting meetingToDelete = await _dbContext.Meetings.FindAsync(id);
            _dbContext.Meetings.Remove(meetingToDelete);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Meeting>> GetAllMeetingsAsync()
        {
            return await _dbContext.Meetings
                .Include(m => m.ResponsiblePerson)
                .ToListAsync();
        }

        public async Task<Meeting> GetMeetingByIdAsync(int id)
        {
            return await _dbContext.Meetings.FindAsync(id);
        }

        public async Task RemoveParticipantAsync(int meetingId, int participantId)
        {
            MeetingParticipant meetingParticipantToDelete = await _dbContext.Participants.FirstOrDefaultAsync(x => x.MeetingId == meetingId && x.ParticipantId == participantId);
            _dbContext.Participants.Remove(meetingParticipantToDelete);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateMeetingAsync(Meeting meeting, int id)
        {
            Meeting meetingToUpdate = await GetMeetingByIdAsync(id);
            _dbContext.Meetings.Entry(meetingToUpdate).CurrentValues.SetValues(meeting);
            _dbContext.Admins.Entry(meetingToUpdate.ResponsiblePerson).CurrentValues.SetValues(meeting.ResponsiblePerson);
            await _dbContext.SaveChangesAsync();
        }
    }
}
