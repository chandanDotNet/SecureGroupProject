using System.ComponentModel.DataAnnotations;

namespace SecureGroup.ViewModel.Models
{
    public class CountryMaster
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }
        public string CountryCode { get; set; }
    }
}
