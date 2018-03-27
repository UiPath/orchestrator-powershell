using System;
using System.Text;
using System.Web.Security;

namespace UiPath.PowerShell.Tests.Util
{
    public class TestRandom
    {
        public static class CharSet
        {
            public const string Alpha = "abcdefghijklmnopqrstuvwzx";
            public const string Numeric = "0123456789";
            public const string Special = "`~!@#$%^&*()-_=+[{]}|;:/?.>,<";  // \ ' and " are intentionally removed to avoid URL escaping issues that autorest client cannot handle correctly
        }

        [Flags]
        public enum CharType
        {
            AlphaUpper  = 1,
            AlphaLower  = 2,
            Numeric     = 4,
            Special     = 8
        }

        public Random Random { get; set; }

        public TestRandom()
        {
            Random = new Random();
        }

        internal string RandomPassword(int minLength = 10, int maxLength = 15)
        {
            var length = Random.Next(minLength, maxLength);

            return Membership.GeneratePassword(length, minLength / 2);
        }


        internal string RandomEmail()
        {
            return $"{RandomAlphaNumeric()}@example.com";
        }

        public string RandomAlphaNumeric(int minLength = 10, int maxLength = 15)
        {
            return RandomString(minLength, maxLength, CharType.AlphaUpper | CharType.AlphaLower | CharType.Numeric);
        }

        public string RandomString()
        {
            return RandomString(10, 15);
        }

        public string RandomString(int mintLength, int maxLength, CharType charType = CharType.AlphaUpper | CharType.AlphaLower | CharType.Numeric | CharType.Special)
        {
            var length = Random.Next(mintLength, maxLength);
            StringBuilder sb = new StringBuilder(length);

            var types = (charType.HasFlag(CharType.AlphaUpper) ? 1 : 0) +
                (charType.HasFlag(CharType.AlphaLower) ? 1 : 0) +
                (charType.HasFlag(CharType.Numeric) ? 1 : 0) +
                (charType.HasFlag(CharType.Special) ? 1 : 0);

            var probabilityAlphaUpper = charType.HasFlag(CharType.AlphaUpper) ? length - types : 0;
            var probabilityAlphaLower = charType.HasFlag(CharType.AlphaLower) ? length - types : 0;
            var probabilityNumeric = charType.HasFlag(CharType.Numeric) ? length - types : 0;
            var probabilitySpecial = charType.HasFlag(CharType.Special) ? length - types : 0;

            while (length > 0)
            {
                Char c = default(Char);
                var roll = Random.Next(probabilityAlphaUpper + probabilityAlphaLower + probabilityNumeric + probabilitySpecial);
                if (roll > probabilityAlphaUpper + probabilityAlphaLower + probabilityNumeric)
                {
                    c = CharSet.Special[Random.Next(CharSet.Special.Length)];
                    --probabilitySpecial;
                }
                else if (roll > probabilityAlphaUpper + probabilityAlphaLower )
                {
                    c = CharSet.Numeric[Random.Next(CharSet.Numeric.Length)];
                    --probabilityNumeric;
                }
                else if (roll > probabilityAlphaUpper )
                {
                    c = Char.ToUpper(CharSet.Alpha[Random.Next(CharSet.Alpha.Length)]);
                    --probabilityAlphaUpper;
                }
                else
                {
                    c = Char.ToLower(CharSet.Alpha[Random.Next(CharSet.Alpha.Length)]);
                    --probabilityAlphaLower;
                }
                sb.Append(c);
                --length;
            }

            return sb.ToString();
        }
    }
}
