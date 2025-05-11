using System.Linq;
using System.Threading.Tasks;
using CampusConnect.Server.Data;
using CampusConnect.Server.Models;

namespace CampusConnect.Server.Services
{
    public class InitFacultyTable {
        private readonly Faculty[] faculties = [
                new Faculty { Name = "Fakultät für Informatik"}
            ];
        private readonly CampusConnectContext _context;

        public InitFacultyTable(CampusConnectContext context)
        {
            this._context = context;
        }

        public async Task FillInFaculties()
        {
            if (CheckIfEmpty())
            {
                foreach (var fac in faculties)
                {
                    await _context.Faculties.AddAsync(fac);
                }
                await _context.SaveChangesAsync();
            }
        }

        public bool CheckIfEmpty()
        {
            int facultyCount = _context.Faculties.Count();
            return facultyCount == 0;
        }
    }
}
