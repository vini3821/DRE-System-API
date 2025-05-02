namespace DRESystem.Domain.New
{
    public class Entry
    {
        public int EntryID { get; set; }
        public DateTime EntryDate { get; set; }
        public string EntryType { get; set; } = string.Empty;
        public decimal Value { get; set; }
        public int FKC { get; set; }
        public int? FKBank { get; set; } // Opcional, pode ser null

        // Navigation properties
        public virtual Collaborator? Collaborator { get; set; }
        public virtual Bank? Bank { get; set; }
    }
}
