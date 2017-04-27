using UnityEngine;
using System.Collections;
using TMPro;
using System;
using DG.Tweening;
using System.Collections.Generic;

public class LevelScreenController : MonoBehaviour {

    public TextMeshPro scoreText;
    public HighscoreCollection highscores;
    public TextMeshPro statusText;
    public static LevelScreenController instance;
    private Vector3 initialScale;
    public GameObject countryLabel;
    private LevelModel[] usaLevels;
    private LevelModel[] japanLevels;
    private int currentLevel = 0;
    public string currentCountry = "usa";
    private LevelModel[] currentLevels;
    public GameObject levelLabel;
    public GameObject localHighscoreText;
    public GameObject levelNumberLabel;
    public int currentLevelNumber;
    public LevelModel currentLevelModel;

    void Awake()
    {
        instance = this;
        initialScale = this.transform.localScale;
        usaLevels = new LevelModel[] {
            new LevelModel(0, "usa", "Houston", "forward", 60f, 35000),
            new LevelModel(0, "usa", "Chicago", "designed", 60f, 35000),
            new LevelModel(0, "usa", "San Francisco", "forward", 60f, 35000),
            new LevelModel(0, "usa", "New York", "designed", 60f, 35000)
        };

        japanLevels = new LevelModel[] {
            new LevelModel(0, "japan", "Tokyo", "forward", 60f, 35000),
            new LevelModel(0, "japan", "Osaka", "designed", 60f, 35000),
            new LevelModel(0, "japan", "Kobe", "forward", 60f, 35000),
            new LevelModel(0, "japan", "Sapporo", "designed", 60f, 35000)
        };

    }

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void ShowCountry(string country)
    {
        if(country == "usa")
        {
            countryLabel.GetComponent<TextMeshPro>().text = "USA";
            currentLevels = usaLevels;
            currentCountry = country;
        }
        if (country == "japan")
        {
            countryLabel.GetComponent<TextMeshPro>().text = "Japan";
            currentLevels = japanLevels;
        }

        SetLevel(0);
    }

    public void SetLevel(int levelNum)
    {
        Debug.Log("SetLevel: " + levelNum);
        currentLevelNumber = levelNum;

        currentLevelModel = currentLevels[currentLevelNumber];

        GetHighscoreFromServer(levelNum);
        SetLocalHighscore();

        levelLabel.GetComponent<TextMeshPro>().text = currentLevelModel.levelName;

        float highestScore = SaveGame.GetHighestScoreFromLevel(currentLevels[currentLevelNumber]);
        string status = "";
        string beat = "";
        if (currentLevelModel.type == "designed")
        {
            if (currentLevelModel.goalTime < highestScore || highestScore == 0)
            {
                status = "<color=red>Uncompleted</color>";
            }
            else
            {
                status = "<color=green>Completed</color>";
            }
            beat = "Beat: " + GameTimeController.FloatToTime(currentLevelModel.goalTime, "0:00.0");
        }
        else
        {
            if (currentLevelModel.goalScore < highestScore)
            {
                status = "<color=green>Completed</color>";
            }
            else
            {
                status = "<color=red>Uncompleted</color>";
            }
            beat = "Beat: " + string.Format("{0:n0}", currentLevelModel.goalScore) ;
        }

        levelNumberLabel.GetComponent<TextMeshPro>().text = (levelNum + 1).ToString();

        string mainScreenText = "<size=4>Mode: " + currentLevels[currentLevelNumber].type + "</size>\n\n";
        mainScreenText += "Best Score:\n<size=8>" + highestScore + "</size>\n\n";
        mainScreenText += beat + "\n";
        mainScreenText += "Status: " + status;

        mainScreenLabel.GetComponent<TextMeshPro>().text = mainScreenText;
    }

    void SetLocalHighscore()
    {
        Debug.Log("SetLocalHighscore");
        List<Save> localHighscore = SaveGame.GetLocalHighscore(currentLevelModel);
        string localHighscoreText = "";
        foreach (Save save in localHighscore)
        {
            if(currentLevelModel.type == "forward") {
                localHighscoreText += save.playerName + " : " + string.Format("{0:n0}", save.score);
            }
            else
            {
                localHighscoreText += save.playerName + " : " + GameTimeController.FloatToTime(save.time, "0:00.0") ;
            }
        }
    }

