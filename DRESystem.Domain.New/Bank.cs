namespace DRESystem.Domain.New
{
    public class Bank
    {
        public int BankID { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;

        // Você pode adicionar propriedades adicionais se necessário
        public virtual ICollection<Entry>? Entries { get; set; }
    }
}
