namespace Electrociti.Models
{
    public class Client
    {
        public int ClientId { get; set; }
        public string ClientName { get; set; }
        public string ClientSurname { get; set; }
        public string ClientPatronomic { get; set;}
        public string ClientAddress { get; set; }
        public string ClientPhone { get; set; }
        public double ClientRate { get; set; }
        public DateTime ClitentBirthday { get; set; }
        public DateTime ClientRegistrationDate { get; set;}
        public string ClientPassword{ get; set; }

        public int MasterCommentId { get; set; }
        public CommentMaster MasterComment {  get; set; }
    }
}
