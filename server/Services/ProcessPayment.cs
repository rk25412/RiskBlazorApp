using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Clear.Risk
{
    public class ProcessPayment
    {
        public static async Task<Charge> PayAsync(PayModel payModel)
        {
            try
            {
                StripeConfiguration.ApiKey = "sk_test_hsU8vxIYVTD5rH3zpdmBZxqH00h3Mto3hT";
                //StripeConfiguration.ApiKey = "sk_hsU8vxIYVTD5rH3zpdmBZxqH00h3Mto3hT";

                var options = new TokenCreateOptions
                {
                    Card = new TokenCardOptions
                    {
                        Number = payModel.CardNumder,
                        ExpMonth = payModel.Month,
                        ExpYear = payModel.Year,
                        Cvc = payModel.CVC
                    },
                };

                var serviceToken = new TokenService();
                Token stripeToken = await serviceToken.CreateAsync(options);

                var chargeOptions = new ChargeCreateOptions
                {
                    Amount = (long)payModel.Amount,
                    Currency = payModel.Currency,
                    Description = "Payment Deposit",
                    Source = stripeToken.Id
                };

                var chargeService = new ChargeService();
                Charge charge = await chargeService.CreateAsync(chargeOptions);

                return charge;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }

    public class PayModel
    {
         
        public string CustomerName { get; set; }

         
        public string Email { get; set; }

        
        public string Address { get; set; }

         
        public string CardNumder { get; set; }

        
        public int Month { get; set; }

         
        public int Year { get; set; }

         
        public string CVC { get; set; }

        
        public decimal Amount { get; set; }

        public string Currency { get; set; }
    }
}
