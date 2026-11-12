using Microsoft.AspNetCore.Identity;

namespace Identity.App.Data.Entities
{
    public class AppRole : IdentityRole<Guid>
    {
        public AppRole()
        {
        }

        public AppRole(string name) : base(name)
        {
        }
    }
}
