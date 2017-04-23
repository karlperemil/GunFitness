using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using DG.Tweening;

public class Intro : MonoBehaviour {
    private GameObject trackedHeadPos;
    private GameObject logo;
    private GameObject headset;
    public List<GameObject> slides;

    // Use this for initialization
    void Start () {
        headset = GameObject.Find("Camera (eye)");
        trackedHeadPos = GameObject.Find("TrackedHeadPos");
        logo = GameObject.Find("IntroLogo");
        int count = 0;
        foreach (GameObject go in slides)
        {
            go.transform.localScale = new Vector3();
            go.transform.DOScale(3f, .5f).SetDelay(count * 4f);
            go.transform.DOScale(3.5f, 3f).SetDelay(count * 4f + .5f);
            go.transform.DOScale(0f, .5f).SetDelay((count+1) * 4f - .5f);
            count++;
        }

        //StartCoroutine(ExecuteAfterTime(count *4f + 5f));
    }


    IEnumerator ExecuteAfterTime(float time)
    {
        yield return new WaitForSeconds(time);

        // Code to execute after the delay
        Debug.Log("Pre load scene");
        SceneManager.LoadScene("reboot1");
    }

    // Update is called once per frame
    void Update () {
        foreach(GameObject slide in slides)
        {
            slide.transform.position = Vector3.Lerp(slide.transform.position, trackedHeadPos.transform.position,1f * Time.deltaTime);
            slide.transform.LookAt(headset.transform.position);
        }
	}
}
