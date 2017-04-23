using UnityEngine;
using System.Collections;
using DG.Tweening;

public class TriggerDoor : MonoBehaviour {
    private Vector3 origPos;

    // Use this for initialization
    void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void Awake()
    {
        origPos = this.transform.localPosition;
    }


    public void OpenDoor()
    {
        
        float newYpos = transform.localPosition.y - transform.localScale.y;
        Debug.Log("Open Door! Origpos: " + origPos + " newYpos: " + newYpos);
        this.transform.DOLocalMoveY(newYpos, .5f);
        
    }

    public void ResetDoor()
    {
        Debug.Log("ResetDoor: " + transform.localPosition +" origPos:"+origPos);
        this.transform.localPosition = origPos;
    }
}
