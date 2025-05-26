using CampusConnect.Server.Data;
using CampusConnect.Server.Interfaces;
using CampusConnect.Server.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CampusConnect.Server.Services
{
    public class InitUserRolesService
    {
        public void InitUserRolesTable(CampusConnectContext context)
        {
            if (!context.UserRoles.Any(role => role.RoleName == "Admin"))
            {
                var role = new UserRole
                {
                    RoleName = "Admin",
                    RoleDescription = "Admin-Rolle mit allen Rechten",
                    Permissions = new List<string>()
                    {
                        "Lesen",
                        "Schreiben",
                        "Loeschen"
                    }
                };

                context.UserRoles.Add(role);
            }

            context.SaveChanges();
        } 
    }
}
