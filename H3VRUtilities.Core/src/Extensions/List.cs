using System;
using System.Collections.Generic;
using Random = System.Random;

namespace H3VRUtilities.Extensions
{
    public static partial class Extensions
    {
        private static Random? random;

        /// <summary>
        /// Returns a random item from the list when called
        /// </summary>
        /// <returns>
        /// A random item from the list
        /// </returns>
        /// <exception cref="System.IndexOutOfRangeException">
        /// Thrown when the given list is empty
        /// </exception>
        public static T GetRandom<T>(this List<T> list)
        {
            if (list.Count < 1)
                throw new IndexOutOfRangeException("Cannot get random item from empty list!");

            random ??= new Random();

            return list[random.Next(list.Count)];
        }
    }
}