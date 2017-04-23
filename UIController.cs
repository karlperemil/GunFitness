using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UIController : MonoBehaviour {
    public static UIController instance;
    public List<UIBase> UIComponents = new List<UIBase>();

    // Use this for initialization
    void Start () {
	}
    void Awake()
    {
        instance = this;
    }

    void Update()
    {
        UnHover();
    }

    public void RegisterComponent(GameObject go)
    {
        UIComponents.Add(go.GetComponent<UIBase>());
    }

    public void UnHover()
    {
        foreach(UIBase key in UIComponents)
        {
            key.UnHover();
        }
    }

    public void UITriggered(string keyType, string keyText)
    {
        if(keyType == "keyboard")
        {
            ScoreScreenController.instance.KeyTriggered(keyText);
        }
        if (keyType == "button" && keyText == "erase")
        {
            ScoreScreenController.instance.EraseTriggered();
        }
        if(keyType == "button" && keyText == "OK")
        {
            GameController.instance.ScoreDone();
        }
        if(keyType == "button" && keyText == "start")
        {
            GameController.instance.StartGame();
        }
        if (keyType == "button" && keyText == "restart")
        {
            GameController.instance.GameOver();
        }
    }
}
