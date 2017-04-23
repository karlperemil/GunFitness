using UnityEngine;
using DG.Tweening;
using TMPro;
using System.Collections.Generic;

public class ScoreScreenController : MonoBehaviour {
    public static ScoreScreenController instance;
    private GameObject scoreText;
    private GameObject scoreTitle;
    public TextMeshPro playerName;
    public GameObject keyPrefab;
    private bool firstTime = true;

    // Use this for initialization
    void Start () {
        scoreTitle = GameObject.Find("ScoreTitle");
        scoreText = GameObject.Find("ScoreText");
        CreateKeyboard();
        Hide();
        
    }
    void Awake()
    {
        instance = this;
        Debug.Log("SS Awake");
    }

    public void Show(int score, float endTime)
    {
        scoreText.GetComponent<TextMeshPro>().text = string.Format("{0:n0}", score);
        int count = 0;
        foreach(Transform child in transform)
        {
            child.gameObject.SetActive(true);
            child.transform.localScale = new Vector3();
            child.transform.DOScale(child.GetComponent<UIBase>().initialScale, .4f).SetDelay(count * .05f);
            Debug.Log(child.gameObject.name);
            Debug.Log(child.GetComponent<UIBase>().initialScale);
            count++;
        }
    }

    public void Hide(float t = .5f)
    {
        int count = 0;
        foreach (Transform child in transform)
        {
            child.transform.DOScale(0f, t).OnComplete(() => child.gameObject.SetActive(false) );
            count++;
        }
    }

    public void KeyTriggered(string key)
    {
        Debug.Log("Key triggered " + key);
        if (firstTime)
        {
            playerName.text = key;
            firstTime = false;
        }
        else if (playerName.text.Length < 10)
        {
            playerName.text += key;
        }
    }

    public void EraseTriggered()
    {
        if (playerName.text.Length == 0)
            return;
        playerName.text = playerName.text.Substring(0, playerName.text.Length - 1);
    }

    public void CreateKeyboard()
    {
        string keys = "qwertyuiop";
        string keys2 = "asdfghjkl";
        string keys3 = "zxcvbnm";
        List<UITextButton> keyList = new List<UITextButton>();
        int count = 0;
        foreach (char c in keys)
        {
            Vector3 keyPos = new Vector3((float)(count % 10) * .6f - 2.7f, -1.8f, 0f);
            GameObject key = (GameObject)Instantiate(keyPrefab, transform);
            key.transform.localPosition = keyPos;
            UITextButton keyScript = key.GetComponent<UITextButton>();
            keyScript.text.text = c.ToString();
            keyList.Add(keyScript);
            keyScript.initialScale = new Vector3(.5f, .5f,.01f);
            keyScript.scaleSet = true;
            keyScript.keyName = c.ToString();
            keyScript.transform.localScale = new Vector3();
            count++;
        }
        count = 0;
        foreach (char c in keys2)
        {
            Vector3 keyPos = new Vector3((float)(count % 9) * .6f + .3f - 2.7f, -.6f - 1.8f, 0f);
            GameObject key = (GameObject)Instantiate(keyPrefab, transform);
            key.transform.localPosition = keyPos;
            UITextButton keyScript = key.GetComponent<UITextButton>();
            Debug.Log(keyScript);
            keyScript.text.text = c.ToString();
            keyList.Add(keyScript);
            keyScript.initialScale = new Vector3(.5f, .5f, .01f);
            keyScript.scaleSet = true;
            keyScript.keyName = c.ToString();
            keyScript.transform.localScale = new Vector3();
            count++;
        }
        count = 0;
        foreach (char c in keys3)
        {
            Vector3 keyPos = new Vector3((float)(count % 8) * .6f + .6f - 2.7f, -1.2f - 1.8f, 0f);
            GameObject key = (GameObject)Instantiate(keyPrefab, transform);
            key.transform.localPosition = keyPos;
            UITextButton keyScript = key.GetComponent<UITextButton>();
            keyScript.text.text = c.ToString();
            keyList.Add(keyScript);
            keyScript.initialScale = new Vector3(.5f, .5f, .01f);
            keyScript.scaleSet = true;
            keyScript.keyName = c.ToString();
            keyScript.transform.localScale = new Vector3();
            count++;
        }
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
