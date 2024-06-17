using Moq;
using BaigiamasisDarbas.Services;
using BaigiamasisDarbas.Contracts;
using BaigiamasisDarbas.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

public class WorkerServiceTests
{
    private readonly Mock<IWorkerRepository> _mockRepository;
    private readonly Mock<IWorkerRepository> _mockCacheRepository;
    private readonly WorkerService _workerService;

    public WorkerServiceTests()
    {
        _mockRepository = new Mock<IWorkerRepository>();
        _mockCacheRepository = new Mock<IWorkerRepository>();
        _workerService = new WorkerService(_mockRepository.Object, _mockCacheRepository.Object);
    }


    [Fact]
    public async Task GetAllWorkersAsync_ReturnsAllWorkers()
    {
        // Arrange
        List<Worker> workers = new List<Worker> 
        {
            new Worker { Id = 1, Name = "Worker1" },
            new Worker { Id = 2, Name = "Worker2" },
            new Worker { Id = 3, Name = "Worker3" }
        };
        _mockRepository.Setup(repo => repo.GetAllAsync())
            .ReturnsAsync(workers);


        // Act
        var result = await _workerService.GetAllWorkersAsync();

        // Assert
        Assert.Equal(3, ((List<Worker>)result).Count);
    }

    [Fact]
    public async Task AddWorkerAsync_AddsWorker()
    {
        // Arrange
        var newWorker = new Worker { Id = 4, Name = "Worker4" };
        _mockRepository.Setup(repo => repo.AddAsync(newWorker))
            .Returns(Task.CompletedTask);
        _mockCacheRepository.Setup(repo => repo.AddAsync(newWorker))
            .Returns(Task.CompletedTask);

        // Act
        await _workerService.AddWorkerAsync(newWorker);

        // Assert
        _mockRepository.Verify(repo => repo.AddAsync(newWorker), Times.Once);
        _mockCacheRepository.Verify(repo => repo.AddAsync(newWorker), Times.Once);
    }

    [Fact]
    public async Task DeleteWorkerAsync_DeletesWorker()
    {
        // Arrange
        var workerId = 1;
        _mockRepository.Setup(repo => repo.DeleteAsync(workerId))
            .Returns(Task.CompletedTask);
        _mockCacheRepository.Setup(repo => repo.DeleteAsync(workerId))
            .Returns(Task.CompletedTask);

        // Act
        await _workerService.DeleteWorkerAsync(workerId);

        // Assert
        _mockRepository.Verify(repo => repo.DeleteAsync(workerId), Times.Once);
        _mockCacheRepository.Verify(repo => repo.DeleteAsync(workerId), Times.Once);
    }

    [Fact]
    public async Task GetAdminsInfoAsync_ReturnsAllAdmins()
    {
        // Arrange
        List<Admin> admins = new List<Admin>
        {
        new Admin { Id = 1, Name = "Admin1" },
        new Admin { Id = 2, Name = "Admin2" }
        };
        _mockRepository.Setup(repo => repo.GetAdminsInfoAsync())
            .ReturnsAsync(admins);
        _mockCacheRepository.Setup(repo => repo.GetAdminsInfoAsync())
            .ReturnsAsync(new List<Admin>());

        // Act
        var result = await _workerService.GetAdminsInfoAsync();

        // Assert
        Assert.Equal(2, ((List<Admin>)result).Count);
    }

    [Fact]
    public async Task UpdateWorkerAsync_UpdatesWorker()
    {
        // Arrange
        var workerId = 1;
        var worker = new Worker { Id = workerId, Name = "UpdatedWorker" };
        _mockRepository.Setup(repo => repo.UpdateAsync(worker, workerId))
            .Returns(Task.CompletedTask);
        _mockCacheRepository.Setup(repo => repo.UpdateAsync(worker, workerId))
            .Returns(Task.CompletedTask);

        // Act
        await _workerService.UpdateWorkerAsync(worker, workerId);

        // Assert
        _mockRepository.Verify(repo => repo.UpdateAsync(worker, workerId), Times.Once);
        _mockCacheRepository.Verify(repo => repo.UpdateAsync(worker, workerId), Times.Once);
    }

    


}
