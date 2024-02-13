using System.ComponentModel.DataAnnotations;

namespace WepPha2.ViewModels
{
    public class EditSupplierViewModel
    {
        public int SupplierId { get; set; }

        public string CompanyName { get; set; }

        public string MobilePhone { get; set; }

        public string EmailAddress { get; set; }
        public string Address { get; set; }
    }
}
