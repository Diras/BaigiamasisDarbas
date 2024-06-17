using BaigiamasisDarbas.Contracts;
using BaigiamasisDarbas.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BaigiamasisDarbas.Repositories
{
    public class CacheWorkerRepository : IWorkerRepository
    {
        private IMongoCollection<Worker> _workers;

        public CacheWorkerRepository(IMongoClient mongoClient)
        {

            var database = mongoClient.GetDatabase("Learning");
            _workers = database.GetCollection<Worker>("Workers");
        }

        public async Task AddAsync(Worker worker)
        {
            await _workers.InsertOneAsync(worker);
        }

        public async Task DeleteAsync(int id)
        {
            var filter = Builders<Worker>.Filter.Eq(w => w.Id, id);
            await _workers.DeleteOneAsync(filter);
        }

        public async Task<IEnumerable<Admin>> GetAdminsInfoAsync()
        {
            var workers = await _workers.Find(_ => true).ToListAsync();
            return workers.OfType<Admin>();
        }

        public async Task<IEnumerable<Worker>> GetAllAsync()
        {
            return await _workers.Find(_ => true).ToListAsync();
        }

        public async Task<Worker> GetByIdAsync(int id)
        {
            var filter = Builders<Worker>.Filter.Eq(w => w.Id, id);
            return await _workers.Find(filter).FirstOrDefaultAsync();
        }

        public async Task UpdateAsync(Worker worker, int id)
        {
            var filter = Builders<Worker>.Filter.Eq(w => w.Id, id);
            await _workers.ReplaceOneAsync(filter, worker);
        }
    }
}
