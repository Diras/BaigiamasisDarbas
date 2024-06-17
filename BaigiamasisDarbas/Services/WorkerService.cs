using BaigiamasisDarbas.Contracts;
using BaigiamasisDarbas.Models;
using MongoDB.Driver;
using Serilog;

namespace BaigiamasisDarbas.Services
{
    public class WorkerService : IWorkerService
    {
        private readonly IWorkerRepository _repository;
        private readonly IWorkerRepository _cacheRepository;

        public WorkerService(IWorkerRepository repository, IWorkerRepository cacheRepository)
        {
            _repository = repository;
            _cacheRepository = cacheRepository;
        }

        public WorkerService() { }

        public async Task AddWorkerAsync(Worker worker)
        {
            try
            {
                await _repository.AddAsync(worker);
                await _cacheRepository.AddAsync(worker);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred while adding worker");
                throw;
            }
        }

        public async Task DeleteWorkerAsync(int id)
        {
            try
            {
                await _repository.DeleteAsync(id);
                await _cacheRepository.DeleteAsync(id);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred while deleting worker with ID: {Id}", id);
                throw;
            }
        }

        public async Task<IEnumerable<Admin>> GetAdminsInfoAsync()
        {
            try
            {
                var admins = await _cacheRepository.GetAdminsInfoAsync();
                if (!admins.Any())
                {
                    admins = await _repository.GetAdminsInfoAsync();
                    foreach (var admin in admins)
                    {
                        await _cacheRepository.AddAsync(admin);
                    }
                }
                return admins;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred while getting admins info");
                throw;
            }
        }

        public async Task<IEnumerable<Worker>> GetAllWorkersAsync()
        {
            try
            {
                var workers = await _cacheRepository.GetAllAsync();
                if (!workers.Any())
                {
                    workers = await _repository.GetAllAsync();
                    foreach (var worker in workers)
                    {
                        await _cacheRepository.AddAsync(worker);
                    }
                }
                return workers;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred while getting all workers");
                throw;
            }
        }

        public async Task UpdateWorkerAsync(Worker worker, int id)
        {
            try
            {
                await _repository.UpdateAsync(worker, id);
                await _cacheRepository.UpdateAsync(worker, id);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred while updating worker with ID: {Id}", id);
                throw;
            }
        }
    }
}
