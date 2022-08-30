using OtpNet;
using System.Security.Cryptography;
using System.Text;
using System;

namespace TestTotp_Authen.TOTP
{
    public class TotpG
    {


        private const long UnixEpochTicks = 621355968000000000L;
        private const long TicksToSeconds = 10000000L;
        private int _step;
        private int _totpSize;
        private byte[] _key;
      //  private IEncryptor _encryptor;


      
        public TotpG Length(int length)
        {
            _totpSize = length;
            return this;
        }

      
        public TotpG ValidFor(int timeSpan)
        {
            this._step = timeSpan;
            return this;
        }

        public TotpG Secret(byte[] key)
        {
            _key = key;
            return this;
        }


        public string Compute()
        {
            var window = CalculateTimeStepFromTimestamp(DateTime.UtcNow);

            var data = GetBigEndianBytes(window);

            var hmac = new HMACSHA1 { Key = _key };
            var hmacComputedHash = hmac.ComputeHash(data);

            var offset = hmacComputedHash[hmacComputedHash.Length - 1] & 0x0F;
            var otp = (hmacComputedHash[offset] & 0x7f) << 24
                      | (hmacComputedHash[offset + 1] & 0xff) << 16
                      | (hmacComputedHash[offset + 2] & 0xff) << 8
                      | (hmacComputedHash[offset + 3] & 0xff) % 1000000;

            var result = Digits(otp, _totpSize);

            return result;
        }
 
        public int GetRemainingSeconds()
        {
            return _step - (int)(((DateTime.UtcNow.Ticks - UnixEpochTicks) / TicksToSeconds) % _step);
        }

        private static byte[] GetBigEndianBytes(long input)
        {
            
            var data = BitConverter.GetBytes(input);
            Array.Reverse(data);
            return data;
        }

        private long CalculateTimeStepFromTimestamp(DateTime timestamp)
        {
            var unixTimestamp = (timestamp.Ticks - UnixEpochTicks) / TicksToSeconds;
            var window = unixTimestamp / _step;
            return window;
        }

        private static string Digits(long input, int digitCount)
        {
            var truncatedValue = ((int)input % (int)Math.Pow(10, digitCount));
            return truncatedValue.ToString().PadLeft(digitCount, '0');
        }

        

    }
}
