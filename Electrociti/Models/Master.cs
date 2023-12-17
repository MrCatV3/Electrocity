namespace Electrociti.Models
{
    public class Master
    {
        public int MasterId { get; set; }
        public string MasterName { get; set; }
        public string MasterPhone { get; set; }
        public string MasterRate { get; set; }
        public string MasterAddress { get; set; }
        public DateTime MasterBirthday { get; set; }
        public DateTime MasterRegistrationDate { get; set; }
        public string MasterPassword { get; set; }

        public int ClientCommentId { get; set; }
        public CommentClient ClientComment { get; set; }
        public int ServiceId { get; set; }
        public Service ServiceMaster { get; set; }

    }
}
