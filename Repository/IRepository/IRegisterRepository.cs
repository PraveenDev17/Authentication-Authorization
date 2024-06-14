using Assignment.Models;

namespace Assignment.Services.IServices
{
    public interface IRegisterRepository
    {
        Task<bool> Create(Register entity);
        bool IsUserExist(string uname,string email);
        Task Save();
    }
}