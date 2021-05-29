using System;

namespace Bimodal.Test.Common
{
    public sealed class CustomerViewModel
    {
        public Guid Id { get; set; }

        public string DocumentNumber { get; set; }

        public string FullName { get; set; }

        public string Address { get; set; }

        public string PhoneNumber { get; set; }
    }
}
