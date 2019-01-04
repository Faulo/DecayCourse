using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class HighScoreData
{
    public static float Time = 0;
    public static string Text = "";
    public static void Update(float time, string text) {
        if (Time < time) {
            Time = time;
            Text = text;
        }
    }
}
