using UnityEngine;
using System.Collections;

public class RotatingLevel : MonoBehaviour {

    public float rotationSpeed = 25f;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        transform.Rotate(Vector3.forward * Time.deltaTime * rotationSpeed, Space.Self);
	}
}
