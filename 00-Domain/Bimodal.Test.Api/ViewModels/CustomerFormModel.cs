using System.ComponentModel.DataAnnotations;

namespace Bimodal.Test.ViewModels
{
    public sealed class CustomerFormModel
    {
        [Required]
        [Range(1, int.MaxValue)]
        public int Dni { get; set; }

        [Required]
        public string FullName { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int PhoneNumber { get; set; }
    }
}
