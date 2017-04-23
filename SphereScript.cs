using UnityEngine;
using System.Collections;

public class SphereScript : MonoBehaviour {
    private GameObject cameraTarget;

    private float speed = 0f;
    // Use this for initialization
    void Start () {

        cameraTarget = GameObject.Find("Camera (head)");
	}
	
	// Update is called once per frame
	void Update () {
        transform.Translate(-Vector3.forward * Time.deltaTime * speed);
	}
}
