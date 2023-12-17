namespace Electrociti.Models
{
    public class CommentMaster
    {
        public int MasterCommentId { get; set; }
        public string MasterCommentText { get; set;}

        public int ClientId {  get; set; }
        public Client ClientComment {  get; set; }
    }
}
