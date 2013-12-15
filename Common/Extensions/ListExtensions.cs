using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Extensions
{
    public static class ListExtensions
    {
        public static TOutput GetOneResult<TInput, TOutput>(this List<TInput> weatherInfos, Func<TInput, bool> predicate, Func<TInput, TOutput> select) where TOutput : class
        {
            return weatherInfos
                    .Where(predicate)
                    .Select(select)
                    .FirstOrDefault();
        }

        public static List<TOutput> GetMultipleResults<TInput, TOutput>(this List<TInput> weatherInfos,
                                      Func<TInput, bool> predicate, Func<TInput, TOutput> select)
        {
            var values = weatherInfos
                            .Where(predicate)
                            .Select(select).ToList();

            if (values.Any())
            {
                return values;
            }

            return new List<TOutput> { default(TOutput) };
        }
    }
}
