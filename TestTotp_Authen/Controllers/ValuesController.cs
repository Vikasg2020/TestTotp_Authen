using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using OtpNet;
using System;
using TestTotp_Authen.TOTP;
using static System.Net.WebRequestMethods;

namespace TestTotp_Authen.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {


        
        private readonly IConfiguration _config;

        public ValuesController(IConfiguration config)
        {
            _config = config;
        }

        //[HttpPost]
        //public string GenerateOtp(string email)
        //{
        //    var secretKey = _config.GetValue<string>("OTP:secretKey");
        //    if (secretKey == null)
        //    {
        //        return "null Secret Key";
        //    }
        //    var encoding = new System.Text.ASCIIEncoding();
        //    var key = encoding.GetBytes(secretKey + email);
        //    //  var userEmail = Base32Encoding.ToBytes(email);



        //    var totp = new TotpG().Secret(key).Length(6).ValidFor(120);
        //    var otp = totp.Compute();
        //    var remainingTime = totp.GetRemainingSeconds();

        //    return otp.ToString();

        //}

        //[HttpPost]
        //[Route("verify/otp")]
        //public string VerifyOTP(string email , string vOtp)
        //{
        //    var secretKey = _config.GetValue<string>("OTP:secretKey");
        //    if (secretKey == null)
        //    {
        //        return "null Secret Key";
        //    }
        //    var encoding = new System.Text.ASCIIEncoding();
        //    var key = encoding.GetBytes(secretKey + email);

        //    var totp = new TotpG().Secret(key).Length(6).ValidFor(120);
        //    var otp = totp.Compute();


        //    var remainingTime = totp.GetRemainingSeconds();

        //   if(remainingTime > 0)
        //    {
        //        if (otp.Equals(vOtp))
        //        {
        //            return "valid user";
        //        }
        //    }

            

        //    return "invalid user";


        //}


        [HttpPost]
        [Route("api/TotpGenerate")]
        public string TotpGenerate(string email)
        {
            var correctTime=DateTime.Now;   

            var correction = new TimeCorrection(correctTime);

            var secretKey = _config.GetValue<string>("OTP:secretKey");
            if (secretKey == null)
            {
                return "null Secret Key";
            }
            var encoding = new System.Text.ASCIIEncoding();
            var key = encoding.GetBytes(secretKey + email);

            var totp = new Totp(key, totpSize: 6, step: 180);

            var totpCode = totp.ComputeTotp(DateTime.UtcNow);

            //var remainingSeconds = totp.RemainingSeconds(DateTime.UtcNow);


            //var window = new VerificationWindow(previous: 1, future: 1);
             
            return totpCode;    


        }


        [HttpPost]
        [Route("api/TotpVerify")]
        public string TotpVerify(string email, string VTotp)
        {
            //var correctTime = DateTime.Now;

            

            var secretKey = _config.GetValue<string>("OTP:secretKey");
            if (secretKey == null)
            {
                return "null Secret Key";
            }
            var encoding = new System.Text.ASCIIEncoding();
            var key = encoding.GetBytes(secretKey + email);

            var totp = new Totp(key, totpSize: 6, step: 180);

            var totpCode = totp.ComputeTotp(DateTime.UtcNow);

            //var remainingSeconds = totp.RemainingSeconds(DateTime.UtcNow);

            //var correction = new TimeCorrection(correctTime);

            //var window = new VerificationWindow(previous: 1, future: 1);


            //if (remainingSeconds > 0)
            //{ 
                if (totpCode.Equals(VTotp))
                {
                    return "valid user";
                } 
            //}
            //else
            //{
            //    return " time out";
            //}
              
            return "invalid user";
             
        }
         
    }
}
