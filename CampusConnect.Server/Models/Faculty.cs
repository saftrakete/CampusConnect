namespace CampusConnect.Server.Models
{
    public partial class Faculty
    {
        public Faculty() { }

        public Faculty(string name)
        {
            Name = name;
        }

        public int FacultyId { get; set; }
        public string Name { get; set; }
    }
}
