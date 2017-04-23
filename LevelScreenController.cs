using UnityEngine;
using System.Collections;
using TMPro;
using System;
using System.Text;
using DG.Tweening;

public class LevelScreenController : MonoBehaviour {

    public TextMeshPro scoreText;
    public HighscoreCollection highscores;
    public TextMeshPro statusText;
    public static LevelScreenController instance;
    private Vector3 initialScale;

    void Awake()
    {
        instance = this;
        initialScale = this.transform.localScale;
    }

	// Use this for initialization
	void Start () {
        StartCoroutine(GetHighscoreFromServer(1));
	}
	
	// Update is called once per frame
	void Update () {
	
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
            scoreText.text += "<color=blue>" + entry.playername + ":</color> <color=yellow>" + string.Format("{0:n0}", entry.highscore)  + "</color>\n";
            if (count > 5)
            {
                yield break;
            }
        }
        statusText.text = "";
    }

    private string saveHighscoreURL = "http://gunfitness.herokuapp.com/sendhighscore";
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

