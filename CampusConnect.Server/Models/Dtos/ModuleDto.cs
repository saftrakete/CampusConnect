using CampusConnect.Server.Enums;
using System.Collections.Generic;

namespace CampusConnect.Server.Models.Dtos
{
    public class ModuleDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Difficulty { get; set; }
        public int FacultyId { get; set; }
    }
}
