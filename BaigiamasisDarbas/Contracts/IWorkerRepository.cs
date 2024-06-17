using BaigiamasisDarbas.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaigiamasisDarbas.Contracts
{
    public interface IWorkerRepository
    {
        Task<IEnumerable<Worker>> GetAllAsync();
        Task<Worker> GetByIdAsync(int id);
        Task AddAsync(Worker worker);
        Task UpdateAsync(Worker worker, int id);
        Task DeleteAsync(int id);
        Task<IEnumerable<Admin>> GetAdminsInfoAsync();

    }
}
