using CampusConnect.Server.Models;
using CampusConnect.Server.Data;
using System.Linq;

namespace CampusConnect.Server.Services
{
    public class InitDegreeTable
    {
        private Degree[] degrees = [];
        private readonly CampusConnectContext _context;

        public InitDegreeTable(CampusConnectContext context)
        {
            this._context = context;
        }

        public void createDegreeEntities()
        {
            this.degrees = [
                    new Degree {Name = "Informatik", Faculty = _context.Faculties.First()},
                    new Degree {Name = "Ingenieurinformatk", Faculty = _context.Faculties.First()}
                ];
        }

        public async void FillInDegrees()
        {
            createDegreeEntities();

            if (CheckIfEmpty())
            {
                foreach (var deg in degrees)
                {
                    await _context.Degrees.AddAsync(deg);
                }
                await _context.SaveChangesAsync();
            }
        }

        public bool CheckIfEmpty()
        {
            int degreeCount = _context.Degrees.Count();
            return degreeCount == 0;
        }
    }
}
