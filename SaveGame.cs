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
        FileStream file = File.Create(Application.persistentDataPath + "/gameTimes3.gd");
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
        if (File.Exists(Application.persistentDataPath + "/gameTimes3.gd"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/gameTimes3.gd", FileMode.Open);
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

    public static List<Save> GetLocalHighscore(LevelModel model)
    {
        List<Save> correctLevels = new List<Save>();
        foreach (Save save in savedGames)
        {
            if (save.levelName == model.levelName && save.country == model.levelCountry)
            {
                correctLevels.Add(save);
            }
        }

        if (model.type == "forward")
        {
            correctLevels.Sort((x, y) => -1 * x.score.CompareTo(y.score));
        }

        else
        {
            correctLevels.Sort((x, y) => x.time.CompareTo(y.time));
        }

        return correctLevels;
    }

    public static float GetHighestScoreFromLevel(LevelModel model)
    {
        List<Save> correctLevels = GetLocalHighscore(model);
        if(correctLevels.Count == 0)
        {
            return 0;
        }
        if (model.type == "forward")
        {
            return correctLevels[0].score;
        }
        
        else
        {
            return correctLevels[0].time;
        }
    }
}

[System.Serializable]
public class Save
{
    public float time;
    public int shotsFired;
    public int targetsHit;
    public int score;
    public string type;
    public string playerName;
    public string levelName;
    public string country;
    public Save(float time, string levelName, string playerName, int score, string country, string type)
    {
        this.time = time;
        this.levelName = levelName;
        this.playerName = playerName;
        this.score = score;
        this.country = country;
        this.type = type;
    }
}
