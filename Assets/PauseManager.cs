using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PauseManager : MonoBehaviour
{
    public bool paused;

    public  List<AudioSource> sources;

    public  List<float> pitches;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        paused = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown("escape")){
            if(!paused){
                paused = true;
                pitches.Clear();
                sources.Clear();
                Time.timeScale = 0;
                //error, fix this!!
                //sources.AddRange(FindObjectsOfType(typeof(AudioSource)));
                foreach(AudioSource aud in sources){
                    pitches.Add(aud.pitch);
                    aud.pitch = 0f;
                }
            } else {
                paused = false;
                for(int i = 0; i < sources.Count; i++){
                    sources[i].pitch = pitches[i];
                }
                Time.timeScale = 1;
            }
        }
    }
}
