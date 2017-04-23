using UnityEngine;
using System;

public static class GameModel
{
    public static bool hasPlayedTutorial = false;
    public static float totalPlaytime { get; set; }
    public static int totalScore { get; set; }
    public static int money { get; set; }
    public static int shotsFired { get; set; }
    public static string playerName { get; set; }
}