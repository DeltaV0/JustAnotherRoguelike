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

    public Collider2D[] collider;

    public Color[] colours;

    public Material LineMat;

    public bool Stunned;

    public float speedMult;

    public List<GameObject> rederobjs = new List<GameObject>();

    //private List<Vector2> vectors = new List<Vector2>();

    public List<LineRenderer> Drawers = new List<LineRenderer>();
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Burning = false;
        
       // InitVectorDrawers();
    }

    public void Stun(float time){
        StartCoroutine(st(time));
    }

    IEnumerator st(float time){
        Stunned = true;
        for(int i = 0; i < (int)(50f * time); i++){
            Stunned = true;
            yield return new WaitForSeconds(0.02f);
        }
        Stunned = false;
    }

    public void Slow(float slow){
        StartCoroutine(Sl(slow));
    }

    IEnumerator Sl(float slow){
        speedMult = slow;
        for(int i = 0; i < (int)(50 * 5); i++){
            speedMult = slow;
            yield return new WaitForSeconds(0.02f);
        }
        speedMult = 1f;
    }

    public void InitVectorDrawers()
    {
        for(int p = 0; p < collider.Length; p++){
        PhysicsShapeGroup2D phys = new PhysicsShapeGroup2D();
        collider[p].GetShapes(phys);
        for(int i = 0; i < phys.shapeCount; i++)
        {
            //List<Vector2> vectors2 = new List<Vector2>();
            //phys.GetShapeVertices(i, vectors2);
            //vectors.AddRange(vectors2);
            GameObject g = new GameObject();
            rederobjs.Add(g);
            g.transform.parent = transform;
            
            LineRenderer lr = g.AddComponent(typeof(LineRenderer)) as LineRenderer;
            lr.widthMultiplier = 0.05f;
            lr.material = LineMat;
            lr.loop = true;
            Drawers.Add(lr);
            
        }
        }
    }

    public void RenderLines()
    {
        for(int p = 0; p < collider.Length; p++){
        PhysicsShapeGroup2D phys = new PhysicsShapeGroup2D();
        collider[p].GetShapes(phys);
        Vector2 xAxis = new Vector2(Mathf.Cos(rb.rotation * Mathf.Deg2Rad), Mathf.Sin(rb.rotation * Mathf.Deg2Rad));
        Vector2 yAxis = new Vector2(-1 * xAxis.y, xAxis.x);
        //List<Vector2> vectors = new List<Vector2>();
        //for(int i = 0; i < collider.Length; i++)
        //{
            List<Vector2> vectors2 = new List<Vector2>();
            phys.GetShapeVertices(0, vectors2);
            Drawers[p].positionCount = vectors2.Count;
            Drawers[p].endColor = colours[p];
            Drawers[p].startColor = colours[p];
            Drawers[p].sortingOrder = 3 + p;
            for(int q = 0; q < vectors2.Count; q++)
            {
            Drawers[p].SetPosition(q, new Vector3(vectors2[q].x * xAxis.x + vectors2[q].y *yAxis.x,vectors2[q].x * xAxis.y + vectors2[q].y*yAxis.y,0) + transform.position);
            }
            
        //}
        }
        
        //outlineRenderer.loop = true;
        
        
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
