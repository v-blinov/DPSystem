using System;

namespace ApiDPSystem.Entities
{
    public class RefreshTokenInfo
    {
        public int Id { get; set; }
        public string UserId { get; set; } // Linked to the AspNet Identity User Id
        public string RefreshToken { get; set; }
        public string JwtId { get; set; } // Map the token with jwtId
        public DateTime ExpiryDate { get; set; } // Refresh token is long lived it could last for months.
    }
}
