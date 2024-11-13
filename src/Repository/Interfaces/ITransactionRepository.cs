using Repository.Base;
using Repository.Entities;

namespace Repository.Interfaces;

public interface ITransactionRepository : IBaseRepository<Transaction>
{
    /*Task<Transaction?> GetTransactionWithDetailsAsync(int id);*/

    Task SaveChangesAsync();
}