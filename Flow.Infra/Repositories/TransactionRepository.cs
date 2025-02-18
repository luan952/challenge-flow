using Flow.Core.Entities;
using Flow.Core.Repositories;
using Flow.Infra.Data;

namespace Flow.Infra.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly RelationalDbContext _context;
        public TransactionRepository(RelationalDbContext context)
        {
            _context = context;
        }

        public async Task AddTransaction(Transaction transaction) =>
            await _context.Transactions.AddAsync(transaction);
    }
}
