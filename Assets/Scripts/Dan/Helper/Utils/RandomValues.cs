using System.Collections.Generic;
using UnityEngine;

namespace Dan.Helper.Utils
{
    public static class RandomValues
    {
        /// <summary>
        /// Get random value from x to y
        /// </summary>
        /// <param name="vectorRange"> Vector Range</param>
        /// <returns></returns>
        public static float RandomRange(this Vector2 vectorRange)
        {
            return Random.Range(vectorRange.x, vectorRange.y);
        }
        
        /// <summary>
        /// Get random value from x to y
        /// </summary>
        /// <param name="vectorRange"> Vector Range</param>
        /// <returns></returns>
        public static float RandomRange(this Vector2Int vectorRange)
        {
            return Random.Range(vectorRange.x, vectorRange.y);
        }

        /// <summary>
        /// Get random item from the list
        /// </summary>
        /// <param name="list">List</param>
        /// <typeparam name="T">Random item of type</typeparam>
        /// <returns>Random item from list</returns>
        public static T GetRandom<T>(this IList<T> list)
        {
            return list.Count == 0 ? default : list[Random.Range(0, list.Count)];
        }
        
        /// <summary>
        /// Get random item from the array list
        /// </summary>
        /// <param name="arrayList">Array list</param>
        /// <typeparam name="T">Random item of type</typeparam>
        /// <returns>Random item from array list</returns>
        public static T GetRandom<T>(this T[] arrayList)
        {
            return arrayList.Length == 0 ? default : arrayList[Random.Range(0, arrayList.Length)];
        }
    }
}