using UnityEngine;
using System.Collections;

public class AudioController : MonoBehaviour {
    public AudioSource audioSource;
    public AudioClip[] clips;
    private bool mute = false;

    // Use this for initialization
    void Start () {
        audioSource = GetComponent<AudioSource>();
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void PlaySound(string clip, float volume = 1f, Vector3 position = new Vector3())
    {
        if (mute)
            return;
        int clipIndex = 0;
        switch (clip)
        {
            case "gun":
                clipIndex = Random.Range(0, 2);
                break;
            case "glassBreak":
                clipIndex = Random.Range(3, 5);
                break;
            case "wrongTarget":
                clipIndex = 6;
                break;
            case "perfectRound":
                clipIndex = 7;
                break;
            case "start":
                clipIndex = 8;
                break;
            case "countdown":
                clipIndex = 9;
                break;

        }

        

        AudioSource.PlayClipAtPoint(clips[clipIndex], position, volume);
    }

    public void SoundEvent(string se){
        switch(se){
            case "menu-item-hover":
                PlaySound("start");
                break;
        }
    }
}
