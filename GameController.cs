using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using DG.Tweening;

public class GameController : MonoBehaviour {

    public static GameController instance;
    private SteamVR_PlayArea playArea;
    public GameObject targetBox;
    public float timeBetweenBoxes = 5f;

    private int score = 0;
    private int currentLevel = -1;
    private GameObject currentLevelGO;
    private int yellowTargetsKilled;
    private int blueTargetsKilled;
    private int levelBlueTargets;
    private int levelYellowTargets;

    public GameObject[] levels;

    //public GameObject[] levelsFitness;
    // public GameObject[] levelsHead;
    //public GameObject[] levelTutorial;
    //public GameObject[] levelMixed;
    //public GameObject[] levelKinetic;
    //public GameObject[] levelLegDay;
    //public GameObject[] levelBoxing;

    internal float heightOffset = 1.85f;

    public GameObject[] activeLevel;

    private float startTime;
    private float initTime;
    private float endTime;
    private float bestTime = 99999f;
    //public GameObject timeElapsed;
    private bool gameStarted;
    private int shotsFired = 0;
    private float animationOutTime = .25f;
    public GameObject blockWallAnimated;
    private int headTargetsKilled = 0;
    private int levelHeadTargets = 0;
    private float origHeight = 1.85f;

    //private GameObject standCenter;
    private GameObject headHeight;
    private GameObject cameraEye;
    private GameObject levelContainer;
    private GameObject rotationContainer;
    public GameObject forwardHolder;
    public GameObject restartButton;

    public bool debugging = true;

    void Awake()
    {
        instance = this;
    }

    // Use this for initialization
    void Start () {
        playArea = GameObject.Find("[CameraRig]").GetComponent<SteamVR_PlayArea>();
        headHeight = GameObject.Find("HeadHeight");
        cameraEye = GameObject.Find("Camera (eye)");
        timeRemainingGO = GameObject.Find("TimeRemaining");
        levelContainer = new GameObject("LevelContainer");
        rotationContainer = new GameObject("RotationContainer");
        restartButton.GetComponent<UIImageButton>().Hide(0f);
        GameObject.Find("Level Sections").SetActive(false);
        DOTween.Init(false, true, LogBehaviour.ErrorsOnly);
        SaveGame.LoadGame();


        initTime = Time.time;

        StartCoroutine(OpenMenu(2f));
        //gameTimeController = gameTime.GetComponent<GameTimeController>();

        if (!debugging)
        {
            GameObject.Find("Computer").SetActive(false);
            GameObject.Find("Prefabs").SetActive(false);
        }

    }

    IEnumerator OpenMenu(float time)
    {
        yield return new WaitForSeconds(time);
        //menu.SetActive(true);
        //gameTimeController.ShowHighscores();
    }


    // Update is called once per frame
    void Update () {
        if (gameStarted)
        {
            float currentTime = Time.time - initTime;
            //timeElapsed.GetComponent<TextMesh>().text = gameTimeController.FloatToTime(currentTime, "#00:00");
            //playTime += Time.deltaTime;
        }
    }

    public void HitBox(GameObject targetBox)
    {
        ObjectPool.instance.PoolObject(targetBox);
        //ac.PlaySound("glassBreak", 1f, targetBox.transform.position);
        string boxColor = targetBox.tag == "TargetYellow" ? "Controller (left) Yellow" : "Controller (right) Blue";
        GameObject partBoxExplodeClone = ObjectPool.instance.GetObjectForType("BoxExplode", false);
        partBoxExplodeClone.transform.position = targetBox.transform.position;
        partBoxExplodeClone.transform.rotation = targetBox.transform.rotation;
        ParticleSystem partSys = partBoxExplodeClone.GetComponent<ParticleSystem>();
        partSys.startSpeed = 0.1f * 100f;
        partSys.startSize = 1f;
        ParticleSystemRenderer partRend = partSys.GetComponent<ParticleSystemRenderer>();
        if (targetBox.tag == "TargetBlue")
        {
            partRend.material.color = Color.blue;
            partRend.material.EnableKeyword("_EMISSION");
            partRend.material.SetColor("_EmissionColor", Color.blue);
        }
        else
        {
            partRend.material.color = Color.yellow;
            partRend.material.EnableKeyword("_EMISSION");
            partRend.material.SetColor("_EmissionColor", Color.yellow);
        }
        partSys.Clear();
        partSys.Play();
        StartCoroutine(PoolObject(partBoxExplodeClone));
        //ac.PlaySound("glassBreak",1f,targetBox.transform.position);
        ObjectPool.instance.PoolObject(targetBox);

        if (targetBox.tag == "TargetYellow")
            yellowTargetsKilled++;
        if (targetBox.tag == "TargetBlue")
            blueTargetsKilled++;

        TargetKilled();

        score += 100;

        ForwardController.instance.HitBox(targetBox);
    }

