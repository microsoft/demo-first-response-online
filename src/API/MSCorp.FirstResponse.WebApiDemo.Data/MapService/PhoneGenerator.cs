using System;

namespace MSCorp.FirstResponse.WebApiDemo.MapService
{
    public static class PhoneGenerator
    {
        private static readonly Random random = new Random();

        public static string GetPhone(int length, string format)
        {
            string s = string.Empty;
            for (int i = 0; i < length; i++)
                s = String.Concat(s, random.Next(10).ToString());

            double number = Double.Parse(s);
            string phone = number.ToString(format);

            return phone;
        }
    }
}