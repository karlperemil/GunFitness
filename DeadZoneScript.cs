using UnityEngine;

public class DeadZoneScript : MonoBehaviour {
    private GameController gc;
    private ForwardController fc;

    // Use this for initialization
    void Start () {
        gc = GameObject.Find("[CameraRig]").GetComponent<GameController>();
        fc = GameObject.Find("ForwardHolder").GetComponent<ForwardController>();
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider collider)
    {
        HittingSomething(collider);
    }

    void HittingSomething(Collider collider)
    {
        GameObject collideGO = collider.gameObject;

        Debug.Log(collideGO.name);
        

        if(collideGO.tag == "LevelEnd") {

            Debug.Log("Level End!");
            fc.NextLevel();
        }
        if(collideGO.tag == "TriggerDoor")
        {
            ObjectPool.instance.PoolObject(collideGO.transform.parent.gameObject);
        }

        ObjectPool.instance.PoolObject(collideGO);
    }
}
