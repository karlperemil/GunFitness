using UnityEngine;
using System.Collections;

public class GunController : MonoBehaviour {

    private Valve.VR.EVRButtonId triggerButton = Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger;
    private Valve.VR.EVRButtonId gripButton = Valve.VR.EVRButtonId.k_EButton_Grip;
    private Valve.VR.EVRButtonId axis0 = Valve.VR.EVRButtonId.k_EButton_Axis0;
    private Valve.VR.EVRButtonId axis1 = Valve.VR.EVRButtonId.k_EButton_Axis1;
    private Valve.VR.EVRButtonId axis2 = Valve.VR.EVRButtonId.k_EButton_Axis2;
    private Valve.VR.EVRButtonId axis3 = Valve.VR.EVRButtonId.k_EButton_Axis3;
    private Valve.VR.EVRButtonId axis4 = Valve.VR.EVRButtonId.k_EButton_Axis4;
    private Valve.VR.EVRButtonId padButtonDown = Valve.VR.EVRButtonId.k_EButton_DPad_Down;
    private Valve.VR.EVRButtonId padButtonLeft = Valve.VR.EVRButtonId.k_EButton_DPad_Left;
    private Valve.VR.EVRButtonId padButtonRight = Valve.VR.EVRButtonId.k_EButton_DPad_Right;
    private Valve.VR.EVRButtonId padButtonUp = Valve.VR.EVRButtonId.k_EButton_DPad_Up;
    public bool triggerDown = false;
    public bool triggerUp = false;
    public bool triggerPressed = false;
    public bool gripDown = false;
    public bool gripUp = false;
    public bool gripPressed = false;
    public bool padDown = false;
    public bool padUp = false;
    public bool padPressed = false;
    public SteamVR_Controller.Device controller{get{return SteamVR_Controller.Input((int)trackedObj.index);}}

    public GameObject lineRender;
    private ParticleSystem partSys;
    public GameObject muzzle;
    private string controllerColor = "Blue";

    private SteamVR_TrackedObject trackedObj;
    private float timeOfFire;
    public GameObject gun;
    public GameObject sword;
    private float gunTriggerTime = 0f;
    private float gunVibrationLength = 0.2f;
    public static GunController instance;
    public GameObject laserPointer;
    private LineRenderer laserRender;
    private GameObject laserPointerInstance;

    void Awake()
    {
        instance = this;
    }
    // Use this for initialization
    void Start() {
        trackedObj = this.GetComponent<SteamVR_TrackedObject>();
        Debug.Log(trackedObj);
        Debug.Log(trackedObj.index);
        partSys = muzzle.GetComponent<ParticleSystem>();
        if(this.name == "Controller (left) Yellow")
        {
            this.controllerColor = "Yellow";
        }
        else
        {
            this.controllerColor = "Blue";
        }
        CreateLaserPointer();
        

        sword.SetActive(false);
    }

    private void GunVibration() {
        if (Time.time < gunTriggerTime + gunVibrationLength)
        {
            ushort time = (ushort)Mathf.Min(Time.deltaTime * 1000000f,3999f);
            Debug.Log("Triggering" + time);
            SteamVR_Controller.Input((int)trackedObj.index).TriggerHapticPulse(time, axis0);
        }
    }

    public IEnumerator GunVibrationContinous(float timeToEnd)
    {
        if (Time.time < timeToEnd)
        {
            SteamVR_Controller.Input((int)trackedObj.index).TriggerHapticPulse((ushort)3999, axis0);
            yield return new WaitForSeconds(4f / 1000f);
            StartCoroutine(GunVibrationContinous(timeToEnd));
        }
        else
        {
           yield return null;
        }
    }

    private void CreateLaserPointer()
    {
        laserPointerInstance = Instantiate(laserPointer) as GameObject;
        Vector3 muzzlePos = muzzle.transform.position;
        var line = muzzle.transform.position + (transform.forward * 100f);
        var rotatedLine = Quaternion.AngleAxis(55f, transform.right) * line;
        laserRender = laserPointerInstance.GetComponent<LineRenderer>();
        laserRender.SetPositions(new Vector3[] { muzzlePos, rotatedLine });
        laserRender.enabled = false;
        laserPointerInstance.SetActive(true);
    }


