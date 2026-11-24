using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour
{

    public GameObject PauseMenu;
    public bool paused;

    public static PauseManager me;

    //public AudioListener aud;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        me = this;
        paused = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown("escape")){
            if(!paused){
                paused = true;
                Time.timeScale = 0;
                //error, fix this!!
                //sources.AddRange(FindObjectsOfType(typeof(AudioSource)));
                AudioListener.pause = true;
            } else {
                paused = false;
                AudioListener.pause = false;
                Time.timeScale = 1;
            }
            PauseMenu.SetActive(paused);
        }
    }
}
