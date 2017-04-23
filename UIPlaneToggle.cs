using UnityEngine;
using System.Collections;

public class UIPlaneToggle : MonoBehaviour {

    public string state = "off";
    public bool toggle = true;
    public Texture textureOn;
    public Texture textureOff;
    public bool hovering = false;
    public Renderer rend;
    private Vector3 origPos;
    private Vector3 hoverPos;

    // Use this for initialization
    void Awake () {
        rend = this.GetComponent<Renderer>();
        rend.material.color = Color.white;
        Debug.Log("UIPlane");

        origPos = transform.position;
        hoverPos = origPos;
        hoverPos = hoverPos + (transform.up * .05f);

        if (state == "off") {
            SetOff();
        }
        else
        {
            SetOn();
        }
    }

    public void SetOff(){
        state = "off";
        rend.material.mainTexture = textureOff;
    }

    public void SetOn(){
        state = "on";
        rend.material.mainTexture = textureOn;
    }

    public void ToggleState(){
        if(state == "on"){
            SetOff();
        }
        else if(state == "off"){
            SetOn();
        }
    }

     
    void Update()
    {
        if (hovering)
        {
            rend.material.color = Color.yellow;
            //rend.material.mainTexture = textureOn;
            transform.position = hoverPos;
        }
        else {
            if (state == "off")
            {
                rend.material.color = Color.white;
                transform.position = origPos;
            }
            //rend.material.mainTexture = textureOff;
        }
    }
}
