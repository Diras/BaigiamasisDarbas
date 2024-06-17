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
    internal class WorkerRepository : IWorkerRepository
    {
        private MeetingDbContext _dbContext;

        public WorkerRepository()
        {
            _dbContext = new MeetingDbContext();
        }
        public async Task AddAsync(Worker worker)
        {
            await _dbContext.Workers.AddAsync(worker);
            _dbContext.SaveChanges();
        }

        public async Task DeleteAsync(int id)
        {
            Worker workerToDelete = await _dbContext.Workers.FindAsync(id);
            _dbContext.Workers.Remove(workerToDelete);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Admin>> GetAdminsInfoAsync()
        {
            return await _dbContext.Admins.ToListAsync();
        }

        public async Task<IEnumerable<Worker>> GetAllAsync()
        {
            return await _dbContext.Workers.ToListAsync();
        }

        public async Task<Worker> GetByIdAsync(int id)
        {
            return await _dbContext.Workers.FindAsync(id);
        }

        public async Task UpdateAsync(Worker worker, int id)
        {
            Worker workerForUpdate = await GetByIdAsync(id);
            _dbContext.Workers.Entry(workerForUpdate).CurrentValues.SetValues(worker);
            _dbContext.SaveChanges();
        }
    }
}
