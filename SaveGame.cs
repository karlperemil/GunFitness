using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class SaveGame {

	// Use this for initialization
    public static List<Save> savedGames = new List<Save>();
    public static GameState gameState = new GameState();

    public static void SaveCurrentGame(Save save)
    {
        SaveGame.savedGames.Add(save);
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/gameTimes2.gd");
        bf.Serialize(file, savedGames);
        file.Close();
    }

    public static void SaveProgress()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/gameState.gd");
        bf.Serialize(file, gameState);
        file.Close();
        Debug.Log("Saved Game State:" + gameState.shotsFired + " " + gameState.totalScore + " " + gameState.totalPlaytime);
    }

    public static void LoadGame()
    {
        Debug.Log("LoadGame()");
        if (File.Exists(Application.persistentDataPath + "/gameTimes2.gd"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/gameTimes2.gd", FileMode.Open);
            SaveGame.savedGames = (List<Save>)bf.Deserialize(file);
            file.Close();

            savedGames.Sort(delegate (Save a, Save b) {
                return a.time.CompareTo(b.time);
            });
        }

        if (File.Exists(Application.persistentDataPath + "/gameState.gd"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/gameState.gd", FileMode.Open);
            SaveGame.gameState = (GameState)bf.Deserialize(file);
            file.Close();
        }
    }
}

[System.Serializable]
public class Save
{
    public float time;
    private int shotsFired;
    private int targetsHit;
    private int score;
    public string playerName;
    public string levelName;
    public Save(float time, string levelName, string playerName, int score)
    {
        this.time = time;
        this.levelName = levelName;
        this.playerName = playerName;
        this.score = score;
    }
}
