using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Common
{
    public class PaymentIntentResult
    {
        // lma a3ml object mno egbary a7olo al values de 
        public PaymentIntentResult(string paymentIntentId, string clientSecret)
        {
            PaymentIntentId = paymentIntentId;
            ClientSecret = clientSecret;
        }

        public string PaymentIntentId { get;  } = default!;
        public string ClientSecret { get;  } = default!;
    }
}
