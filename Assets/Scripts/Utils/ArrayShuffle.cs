using System;
using System.Collections.Generic;
using UnityEngine;

public static class ArrayShuffle
{
    public static void Shuffle<T>(this Array array)
    {
        for (int t = 0; t < array.Length; t++)
        {
            T tmp = (T)array.GetValue(t);
            int r = UnityEngine.Random.Range(t, array.Length);
            array.SetValue(array.GetValue(r), t);
            array.SetValue(tmp, r);
        }
    }

    public static void Shuffle<T>(this List<T> list)
    {
        for (int t = 0; t < list.Count; t++)
        {
            T tmp = (T)list[t];
            int r = UnityEngine.Random.Range(t, list.Count);
            list[t] = list[r];
            list[r] = tmp;
        }
    }

    public static T[][] FlipMatrixHorizontally<T>(this T[][] matrix)
    {
        T[][] ret = new T[matrix.Length][];
        for (int i = 0; i < matrix.Length; ++i) {
            ret[i] = new T[matrix[i].Length];
            for (int j = 0; j < matrix[i].Length; ++j) {
                 ret[i][j] = matrix[i][matrix[i].Length - 1 - j];
            }
        }
        return ret;
    }

    public static T[][] FlipMatrixVertically<T>(this T[][] matrix)
    {
        T[][] ret = new T[matrix.Length][];
        for (int i = 0; i < matrix.Length; ++i) {
            ret[i] = matrix[matrix.Length - 1 - i];
        }
        return ret;
    }

}
