using UnityEngine;

public class ShopZone : MonoBehaviour
{
    public GameObject player;

    public Transform playerSpawnpoint;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameObject q = Instantiate(player, playerSpawnpoint.position, transform.rotation);
        q.transform.parent = TemplatesRooms.me.roomHolder;
    }

    // Update is called once per frame
    void Update()
    {
        MusicManager.me.volumes[2] = 0.1f;
        MusicManager.me.volumes[1] = 0f;
        MusicManager.me.volumes[0] = 0f;
    }
}
