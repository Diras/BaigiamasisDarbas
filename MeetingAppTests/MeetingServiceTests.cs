using Moq;
using BaigiamasisDarbas.Services;
using BaigiamasisDarbas.Contracts;
using BaigiamasisDarbas.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

public class MeetingServiceTests
{
    private readonly Mock<IMeetingRepository> _mockRepository;
    private readonly Mock<IMeetingRepository> _mockCacheRepository;
    private readonly MeetingService _meetingService;

    public MeetingServiceTests()
    {
        _mockRepository = new Mock<IMeetingRepository>();
        _mockCacheRepository = new Mock<IMeetingRepository>();
        _meetingService = new MeetingService(_mockRepository.Object, _mockCacheRepository.Object);
    }

    [Fact]
    public async Task GetAllMeetingsAsync_ReturnsAllMeetings()
    {
        // Arrange
        List<Meeting> meetings = new List<Meeting>()
        {
            new Meeting { Id = 1, Name = "Meeting1" },
            new Meeting { Id = 2, Name = "Meeting2" },
            new Meeting { Id = 3, Name = "Meeting3" }
        };
        
        _mockRepository.Setup(repo => repo.GetAllMeetingsAsync())
            .ReturnsAsync(meetings);

        // Act
        var result = await _meetingService.GetAllMeetingsAsync();

        // Assert
        Assert.Equal(3, ((List<Meeting>)result).Count);
    }

    [Fact]
    public async Task AddMeetingAsync_AddsMeeting()
    {
        // Arrange
        var newMeeting = new Meeting { Id = 4, Name = "Meeting4" };
        _mockRepository.Setup(repo => repo.AddMeetingAsync(newMeeting))
            .Returns(Task.CompletedTask);
        _mockCacheRepository.Setup(repo => repo.AddMeetingAsync(newMeeting))
            .Returns(Task.CompletedTask);

        // Act
        await _meetingService.AddMeetingAsync(newMeeting);

        // Assert
        _mockRepository.Verify(repo => repo.AddMeetingAsync(newMeeting), Times.Once);
        _mockCacheRepository.Verify(repo => repo.AddMeetingAsync(newMeeting), Times.Once);
    }

    [Fact]
    public async Task DeleteMeetingAsync_DeletesMeeting()
    {
        // Arrange
        var meetingId = 1;
        _mockRepository.Setup(repo => repo.DeleteMeetingAsync(meetingId))
            .Returns(Task.CompletedTask);
        _mockCacheRepository.Setup(repo => repo.DeleteMeetingAsync(meetingId))
            .Returns(Task.CompletedTask);

        // Act
        await _meetingService.DeleteMeetingAsync(meetingId);

        // Assert
        _mockRepository.Verify(repo => repo.DeleteMeetingAsync(meetingId), Times.Once);
        _mockCacheRepository.Verify(repo => repo.DeleteMeetingAsync(meetingId), Times.Once);
    }


    [Fact]
    public async Task AddParticipantsAsync_AddsParticipant()
    {
        // Arrange
        var newParticipant = new MeetingParticipant { MeetingId = 1, ParticipantId = 1 };
        _mockRepository.Setup(repo => repo.AddParticipantsAsync(newParticipant))
            .Returns(Task.CompletedTask);
        _mockCacheRepository.Setup(repo => repo.AddParticipantsAsync(newParticipant))
            .Returns(Task.CompletedTask);

        // Act
        await _meetingService.AddParticipantsAsync(newParticipant);

        // Assert
        _mockRepository.Verify(repo => repo.AddParticipantsAsync(newParticipant), Times.Once);
        _mockCacheRepository.Verify(repo => repo.AddParticipantsAsync(newParticipant), Times.Once);
    }

    [Fact]
    public async Task GetMeetingByIdAsync_ReturnsMeeting()
    {
        // Arrange
        var meetingId = 1;
        _mockCacheRepository.Setup(repo => repo.GetMeetingByIdAsync(meetingId))
            .ReturnsAsync((Meeting)null);
        _mockRepository.Setup(repo => repo.GetMeetingByIdAsync(meetingId))
            .ReturnsAsync(new Meeting { Id = meetingId, Name = "Meeting1" });

        // Act
        var result = await _meetingService.GetMeetingByIdAsync(meetingId);

        // Assert
        Assert.IsType<Meeting>(result);
        Assert.Equal(meetingId, result.Id);
    }

    [Fact]
    public async Task RemoveParticipantAsync_RemovesParticipant()
    {
        // Arrange
        var meetingId = 1;
        var participantId = 1;
        _mockRepository.Setup(repo => repo.RemoveParticipantAsync(meetingId, participantId))
            .Returns(Task.CompletedTask);
        _mockCacheRepository.Setup(repo => repo.GetMeetingByIdAsync(meetingId))
            .ReturnsAsync(new Meeting { Id = meetingId, Name = "Meeting1", Participants = new List<MeetingParticipant> { new MeetingParticipant { MeetingId = meetingId, ParticipantId = participantId } } });

        // Act
        await _meetingService.RemoveParticipantAsync(meetingId, participantId);

        // Assert
        _mockRepository.Verify(repo => repo.RemoveParticipantAsync(meetingId, participantId), Times.Once);
    }

    [Fact]
    public async Task UpdateMeetingAsync_UpdatesMeeting()
    {
        // Arrange
        var meetingId = 1;
        var meeting = new Meeting { Id = meetingId, Name = "UpdatedMeeting" };
        _mockRepository.Setup(repo => repo.UpdateMeetingAsync(meeting, meetingId))
            .Returns(Task.CompletedTask);
        _mockCacheRepository.Setup(repo => repo.UpdateMeetingAsync(meeting, meetingId))
            .Returns(Task.CompletedTask);

        // Act
        await _meetingService.UpdateMeetingAsync(meeting, meetingId);

        // Assert
        _mockRepository.Verify(repo => repo.UpdateMeetingAsync(meeting, meetingId), Times.Once);
        _mockCacheRepository.Verify(repo => repo.UpdateMeetingAsync(meeting, meetingId), Times.Once);
    }


}
