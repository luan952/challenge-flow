using Flow.Core.Entities;
using Flow.Core.Repositories;
using Flow.Infra.Data;
using MongoDB.Driver;

namespace Flow.Infra.Repositories
{
    public class DailyBalanceRepository : IDailyBalanceRepository
    {
        private readonly MongoDbContext _context;
        public DailyBalanceRepository(MongoDbContext context)
        {
            _context = context;
        }

        public async Task<DailyBalance> FindDailyBalanceByDay(DateTime date)
        {
            return await _context.DailyBalance.Find(d => d.Date.Date == date.Date).FirstOrDefaultAsync();
        }
    }
}
