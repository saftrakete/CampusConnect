using System.Collections.Generic;

namespace CampusConnect.Server.Models.Dtos
{
    public class FacultyDto
    {
        public string Name { get; set; }
        public string FacultyCode { get; set; } // Assuming this is a code or abbreviation for the faculty
        public int[] DegreeIds { get; set; }
        public int[] ModuleIds { get; set; }
    }
}
