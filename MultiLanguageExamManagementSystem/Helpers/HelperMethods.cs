using System.Linq.Expressions;
using System.Security.Cryptography;
using System.Text;

namespace LifeEcommerce.Helpers
{
    public static class HelperMethods
    {
        public static IQueryable<T> WhereIf<T>(this IQueryable<T> query, bool searchCondition, Expression<Func<T, bool>> predicate)
        {
            return searchCondition ? query.Where(predicate) : query;
        }

        public static void Shuffle<T>(List<T> list)
        {
            var rng = new Random();
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        public static string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
        }

        public static bool VerifyPasswordHash(string password, string storedHash)
        {
            var hashOfInput = HashPassword(password);
            return string.Equals(hashOfInput, storedHash);
        }
    }
}
