using BaigiamasisDarbas.Contracts;
using BaigiamasisDarbas.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BaigiamasisDarbas.Repositories
{
    public class CacheMeetingRepository : IMeetingRepository
    {
        private IMongoCollection<Meeting> _meetings;

        public CacheMeetingRepository(IMongoClient mongoClient)
        {
            var database = mongoClient.GetDatabase("Learning");
            _meetings = database.GetCollection<Meeting>("Meetings");
        }

        public async Task AddMeetingAsync(Meeting meeting)
        {
            await _meetings.InsertOneAsync(meeting);
        }

        public async Task AddParticipantsAsync(MeetingParticipant participant)
        {
            var filter = Builders<Meeting>.Filter.Eq(m => m.Id, participant.MeetingId);
            var update = Builders<Meeting>.Update.Push(m => m.Participants, participant);
            await _meetings.UpdateOneAsync(filter, update);
        }

        public async Task DeleteMeetingAsync(int id)
        {
            var filter = Builders<Meeting>.Filter.Eq(m => m.Id, id);
            await _meetings.DeleteOneAsync(filter);
        }

        public async Task<IEnumerable<Meeting>> GetAllMeetingsAsync()
        {
            return await _meetings.Find(_ => true).ToListAsync();
        }

        public async Task<Meeting> GetMeetingByIdAsync(int id)
        {
            var filter = Builders<Meeting>.Filter.Eq(m => m.Id, id);
            return await _meetings.Find(filter).FirstOrDefaultAsync();
        }

        public async Task RemoveParticipantAsync(int meetingId, int participantId)
        {
            var filter = Builders<Meeting>.Filter.Eq(m => m.Id, meetingId);
            var update = Builders<Meeting>.Update.PullFilter(m => m.Participants, p => p.ParticipantId == participantId);
            await _meetings.UpdateOneAsync(filter, update);
        }

        public async Task UpdateMeetingAsync(Meeting meeting, int id)
        {
            var filter = Builders<Meeting>.Filter.Eq(m => m.Id, id);
            await _meetings.ReplaceOneAsync(filter, meeting);
        }
    }
}
