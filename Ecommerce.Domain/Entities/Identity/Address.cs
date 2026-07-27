using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Domain.Entities.Identity
{
    public class Address
    {
        public int Id { get; set; }
        public string Street { get; set; } = default!;
        public string City { get; set; } = default!;
        public string Countery { get; set; } = default!;
        public string FirstName { get; set; } = default!;
        public string LastName { get; set; } = default!;

        #region Navigational Proberty 
        public ApplicationUser User { get; set; } = default!;
        #endregion

        #region Fk
        public string UserId { get; set; } = default!;
        #endregion
    }
}
