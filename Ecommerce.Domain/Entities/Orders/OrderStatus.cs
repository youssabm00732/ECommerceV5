using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Domain.Entities.Orders
{
    public enum OrderStatus
    {
        Pending = 0 , // de al Defult state awl ma b create al Order
        PaymentReceived = 1,
        PaymentFailed = 2 
    }
}
