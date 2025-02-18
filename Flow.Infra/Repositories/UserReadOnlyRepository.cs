using Flow.Core.Repositories;
using Flow.Infra.Data;
using Microsoft.EntityFrameworkCore;

namespace Flow.Infra.Repositories
{
    public class UserReadOnlyRepository : IUserReadOnlyRepository
    {
        private readonly RelationalDbContext _context;
        public UserReadOnlyRepository(RelationalDbContext context) {
            _context = context;
        }
        public async Task<bool> IsUserExistsById(Guid userId) =>
            await _context.Users.AnyAsync(u => u.Id.Equals(userId));
    }
}
