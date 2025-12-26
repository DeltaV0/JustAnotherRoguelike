using UnityEngine;

public class Prefabs : MonoBehaviour
{
    public PlayerStats stats;

    public static Prefabs me;

    public GameObject[] Enemy;

    public GameObject[] Items;

    void Start(){
        me = this;
    }

}
