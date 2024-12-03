using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Model.Auth
{
    public class ForgotPasswordModel
    {
        public string? Email { get; set; }
        public string? NewPassword { get; set; }
    }
}
