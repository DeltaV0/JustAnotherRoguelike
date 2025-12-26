using UnityEngine;
using UnityEngine.AI;

public class EnemyAITest : MonoBehaviour
{
    public NavMeshAgent agent;

    public int health;

    public Room room;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    // Update is called once per frame
    void Update()
    {
        agent.SetDestination(TemplatesRooms.me.player.transform.position);
        transform.eulerAngles = new Vector3(0, 0, 0);

        if(health <= 0){
            room.enemyCount--;
            Destroy(gameObject);
        }
    }
}
