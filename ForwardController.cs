using UnityEngine;
using System.Collections.Generic;

public class ForwardController : MonoBehaviour {

    public float gameSpeed = 1f;
    public float gameSpeedDynamic;
    public float offset = 0f;
    private bool gameStarted = false;
    private int currentLevel = 0;
    public GameObject[] levels;
    private List<GameObject> currentObstacles;
    private int lives = 0;
    private GameObject latestLevelEnd;
    private HeadController headController;
    public static ForwardController instance;

    void Awake()
    {
        instance = this;
    }

    // Use this for initialization
    void Start () {
        currentObstacles = new List<GameObject>();
        gameSpeedDynamic = gameSpeed;
        headController = GameObject.Find("HeadHitbox").GetComponent<HeadController>();
    }
	
	// Update is called once per frame
	void Update () {
        if (gameStarted)
        {
            transform.Translate(-Vector3.forward * Time.deltaTime * gameSpeedDynamic);
        }

        gameSpeedDynamic += Time.deltaTime * 0.02f;
    }


    public void HitBox(GameObject targetBox)
    {
    }

    public void PunchBox(GameObject targetBox) {
    }

    public void HeadHitBox(GameObject headHitBox)
    {
        lives -= 1;
        Debug.Log("Lives are: " + lives);
        if (lives == -1)
        {
            GameController.instance.GameOver();
        }
        else {

            headController.DisplayLivesRemaining(lives);
        }
    }

    public void TriggerGameStart()
    {
        gameStarted = true;
        gameSpeedDynamic = gameSpeed;
    }

    public void GameOver()
    {
        gameStarted = false;
        Debug.Log("Game over");
        currentLevel = 0;
        lives = 0;
        foreach (Transform child in transform)
        {
            ObjectPool.instance.PoolObject(child.gameObject);
        }
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

    }

    public void NextLevel()
    {
        Debug.Log("Next Level" + currentLevel);
        currentLevel++;
        int randomLevel = (int) Mathf.Round(Random.Range(0f, levels.Length-1));
        GameObject level = levels[randomLevel];

        if(currentLevel > 1)
        {
            offset = latestLevelEnd.transform.position.z;
        }
        else
        {
            offset = 25f;
        }

        foreach (Transform child in level.transform)
        {
            GameObject childGo = child.gameObject;
            GameObject pooled = ObjectPool.instance.GetObjectForType(childGo.tag, false);         
            pooled.transform.position = childGo.transform.position;
            pooled.transform.position += new Vector3(0f, -6.223f, 0f + offset);
            if (currentLevel > 0)
            {
                
            }
            pooled.transform.rotation = childGo.transform.rotation;
            pooled.transform.parent = this.transform;

            string t = childGo.tag;
            string n = childGo.name;
            if (t == "BoxBlue" || t == "BoxYellow")
            {
                //base height is 1.85 we move this by a percentage so low balls remaing almost the same and high balls are much lower than normal
                //pooled.transform.Translate(Vector3.up * (GameController.instance.heightPercent) );
            }
            if (n == "LevelEnd")
            {
                pooled.GetComponent<Renderer>().enabled = false;
                latestLevelEnd = pooled;
            }
            else if(t == "TriggerDoorHolder")
            {
                pooled.GetComponentInChildren<TriggerDoorButton>().ResetDoor();
            }
            else {
                TargetScript targetScript = childGo.GetComponent<TargetScript>();
                TargetScript pooledTargetScript = pooled.GetComponent<TargetScript>();
                if (targetScript.isMoving)
                {
                    pooledTargetScript.SetOrigPos();
                    pooledTargetScript.direction = targetScript.direction;
                    pooledTargetScript.distance = targetScript.distance;
                    pooledTargetScript.speed = targetScript.speed;
                    pooledTargetScript.offsetSeconds = targetScript.offsetSeconds;
                    pooledTargetScript.isMoving = true;
                }
                pooledTargetScript.OnShow(childGo.transform.localScale);
            }
        }

        if(currentLevel < 2)
        {
            NextLevel();
        }
    }
}
