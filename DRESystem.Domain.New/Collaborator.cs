namespace DRESystem.Domain.New
{
    public class Collaborator
    {
        public int CollaboratorID { get; set; }
        public string Name { get; set; } = string.Empty;
        public int FKCC { get; set; }
        
        // Navigation properties
        public virtual CC? CostCenter { get; set; }
        public virtual ICollection<Entry>? Entries { get; set; }
    }
}