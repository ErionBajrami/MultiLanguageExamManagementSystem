using System.Linq.Expressions;

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
    }
}
