using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ProyectoFinalTecWeb.Entities;
using ProyectoFinalTecWeb.Entities.Dtos.Auth;
using ProyectoFinalTecWeb.Entities.Dtos.DriverDto;
using ProyectoFinalTecWeb.Entities.Dtos.PassengerDto;
using ProyectoFinalTecWeb.Repositories;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace ProyectoFinalTecWeb.Services
{
    public class AuthService : IAuthService
    {
        private readonly IDriverRepository _drivers;
        private readonly IPassengerRepository _passengers;
        private readonly IConfiguration _configuration;

        public AuthService(
            IDriverRepository drivers,
            IPassengerRepository passengers,
            IConfiguration configuration)
        {
            _drivers = drivers;
            _passengers = passengers;
            _configuration = configuration;
        }

        public async Task<(bool ok, LoginResponseDto? response)> LoginAsync(LoginDto dto)
        {
            var driver = await _drivers.GetByEmailAddress(dto.Email);
            if (driver != null)
            {
                if (!BCrypt.Net.BCrypt.Verify(dto.Password, driver.PasswordHash))
                    return (false, null);

                var (accessToken, expiresIn, jti) = GenerateJwtTokenDriver(driver);
                var refreshToken = GenerateSecureRefreshToken();
                var refreshDays = int.Parse(_configuration["Jwt:RefreshDays"] ?? "14");

                driver.RefreshToken = refreshToken;
                driver.RefreshTokenExpiresAt = DateTime.UtcNow.AddDays(refreshDays);
                driver.RefreshTokenRevokedAt = null;
                driver.CurrentJwtId = jti;

                await _drivers.Update(driver);

                return (true, BuildLoginResponse(driver, accessToken, refreshToken, expiresIn));
            }

            var passenger = await _passengers.GetByEmailAddress(dto.Email);
            if (passenger != null)
            {
                if (!BCrypt.Net.BCrypt.Verify(dto.Password, passenger.PasswordHash))
                    return (false, null);

                var (accessToken, expiresIn, jti) = GenerateJwtTokenPassenger(passenger);
                var refreshToken = GenerateSecureRefreshToken();
                var refreshDays = int.Parse(_configuration["Jwt:RefreshDays"] ?? "14");

                passenger.RefreshToken = refreshToken;
                passenger.RefreshTokenExpiresAt = DateTime.UtcNow.AddDays(refreshDays);
                passenger.RefreshTokenRevokedAt = null;
                passenger.CurrentJwtId = jti;

                await _passengers.Update(passenger);

                return (true, BuildLoginResponse(passenger, accessToken, refreshToken, expiresIn));
            }

            return (false, null);
        }

        public async Task<(bool ok, LoginResponseDto? response)> RefreshAsync(RefreshRequestDto dto)
        {
            var driver = await _drivers.GetByRefreshToken(dto.RefreshToken);
            if (driver != null && IsValidRefreshToken(driver))
            {
                return (true, await RotateDriverToken(driver));
            }

            var passenger = await _passengers.GetByRefreshToken(dto.RefreshToken);
            if (passenger != null && IsValidRefreshToken(passenger))
            {
                return (true, await RotatePassengerToken(passenger));
            }

            return (false, null);
        }

        public async Task<string> RegisterDriverAsync(RegisterDriverDto dto)
        {
            var driver = new Driver
            {
                Email = dto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                Name = dto.Name,
                Licence = dto.Licence,
                Phone = dto.Phone,
                Role = dto.Role
            };

            await _drivers.AddAsync(driver);
            return driver.Id.ToString();
        }

        public async Task<string> RegisterPassengerAsync(RegisterPassengerDto dto)
        {
            var passenger = new Passenger
            {
                Email = dto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                Name = dto.Name,
                Phone = dto.Phone,
                Role = dto.Role
            };

            await _passengers.AddAsync(passenger);
            return passenger.Id.ToString();
        }

        public async Task<bool> ForgotPasswordAsync(string email)
        {
            var driver = await _drivers.GetByEmailAddress(email);
            if (driver != null)
            {
                driver.PasswordResetToken = Guid.NewGuid().ToString();
                driver.PasswordResetTokenExpiresAt = DateTime.UtcNow.AddMinutes(30);
                await _drivers.Update(driver);
                return true;
            }

            var passenger = await _passengers.GetByEmailAddress(email);
            if (passenger != null)
            {
                passenger.PasswordResetToken = Guid.NewGuid().ToString();
                passenger.PasswordResetTokenExpiresAt = DateTime.UtcNow.AddMinutes(30);
                await _passengers.Update(passenger);
                return true;
            }

            return false;
        }

        public async Task<bool> ResetPasswordAsync(string token, string newPassword)
        {
            var driver = await _drivers.GetByPasswordResetToken(token);
            if (driver != null && driver.PasswordResetTokenExpiresAt > DateTime.UtcNow)
            {
                driver.PasswordHash = BCrypt.Net.BCrypt.HashPassword(newPassword);
                driver.PasswordResetToken = null;
                driver.PasswordResetTokenExpiresAt = null;
                await _drivers.Update(driver);
                return true;
            }

            var passenger = await _passengers.GetByPasswordResetToken(token);
            if (passenger != null && passenger.PasswordResetTokenExpiresAt > DateTime.UtcNow)
            {
                passenger.PasswordHash = BCrypt.Net.BCrypt.HashPassword(newPassword);
                passenger.PasswordResetToken = null;
                passenger.PasswordResetTokenExpiresAt = null;
                await _passengers.Update(passenger);
                return true;
            }

            return false;
        }


        private bool IsValidRefreshToken(dynamic user) =>
            user.RefreshTokenExpiresAt.HasValue &&
            user.RefreshTokenExpiresAt > DateTime.UtcNow &&
            !user.RefreshTokenRevokedAt.HasValue;

        private async Task<LoginResponseDto> RotateDriverToken(Driver driver)
        {
            var (accessToken, expiresIn, jti) = GenerateJwtTokenDriver(driver);
            var refreshToken = GenerateSecureRefreshToken();
            var refreshDays = int.Parse(_configuration["Jwt:RefreshDays"] ?? "14");

            driver.RefreshToken = refreshToken;
            driver.RefreshTokenExpiresAt = DateTime.UtcNow.AddDays(refreshDays);
            driver.CurrentJwtId = jti;

            await _drivers.Update(driver);

            return BuildLoginResponse(driver, accessToken, refreshToken, expiresIn);
        }

        private async Task<LoginResponseDto> RotatePassengerToken(Passenger passenger)
        {
            var (accessToken, expiresIn, jti) = GenerateJwtTokenPassenger(passenger);
            var refreshToken = GenerateSecureRefreshToken();
            var refreshDays = int.Parse(_configuration["Jwt:RefreshDays"] ?? "14");

            passenger.RefreshToken = refreshToken;
            passenger.RefreshTokenExpiresAt = DateTime.UtcNow.AddDays(refreshDays);
            passenger.CurrentJwtId = jti;

            await _passengers.Update(passenger);

            return BuildLoginResponse(passenger, accessToken, refreshToken, expiresIn);
        }

        private LoginResponseDto BuildLoginResponse(dynamic user, string accessToken, string refreshToken, int expiresIn) =>
            new LoginResponseDto
            {
                User = new UserDto
                {
                    Id = user.Id,
                    Name = user.Name,
                    Email = user.Email
                },
                Role = user.Role,
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                ExpiresIn = expiresIn,
                TokenType = "Bearer"
            };

        private (string token, int expiresInSeconds, string jti) GenerateJwtTokenDriver(Driver driver)
        {
            return GenerateJwtToken(driver.Id.ToString(), driver.Email, driver.Name, driver.Role);
        }

        private (string token, int expiresInSeconds, string jti) GenerateJwtTokenPassenger(Passenger passenger)
        {
            return GenerateJwtToken(passenger.Id.ToString(), passenger.Email, passenger.Name, passenger.Role);
        }

        private (string token, int expiresInSeconds, string jti) GenerateJwtToken(
            string userId,
            string email,
            string name,
            string role)
        {
            var jwt = _configuration.GetSection("Jwt");
            var key = Convert.FromBase64String(jwt["Key"]!);
            var issuer = jwt["Issuer"];
            var audience = jwt["Audience"];
            var minutes = int.Parse(jwt["ExpiresMinutes"] ?? "60");

            var jti = Guid.NewGuid().ToString();

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, userId),
                new Claim(JwtRegisteredClaimNames.Email, email),
                new Claim(ClaimTypes.Name, name),
                new Claim(ClaimTypes.Role, role),
                new Claim(JwtRegisteredClaimNames.Jti, jti)
            };

            var creds = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer,
                audience,
                claims,
                expires: DateTime.UtcNow.AddMinutes(minutes),
                signingCredentials: creds);

            return (
                new JwtSecurityTokenHandler().WriteToken(token),
                (int)TimeSpan.FromMinutes(minutes).TotalSeconds,
                jti
            );
        }

        private static string GenerateSecureRefreshToken()
        {
            var bytes = RandomNumberGenerator.GetBytes(64);
            return Base64UrlEncoder.Encode(bytes);
        }
    }
}
