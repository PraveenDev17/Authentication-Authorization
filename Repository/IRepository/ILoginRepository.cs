using Assignment.DTO.Register;
using Assignment.Models;
using Microsoft.AspNetCore.Mvc;

namespace Assignment.Repository.IRepository
{
    public interface ILoginRepository
    {
        ActionResult Login (Register entity);
        public ActionResult GetPassword(string name);
        public ActionResult GetRole(string name);
        Task<List<Register>> GetAll();
        Task<Register> Get(string name);
        Task Update(Register entity);
        Task<Register> GetById(int id);
        Task Delete(Register entity);
        Task Save();
    }
}