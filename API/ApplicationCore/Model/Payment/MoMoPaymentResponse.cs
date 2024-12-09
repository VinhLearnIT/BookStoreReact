using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Model.Payment
{
    public class MoMoPaymentResponse
    {
        public string PaymentUrl { get; set; }
        public string ErrorMessage { get; set; }
    }
}
