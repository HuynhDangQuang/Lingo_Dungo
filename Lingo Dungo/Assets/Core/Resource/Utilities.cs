using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utilities
{
    public static string Shuffle(this string str)
    {
        char[] array = str.ToCharArray();
        int n = array.Length;
        while (n > 1)
        {
            n--;
            int k = Random.Range(0, n - 1);
            var value = array[k];
            array[k] = array[n];
            array[n] = value;
        }
        return new string(array);
    }

    public static void ShuffleList<T>(this IList<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = Random.Range(0, n - 1);
            (list[n], list[k]) = (list[k], list[n]);
        }
    }

    public static bool IsOpponent(Combatant obj1, Combatant obj2)
    {
        return (obj1 is Player && obj2 is Enemy) || (obj1 is Enemy && obj2 is Player);
    }
}
