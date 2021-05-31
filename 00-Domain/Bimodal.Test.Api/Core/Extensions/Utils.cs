using Bimodal.Test.Database;
using System;
using System.Linq;
using System.Security.Cryptography;

namespace Bimodal.Test.Api.Extensions
{
    public static class Utils
    {
        public const int SALT_VALUE_SIZE = 8;

        public const int OUTPUT_SIZE = 32;

        public static bool IsExistsBookingNumber(AgencyContext dbContext, string bookingNumber) 
        {
            bool result = dbContext.Bookings.Any(f => f.BookingNumber == bookingNumber);
            return !result;
        }

        public static bool IsExistsCustomer(AgencyContext dbContext, string documentNumber)
        {
            bool result = dbContext.Customers.Any(f => f.Dni == documentNumber);
            return !result;
        }

        public static bool IsExistsEmail(AgencyContext dbContext, string email)
        {
            bool result = dbContext.Users.Any(f => f.Email == email);
            return !result;
        }

        public static bool IsExistsUserName(AgencyContext dbContext, string user)
        {
            bool result = dbContext.Users.Any(f => f.UserName == user);
            return !result;
        }

        public static bool IsExistsCustomer(AgencyContext dbContext, Guid id)
        {
            bool result = dbContext.Users.Any(f => f.Id == id);
            return !result;
        }

        public static bool IsExistsBookingInfo(AgencyContext dbContext, Guid id)
        {
            var result = dbContext.Bookings.FirstOrDefault(f => f.Id == id);
            return result == null;
        }

        public static string GenerateSalt()
        {
            return Convert.ToBase64String(GenerateSaltValue());
        }

        public static byte[] GenerateSaltValue()
        {
            var rngCsp = new RNGCryptoServiceProvider();
            var randomNumber = new byte[SALT_VALUE_SIZE];
            rngCsp.GetBytes(randomNumber);
            return randomNumber;
        }

        public static string HashPassword(string clearData, byte[] saltValue)
        {
            if (saltValue == null)
            {
                throw new InvalidOperationException("You need a salt to hash password securely");
            }

            Rfc2898DeriveBytes pbkdf2 = new Rfc2898DeriveBytes(clearData, saltValue);

            var hashedPassword = pbkdf2.GetBytes(OUTPUT_SIZE);

            return Convert.ToBase64String(hashedPassword);
        }

        public static string HashPassword(string clearData, string salt)
        {
            if (string.IsNullOrWhiteSpace(salt))
            {
                throw new InvalidOperationException("You need a salt to hash password securely");
            }

            return HashPassword(clearData, Convert.FromBase64String(salt));
        }

        public static Tuple<string, string> GenerateRandomPassword()
        {
            var salt = GenerateSalt();
            var passwordHash = HashPassword(Guid.NewGuid().ToString(), salt);
            return new Tuple<string, string>(passwordHash, salt);
        }
    }
}
