using UnityEngine;
using UnityEngine.AI;

public class BasicEnemy : MonoBehaviour
{
    public Rigidbody2D rb;
    public float health;
    public float maxHealth;
    public NavMeshAgent agent;
    public Room room;

    public GameObject Coin;

    public float moveSpeed;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        agent.updatePosition = false;
    }
}
