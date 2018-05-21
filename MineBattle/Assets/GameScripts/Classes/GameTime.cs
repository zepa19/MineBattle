using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameTime
{

    public static float Seconds { get; set; }
    public static int Day { get; set; }
    private static int Speed { get; set; }
    public static float deltaTime;

    public static void Initialize(float Seconds = 21601f, int Day = 0, int Speed = 140)
    {
        GameTime.Seconds = Seconds;
        GameTime.Day = Day;
        GameTime.Speed = Speed;
        deltaTime = 0;
    }

    public static void Update(float dTime)
    {
        deltaTime = dTime * Speed;
        Seconds += deltaTime;

        if (Seconds > 86400)
        {
            Day += (int)Seconds / 86400;
            Seconds = Seconds % 360;
        }
    }



    public static int[] GetDataToArray()
    {
        return new int[] { (int)Seconds, Day };
    }

    new public static string ToString()
    {
        string[] tempTime = TimeSpan.FromSeconds(Seconds).ToString().Split(':');
        return string.Format("Local Time: {0}:{1} | Day: {2}", tempTime[0], tempTime[1], Day + 1);
    }

}
