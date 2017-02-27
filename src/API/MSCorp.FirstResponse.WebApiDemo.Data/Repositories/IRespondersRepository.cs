using System.Collections.Generic;
using System.Threading.Tasks;
using MSCorp.FirstResponse.WebApiDemo.Data.Entities;

namespace MSCorp.FirstResponse.WebApiDemo.Data.Repositories
{
    public interface IRespondersRepository
    {
        IEnumerable<Responder> GetAll();
        Task<IEnumerable<Responder>> GetAllByCityAsync(int city);
    }
}
