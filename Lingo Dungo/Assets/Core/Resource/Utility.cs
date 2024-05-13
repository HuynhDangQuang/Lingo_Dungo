using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utility
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
}
