namespace Electrociti.Models
{
    public class Check
    {
        public int CheckId { get; set; }
        public int CheckTotalCost{ get; set; }

        public int ClientId {  get; set; }
        public Client Client { get; set; }
        public int MasterId { get; set; }
        public Master Master { get; set; }
    }
}
