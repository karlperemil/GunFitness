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
    public GameObject countryLabel;
    public string currentCountry = "usa";
    public static LevelScreenController instance;
    public GameObject levelLabel;
    public GameObject localHighscoreText;
    public GameObject levelNumberLabel;
    public GameObject startButton;
    public int currentLevelNumber;
    public LevelModel currentLevelModel;

    private Vector3 initialScale;
    private LevelModel[] usaLevels;
    private LevelModel[] japanLevels;
    private LevelModel[] chinaLevels;
    private int currentLevel = 0;
    private LevelModel[] currentLevels;

    void Awake()
    {
        instance = this;
        initialScale = this.transform.localScale;
        usaLevels = new LevelModel[] {
            new LevelModel(0, "usa", "Houston", "forward", 60f, 10000, true),
            new LevelModel(0, "usa", "Chicago", "designed", 60f, 35000),
            new LevelModel(0, "usa", "San Francisco", "forward", 60f, 15000),
            new LevelModel(0, "usa", "New York", "designed", 60f, 35000),
        };

        japanLevels = new LevelModel[] {
            new LevelModel(0, "japan", "Tokyo", "forward", 60f, 20000),
            new LevelModel(0, "japan", "Osaka", "designed", 60f, 35000),
            new LevelModel(0, "japan", "Kobe", "forward", 60f, 25000),
            new LevelModel(0, "japan", "Sapporo", "designed", 60f, 35000)
        };

        chinaLevels = new LevelModel[] {
            new LevelModel(0, "china", "Xiamen", "forward", 60f, 45000),
            new LevelModel(0, "japan", "Shanghai", "designed", 60f, 35000),
            new LevelModel(0, "japan", "Guangzhou", "forward", 60f, 55000),
            new LevelModel(0, "japan", "Beijing", "designed", 60f, 35000)
        };

        levelDictionary = new Dictionary<string, LevelModel[]>();
        levelDictionary.Add("usa", usaLevels);
        levelDictionary.Add("japan", japanLevels);
        levelDictionary.Add("china", chinaLevels);

    }

	// Use this for initialization
	void Start () {
        lockedText.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void ShowCountry(string country)
    {
        Debug.Log("ShowCountry");
        if(country != currentCountry)
        {
            changedCountry = true;
        }
        currentCountry = country;
        if (country == "usa")
        {
            countryLabel.GetComponent<TextMeshPro>().text = "USA";
            currentLevels = usaLevels;
            currentDifficulty = "Easy";
        }
        if (country == "china")
        {
            countryLabel.GetComponent<TextMeshPro>().text = "China";
            currentLevels = chinaLevels;
            currentDifficulty = "Medium";
        }
        if(country == "japan")
        {
            countryLabel.GetComponent<TextMeshPro>().text = "Japan";
            currentLevels = japanLevels;
            currentDifficulty = "Hard";
        }

        SetLevel(0);
    }

    public GameObject lockedText;
    private string previousLevel = "undefined";

    public void SetLevel(int levelNum)
    {
        Debug.Log("SetLevel: " + levelNum);
        currentLevelNumber = levelNum;

        currentLevelModel = currentLevels[currentLevelNumber];

        if (!CheckIfLevelUnlocked())
        {
            lockedText.GetComponent<TextMeshPro>().text = "Locked\n<size=2.5> Complete " + previousLevel + " to play.";
            lockedText.SetActive(true);
            mainScreenLabel.SetActive(false);
            startButton.SetActive(false);
        }
        else
        {
            lockedText.SetActive(false);
            mainScreenLabel.SetActive(true);
            startButton.SetActive(true);
        }

        StartCoroutine(GetHighscoreFromServer(levelNum,currentCountry));
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
            beat = "Beat: " + Utils.FloatToTime(currentLevelModel.goalTime, "0:00.0");
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
            beat = "Beat: " + string.Format("{0:n0}", currentLevelModel.goalScore);
        }

        string highScoreStr = highestScore.ToString();
        if (highestScore == 0)
            highScoreStr = "None";

        levelNumberLabel.GetComponent<TextMeshPro>().text = (levelNum + 1).ToString();
        string scoreOrTime = currentLevelModel.type == "designed" ? "Time" : "Score";

        string mainScreenText = "<size=3>Mode: " + Utils.FirstCharToUpper(currentLevels[currentLevelNumber].type) + "</size>\n";
        mainScreenText += "<size=3>Difficulty: " + currentDifficulty + "</size>\n\n";
        mainScreenText += "<size=2.5>Best "+ scoreOrTime+ "</size>\n<size=8>" + highScoreStr + "</size>\n\n";
        mainScreenText += beat + "\n";
        mainScreenText += "Status: " + status;

        mainScreenLabel.GetComponent<TextMeshPro>().text = mainScreenText;
    }

    private bool CheckIfLevelUnlocked()
    {
        if (currentLevelNumber == 0)
            return true;
        bool isUnlocked;
        LevelModel model = currentLevels[currentLevelNumber - 1];
        previousLevel = model.levelName;
        float highscore = SaveGame.GetHighestScoreFromLevel(model);
        if(model.type == "forward")
        {
            if(highscore > model.goalScore)
            {
                isUnlocked = true;
            }
            else
            {
                isUnlocked = false;
            }
        }
        else
        {
            if(highscore < model.goalTime && highscore != 0)
            {
                isUnlocked = true;
            }
            else
            {
                isUnlocked = false;
            }
        }

        return isUnlocked;

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
                localHighscoreText += save.playerName + " : " + Utils.FloatToTime(save.time, "0:00.0") ;
            }
        }
    }

    public void IncreaseLevel()
    {
        if (currentLevelNumber + 1 > 3)
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
        StartCoroutine(SaveHighscoreToServer(level,highscore,playername,time,currentCountry));
    }

    private string highscoreURL = "http://gunfitness.herokuapp.com/gethighscore/";

    IEnumerator GetHighscoreFromServer(int level, string country, int roundScore = 0, string playername = "undefined", float time = 0f)
    {
        Debug.Log("GetHighscoreFromServer");
        statusText.text = "Status: Getting highscores from server...";
        string mode = levelDictionary[currentCountry][currentLevel].type;
        WWW www = new WWW(highscoreURL + level.ToString() + "/" + country + "/" + mode);
        yield return www;
        Debug.Log(www.text);
        var json = JsonUtility.FromJson<HighscoreCollection>(www.text);
        highscores = json;
        scoreText.text = "";
        if(roundScore > 0)
        {
            Array.Resize(ref highscores.highscoreentries, highscores.highscoreentries.Length);
            highscores.highscoreentries[highscores.highscoreentries.Length - 1] = new HighscoreEntry(level, roundScore, playername, "undefined",(float) time,currentCountry);
        }
        int count = 0;
        foreach(HighscoreEntry entry in highscores.highscoreentries)
        {
            scoreText.text +=  entry.playername + ": " + string.Format("{0:n0}", entry.highscore)  + "\n";
            if (count > 9)
            {
                yield break;
            }
        }
        statusText.text = "";
    }

    private string saveHighscoreURL = "http://gunfitness.herokuapp.com/sendhighscore";
    public GameObject mainScreenLabel;
    private bool changedCountry;
    private Dictionary<string, LevelModel[]> levelDictionary;
    private string currentDifficulty;
    

    IEnumerator SaveHighscoreToServer(int level, int highscore, string playername, float time, string country)
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
        form.AddField("country", country);
        WWW www = new WWW(saveHighscoreURL,form);
        yield return www;
        if (!string.IsNullOrEmpty(www.error))
        {
            print(www.error);
        }
        else {
            print(www.text);
        }
        StartCoroutine(GetHighscoreFromServer(1,currentCountry,highscore,playername));
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
    public string country;
    public float time;
    public HighscoreEntry(int level, float highscore, string playername, string _id, float time, string country)
    {
        this.level = level;
        this.highscore = highscore;
        this.playername = playername;
        this._id = _id;
        this.time = time;
        this.country = country;
    }
}

[System.Serializable]
public class HighscoreCollection
{
    public HighscoreEntry[] highscoreentries;
}

