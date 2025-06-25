/// <summary>
/// Provides extension methods for collections.
/// </summary>

/// <remarks>
/// 25/06/2025 by Damian Dalinger: Script Creation.
/// </remarks>

using System.Collections.Generic;
using UnityEngine;

public static class CollectionsExtension
{
    // Returns a random element from the list.
    public static T GetRandom<T>(this List<T> list)
    {
        if (list == null || list.Count == 0)
        {
            return default;
        }

        return list[Random.Range(0, list.Count)];
    }

    // Returns a random element from the array.
    public static T GetRandom<T>(this T[] array)
    {
        if (array == null || array.Length == 0)
        {
            return default;
        }

        return array[Random.Range(0, array.Length)];
    }
}