    public void PunchBox(GameObject targetBox,float speed)
    {
        Debug.Log(speed);
        string boxColor = targetBox.tag == "BoxYellow" ? "Controller (left) Yellow" : "Controller (right) Blue";
        GameObject partBoxExplodeClone = ObjectPool.instance.GetObjectForType("BoxExplode",false);
        partBoxExplodeClone.transform.position = targetBox.transform.position;
        partBoxExplodeClone.transform.LookAt(GameObject.Find(boxColor).transform.position);
        partBoxExplodeClone.transform.Rotate(new Vector3(0, 180f, 0));
        ParticleSystem partSys = partBoxExplodeClone.GetComponent<ParticleSystem>();
        partSys.startSpeed = (speed * 10f) * (speed * 10f);
        partSys.startSize = .3f;
        ParticleSystemRenderer partRend = partSys.GetComponent<ParticleSystemRenderer>();
        if (targetBox.tag == "BoxBlue")
        {
            partRend.material.color = Color.blue;
            partRend.material.EnableKeyword("_EMISSION");
            partRend.material.SetColor("_EmissionColor", Color.blue);
        }
        else
        {
            partRend.material.color = Color.yellow;
            partRend.material.EnableKeyword("_EMISSION");
            partRend.material.SetColor("_EmissionColor", Color.yellow);
        }
        partSys.Clear();
        partSys.Play();
        StartCoroutine(PoolObject(partBoxExplodeClone));
        //ac.PlaySound("glassBreak",1f,targetBox.transform.position);
        ObjectPool.instance.PoolObject(targetBox);
        if (targetBox.tag == "BoxYellow")
            yellowTargetsKilled++;
        if (targetBox.tag == "BoxBlue")
            blueTargetsKilled++;
        TargetKilled();

        score += 100;

        ForwardController.instance.PunchBox(targetBox);

    }

    IEnumerator PoolObject(GameObject go)
    {
        yield return new WaitForSeconds(2f);
        Debug.Log("Pooled BoxExplode");
        ObjectPool.instance.PoolObject(go);
    }

    public void HeadHitBox(GameObject headHitBox){
        ObjectPool.instance.PoolObject(headHitBox);
        headTargetsKilled++;
        TargetKilled();
        ForwardController.instance.HeadHitBox(headHitBox);
    }

    private void TargetKilled(){
        /*
        if (blueTargetsKilled >= levelBlueTargets && yellowTargetsKilled >= levelYellowTargets && headTargetsKilled >= levelHeadTargets)
        {
            if(gameMode != "infinite" || gameMode != "forward")
            {
                NextLevel();
            }
        }*/
    }

    private void perfectRound()
    {
        //ac.PlaySound("perfectRound");
        //perfectShotText.SetActive(true);
        //perfectShotText.transform.localScale = perfectShotStartScale;
        //perfectShotText.GetComponent<AnimateScript>().AnimateIn(.5f,0f);
        StartCoroutine(ExecuteAfterTime(1.5f));
    }

