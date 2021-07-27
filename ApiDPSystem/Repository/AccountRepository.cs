using ApiDPSystem.Data;
using ApiDPSystem.Entities;
using ApiDPSystem.Models;
using System;
using System.Security.Cryptography;

namespace ApiDPSystem.Repository
{
    public class AccountRepository
    {
        private Context _context;

        public AccountRepository(Context context)
        {
            _context = context;
        }

        public string GetRefrashToken(string tokenId, User user)
        {
            var refreshTokenInfo = new RefreshTokenInfo()
            {
                JwtId = tokenId,
                UserId = user.Id,
                ExpiryDate = DateTime.UtcNow.AddMonths(1),
                RefreshToken = GetRandomString()
            };

            _context.RefreshTokenInfoTable.Add(refreshTokenInfo);
            _context.SaveChanges();

            return refreshTokenInfo.RefreshToken;
        }

        private string GetRandomString()
        {
            var randomNumber = new byte[32];
            using var randomNumberGenerator = RandomNumberGenerator.Create();
            randomNumberGenerator.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
    }
}
