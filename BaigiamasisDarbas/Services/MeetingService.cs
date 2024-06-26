﻿using BaigiamasisDarbas.Contracts;
using BaigiamasisDarbas.Models;
using BaigiamasisDarbas.Repositories;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
            await _repository.AddMeetingAsync(meeting);
            await _cacheRepository.AddMeetingAsync(meeting);
        }

        public async Task AddParticipantsAsync(MeetingParticipant participant)
        {
            await _repository.AddParticipantsAsync(participant);
            await _cacheRepository.AddParticipantsAsync(participant);
        }

        public async Task DeleteMeetingAsync(int id)
        {
            await _repository.DeleteMeetingAsync(id);
            await _cacheRepository.DeleteMeetingAsync(id);
        }

        public async Task<IEnumerable<Meeting>> GetAllMeetingsAsync()
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

        public async Task<Meeting> GetMeetingByIdAsync(int id)
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

        public async Task RemoveParticipantAsync(int meetingId, int participantId)
        {
            await _repository.RemoveParticipantAsync(meetingId, participantId);
            var meeting = await _cacheRepository.GetMeetingByIdAsync(meetingId);
            if (meeting != null)
            {
                meeting.Participants.RemoveAll(p => p.ParticipantId == participantId);
                await _cacheRepository.UpdateMeetingAsync(meeting, meetingId);
            }
        }

        public async Task UpdateMeetingAsync(Meeting meeting, int id)
        {
            await _repository.UpdateMeetingAsync(meeting, id);
            await _cacheRepository.UpdateMeetingAsync(meeting, id);
        }
    }
}
