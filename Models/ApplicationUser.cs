using Microsoft.AspNetCore.Identity;

namespace LogiTrack.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; } = string.Empty;
        public DateTime RegisteredAt { get; set; } = DateTime.UtcNow;
        public string Role { get; set; } = "User";
        public bool IsActive { get; set; } = true;
    }
}
