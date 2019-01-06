using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class HighScoreData {
    public static float Time = 0;
    public static string TimeText = "";

    public static int Balloons = 0;

    public static void UpdateTime(float time, string text) {
        if (Time < time) {
            Time = time;
            TimeText = text;
        }
    }
    public static void UpdateBalloons(int balloons) {
        if (Balloons < balloons) {
            Balloons = balloons;
        }
    }
}
