using System;
using System.Collections.Generic;

namespace BBRUtilities
{

    public class Comparer
    {
        public static Comparer<U> Get<U>(Func<U, U, int> func)
        {
            return new Comparer<U>(func);
        }
    }

    public class Comparer<T> : Comparer, IComparer<T>, IEqualityComparer<T>
    {

        private Func<T, T, int> comparisonFunction;

        public Comparer(Func<T, T, int> func)
        {
            comparisonFunction = func;
        }

        public int Compare(T x, T y)
        {
            return comparisonFunction(x, y);
        }

        public bool Equals(T x, T y)
        {
            return comparisonFunction(x, y) == 0;
        }

        public int GetHashCode(T obj)
        {
            return obj.GetHashCode();
        }
    }
}