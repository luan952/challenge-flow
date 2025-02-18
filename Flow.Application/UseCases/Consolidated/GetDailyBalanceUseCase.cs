
using Flow.Core.Entities;
using Flow.Infra.Data;
using MongoDB.Driver;

namespace Flow.Application.UseCases.Consolidated
{
    public class GetDailyBalanceUseCase : IGetDailyBalanceUseCase
    {
        private readonly MongoDbContext _context;
        public GetDailyBalanceUseCase(MongoDbContext context)
        {
            _context = context;
        }
        public async Task<DailyBalance> Execute(DateTime date)
        {
            var filter = Builders<DailyBalance>.Filter.Eq(d => d.Date, date.Date);
            var dailyBalance = await _context.DailyBalance.Find(d => d.Date == date).FirstOrDefaultAsync();
            return dailyBalance;
        }
    }
}