    public void IncreaseLevel()
    {
        if (currentLevelNumber + 1 > 4)
            return;
        currentLevelNumber++;
        SetLevel(currentLevelNumber);
    }

    public void DecreaseLevel()
    {
        if (currentLevelNumber - 1 < 0)
            return;
        currentLevelNumber--;
        SetLevel(currentLevelNumber);
    }

    public void Hide(float t = .3f)
    {
        transform.DOScale(0f, t);
    }

    public void Show(float t = .3f)
    {
        transform.DOScale(initialScale, t);
    }

    public void SaveHighscore(int level, int highscore, string playername, float time)
    {
        StartCoroutine(SaveHighscoreToServer(level,highscore,playername,time));
    }

    private string highscoreURL = "http://gunfitness.herokuapp.com/gethighscore/";

    IEnumerator GetHighscoreFromServer(int level, int roundScore = 0, string playername = "undefined", float time = 0f)
    {
        Debug.Log("GetHighscoreFromServer");
        statusText.text = "Status: Getting highscores from server...";
        WWW www = new WWW(highscoreURL + level.ToString());
        yield return www;
        Debug.Log(www.text);
        var json = JsonUtility.FromJson<HighscoreCollection>(www.text);
        highscores = json;
        scoreText.text = "";
        if(roundScore > 0)
        {
            Array.Resize(ref highscores.highscoreentries, highscores.highscoreentries.Length);
            highscores.highscoreentries[highscores.highscoreentries.Length - 1] = new HighscoreEntry(level, roundScore, playername, "undefined",(float) time);
        }
        int count = 0;
        foreach(HighscoreEntry entry in highscores.highscoreentries)
        {
            scoreText.text +=  entry.playername + ":" + string.Format("{0:n0}", entry.highscore)  + "\n";
            if (count > 5)
            {
                yield break;
            }
        }
        statusText.text = "";
    }

    private string saveHighscoreURL = "http://gunfitness.herokuapp.com/sendhighscore";
    public GameObject mainScreenLabel;

    IEnumerator SaveHighscoreToServer(int level, int highscore, string playername, float time)
    {
        Debug.Log("SaveHighscoreToServer");
        statusText.text = "Status: Saving highscore to server...";
        WWWForm form = new WWWForm();
        string key = CreateKey(level, highscore, playername);
        form.AddField("level", level.ToString());
        form.AddField("highscore", highscore.ToString());
        form.AddField("playername", playername);
        form.AddField("time", time.ToString());
        form.AddField("key", key);
        WWW www = new WWW(saveHighscoreURL,form);
        yield return www;
        if (!string.IsNullOrEmpty(www.error))
        {
            print(www.error);
        }
        else {
            print(www.text);
        }
        StartCoroutine(GetHighscoreFromServer(1,highscore,playername));
    }

    String CreateKey(int level, int highscore, string playername)
    {
        int levelNum = 0;
        foreach (char c in level.ToString())
        {
            Debug.Log((int)c);
            levelNum += (int)c;
        }
        foreach (char c in highscore.ToString())
        {
            Debug.Log((int)c);
            levelNum += (int)c;
        }
        foreach (char c in playername.ToString())
        {
            Debug.Log( (int)c);
            levelNum += (int)c;
        }
        Debug.Log(levelNum);
        return levelNum.ToString();
    }
}

[System.Serializable]
public class HighscoreEntry
{
    public int level;
    public float highscore;
    public string playername;
    public string _id;
    public float time;
    public HighscoreEntry(int level, float highscore, string playername, string _id, float time)
    {
        this.level = level;
        this.highscore = highscore;
        this.playername = playername;
        this._id = _id;
        this.time = time;
    }
}

[System.Serializable]
public class HighscoreCollection
{
    public HighscoreEntry[] highscoreentries;
}

