using Microsoft.AspNetCore.Identity;

namespace Identity.App.Data.Entities
{
    public class AppUser : IdentityUser<Guid>
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Document { get; set; }
    }
}
