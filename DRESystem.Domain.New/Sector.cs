namespace DRESystem.Domain.New
{
    public class Sector
    {
        public int SectorID { get; set; }
        public string Name { get; set; } = string.Empty;
        
        // Navigation properties
        public virtual ICollection<CC>? CostCenters { get; set; }
    }
}