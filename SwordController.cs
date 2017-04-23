using UnityEngine;
using System.Collections;
using DG.Tweening;

public class SwordController : MonoBehaviour {

    public string swordColor = "yellow";
    private GameController gc;
    private Vector3 lastFramePos = new Vector3();
    private Vector3 thirdFramePos = new Vector3();
    private float lastFrameSpeed;
    private Vector3 posNow;

    // Use this for initialization
    void Start () {
        gc = GameObject.Find("[CameraRig]").GetComponent<GameController>();
	}
	
	// Update is called once per frame
	void Update () {
        posNow = transform.position;
        lastFrameSpeed = Vector3.Distance(posNow, lastFramePos);
        thirdFramePos = lastFramePos;
        lastFramePos = posNow;
	
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

    public void OnShow()
    {
        this.transform.localPosition = new Vector3(0f, 0f, -0.0986f);
        this.transform.DOLocalMove(new Vector3(0f, -0.1172f, -0.0195f), 0.1f).From();
        this.transform.localScale = new Vector3();
        this.transform.DOScale(1f, 0.1f);
    }
    public void OnHide()
    {
        this.transform.DOLocalMove(new Vector3(0f, -0.1172f, -0.0195f), 0.1f);
        this.transform.DOScale(0f, 0.1f).OnComplete(onHideComplete);
    }

    void onHideComplete()
    {
        this.gameObject.SetActive(false);
    }

    void HittingSomething(Collider collider)
    {
        Debug.Log("Hitting something yo");
        if(lastFrameSpeed > 0.01f) {
            GameObject collideGO = collider.gameObject;
            if (collideGO.tag == "BoxYellow" && swordColor == "yellow")
            {
                gc.PunchBox(collideGO, lastFrameSpeed);
                StartCoroutine(GameObject.Find("Controller (left) Yellow").GetComponent<GunController>().GunVibrationContinous(Time.time + .15f));
            }
            else if (collideGO.tag == "BoxBlue" && swordColor == "blue")
            {
                gc.PunchBox(collideGO, lastFrameSpeed);
                StartCoroutine(GameObject.Find("Controller (right) Blue").GetComponent<GunController>().GunVibrationContinous(Time.time + .15f));
            }
            else if (collideGO.tag == "BoxBlue" || collideGO.tag == "BoxYellow")
            {
                Debug.Log("Hit a sphere");
               // collideGO.AddComponent<TargetScript>().Punch(transform.position);
               // Destroy(collider.gameObject);
                //todo: play some sound
            }
        }
    }
}
