using UnityEngine;

public class Prefabs : MonoBehaviour
{
    public PlayerStats stats;

    public static Prefabs me;

    public GameObject[] EnemyInit;

    //public GameObject[] SupportEnemy;

    public GameObject[] All;

    public GameObject[] ChinaEnemies;

    public GameObject[] Items;

    public GameObject[] enemyDeath;

    public GameObject[] additions;

    public AudioSource aud;

    public AudioClip[] audioClips;

    public GameObject[] PlaceableThings;

    public GameObject[] Misc;

    void Start(){
        me = this;
    }

    public void PlaySFX(int a, float b){
        aud.PlayOneShot(audioClips[a], b);
    }

}
