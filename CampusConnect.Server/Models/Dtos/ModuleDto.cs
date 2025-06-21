using CampusConnect.Server.Enums;

namespace CampusConnect.Server.Models.Dtos
{
    public class ModuleDto
    {
        public string Name { get; set; }
        public int FacultyId { get; set; }
        public DifficultyEnum Difficulty { get; set; }
    }
}
