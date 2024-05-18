#region

using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = System.Random;

#endregion

namespace Core.Utilities
{
    public static class BasicEx
    {
        public static T CoerceIn<T>(T value, T min, T max) where T : IComparable<T>
        {
            if (value.CompareTo(min) < 0)
            {
                return min;
            }

            if (value.CompareTo(max) > 0)
            {
                return max;
            }

            return value;
        }

        public static bool IsNull<T>(this T item) where T : class
        {
            if (item is null)
            {
                return true;
            }

            // UnityObjectの場合、特別なnullチェックが必要
            if (item is Object unityObject)
            {
                return unityObject == null;
            }

            // IComparable<T>を実装していない場合は、デフォルトのnullチェック
            return item.Equals(null);
        }

        public static List<T> ToList<T>(this T value) where T: class => new() { value, };
        
        public static List<T> ToList<T>(this T[] value) where T: class => new(value);

        public static T CoerceAtLeast<T>(T value, T min) where T : IComparable<T> =>
            value.CompareTo(min) < 0 ? min : value;

        public static T CoerceAtMost<T>(T value, T max) where T : IComparable<T> =>
            value.CompareTo(max) > 0 ? max : value;

        public static bool IsNullOrBlank(string value) => string.IsNullOrWhiteSpace(value);

        public static bool IsNullOrEmpty(string value) => string.IsNullOrEmpty(value);

        public static T Max<T>(params T[] values) where T : IComparable<T> => values.Max();

        public static T Min<T>(params T[] values) where T : IComparable<T> => values.Min();

        public static float Abs(float value) => value < 0 ? -value : value;

        public static double Abs(double value) => value < 0 ? -value : value;

        public static int Abs(int value) => value < 0 ? -value : value;

        public static bool IsNullOrEmpty<T>(IEnumerable<T> collection) => collection == null || !collection.Any();

        public static int Sum(IEnumerable<int> numbers) => numbers.Sum();

        public static decimal Average(IEnumerable<decimal> numbers) => numbers.Average();

        public static IEnumerable<T> DistinctBy<T, TKey>(this IEnumerable<T> items, Func<T, TKey> keySelector)
        {
            if (items == null)
            {
                yield break;
            }

            var seenKeys = new HashSet<TKey>();

            foreach (var element in items)
            {
                if (seenKeys.Add(keySelector(element)))
                {
                    yield return element;
                }
            }
        }

        public static string Repeat(this string value, int count)
        {
            if (string.IsNullOrEmpty(value))
            {
                return value;
            }

            return string.Concat(Enumerable.Repeat(value, count));
        }

        public static IEnumerable<T> TakeLast<T>(IEnumerable<T> source, int count)
        {
            var enumerable = source.ToList();
            return (IEnumerable<T>)enumerable.Skip(Math.Max(0, enumerable.Count() - count));
        }

        public static IEnumerable<T> Drop<T>(IEnumerable<T> source, int count) => source.Skip(count);

        public static IEnumerable<T> TakeWhile<T>(IEnumerable<T> source, Func<T, bool> predicate) =>
            source.TakeWhile(predicate);

        public static IEnumerable<T> DropWhile<T>(IEnumerable<T> source, Func<T, bool> predicate)
        {
            if (source == null)
            {
                yield break;
            }

            var yielding = false;

            foreach (var item in source)
            {
                if (!yielding && !predicate(item))
                {
                    yielding = true;
                }

                if (yielding)
                {
                    yield return item;
                }
            }
        }

        public static T MaxBy<T, TKey>(IEnumerable<T> source, Func<T, TKey> selector) where TKey : IComparable<TKey> =>
            source.OrderByDescending(selector).FirstOrDefault();

        public static T MinBy<T, TKey>(IEnumerable<T> source, Func<T, TKey> selector) where TKey : IComparable<TKey> =>
            source.OrderBy(selector).FirstOrDefault();

        public static IEnumerable<int> GenerateRange(int start, int count) => Enumerable.Range(start, count);

        public static DateTime Tomorrow() => DateTime.Today.AddDays(1);

