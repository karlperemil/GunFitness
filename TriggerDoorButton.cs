using UnityEngine;
using System.Collections;

public class TriggerDoorButton : MonoBehaviour {

    public GameObject triggerDoor;
    public bool hasOpened = false;

    // Use this for initialization
    void Start () {
	
	}

    public void HitDoor()
    {
        Debug.Log("Opening Door Trigger!");
        if (!hasOpened){ 
            triggerDoor.GetComponent<TriggerDoor>().OpenDoor();
            hasOpened = true;
        }
    }

    public void ResetDoor()
    {
        hasOpened = false;
        triggerDoor.GetComponent<TriggerDoor>().ResetDoor();
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
