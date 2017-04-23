using UnityEngine;
using System.Collections;
using DG.Tweening;
using System;

public class UIBase : MonoBehaviour {
    public Vector3 initialScale;
    internal bool scaleSet = false;
    internal int frameHover = 0;

    void Awake()
    {
        this.initialScale = this.transform.localScale;
    }

    // Use this for initialization
    void Start () {
        Init();
	}

    public void Init()
    {
        UIController.instance.RegisterComponent(this.gameObject);
    }

    public void Show(float t = .3f)
    {
        this.transform.localScale = new Vector3();
        this.transform.DOScale(initialScale, t);
    }

    public void Hide(float t = .3f)
    {
        this.transform.DOScale(0f, t);
    }

    public virtual void OnHover()
    {
        frameHover = Time.frameCount;
    }

    public virtual void UnHover()
    {
        if(frameHover == Time.frameCount)
        {
            return;
        }
    }

    public virtual void OnTrigger()
    {
        Debug.Log("OnTrigger in " + this.name);
    }

    public void HideImmediate()
    {
        this.transform.localScale = new Vector3();
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
