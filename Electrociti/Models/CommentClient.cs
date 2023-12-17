namespace Electrociti.Models
{
    public class CommentClient
    {
        public int ClientCommentId { get; set; }
        public string ClientCommentText { get; set; }

        public int MasterId {  get; set; }
        public Master MasterComment {  get; set; }
    }
}
