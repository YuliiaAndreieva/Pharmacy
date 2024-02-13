using System.ComponentModel.DataAnnotations.Schema;
using WepPha2.Models;

namespace WepPha2.ViewModels
{
    public class ViewCartModel
    {
        public int MedicineId { get; set; }
        public int Count { get; set; }

    }
}
