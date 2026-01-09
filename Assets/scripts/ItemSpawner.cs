using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public bool enemy;

    public Room room;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(enemy){
            if(room.enemyCount > 1){
                GameObject e = Instantiate(Prefabs.me.All[Random.Range(0, Prefabs.me.All.Length)], transform.position, Quaternion.identity);
                e.transform.GetComponent<EnemyIdentifier>().AI.room = room;
                Destroy(gameObject);
            } else {
                GameObject e = Instantiate(Prefabs.me.EnemyInit[Random.Range(0, Prefabs.me.EnemyInit.Length)], transform.position, Quaternion.identity);
                e.transform.GetComponent<EnemyIdentifier>().AI.room = room;
                Destroy(gameObject);
            }
            
        } else {
            GameObject e = Instantiate(Prefabs.me.Items[Random.Range(0, Prefabs.me.Items.Length)], transform.position, Quaternion.identity);
            e.transform.parent = TemplatesRooms.me.roomHolder;
            Destroy(gameObject);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
