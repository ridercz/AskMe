using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Altairis.AskMe.Data {
    public class AskDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, int> {

        public AskDbContext(DbContextOptions<AskDbContext> options)
            : base(options) { }

    }
}
