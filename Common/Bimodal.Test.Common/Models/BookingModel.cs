using System;
using System.ComponentModel.DataAnnotations;

namespace Bimodal.Test.Common
{
    public class BookingFormModel
    {
        [Required]
        [RegularExpression("([0-9]+)", ErrorMessage = "Only number characters")]
        public string BookingNumber { get; set; }

        [Required]
        [RegularExpression("([0-9]+)", ErrorMessage = "Only number characters")]
        public int NumberOfPlaces { get; set; }

        [Required]
        public string Origin { get; set; }

        [Required]
        public string Destination { get; set; }

        [Required]
        public decimal BasePrice { get; set; }
    }

    public class BookingUpdateFormModel
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        [RegularExpression("([0-9]+)", ErrorMessage = "Only number characters")]
        public string BookingNumber { get; set; }

        [Required]
        [RegularExpression("([0-9]+)", ErrorMessage = "Only number characters")]
        public int NumberOfPlaces { get; set; }

        [Required]
        public string Origin { get; set; }

        [Required]
        public string Destination { get; set; }

        [Required]
        public decimal BasePrice { get; set; }
    }

    public class ReserveFormModel 
    {
        [Required]
        public Guid CustomerId { get; set; }

        [Required]
        public Guid BookingId { get; set; }
    }
}
