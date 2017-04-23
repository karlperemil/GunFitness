using UnityEngine;
using System.Collections;

[System.Serializable]
public class GameState
{
    public bool hasPlayedTutorial = false;
    public float totalPlaytime = 0;
    public int totalScore = 0;
    public int money = 0;
    public int shotsFired = 0;
    public string playerName = "Player Name";

    public GameState()
    {

    }

}