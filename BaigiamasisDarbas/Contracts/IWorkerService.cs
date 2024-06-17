using BaigiamasisDarbas.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaigiamasisDarbas.Contracts
{
    public interface IWorkerService
    {
        Task<IEnumerable<Worker>> GetAllWorkersAsync();
        Task AddWorkerAsync(Worker worker);
        Task UpdateWorkerAsync(Worker worker, int id);
        Task DeleteWorkerAsync(int id);
        Task<IEnumerable<Admin>> GetAdminsInfoAsync();
    }
}
