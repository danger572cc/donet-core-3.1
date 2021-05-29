using Bimodal.Test.Database;
using Kledex.Queries;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Bimodal.Test.Models
{
    public class CustomersListModel : Query<IList<Customer>>
    {
    }

    public class CustomerQueryModel : Query<Customer> 
    {
        public Guid Id { get; set; }
    }

    public sealed class CustomerFormModel
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
}
