namespace Electrociti.Models
{
    public class CommentEmployee
    {
        public int CommentEmployeeId { get; set; }
        
        public int CommentEmployee_ { get; set; }
        public Comment Comment { get; set; }
        public int EmployeeComment {  get; set; }
        public Employee Employee { get; set; }
        
    }
}
