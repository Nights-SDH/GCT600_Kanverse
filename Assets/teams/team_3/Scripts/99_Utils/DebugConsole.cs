using System.Collections.Generic;
using UnityEngine;

public static class DebugConsole
{
    public static void Error(string message)
    {
        Debug.LogError(message);
    }

    public static void Warning(string message)
    {
        Debug.LogWarning(message);
    }

    public static void Log(string[] messages)
    {
        string totalMessage = "";
        foreach (string message in messages)
        {
            totalMessage += message;
        }
        Debug.Log(totalMessage);
    }

    public static void Log(string message)
    {
        Debug.Log(message);
    }

    public static void Log(float message)
    {
        Debug.Log(message);
    }

    public static void Log(bool message)
    {
        Debug.Log(message);
    }

    public static void Log(Vector2 vector)
    {
        Debug.Log(vector);
    }

    // public static void Log(List<Skill> skillList)
    // {
    //     foreach (Skill skill in skillList)
    //     {
    //         Log(skill.skillName.ToString());
    //     }
    // }
}