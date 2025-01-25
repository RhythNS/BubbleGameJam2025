using System.Collections.Generic;
using UnityEngine;

public static class RandomUtil
{
    public static T Element<T>(T[] array) => array[Random.Range(0, array.Length)];

    public static T Element<T>(List<T> array) => array[Random.Range(0, array.Count)];

    public static void Shuffle<T>(List<T> array)
    {
        for (int i = 0; i < array.Count; i++)
        {
            int randomIndex = Random.Range(i, array.Count);
            T temp = array[i];
            array[i] = array[randomIndex];
            array[randomIndex] = temp;
        }
    }
}
