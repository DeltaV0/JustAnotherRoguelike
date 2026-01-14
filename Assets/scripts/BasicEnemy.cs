using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic;

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

    public bool Burning;

    public Collider2D collider;

    public LineRenderer outlineRenderer;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Burning = false;
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        agent.updatePosition = false;
    }

    public void RenderLines()
    {
        PhysicsShapeGroup2D phys = new PhysicsShapeGroup2D();
        collider.GetShapes(phys);
        List<Vector2> vectors = new List<Vector2>();
        for(int i = 0; i < phys.shapeCount; i++)
        {
            List<Vector2> vectors2 = new List<Vector2>();
            phys.GetShapeVertices(i, vectors2);
            vectors.AddRange(vectors2);
        }
        outlineRenderer.positionCount = vectors.Count;
        outlineRenderer.loop = true;
        Vector2 xAxis = new Vector2(Mathf.Cos(rb.rotation * Mathf.Deg2Rad), Mathf.Sin(rb.rotation * Mathf.Deg2Rad));
        Vector2 yAxis = new Vector2(-1 * xAxis.y, xAxis.x);
        for(int i = 0; i < vectors.Count; i++)
        {
            outlineRenderer.SetPosition(i, new Vector3(vectors[i].x * xAxis.x + vectors[i].y *yAxis.x,vectors[i].x * xAxis.y + vectors[i].y*yAxis.y,0) + transform.position);
        }
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
