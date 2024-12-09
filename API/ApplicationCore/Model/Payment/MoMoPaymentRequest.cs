using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Model.Payment
{
    public class MoMoPaymentRequest
    {
        public string OrderInfo { get; set; }
        public decimal Amount { get; set; }
    }
}
