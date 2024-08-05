using System;
using System.Collections.Generic;

namespace Otus.Teaching.PromoCodeFactory.Core.Domain.Administration
{
    public class Employee
        : BaseEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string FullName
        {
            get => $"{FirstName} {LastName}";
            set { }
        }

        public string Email { get; set; }

        public virtual Role Role { get; set; }

        public int AppliedPromocodesCount { get; set; }
    }
}