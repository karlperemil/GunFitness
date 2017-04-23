using UnityEngine;
using System.Collections;
using System;

public class Menu : MonoBehaviour  {

    public bool menuActive = false;

    public string level = "menu-level-tutorial";
    public bool sound = true;
    public bool music = true;

    public GameObject[] menuItems;
    public GameObject[] UIItems;

    public GameObject menuDecal;

    public GameObject muzzle;
    private GameObject menuPointer;
    private AnimateScript animateScript;
    private GameController gc;
    private AudioController ac;
    private GameObject lineRender;
    public GameObject menuLine;

    public GameTimeController gameTimeController;

    void Start () {
        //menuPointer = GameObject.Find("Menu-Pointer");
        gc = GameObject.Find("[CameraRig]").GetComponent<GameController>();
        ac = GameObject.Find("[CameraRig]").GetComponent<AudioController>();
        animateScript = gameObject.AddComponent<AnimateScript>();
        Debug.Log("Start of menu");
        CreateMenuLine();

        gameTimeController = GameObject.Find("GameTime").GetComponent<GameTimeController>();

        menuDecal.SetActive(false);

        Debug.Log(level);
        setCorrectLevel(level);

    }

    private void CreateMenuLine()
    {
        lineRender = GameObject.Find("LineRenderPrefabYellow");
        Vector3 muzzlePos = muzzle.transform.position;
        var line = muzzle.transform.position + (transform.forward * 100f);
        var rotatedLine = Quaternion.AngleAxis(55f, transform.right) * line;
        menuLine = Instantiate(lineRender) as GameObject;
        menuLine.GetComponent<LineRenderer>().SetPositions(new Vector3[] { muzzlePos, rotatedLine });
        menuLine.GetComponent<LineRenderer>().enabled = true;
        menuLine.SetActive(false);
    }

    public void setMenuActive(){
        menuActive = true;
        //menuPointer.SetActive(true);
        //animateIn();
        setCorrectLevel(level);

    }

    public void setMenuInactive(){
        menuActive = false;
        //menuPointer.SetActive(false);
        //animateOut();
    }

    private void setCorrectLevel(string menuItemName)
    {
        foreach (GameObject menuItem in menuItems)
        {
            if (menuItem.name.IndexOf("menu-level") >= 0 && menuItem.name.IndexOf(menuItemName) == -1)
            {
                menuItem.GetComponent<UIPlaneToggle>().SetOff();
            }
            if (menuItem.name.IndexOf("menu-level") >= 0 && menuItem.name.IndexOf(menuItemName) >= 0)
            {
                menuItem.GetComponent<UIPlaneToggle>().SetOn();
                Debug.Log(menuItemName);
            }
        }
        gameTimeController.ShowTimeForLevel(menuItemName);
    }

    public void HitMenuItem(string menuItemName){
        Debug.Log(menuItemName);
        if(!menuActive) return;
        foreach(GameObject menuGO in menuItems){
            Debug.Log(menuGO);
            if(menuItemName == menuGO.name){
                menuGO.GetComponent<UIPlaneToggle>().ToggleState();
                ac.SoundEvent("menu-item-hover");
                if (menuItemName == "menu-start")
                    menuGO.GetComponent<UIPlaneToggle>().SetOff();
            }
        }

        Debug.Log(menuItemName.IndexOf("menu-start"));

        if(menuItemName.IndexOf("menu-level") >= 0){
            level = menuItemName;
            setCorrectLevel(menuItemName);
        }
        if(menuItemName.IndexOf("menu-start") >= 0)
        {
            gc.StartGame();
        }
        if(menuItemName.IndexOf("menu-gun") >= 0){
            //gun = menuItemName;
        }
        if(menuItemName.IndexOf("menu-mode-survival") >= 0){
            //mode = menuItemName;
            GameObject.Find("menu-mode-time-trial").GetComponent<UIPlaneToggle>().SetOff();
        }
        if(menuItemName.IndexOf("menu-mode-time-trial") >= 0){
            //mode = menuItemName;
            GameObject.Find("menu-mode-survival").GetComponent<UIPlaneToggle>().SetOff();
        }
        if(menuItemName.IndexOf("menu-sound-on") >= 0){
            sound = true;
        }
        if(menuItemName.IndexOf("menu-sound-off") >=0){
            sound = false;
        }
        if(menuItemName.IndexOf("menu-music-on") >= 0){
            music = true;
        }
        if(menuItemName.IndexOf("menu-music-off") >= 0){
            music = false;
        }
        if(menuItemName.IndexOf("menu-close") >= 0){
            setMenuInactive();
        }
    }

    public void HitUIItem(string uiItemName){
        foreach(GameObject uiGO in UIItems){
            if(uiItemName == uiGO.name){
                uiGO.GetComponent<UIPlaneToggle>().ToggleState();
                ac.SoundEvent(uiItemName);
            }
        }

        if(uiItemName.IndexOf("ui-game-start") >= 0){
            gc.StartGame();
        }
        if(uiItemName.IndexOf("ui-game-restart") >= 0){
            gc.GameOver();
        }
        if(uiItemName.IndexOf("ui-game-options") >= 0){
            setMenuActive();
        }
    }

    void Update(){
        RaycastHit hit;
        if (Physics.Raycast(muzzle.transform.position, muzzle.transform.forward, out hit)){
            GameObject go = hit.transform.gameObject;
            if(menuActive){
                if (hit.transform && go.name.IndexOf("menu") >= 0){
                    menuDecal.transform.position = hit.point;
                    UnHoverAllOthers(go);
                    UpdateMenuLine(hit);
                    go.GetComponent<UIPlaneToggle>().hovering = true;
                    if(GameObject.Find("Controller (right)").GetComponent<GunController>().triggerDown)
                        HitMenuItem(go.name);
                }
                else
                {
                    //not hovering over a menu item so hiding decal
                    menuDecal.transform.position = new Vector3(0f, -100f, 0);
                    //not hovering over menu item so unhover all
                    UnHoverAllOthers();
                    UpdateMenuLine();
                }   
            }
            else {
                if(hit.transform && go.name.IndexOf("ui-game") >= 0){
                    go.GetComponent<UIPlaneToggle>().hovering = true;
                    HitUIItem(go.name);
                    ac.SoundEvent("ui-item-hover");
                }
            }

        }
        else
        {
            //not hovering over a menu item so hiding decal
            menuDecal.transform.position = new Vector3(0f,-100f,0);

            //not hovering over menu item so unhover all
            UnHoverAllOthers();

            //not hovering over anything so hide menu line
            UpdateMenuLine();
        }

    }
    private void UpdateMenuLine()
    {
        menuLine.SetActive(false);
    }
    private void UpdateMenuLine(RaycastHit hit)
    {
        menuLine.SetActive(true);
        menuLine.GetComponent<LineRenderer>().SetPositions(new Vector3[] { muzzle.transform.position, hit.point });
    }
    private void UnHoverAllOthers()
    {
        foreach (GameObject menuItem in menuItems)
        {
            menuItem.GetComponent<UIPlaneToggle>().hovering = false;
        }
    }
    private void UnHoverAllOthers(GameObject go)
    {
        foreach(GameObject menuItem in menuItems)
        {
            if (menuItem != go)
            {
                menuItem.GetComponent<UIPlaneToggle>().hovering = false;
            }
        }
    }

    void animateIn(){
        Debug.Log("AnimateIn");
        animateScript.AnimateIn(.5f,0f);
    }

    void animateOut(){
        animateScript.AnimateOut(.5f,0f);
    }
}
