using Assignment.DTO.Register;
using Assignment.Models;
using Microsoft.AspNetCore.Mvc;

namespace Assignment.Services.IServices
{
    public interface ILoginServices
    {
        ActionResult LoginUser(Register entity);
        ActionResult GetRole(string uname);
        Task<Register> Get(string name);
        ActionResult Decrypt(string credit_card);
        Task Update(Register entity);
        Task<List<Register>> GetAll();
        Task<Register> GetById(int id);
        Task Delete(Register entity);
    }
}