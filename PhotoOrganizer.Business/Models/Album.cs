namespace PhotoOrganizer.Business.Models
{
    public class Album
    {
        public string Name { get; set; }
        public string Source { get; set; }
        public string Destination { get; set; }
        public string[] SubFolders { get; set; } = new string[0];
    }
}