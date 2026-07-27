using Ecommerce.Application.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Contracts
{
    public interface IPaymentGetWay
    {
        // h3ml two Function : create payment intent , update payment intent (lw 4elt item mn al basket aw deft)
        // da m3nah an result al functions de aw al return hyb2a gy mn al Stripe .. fa hro7 a3ml 4kl al result bta3 stripe 

        Task<PaymentIntentResult> CreatePaymentIntentAsync(decimal amount, string currency, CancellationToken ct = default);
        Task<PaymentIntentResult> UpdatePaymentIntentAsync(string PaymentIntentId,decimal amount, CancellationToken ct = default);

    }
}