    // Update is called once per frame
    void Update() {
        if (controller == null)
        {
            Debug.Log("no controller found");
            return;
        }

        triggerDown = controller.GetPressDown(triggerButton);
        triggerUp = controller.GetPressUp(triggerButton);
        triggerPressed = controller.GetPress(triggerButton);

        gripDown = controller.GetPressDown(gripButton);
        gripUp = controller.GetPressUp(gripButton);
        gripPressed = controller.GetPress(gripButton);

        padDown = controller.GetPressDown(padButtonDown) ? true : padDown;
        padDown = controller.GetPressDown(padButtonLeft) ? true : padDown;
        padDown = controller.GetPressDown(padButtonRight) ? true : padDown;
        padDown = controller.GetPressDown(padButtonUp) ? true : padDown;

        padPressed = controller.GetPress(padButtonDown) ? true : padDown;
        padPressed = controller.GetPress(padButtonLeft) ? true : padDown;
        padPressed = controller.GetPress(padButtonRight) ? true : padDown;
        padPressed = controller.GetPress(padButtonUp) ? true : padDown;

        padUp = controller.GetPressUp(padButtonDown) ? true : padDown;
        padUp = controller.GetPressUp(padButtonLeft) ? true : padDown;
        padUp = controller.GetPressUp(padButtonRight) ? true : padDown;
        padUp = controller.GetPressUp(padButtonUp) ? true : padDown;

        RaycastHit hit;
        if (Physics.Raycast(muzzle.transform.position, muzzle.transform.forward, out hit))
        {
            
        }
        GameObject go;
        bool hitUI = false;
        laserRender.enabled = false;
        if (hit.transform)
        {
            go = hit.transform.gameObject;

            if (go.tag == "UI")
            {
                hitUI = true;
                laserRender.enabled = true;
                laserRender.SetPositions(new Vector3[] { muzzle.transform.position, hit.point });
                go.GetComponent<UIBase>().OnHover();
            }

            if (triggerDown && !gripPressed)
            {
                Debug.Log("Shot an object, tag: " + go.tag + " - name: " + go.name);
                bool correctHit = false;
                if (go.tag == "TargetYellow" && controllerColor == "Yellow")
                {
                    correctHit = true;
                }
                if (go.tag == "TargetBlue" && controllerColor == "Blue")
                {
                    correctHit = true;
                }
                if (go.tag == "TriggerDoorButton")
                {
                    go.GetComponent<TriggerDoorButton>().HitDoor();
                }
                if (correctHit)
                {

                    hit.transform.gameObject.GetComponent<TargetScript>().Hit(hit);

                    GameController.instance.HitBox(go);

                }

                if (go.tag == "TargetBlue" && controllerColor == "Yellow")
                {
                    // ac.PlaySound("wrongTarget");
                }
                if (go.tag == "TargetYellow" && controllerColor == "Blue")
                {
                    //ac.PlaySound("wrongTarget");
                }

                if (go.name == "StartBox")
                {
                    GameController.instance.StartGame();
                    go.SetActive(false);
                }

                if (go.name == "Restart")
                {
                    GameController.instance.GameOver();
                }
                if (go.tag == "UI")
                {
                    go.GetComponent<UIBase>().OnTrigger();
                }
            }

        }

        if (triggerDown && !gripPressed && !hitUI)
        {
            Debug.Log("pew pew!");

            GameController.instance.ShotFired();
            gunTriggerTime = Time.time;
            //StartCoroutine(GunVibrationContinous(gunTriggerTime+gunVibrationLength));

            timeOfFire = Time.time;

            //ac.PlaySound("gun", .4f, this.transform.position);

            Vector3 from = trackedObj.transform.position;
            Vector3 to = from * 10f;
            partSys.Play();

            Vector3 muzzlePos = muzzle.transform.position;
            var line = muzzle.transform.position + (muzzle.transform.forward * 100f);
            var rotatedLine = Quaternion.AngleAxis(55f, transform.right) * line;
            GameObject newLineRender = Instantiate(lineRender) as GameObject;
            newLineRender.GetComponent<LineRenderer>().SetPositions(new Vector3[] { muzzlePos, line });
            newLineRender.GetComponent<LineRenderer>().enabled = true;
            newLineRender.SetActive(true);
            newLineRender.GetComponent<GunBeam>().Fade();
            Destroy(newLineRender, .5f);
        }

        //light.intensity = 8f - 8f * (Time.time - timeOfFire) / .1f;

        if (gripDown)
        {
            sword.SetActive(true);
            sword.GetComponent<SwordController>().OnShow();

        }
        if (gripUp)
        {
            sword.GetComponent<SwordController>().OnHide();
        }
    }
}
