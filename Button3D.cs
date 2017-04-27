using UnityEngine;
using System.Collections;
using DG.Tweening;

public class Button3D : UIBase {

    public GameObject model;
    private Vector3 idlePos;
    private Vector3 hoverPos;
    private Renderer rend;
    public bool selected = false;
    private Color hoverColor = new Color(0, 0.8f, .9f);
	// Use this for initialization
    void Awake()
    {
        keyType = "map";
        idlePos = model.transform.localPosition;
        hoverPos = idlePos + new Vector3(0f, 0f, -.3f);
        rend = model.GetComponent<Renderer>();
        //rend.material.color = Color.red;
        //foreach(Transform child in model.transform)
        //{
        //    child.gameObject.GetComponent<Renderer>().material.color = Color.red;
        //}
    }
	void Start () {
        base.Init();
    }

    public override void OnHover()
    {
        base.OnHover();
        model.transform.DOLocalMove(hoverPos,.3f);
        rend.material.color = hoverColor;
        foreach (Transform child in model.transform)
        {
            child.gameObject.GetComponent<Renderer>().material.color = hoverColor;
        }
    }

    public override void UnHover()
    {
        base.UnHover();
        if (selected)
            return;
        model.transform.DOLocalMove(idlePos, .3f);
        rend.material.color = Color.gray;
        foreach (Transform child in model.transform)
        {
            child.gameObject.GetComponent<Renderer>().material.color = Color.gray;
        }
    }

    public override void OnTrigger()
    {
        UIController.instance.UITriggered(keyType, keyName);
        base.OnTrigger();
        selected = true;
    }

    // Update is called once per frame
    void Update () {
	
	}
}
