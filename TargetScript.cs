using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TargetScript : MonoBehaviour {
    // Use this for initialization
    public string type = "undefined";
    public float distanceFromPlayer = 0.7f;
    public bool isBox = true;
    public float scaleValue = 1f;
    public float animateInTime = 0.3f;
    public bool isMoving = false;
    public bool hasStarted = false;
    public Vector3 direction;
    private Vector3 origPosition;
    public float offsetSeconds = 0f;
    public float distance = 4f;
    public float speed = 1f;
    private Vector3 origScale;

    public void ResetPositions()
    {
        transform.localScale = origScale;
        transform.localPosition = origPosition;

        if(type == "TriggerDoorButton")
        {
            GetComponent<TriggerDoorButton>().hasOpened = false;
        }
    }


	void Awake () {
        SetOrigPos();
    }

    public void SetOrigPos()
    {
        origPosition = transform.localPosition;
        origScale = transform.localScale;
    }

    void Update()
    {
        if (isMoving && hasStarted)
        {
            Vector3 newPos = origPosition;
            newPos = newPos + direction * (Mathf.Sin(Time.time + offsetSeconds * speed) * distance);
            transform.localPosition = newPos;
        }
    }

    public void OnShow(Vector3 initialScale )
    {
        this.transform.localScale = new Vector3();
        this.transform.DOScale(initialScale * scaleValue, animateInTime);
        if (isMoving)
        {
            StartCoroutine(StartMovement());
        }
    }

    IEnumerator StartMovement()
    {
        yield return new WaitForSeconds(animateInTime);
        this.hasStarted = true;
    }

    public void Hit(RaycastHit hit)
    {
        Debug.Log("Hit block");

    }

    public void Punch(Vector3 hit)
    {

    }
}
