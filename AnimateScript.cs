using UnityEngine;
using System.Collections;
using System;

public class AnimateScript : MonoBehaviour {
    private float timeStart;
    private bool isAnimateOut;
    private float animationTime;
    private bool isAnimateIn;
    private Vector3 finalScale;
    private float animationDelay;
    private Vector3 startScale;
    private Quaternion finalRot;
    private bool isAnimateToPos;
    private Vector3 startPos;
    private Quaternion startRot;
    private Vector3 finalPos;
    public bool active;
    public Vector3 origPos;
    public Quaternion origRot;
    public Vector3 origScale;

    void Start()
    {
        origPos = transform.position;
        origRot = transform.rotation;
        origScale = transform.localScale;
    }

    void Update()
    {
        if (!AnimationStillActive())
        {
            isAnimateOut = false;
            isAnimateIn = false;
            return;
        }
        float timeNow = Time.time;
        float timeElapsed = timeNow - timeStart;
        if (isAnimateOut & timeNow > timeStart + animationDelay)
        {
            Vector3 t = transform.localScale;
            float scaleVal = 1f - EaseOut(timeNow - timeStart, 0f, 1f, animationTime);

            transform.localScale = new Vector3(t.z * scaleVal, t.y * scaleVal, t.z * scaleVal);
        }
        if (isAnimateIn & timeNow > timeStart + animationDelay)
        {
            Vector3 t = finalScale;
            float scaleVal = EaseIn(timeNow - timeStart, 0f, 1f, animationTime);
            transform.localScale = new Vector3(t.x * scaleVal, t.y * scaleVal, t.z * scaleVal);
        }
        if(isAnimateToPos & timeNow > timeStart + animationDelay)
        {
            float scaleVal = EaseInOut(timeNow - timeStart, 0f, 1f, animationTime);
            transform.localScale = Vector3.Lerp(startScale, finalScale, scaleVal);
            transform.rotation = Quaternion.Lerp(startRot, finalRot, scaleVal);
            transform.position = Vector3.Lerp(startPos, finalPos, scaleVal);
        }
    }

    private float EaseInOut(float t, float b, float c, float d)
    {
        return -c/2f * ( (float)Math.Cos( (float)Math.PI*t/d) - 1f) + b;
    }

    private float EaseIn(float t, float b, float c, float d)
    {
        return -c * (float)Math.Cos(t / d * ((float)Math.PI / 2)) + c + b;
    }

    private float EaseOut(float t, float b, float c, float d)
    {
        return c * (float)Math.Sin(t / d * ((float)Math.PI / 2)) + b;
    }

    private bool AnimationStillActive()
    {
        return timeStart + animationDelay + animationTime > Time.time;
    }

    public void AnimateOut(float time, float delay = 0f)
    {
        this.animationDelay = delay;
        startScale = transform.localScale;
        animationTime = time;
        timeStart = Time.time;
        isAnimateOut = true;
    }

    public void AnimateIn(float time, float delay = 0f)
    {
        this.animationDelay = delay;
        finalScale = transform.localScale;
        transform.localScale = new Vector3();
        animationTime = time;
        timeStart = Time.time + delay;
        isAnimateIn = true;
    }

    public void AnimateToPosition(float time, Transform to, float delay = 0f)
    {
        this.animationDelay = delay;
        startScale = transform.localScale;
        finalScale = to.localScale;
        startPos = transform.position;
        finalPos = to.position;
        startRot = transform.rotation;
        finalRot = to.rotation;
        animationTime = time;
        timeStart = Time.time + delay;
        isAnimateToPos = true;
    }

    public void AnimateOrigin(float time, float delay = 0f)
    {
        this.animationDelay = delay;
        startScale = transform.localScale;
        finalScale = origScale;
        startPos = transform.position;
        finalPos = origPos;
        startRot = transform.rotation;
        finalRot = origRot;
        animationTime = time;
        timeStart = Time.time + delay;
        isAnimateToPos = true;
    }
}