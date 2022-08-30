using Microsoft.Extensions.Configuration;
using OtpNet;
using System;

namespace TestTotp_Authen.TOTP
{
    public class TotpTest
    {


        private readonly IConfiguration _config;

        public TotpTest(IConfiguration config)
        {
            _config = config;
        }   

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



        //    var totp = new TotpG().Secret(key).Length(6).ValidFor(TimeSpan.FromSeconds(5));
        //    var otp = totp.Compute();


        //    return otp.ToString();  

        //}

    }
}
