using Assignment.Models;
using Microsoft.AspNetCore.Mvc;

namespace Assignment.Services.IServices
{
    public interface IRegisterServices
    {
        Task<bool> RegisterUser(Register entity);
        public ActionResult Encrypt(string plaintext);
    }
}