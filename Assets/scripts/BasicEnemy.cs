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

    public ParticleSystem healparts;

    public AudioSource aud;

    public AudioClip[] audio;

    public int cost;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        agent.updatePosition = false;
    }

    public void heal(float h){
        health += h;
        GameObject a = Instantiate(Prefabs.me.Misc[3], transform.position, Quaternion.identity);
        a.GetComponent<HealthChangeText>().Activate(h);
        healparts.Play();
        if(health > maxHealth){
            health = maxHealth;
        }
    }
}
