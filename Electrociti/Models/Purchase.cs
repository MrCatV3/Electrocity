namespace Electrociti.Models
{
    public class Purchase
    {
        public int PurchaseId { get; set; }
        public int PurchaseTotalCost { get; set; }
        public DateTime PurchaseDate { get; set; }

        public int PurchaseEmployee { get; set; }
        public Employee Employee { get; set; }

    }
}
