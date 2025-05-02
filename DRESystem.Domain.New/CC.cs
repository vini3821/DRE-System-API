namespace DRESystem.Domain.New
{
    public class CC
    {
        public int CCID { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int FKRegion { get; set; }
        public int FKSector { get; set; }
        
        // Navigation properties
        public virtual Region? Region { get; set; }
        public virtual Sector? Sector { get; set; }
        public virtual ICollection<Collaborator>? Collaborators { get; set; }
    }
}