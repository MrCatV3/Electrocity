﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Electrociti.Models
{
    public class Employee
    {
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeSurname { get; set; }
        public string EmployeePatronomic { get; set; }
        public string EmployeeAddress { get; set; }
        [Required(ErrorMessage = "Телефон обязателен для заполнения.")]
        [RegularExpression(@"^\d{11}$", ErrorMessage = "Телефон должен состоять из 11 цифр.")]
        public string EmployeePhone { get; set; }
        public string EmployeeRate { get; set; }
        public DateTime EmployeeBirthday { get; set; }
        public DateTime EmployeeRegistrationDate { get; set; }
        [Required(ErrorMessage = "Пароль обязателен для заполнения.")]
        public string EmployeePassword { get; set; }
        public string EmployeeImage {  get; set; }
        public int EmployeeRole { get; set; }
        public string EmployeeDescription { get; set; }
    }
}
