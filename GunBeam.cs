using UnityEngine;
using System.Collections;
using DG.Tweening;

public class GunBeam : MonoBehaviour {
    private Material mat;
    // Use this for initialization
    void Start () {
        mat = GetComponent<Renderer>().material;
	}

    public void Fade()
    {
        Debug.Log("Fade");
        //mat.DOFade(0, 0.5f);
        GetComponent<Renderer>().material.DOFade(0f,0.4f);
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
