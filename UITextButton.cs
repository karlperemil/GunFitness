using UnityEngine;
using System.Collections;
using TMPro;
using System;

public class UITextButton : UIBase {

    public TextMeshPro text;
    public Color32 color;
    public Color32 hoverColor;

    void Awake()
    {
        keyType = "button";
    }

	// Use this for initialization
	void Start () {
        text = GetComponentInChildren<TextMeshPro>();
        text.color = color;
        base.Init();
	}

    public override void OnHover() {
        base.OnHover();
        text.color = hoverColor;
    }

    public override void UnHover()
    {
        base.UnHover();
        text.color = color;
    }

    public override void OnTrigger() {
        UIController.instance.UITriggered(keyType, keyName);
        base.OnTrigger();
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
