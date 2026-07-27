using Ecommerce.Application.Common;
using Ecommerce.Application.Contracts;
using Ecommerce.Application.Services;
using Microsoft.Extensions.Options;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Infrastructure.Payments
{
    public class StripePaymentGetway : IPaymentGetWay
    {
        #region Use Service from Stripe 
        private readonly PaymentIntentService _paymentIntentService = new();
        #endregion

        #region DI
        public StripePaymentGetway(IOptions<PaymentGetwaySettings> options)
        {
            StripeConfiguration.ApiKey = options.Value.SecretKey; // lma a3ml object mn al Class fa al Ctor bta3o hys2l fen al Api key bta3k = mghzo fel Appsetting.json 

        } 
        #endregion
        public async Task<PaymentIntentResult> CreatePaymentIntentAsync(decimal amount, string currency, CancellationToken ct = default)
        {
            // h7dr object mn stripe nfso feh tfasil al Payment Intent bta3te 
            var IntentOptions = new PaymentIntentCreateOptions
            {
                Amount = (long)amount,
                Currency = currency.ToLower(),
                PaymentMethodTypes = ["card"]
            };
            // a3ml Create Intent aly 3mltha de fel stripe 
            var Intent = await _paymentIntentService.CreateAsync(IntentOptions, cancellationToken:ct);

            return new PaymentIntentResult(Intent.Id, Intent.ClientSecret);
            
        }

        public async Task<PaymentIntentResult> UpdatePaymentIntentAsync(string PaymentIntentId, decimal amount, CancellationToken ct = default)
        {
            // h7dr object mn stripe nfso feh tfasil al Payment Intent bta3te 
            var IntentOptions = new PaymentIntentUpdateOptions
            {
                Amount = (long)amount, // h8ir fel Options al amount bs dlw2t 
            };
            // a3ml Create Intent aly 3mltha de fel stripe 
            var Intent = await _paymentIntentService.UpdateAsync(PaymentIntentId, IntentOptions, cancellationToken: ct);

            return new PaymentIntentResult(Intent.Id, Intent.ClientSecret);

        }
    }
   
}
