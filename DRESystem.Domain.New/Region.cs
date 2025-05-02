namespace DRESystem.Domain.New
{
    public class Region
    {
        public int RegionID { get; set; }
        public string Name { get; set; } = string.Empty;

        // Navigation properties
        public virtual ICollection<CC> CostCenters { get; set; } = new List<CC>();
    }
}
