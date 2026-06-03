using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using API.Models;
using API.Services;
using API.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class AccountController : ControllerBase
    {
        private readonly AccountService _accountService;
        private readonly IConfiguration _configuration;
        private readonly IEmailService _emailService;

        private readonly string _frontendBaseUrl = "https://secureloginapp.ddns.net";

        public AccountController(AccountService accountService, IConfiguration configuration, IEmailService emailService)
        {
            _accountService = accountService;
            _configuration = configuration;
            _emailService = emailService;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_accountService.GetAll());
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var account = _accountService.Get(id);
            if (account == null) return NotFound();
            return Ok(account);
        }

        [HttpPost]
        public IActionResult Create(Account account)
        {
            _accountService.AddAccount(account);
            return Ok(account);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _accountService.DeleteAccount(id);
            return NoContent();
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, Account account)
        {
            if (id != account.Id) return BadRequest();
            _accountService.UpdateAccount(account);
            return NoContent();
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            var existingUser = _accountService.GetAll()
                .FirstOrDefault(u => u.Email.ToLower() == request.Email.ToLower());

            if (existingUser != null)
                return BadRequest("Email already exists");

            var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
            var verificationToken = Guid.NewGuid().ToString();

            var verificationLink =
                $"{_frontendBaseUrl}/api/account/verify-success?token={Uri.EscapeDataString(verificationToken)}";

            _accountService.AddAccount(new Account
            {
                Name = request.Name,
                Email = request.Email,
                Password = passwordHash,
                EmailVerificationToken = verificationToken
            });

            await _emailService.SendEmailAsync(
                request.Name,
                request.Email,
                verificationLink
            );

            return Ok(new { message = "Account created" });
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public IActionResult Login(LoginRequest loginRequest)
        {
            var user = _accountService.GetAll()
                .FirstOrDefault(u => u.Email == loginRequest.Email.ToLower());

            if (user == null) return Unauthorized();

            var validPassword = BCrypt.Net.BCrypt.Verify(loginRequest.Password, user.Password);

            if (!validPassword) return Unauthorized();

            if (!user.EmailVerified)
                return Unauthorized(new { message = "Please verify your email first" });

            var token = GenerateJwtToken(user);

            return Ok(new { token });
        }

        private string GenerateJwtToken(Account user)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(15),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        [AllowAnonymous]
        [HttpGet("verify-success")]
        public IActionResult VerifyEmail(string token)
        {
            if (string.IsNullOrEmpty(token))
                return Redirect($"{_frontendBaseUrl}/verify-success?error=missing");

            var user = _accountService
                .GetAll()
                .FirstOrDefault(u => u.EmailVerificationToken == token);

            if (user == null)
                return Redirect($"{_frontendBaseUrl}/verify-success?error=invalid");

            user.EmailVerified = true;
            user.EmailVerificationToken = null;

            _accountService.UpdateAccount(user);

            return Redirect($"{_frontendBaseUrl}/verify-success?status=success");
        }

        [Authorize]
        [HttpGet("profile")]
        public IActionResult Profile()
        {
            return Ok("Protected route");
        }
    }
}