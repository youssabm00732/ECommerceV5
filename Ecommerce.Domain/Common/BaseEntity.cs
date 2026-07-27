using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Domain.Common
{
    public class BaseEntity <Tkey>
    {
        public Tkey Id { get; set; } = default!;
    }
}