        public static DateTime Yesterday() => DateTime.Today.AddDays(-1);

        public static bool IsLeapYear(int year) => DateTime.IsLeapYear(year);

        public static string Format(this DateTime date, string format) => date.ToString(format);


        [CanBeNull]
        public static T FirstOrNull<T>(this IEnumerable<T> source) where T : class => source.FirstOrDefault();

        [CanBeNull]
        public static T LastOrNull<T>(this IEnumerable<T> source) where T : class => source.LastOrDefault();

        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source)
        {
            var rand = new Random();
            var elements = source.ToArray();
            for (var i = elements.Length - 1; i > 0; i--)
            {
                var swapIndex = rand.Next(i + 1);
                (elements[i], elements[swapIndex]) = (elements[swapIndex], elements[i]);
            }

            return elements;
        }

        public static void ExecuteSafe(Action action)
        {
            try
            {
                action();
            }
            catch (Exception ex)
            {
                // ログ記録や例外処理
                Console.WriteLine("Exception caught: " + ex.Message);
            }
        }

        public static T ExecuteWithResultSafe<T>(Func<T> func)
        {
            try
            {
                return func();
            }
            catch (Exception ex)
            {
                // ログ記録や例外処理
                Console.WriteLine("Exception caught: " + ex.Message);
                return default;
            }
        }

        public static void Deconstruct<T>(this IList<T> list, out T first, out IList<T> rest)
        {
            if (list == null)
            {
                rest = new List<T>();
                first = default;
                return;
            }

            first = list.Count > 0 ? list[0] : default;
            rest = (IList<T>)list.Skip(1).ToList();
        }

        public static IDictionary<TKey, TValue> ToDictionary<TKey, TValue>(
            this IEnumerable<(TKey Key, TValue Value)> tuples)
        {
            return tuples.ToDictionary(t => t.Key, t => t.Value);
        }

        [CanBeNull]
        public static T Find<T>(this IEnumerable<T> source, Func<T, bool> predicate) where T : class =>
            source.FirstOrDefault(predicate);

        public static IEnumerable<T> Filter<T>(this IEnumerable<T> source, Func<T, bool> predicate) =>
            source.Where(predicate);

        public static bool Any<T>(this IEnumerable<T> source, Func<T, bool> predicate) => source.Where(predicate).Any();

        public static bool None<T>(this IEnumerable<T> source, Func<T, bool> predicate) => !source.Any(predicate);

        [CanBeNull]
        public static T FirstOrNull<T>(this IEnumerable<T> source, Func<T, bool> predicate) where T : class =>
            source.FirstOrDefault(predicate);

        [CanBeNull]
        public static T SingleOrNull<T>(this IEnumerable<T> source, Func<T, bool> predicate) where T : class
        {
            try
            {
                return source.SingleOrDefault(predicate);
            }
            catch
            {
                return null;
            }
        }

        [CanBeNull]
        public static T ElementAtOrNull<T>(this IEnumerable<T> source, int index) where T : class
        {
            var enumerable = source.ToList();
            if (enumerable.Count > index)
            {
                return (T)enumerable.ElementAt(index);
            }

            return null;
        }

        [CanBeNull]
        public static T FindLast<T>(this IEnumerable<T> source, Func<T, bool> predicate) where T : class =>
            source.LastOrDefault(predicate);

        public static int FindIndex<T>(this IList<T> list, Func<T, bool> predicate)
        {
            if (list == null)
            {
                return -1;
            }

            for (var i = 0; i < list.Count; i++)
            {
                if (predicate(list[i]))
                {
                    return i;
                }
            }

            return -1;
        }

        public static int FindLastIndex<T>(this IList<T> list, Func<T, bool> predicate)
        {
            if (list == null)
            {
                return -1;
            }

            for (var i = list.Count - 1; i >= 0; i--)
            {
                if (predicate(list[i]))
                {
                    return i;
                }
            }

            return -1;
        }

        public static void DestroyGameObject<T>(this T source) where T : MonoBehaviour
        {
            Object.Destroy(source.gameObject);
        }
    }
}