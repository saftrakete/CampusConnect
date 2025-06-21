namespace CampusConnect.Server.Models.Dtos
{
    public class DegreeDto
    {
        public string Name { get; set; }
        public int FacultyId { get; set; }
        public int[] MandatoryModuleIds { get; set; }
    }
}
