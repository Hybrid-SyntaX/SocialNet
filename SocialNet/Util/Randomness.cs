using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace SocialNet.Util
{
    public static class Randomness
    {
        public static Random SuperRandom
        {
            get
            {
                return new Random(Guid.NewGuid().GetHashCode());
            }
        }

        public static T RandomMember<T>(IList<T> list)
        {
            return list[new Random(Guid.NewGuid().GetHashCode()).Next(0, list.Count())];
        }

        public static T RandomEnum<T>()
        {
            Array values = Enum.GetValues(typeof(T));
            Random random = new Random(Guid.NewGuid().GetHashCode());
            T randomEnum = (T)values.GetValue(random.Next(values.Length));

            return randomEnum;
        }

        public static string RandomName(int length)
        {
            string consonant = "BCDFGHJKLMNPQRSTVXZWY";
            string vowels = "AEIOU";
            var random = new Random(Guid.NewGuid().GetHashCode());
            var conran = new Random(Guid.NewGuid().GetHashCode());
            var vonran = new Random(Guid.NewGuid().GetHashCode());

            string word = string.Empty;
            for (int i = 0; i < random.Next(1, length); i++)
            {
                word += consonant[conran.Next(0, consonant.Length)].ToString() + vowels[vonran.Next(0, vowels.Length)].ToString();
            }
            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(word.ToLower());
        }

        public static string RandomAlphanumericName(int length)
        {
            int part = (int)length / 3;
            string word = RandomName(part) + RandomName(part) + RandomNumberString(part);
            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(word.ToLower());
        }

        public static string RandomAlphaspacename(int length)
        {
            return RandomName(length - 3 + 1) + " " + RandomName(length);
        }

        public static string RandomNumberString(int length)
        {
            const string chars = "0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[SuperRandom.Next(s.Length)]).ToArray());
        }

        public static string Email(int lenth)
        {
            return string.Format("{0}@{1}.{2}", RandomAlphanumericName(5), RandomName(4), RandomName(3));
        }

        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random(Guid.NewGuid().GetHashCode());
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public static double RandomPercent(int decimalPoint = 2)
        {
            return Math.Round(SuperRandom.NextDouble(), decimalPoint);
        }

        public static DateTime RandomDate()
        {
            Random random = new Random(Guid.NewGuid().GetHashCode());

            int randomYear = random.Next(1900, 2000);
            int randomMonth = random.Next(1, 12);
            int randomDay = random.Next(1, DateTime.DaysInMonth(randomYear, randomMonth));

            return new DateTime(randomYear, randomMonth, randomDay);
        }

        public static PropertyInfo RandomProperty<T>()
        {
            Random random = new Random(Guid.NewGuid().GetHashCode());
            var properties = typeof(T).GetProperties();
            int index = random.Next(0, properties.Count() - 1);
            return properties[index];
        }
    }
}
