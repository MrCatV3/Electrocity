using Electrociti.Models;

namespace Electrociti.ViewModels
{
    public class ServicesViewModel
    {
        public List<Service> Service { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
    }

}
