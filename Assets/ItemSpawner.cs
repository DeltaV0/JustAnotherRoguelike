using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameObject e = Instantiate(Prefabs.me.Items[Random.Range(0, Prefabs.me.Items.Length)], transform.position, Quaternion.identity);
        e.transform.parent = TemplatesRooms.me.roomHolder;
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
