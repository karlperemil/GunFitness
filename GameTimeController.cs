using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class GameTimeController : MonoBehaviour {

    public GameObject newRecord;
    public GameObject roundTime;
    public GameObject recordTime;
    public GameObject highScoreMenu;

    private Dictionary<string, List<Save>> levelTimes = new Dictionary<string, List<Save>>();
    private float recordTimeForLevel;
    private string currentLevelName;

    public GameObject[] localTimeLabels;
    public GameObject[] worldTimeLables;

    // Use this for initialization
    void Start () {
        SaveGame.LoadGame();
    }

    public void ShowTimeForLevel(string levelName)
    {
        HideAllLabels();
        currentLevelName = levelName;
        Dictionary<string, List<Save>> allLevels = new Dictionary<string, List<Save>>();

        List<Save> levelSaves = new List<Save>();
        foreach (Save savedTime in SaveGame.savedGames)
        {
            if(savedTime.levelName == levelName)
            {
                levelSaves.Add(savedTime);
            }
        }

        levelSaves.Sort(delegate (Save x, Save y)
        {
            return x.time.CompareTo(y.time);
        });

        int count = 0;
        foreach(Save levelSave in levelSaves)
        {
            string playerName = levelSaves[count].playerName;
            if(playerName.Length > 15)
            {
                playerName = playerName.Substring(0, 14) + "...";
            }
            localTimeLabels[count].GetComponent<TextMesh>().text = playerName + "\n" + FloatToTime(levelSaves[count].time, "0:00.0");
            localTimeLabels[count].SetActive(true);
            count += 1;
            if (count >= 3)
                break;
        }
    }

    private void HideAllLabels()
    {
        foreach(GameObject go in localTimeLabels)
        {
            go.SetActive(false);
        }
        foreach(GameObject go in worldTimeLables)
        {
            go.SetActive(false);
        }
    }

    public void LevelComplete(float finalTime, string levelName)
    {
        List<Save> levelSaves = new List<Save>();
        foreach (Save savedTime in SaveGame.savedGames)
        {
            if (savedTime.levelName == levelName)
            {
                levelSaves.Add(savedTime);
            }
        }

        levelSaves.Sort(delegate (Save x, Save y)
        {
            return x.time.CompareTo(y.time);
        });

        float oldRecordTime = 9999999f;
        if (levelSaves.Count > 0)
        {
            oldRecordTime = levelSaves[0].time;
        }

        if(finalTime < oldRecordTime)
        {
            newRecord.SetActive(true);
            recordTime.GetComponent<TextMesh>().text = "Record: " + FloatToTime(finalTime, "0:00.0");
        }
        else
        {
            recordTime.GetComponent<TextMesh>().text = "Record:: " + FloatToTime(oldRecordTime, "0:00.0");
            newRecord.SetActive(false);
        }

        roundTime.GetComponent<TextMesh>().text = "Time: " + FloatToTime(finalTime, "0:00.0");
        roundTime.SetActive(true);
        recordTime.SetActive(true);
        highScoreMenu.SetActive(true);

        Save newRecordTime = new Save(finalTime, levelName, "Utmanings Djävulen 2",1000);
        SaveGame.SaveCurrentGame(newRecordTime);

        ShowTimeForLevel(levelName);
    }

    public void Hide()
    {
        newRecord.SetActive(false);
        recordTime.SetActive(false);
        roundTime.SetActive(false);
        highScoreMenu.SetActive(false);
    }

    public void ShowHighscores()
    {
        highScoreMenu.SetActive(true);
    }
	
	// Update is called once per frame
	void Update () {
	    
	}

    public string FloatToTime(float toConvert, string format)
    {
        switch (format)
        {
            case "00.0":
                return string.Format("{0:00}:{1:0}",
                    Mathf.Floor(toConvert) % 60,//seconds
                    Mathf.Floor((toConvert * 10) % 10));//miliseconds
                break;
            case "#0.0":
                return string.Format("{0:#0}:{1:0}",
                    Mathf.Floor(toConvert) % 60,//seconds
                    Mathf.Floor((toConvert * 10) % 10));//miliseconds
                break;
            case "00.00":
                return string.Format("{0:00}:{1:00}",
                    Mathf.Floor(toConvert) % 60,//seconds
                    Mathf.Floor((toConvert * 100) % 100));//miliseconds
                break;
            case "00.000":
                return string.Format("{0:00}:{1:000}",
                    Mathf.Floor(toConvert) % 60,//seconds
                    Mathf.Floor((toConvert * 1000) % 1000));//miliseconds
                break;
            case "#00.000":
                return string.Format("{0:#00}:{1:000}",
                    Mathf.Floor(toConvert) % 60,//seconds
                    Mathf.Floor((toConvert * 1000) % 1000));//miliseconds
                break;
            case "#0:00":
                return string.Format("{0:#0}:{1:00}",
                    Mathf.Floor(toConvert / 60),//minutes
                    Mathf.Floor(toConvert) % 60);//seconds
                break;
            case "#00:00":
                return string.Format("{0:#00}:{1:00}",
                    Mathf.Floor(toConvert / 60),//minutes
                    Mathf.Floor(toConvert) % 60);//seconds
                break;
            case "0:00.0":
                return string.Format("{0:0}:{1:00}.{2:0}",
                    Mathf.Floor(toConvert / 60),//minutes
                    Mathf.Floor(toConvert) % 60,//seconds
                    Mathf.Floor((toConvert * 10) % 10));//miliseconds
                break;
            case "#0:00.0":
                return string.Format("{0:#0}:{1:00}.{2:0}",
                    Mathf.Floor(toConvert / 60),//minutes
                    Mathf.Floor(toConvert) % 60,//seconds
                    Mathf.Floor((toConvert * 10) % 10));//miliseconds
                break;
            case "0:00.00":
                return string.Format("{0:0}:{1:00}.{2:00}",
                    Mathf.Floor(toConvert / 60),//minutes
                    Mathf.Floor(toConvert) % 60,//seconds
                    Mathf.Floor((toConvert * 100) % 100));//miliseconds
                break;
            case "#0:00.00":
                return string.Format("{0:#0}:{1:00}.{2:00}",
                    Mathf.Floor(toConvert / 60),//minutes
                    Mathf.Floor(toConvert) % 60,//seconds
                    Mathf.Floor((toConvert * 100) % 100));//miliseconds
                break;
            case "0:00.000":
                return string.Format("{0:0}:{1:00}.{2:000}",
                    Mathf.Floor(toConvert / 60),//minutes
                    Mathf.Floor(toConvert) % 60,//seconds
                    Mathf.Floor((toConvert * 1000) % 1000));//miliseconds
                break;
            case "#0:00.000":
                return string.Format("{0:#0}:{1:00}.{2:000}",
                    Mathf.Floor(toConvert / 60),//minutes
                    Mathf.Floor(toConvert) % 60,//seconds
                    Mathf.Floor((toConvert * 1000) % 1000));//miliseconds
                break;
        }
        return "error";
    }
}
