using System.Collections.Generic;
using UnityEngine;

public class RandomSystem
{
    public static T GetRandom<T>(List<T> list)
    {
        return list[Random.Range(0, list.Count)];
    }

    public static bool RandomBool(float truePercentage)
    {
        return Random.Range(0.0f, 100.0f) < truePercentage;
    }

    public static int RandomInt(int max, int min = 0)
    {
        return Random.Range(min, max);
    }

    public static float RandomFloat(float max, float min = 0.0f)
    {
        return Random.Range(min, max);
    }
}