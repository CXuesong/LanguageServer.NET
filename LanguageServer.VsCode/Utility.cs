using System;
using System.Collections.Generic;
using System.Text;

namespace LanguageServer.VsCode
{
    internal static class Utility
    {
        public static int IndexOf<T>(this IList<T> source, IList<T> match)
        {
            return IndexOf<T>(source, match, EqualityComparer<T>.Default);
        }

        public static int IndexOf<T>(this IList<T> source, IList<T> match, IEqualityComparer<T> comparer)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (match == null) throw new ArgumentNullException(nameof(match));
            int iub = source.Count - match.Count + 1;
            // Brute force
            for (int i = 0; i < iub; i++)
            {
                for (int j = 0; j < match.Count; j++)
                {
                    if (!comparer.Equals(source[i + j], match[j])) goto NEXT;
                }
                return i;
                NEXT:
                ;
            }
            return -1;
        }
    }
}
