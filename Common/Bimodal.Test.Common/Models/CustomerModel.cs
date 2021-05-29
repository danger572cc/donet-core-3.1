using System;
using System.ComponentModel.DataAnnotations;

namespace Bimodal.Test.Common
{
    public class CustomerFormModel
    {
        [Required]
        [RegularExpression("([0-9]+)", ErrorMessage = "Only number characters")]
        public string Dni { get; set; }

        [Required]
        public string FullName { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        [RegularExpression("([0-9]+)", ErrorMessage = "Only number characters")]
        public string PhoneNumber { get; set; }
    }

    public class CustomerUpdateFormModel
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public string FullName { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        [RegularExpression("([0-9]+)", ErrorMessage = "Only number characters")]
        public string PhoneNumber { get; set; }
    }
}
