namespace Webshop.Api.Models
{
    public class RefreshToken
    {
        public int Id { get; set; }

        // Csak hashelve tároljuk Bálint bácsi
        public string TokenHash { get; set; } = string.Empty;

        public int UserId { get; set; }
        public User User { get; set; } = default!;

        public DateTime ExpiresAt { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string CreatedByIp { get; set; } = string.Empty;

        public DateTime? RevokedAt { get; set; }
        public string? RevokedByIp { get; set; }

        public string? ReplacedByTokenHash { get; set; }

        public bool IsExpired => DateTime.UtcNow >= ExpiresAt;
        public bool IsRevoked => RevokedAt != null;
        public bool IsActive => !IsRevoked && !IsExpired;
    }
}
