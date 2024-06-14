using Assignment.Data;
using Assignment.Models;
using Assignment.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Assignment.Repository
{
    [AutoValidateAntiforgeryToken]
    public class LoginRepository : ILoginRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public LoginRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Delete(Register entity)
        {
            _dbContext.Register.Remove(entity);
            await Save();
        }

        public async Task<Register> Get(string name)
        {
            var userData = await _dbContext.Register.FirstOrDefaultAsync(x => x.UserName == name);
            return userData;
        }

        public async Task<List<Register>> GetAll()
        {
            var data = await _dbContext.Register.ToListAsync();
            return data;
        }

        public async Task<Register> GetById(int id)
        {
            var userById = await _dbContext.Register.FindAsync(id);
            return userById;
        }

        public ActionResult GetPassword(string name)
        {
            var user = _dbContext.Register.FirstOrDefault(x => x.UserName.Trim() == name.Trim());
            if (user != null)
            {
                return new JsonResult(user.Password);
            }
            return null;
        }

        public ActionResult GetRole(string name)
        {
            var user = _dbContext.Register.FirstOrDefault(x => x.UserName.Trim() == name.Trim());
            if (user != null)
            {
                return new JsonResult(user.Role);
            }
            return new JsonResult(null);
        }

        public ActionResult Login(Register entity)
        {
            var name = entity.UserName;
            ActionResult result = GetPassword(name);
            var storedPassword = "";

            if (result is JsonResult jsonResult && jsonResult.Value != null)
            {
                storedPassword = jsonResult.Value.ToString();
                if (!string.IsNullOrEmpty(storedPassword) && BCrypt.Net.BCrypt.Verify(entity.Password, storedPassword))
                {
                    var user = _dbContext.Register.FirstOrDefault(x => x.UserName.Trim() == name.Trim());
                    if (user != null)
                    {
                        return new JsonResult(user.Role);
                    }
                }
            }
            return null;

        }

        public async Task Save()
        {
            await _dbContext.SaveChangesAsync();
        }

        public async Task Update(Register entity)
        {
            var existingEntity = await _dbContext.Register.FindAsync(entity.Id);
            if (existingEntity != null)
            {
                entity.Password = existingEntity.Password;
                _dbContext.Entry(existingEntity).State = EntityState.Detached;
            }
            _dbContext.Register.Attach(entity);
            _dbContext.Entry(entity).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }
    }
}
