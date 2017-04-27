using UnityEngine;
using System.Collections;

public class LevelModel {
    public int levelNumber = 1;
    public string levelCountry = "Tokyo";
    public string levelName;
    public string type = "Track";
    public float goalTime = 60f;
    public float goalScore = 30000f;
    private bool unlocked = false;
	// Use this for initialization
	void Start () {
	
	}

    public LevelModel(int levelNumber, string levelCountry, string levelName, string type, float goalTime, float goalScore)
    {
        this.levelNumber = levelNumber;
        this.levelCountry = levelCountry;
        this.levelName = levelName;
        this.type = type;
        this.goalTime = goalTime;
        this.goalScore = goalScore;
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
