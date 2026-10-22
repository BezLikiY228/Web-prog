using Microsoft.AspNetCore.Identity;

namespace UniversityApi.Identity
{
    public class ApplicationUser : IdentityUser
    {
        public int? StudentId { get; set; }

        public int? TeacherId { get; set; }
    }
}