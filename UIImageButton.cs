using UnityEngine;
using System.Collections;
using TMPro;
using System;

public class UIImageButton : UIBase
{

    public Texture hoverTexture;
    private Renderer rend;
    private Texture startTexture;

    void Awake()
    {
        keyType = "button";
    }

    // Use this for initialization
    void Start()
    {
        rend = GetComponent<Renderer>();
        startTexture = rend.material.mainTexture;
        base.Init();
    }

    public override void OnHover()
    {
        base.OnHover();
        rend.material.mainTexture = hoverTexture;
    }

    public override void UnHover()
    {
        base.UnHover();
        rend.material.mainTexture = startTexture;
    }

    public override void OnTrigger()
    {
        UIController.instance.UITriggered(keyType, keyName);
        base.OnTrigger();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
