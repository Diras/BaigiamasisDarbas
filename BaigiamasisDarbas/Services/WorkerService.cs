using Amazon.Runtime;
using BaigiamasisDarbas.Contracts;
using BaigiamasisDarbas.Models;
using BaigiamasisDarbas.Repositories;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
            await _repository.AddAsync(worker);
            await _cacheRepository.AddAsync(worker);
        }

        public async Task DeleteWorkerAsync(int id)
        {
            await _repository.DeleteAsync(id);
            await _cacheRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<Admin>> GetAdminsInfoAsync()
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

        public async Task<IEnumerable<Worker>> GetAllWorkersAsync()
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

        public async Task UpdateWorkerAsync(Worker worker, int id)
        {
            await _repository.UpdateAsync(worker, id);
            await _cacheRepository.UpdateAsync(worker, id);
        }
    }
}
