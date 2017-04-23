using UnityEngine;
using System.Collections;
using System;
using DG.Tweening;
using TMPro;

public class HeadController : MonoBehaviour {

    private GameController gc;
    public GameObject livesRemainingText;

    // Use this for initialization
    void Start () {
        gc = GameObject.Find("[CameraRig]").GetComponent<GameController>();
        livesRemainingText.transform.localScale = new Vector3(0f, 0f, 0f);
    }

    void OnTriggerEnter(Collider collider)
    {
        HittingSomething(collider);
    }

    void OnTriggerStay(Collider collider)
    {
        //HittingSomething(collider);
    }

    void OnTriggerExit(Collider collider)
    {
        //HittingSomething(collider);
    }

    void HittingSomething(Collider collider)
    {
        Debug.Log("Head hit someting");
        GameObject collideGO = collider.gameObject;
        if (collideGO.tag == "HeadBox"){
            Debug.Log("Head hit HeadBox");
            gc.HeadHitBox(collideGO);
        }
        else if(collideGO.tag == "TriggerDoor")
        {
            gc.HeadHitBox(collideGO);
        }
    }

    public void DisplayLivesRemaining(int lives)
    {
        Debug.Log("DisplayLivesRemaining " + lives);
        livesRemainingText.GetComponent<TextMeshPro>().text = lives + "\n❤";
        livesRemainingText.transform.DOScale(.11f, .3f);
        livesRemainingText.transform.DOScale(0f, .3f).SetDelay(2f);
    }
}
