using System;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using ApiDPSystem.Data;
using ApiDPSystem.Entities;
using ApiDPSystem.Models;

namespace ApiDPSystem.Repository
{
    public class AccountRepository
    {
        private readonly Context _context;

        public AccountRepository(Context context)
        {
            _context = context;
        }

        public async Task<string> GetRefreshTokenAsync(string tokenId, User user)
        {
            var refreshTokenInfo = new RefreshTokenInfo
            {
                JwtId = tokenId,
                UserId = user.Id,
                ExpiryDate = DateTime.UtcNow.AddMonths(1),
                RefreshToken = GetRandomString()
            };

            _context.RefreshTokenInfoTable.Add(refreshTokenInfo);
            await _context.SaveChangesAsync();

            return refreshTokenInfo.RefreshToken;
        }

        private string GetRandomString()
        {
            var randomNumber = new byte[32];
            using var randomNumberGenerator = RandomNumberGenerator.Create();
            randomNumberGenerator.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        public RefreshTokenInfo GetStoredRefreshToken(string requestRefreshToken)
        {
            return _context.RefreshTokenInfoTable.FirstOrDefault(x => x.RefreshToken == requestRefreshToken);
        }

        public async Task UpdateStoredRefreshTokenAsync(RefreshTokenInfo storedRefreshToken)
        {
            _context.RefreshTokenInfoTable.Update(storedRefreshToken);
            await _context.SaveChangesAsync();
        }
    }
}