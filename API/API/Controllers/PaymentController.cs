using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text.Json;
using System.Text;
using ApplicationCore.Model.Payment;

namespace API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private const string PartnerCode = "MOMO";
        private const string AccessKey = "F8BBA842ECF85";
        private const string SecretKey = "K951B6PE1waDMi640xX08PD3vg6EkVlz";
        private const string Endpoint = "https://test-payment.momo.vn/v2/gateway/api/create";
        private const string RedirectUrl = "http://localhost:3000/payment";
        private const string IpnUrl = "http://localhost:3000/payment";
        private const string extraData = "";
        private const string requestType = "payWithMethod";
        //private const string requestType = "payWithATM";

        [HttpPost]
        public async Task<ActionResult<MoMoPaymentResponse>> CreatePayment([FromBody] MoMoPaymentRequest request)
        {
            var orderId = Guid.NewGuid().ToString();
            var requestId = Guid.NewGuid().ToString();

            var rawSignature = $"accessKey={AccessKey}&amount={request.Amount}&extraData={extraData}&ipnUrl={IpnUrl}&orderId={orderId}&orderInfo={request.OrderInfo}&partnerCode={PartnerCode}&redirectUrl={RedirectUrl}&requestId={requestId}&requestType={requestType}";

            string signature = GetSignature(rawSignature, SecretKey);

            var paymentRequest = new
            {
                partnerCode = PartnerCode,
                partnerName = "MoMo Payment",
                storeId = "TestStore",
                requestId = requestId,
                amount = request.Amount,
                orderId = orderId,
                orderInfo = request.OrderInfo,
                redirectUrl = RedirectUrl,
                ipnUrl = IpnUrl,
                requestType = requestType,
                extraData = extraData,
                signature = signature
            };

            var httpClient = new HttpClient();
            var content = new StringContent(JsonSerializer.Serialize(paymentRequest), Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync(Endpoint, content);

            if (response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                var jsonResponse = JsonSerializer.Deserialize<JsonElement>(responseBody);

                return Ok(new MoMoPaymentResponse
                {
                    PaymentUrl = jsonResponse.GetProperty("payUrl").GetString()
                });
            }

            return BadRequest(new MoMoPaymentResponse
            {
                ErrorMessage = "Unable to create payment."
            });
        }

        private string GetSignature(string data, string key)
        {
            using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(key));
            var hashBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(data));
            return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
        }
    }
}