    IEnumerator ExecuteAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        //perfectShotText.GetComponent<AnimateScript>().AnimateOut(.5f,0f);
    }

    public void ShotFired()
    {
        shotsFired++;
    }

    public void StartGame()
    {
        Debug.Log("Start Game!");

        LevelScreenController.instance.Hide();

        if (Vector3.Distance(headHeight.transform.position, cameraEye.transform.position) > 0.5f)
        {
            ShowMoveCenter();
            //return;
        }

        float currentHeight = cameraEye.transform.position.y + 0.15f;

        //gameTimeController.Hide();
        //menuController.menuLine.SetActive(false);

        heightOffset = origHeight - currentHeight;
        heightOffset = Mathf.Min(.25f, heightOffset);
        heightOffset = Mathf.Max(-.35f, heightOffset);

        heightPercent = Math.Min(currentHeight - 1.5f, .5f);
        heightPercent = Math.Max(heightPercent, 0f);
        heightPercent = heightPercent / .5f;

        /*
        switch (menuController.level)
        {
            case "menu-level-tutorial":
                activeLevel = levelTutorial;
                break;
            case "menu-level-1":
                activeLevel = levelMixed;
                break;
            case "menu-level-2":
                activeLevel = levelKinetic;
                break;
            case "menu-level-3":
                activeLevel = levelLegDay;
                break;
            case "menu-level-4":
                activeLevel = levelBoxing;
                break;
            default:

                break;
        }

        menu.SetActive(false);

        countDown3.SetActive(true);
        StartCoroutine(Show2());
        StartCoroutine(Show1());
        StartCoroutine(Hide1());
        ac.PlaySound("countdown");

        */

        TriggerGameStart();
    }

    private void ShowMoveCenter()
    {
        //standCenter.SetActive(true);
        StartCoroutine(HideStandCenter());
    }

    IEnumerator HideStandCenter()
    {
        yield return new WaitForSeconds(2.5f);
        //standCenter.SetActive(false);
    }

    public void TriggerGameStart()
    {
        gameStarted = true;
        if(gameMode == "forward")
        {
            ForwardController.instance.TriggerGameStart();
        }
        restartButton.GetComponent<UIImageButton>().Show();
        LevelScreenController.instance.Hide();
        NextLevel();
        Debug.Log("Start Game function");
        startTime = Time.time;
    }

    public void NextLevel()
    {
        if (currentLevel == -1)
        {
            CreateNewLevel();
        }
        else {
            AnimateLevelOut();
        }
    }

    private void CreateNewLevel()
    {
        CleanOldLevel();
        if(gameMode == "forward")
        {
            ForwardController.instance.NextLevel();
        }
        else
        {
            CreateNewDesignedLevel();
        }
    }

    private GameObject timeRemainingGO;
    void InfiniteCountdown()
    {
        timeSurvived += 1;
        timeRemaining -= 1;
        timeRemainingGO.GetComponent<TextMesh>().text = timeRemaining + "\n<size=15>SECONDS REMAINING</size>";
        if (timeRemaining <= 0)
        {
            GameOver();
        }
    }

    public List<GameObject> infiniteLevels;
    private string gameMode = "forward";
    private int timeRemaining;
    private int timeSurvived = 0;
    private float levDirectionY = 0f;
    private float levDirectionX = 0f;
    private List<String> levelOrder = new List<String>(new String[] { "BoxHexagonBlue","BoxHexagonYellow","TargetBlue","TargetYellow" });

    private void CreateNewDesignedLevel() {
        currentLevel++;
        if (currentLevel >= activeLevel.Length)
        {
            //currentLevel = 0;
            GameOver();
            return;
        }
        currentLevelGO = activeLevel[currentLevel];
        SetLevelPositions();
    }

    private void SetLevelPositions() {
        bool wallExsists = false;

        foreach (Transform child in currentLevelGO.transform)
        {
            GameObject childGo = child.gameObject;
            if (childGo.name == "Guard")
            {
                var blockWallAnimateScript = blockWallAnimated.GetComponent<AnimateScript>();
                blockWallAnimateScript.AnimateToPosition(.5f, childGo.transform);
                wallExsists = true;
                continue;
            }
 
            GameObject pooled = ObjectPool.instance.GetObjectForType(childGo.name, true);
            Debug.Log("ChildGo=" + childGo.name);
            pooled.transform.position = childGo.transform.position;
            pooled.transform.rotation = childGo.transform.rotation;
            pooled.transform.parent = levelContainer.transform;
            string t = childGo.tag;
            string n = childGo.name;
            if(t == "TargetYellow" || t == "TargetBlue")
            {
                pooled.GetComponent<TargetScript>().ResetPositions();
            }
            if (t == "TargetYellow" || t == "BoxYellow")
            {
                levelYellowTargets++;
            }
            if (t == "TargetBlue" || t == "BoxBlue")
            {
                levelBlueTargets++;
            }
            if(t == "BoxBlue" || t == "BoxYellow")
            {
                pooled.transform.Translate(Vector3.up * -heightOffset);
            }

            if(t == "HeadBox")
            {
                levelHeadTargets++;
            }

            if(t == "TargetYellow" || t == "TargetBlue" || t == "BoxYellow" || t == "BoxBlue") {
                if (currentLevelGO.name.IndexOf("Rotating") == -1)
                {
                    childGo.transform.LookAt(GameObject.Find("HeadHeight").transform);
                    childGo.transform.rotation *= Quaternion.Euler(0, 0, 0);
                }
            }

        }

        levelContainer.transform.position = currentLevelGO.transform.position;
        if (gameMode == "infinite")
        {
            levelContainer.transform.rotation *= Quaternion.Euler(0f, levDirectionY, 0f);
        }

        if (!wallExsists)
        {
            blockWallAnimated.GetComponent<AnimateScript>().AnimateOrigin(.5f);
        }

       AnimateLevelIn();
    }

    private void AnimateLevelOut()
    {
        
        foreach(Transform child in levelContainer.transform)
        {
            bool animate = true;
            GameObject GOToAnimate = child.transform.gameObject;
            if (animate)
            {
                //AnimateScript s = GOToAnimate.AddComponent<AnimateScript>();
                //s.AnimateOut(animationOutTime, 0f);
            }
        }

        StartCoroutine(OnLevelOut());
    }

    IEnumerator OnLevelOut()
    {
        yield return new WaitForSeconds(animationOutTime);
        CleanOldLevel();
        CreateNewLevel();
    }

    private void AnimateLevelIn()
    {
        foreach (Transform child in levelContainer.transform)
        {
            GameObject GOToAnimate = child.transform.gameObject;
            GOToAnimate.GetComponent<TargetScript>().OnShow(new Vector3());
        }
    }

    public void GameOver()
    {
        gameStarted = false;
        CancelInvoke();
        Debug.Log("Game Over! " + score);
        CleanOldLevel();
        ForwardController.instance.GameOver();
        currentLevel = -1;
        //timeRemainingGO.SetActive(false);
        endTime = Time.time - startTime;
        restartButton.GetComponent<UIImageButton>().Hide();
        ScoreScreenController.instance.Show(score, endTime);
        blockWallAnimated.GetComponent<AnimateScript>().AnimateOrigin(1f);
        PoolLevel();
    }

    public string playername = "undefined";

    public float heightPercent { get; private set; }

    public void ScoreDone()
    {
        ScoreScreenController.instance.Hide();
        LevelScreenController.instance.SaveHighscore(1, score, playername, endTime);
        SaveGame.SaveCurrentGame(new Save(endTime, currentLevel.ToString(), playername, score));
        SaveGame.gameState.totalScore += score;
        SaveGame.gameState.totalPlaytime += endTime;
        SaveGame.gameState.money += score;
        SaveGame.gameState.shotsFired += shotsFired;
        SaveGame.SaveProgress();
    }

    void PoolLevel()
    {
        foreach(Transform child in levelContainer.transform)
        {
            ObjectPool.instance.PoolObject(child.gameObject);
        }
    }

    public void CleanOldLevel()
    {
        Debug.Log("Clean old level function");
        levelYellowTargets = 0;
        levelBlueTargets = 0;
        blueTargetsKilled = 0;
        yellowTargetsKilled = 0;
        shotsFired = 0;
        if (currentLevel != -1)
            PoolLevel();
    }

}
