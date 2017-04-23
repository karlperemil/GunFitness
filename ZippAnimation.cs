using UnityEngine;
using System.Collections;

public class ZippAnimation : MonoBehaviour {

    Vector3 origPosition;
    public float offsetSeconds = 0f;
    public float distance = 4f;
    public float speed = 1f;
    public Vector3 direction;
	// Use this for initialization
	void Start () {
        origPosition = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 newPos = origPosition;
        newPos = newPos + direction * (Mathf.Sin(Time.time + offsetSeconds * speed) * distance);
        transform.position = newPos;
	}
}
