namespace Electrociti.Models
{
    public class Employee
    {
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeSurname { get; set; }
        public string EmployeePatronomic { get; set; }
        public string EmployeeAddress { get; set; }
        public string EmployeePhone { get; set; }
        public DateTime EmployeeBirthday { get; set; }
        public DateTime EmployeeRegistrationDate { get; set; }
        public string EmployeePassword { get; set; }


        public string CommentEmployee { get; set; }
        public Comment Comment {  get; set; }
        public int RoleEmployee {  get; set; }
        public RoleEmployee Role { get; set; }
    }
}
