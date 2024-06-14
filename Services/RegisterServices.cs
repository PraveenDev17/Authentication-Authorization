using Assignment.Models;
using Assignment.Services.IServices;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;

namespace Assignment.Services
{
    public class RegisterServices : IRegisterServices
    {
        private readonly IRegisterRepository _registerRepository;
        private readonly IDataProtectionProvider _dataProtectionProvider;

        public RegisterServices(IRegisterRepository registerRepository, IDataProtectionProvider dataProtectionProvider)
        {
            _registerRepository = registerRepository;
            _dataProtectionProvider = dataProtectionProvider;
        }

        public async Task<bool> RegisterUser(Register entity)
        {
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(entity.Password);
            entity.Password = hashedPassword;
            ActionResult result = Encrypt(entity.Credit_Card);
            if (result is JsonResult jsonResult)
            {
                entity.Credit_Card = jsonResult.Value.ToString();
            }
            return await _registerRepository.Create(entity);
        }

        public ActionResult Encrypt(string plaintext)
        {
            var pkey = Environment.GetEnvironmentVariable("EKEY");;
            var protector = _dataProtectionProvider.CreateProtector(pkey);
            return new JsonResult(protector.Protect(plaintext));
        }
    }
}
