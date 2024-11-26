using ApplicationCore.DTOs;
using ApplicationCore.Interfaces;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using ApplicationCore.Entities;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PaymentDTO>>> GetPayments()
        {
            return Ok(await _paymentService.GetAllPaymentsAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PaymentDTO>> GetPaymentById(int id)
        {
            return Ok(await _paymentService.GetPaymentByIdAsync(id));
        }

        [HttpPost]
        public async Task<ActionResult<PaymentDTO>> CreatePayment(PaymentDTO paymentDto)
        {
            var createdPayment = await _paymentService.CreatePaymentAsync(paymentDto);
            return CreatedAtAction(nameof(GetPaymentById), new { id = createdPayment.PaymentID }, createdPayment);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<PaymentDTO>> UpdatePayment(int id, PaymentDTO paymentDto)
        {
            return Ok(await _paymentService.UpdatePaymentAsync(id, paymentDto));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeletePayment(int id)
        {
            return Ok(await _paymentService.DeletePaymentAsync(id));
        }
    }
}
