using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Assignment.Models;
using Assignment.Repository.IRepository;
using Assignment.Services.IServices;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Assignment.Services
{
    public class LoginServices : ILoginServices
    {
        private readonly IConfiguration _configuration;
        private readonly ILoginRepository _loginRepository;
        private readonly IDataProtectionProvider _dataProtectionProvider;

        public LoginServices(IConfiguration configuration, ILoginRepository loginRepository, IDataProtectionProvider dataProtectionProvider)
        {
            _configuration = configuration;
            _loginRepository = loginRepository;
            _dataProtectionProvider = dataProtectionProvider;
        }

        public async Task<Register> Get(string name)
        {
            return await _loginRepository.Get(name);
        }

        public async Task<List<Register>> GetAll()
        {
            return await _loginRepository.GetAll();
        }

        public ActionResult GetRole(string uname)
        {
            return _loginRepository.GetRole(uname);
        }

        public ActionResult LoginUser(Register entity)
        {
            var result = _loginRepository.Login(entity);
            if (result != null)
            {
                var issuer = Environment.GetEnvironmentVariable("JWT_ISSUER");
                var audience = Environment.GetEnvironmentVariable("JWT_AUDIENCE");
                var jwtKey = Environment.GetEnvironmentVariable("JWT_KEY");

                var key = Encoding.UTF8.GetBytes(jwtKey);
                var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature);

                string role = "";
                ActionResult getRole = GetRole(entity.UserName);
                if (getRole is JsonResult jsonResult && jsonResult.Value != null)
                {
                    role = jsonResult.Value.ToString();
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, entity.UserName),
                        new Claim(ClaimTypes.Role, role)
                    };

                    var expires = DateTime.UtcNow.AddMinutes(10);
                    var tokenDescriptor = new SecurityTokenDescriptor
                    {
                        Subject = new ClaimsIdentity(claims),
                        Expires = expires,
                        Issuer = issuer,
                        Audience = audience,
                        SigningCredentials = signingCredentials
                    };
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var token = tokenHandler.CreateToken(tokenDescriptor);
                    var jwtToken = tokenHandler.WriteToken(token);
                    return new JsonResult(jwtToken);
                }
            }
            return null;
        }

        public async Task Update(Register entity)
        {
            entity.Credit_Card = Encrypt(entity.Credit_Card);
            await _loginRepository.Update(entity);
        }

        public async Task<Register> GetById(int id)
        {
            return await _loginRepository.GetById(id);
        }

        public async Task Delete(Register entity)
        {
            await _loginRepository.Delete(entity);
        }

        public ActionResult Decrypt(string credit_card)
        {

            var pkey = Environment.GetEnvironmentVariable("EKEY");
            var protector = _dataProtectionProvider.CreateProtector(pkey);
            var decrypted_data = protector.Unprotect(credit_card);
            int numberOfDigitsToMask = decrypted_data.Length - 4;
            return new JsonResult(new string('*', numberOfDigitsToMask) + decrypted_data.Substring(numberOfDigitsToMask));
        }

        public string Encrypt(string plaintext)
        {
            var pkey = _configuration["MySecrets:EKEY"];
            var protector = _dataProtectionProvider.CreateProtector(pkey);
            return protector.Protect(plaintext);
        }
    }
}
