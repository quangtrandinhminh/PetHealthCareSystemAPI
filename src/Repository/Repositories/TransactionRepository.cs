using Repository.Base;
using Repository.Entities;
using Repository.Interfaces;

namespace Repository.Repositories;

public class TransactionRepository : BaseRepository<Transaction>, ITransactionRepository
{
    /*public async Task<Transaction?> GetTransactionWithDetailsAsync(int id) =>
        await TransactionDao.GetTransactionWithDetailsAsync(id);*/

    public async Task<Transaction?> CreateTransactionAsync(Transaction transaction)
    {
        await using var context = new AppDbContext();
        await context.Set<Transaction>().AddAsync(transaction);
        return transaction;
    }

    // save changes
    public async Task SaveChangesAsync()
    {
        await using var context = new AppDbContext();
        await context.SaveChangesAsync();
    }
}