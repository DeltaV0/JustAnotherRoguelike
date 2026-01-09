using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager me;

    public AudioSource[] aud;

    //public float[] targets;

    public float[] volumes;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        me = this;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        aud[0].volume = Mathf.Lerp(aud[0].volume, volumes[0], 0.025f);
        aud[1].volume = Mathf.Lerp(aud[1].volume, volumes[1], 0.025f);
        aud[2].volume = Mathf.Lerp(aud[2].volume, volumes[2], 0.025f);
    }
}
