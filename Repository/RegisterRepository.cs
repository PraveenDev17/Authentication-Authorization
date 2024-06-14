using Assignment.Data;
using Assignment.Models;
using Assignment.Services.IServices;

namespace Assignment.Repository
{
    public class RegisterRepository : IRegisterRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public RegisterRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> Create(Register entity)
        {
            if (IsUserExist(entity.UserName, entity.Email))
            {
                return false;
            }
            await _dbContext.Register.AddAsync(entity);
            await Save();
            return true;
        }

        public bool IsUserExist(string uname, string email)
        {
            var usernameExists = _dbContext.Register.Any(x => x.UserName.Trim() == uname.Trim());
            var emailExists = _dbContext.Register.Any(x => x.Email.Trim() == email.Trim());
            return usernameExists || emailExists;
        }

        public async Task Save()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}
