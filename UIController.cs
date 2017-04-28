using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UIController : MonoBehaviour {
    public static UIController instance;
    public List<UIBase> UIComponents = new List<UIBase>();
    private List<Button3D> mapButtons;

    // Use this for initialization
    void Start () {
	}
    void Awake()
    {
        instance = this;
        mapButtons = new List<Button3D>();
    }

    void Update()
    {
        UnHover();
    }

    public void RegisterComponent(GameObject go)
    {
        UIBase ui = go.GetComponent<UIBase>();

        UIComponents.Add(ui);

        if (ui.keyType == "map")
            mapButtons.Add(go.GetComponent<Button3D>());
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
        Debug.Log("UITriggered " + keyType +" "+ keyText);
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
        if (keyText == "map_usa")
            LevelScreenController.instance.ShowCountry("usa");
        if (keyText == "map_china")
            LevelScreenController.instance.ShowCountry("china");



        if (keyType == "map")
        {
            foreach(Button3D button in mapButtons)
            {
                if(button.keyName != keyText)
                {
                    button.selected = false;
                }
            }
        }

        if (keyText == "main_nextlevel")
            LevelScreenController.instance.IncreaseLevel();
        if (keyText == "main_prevlevel")
            LevelScreenController.instance.DecreaseLevel();
    }
}
