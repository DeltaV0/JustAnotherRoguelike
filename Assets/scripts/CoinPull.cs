using UnityEngine;

public class CoinPull : MonoBehaviour
{
    public Rigidbody2D rb;

    public GameObject g;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(Vector3.Distance(transform.position, TemplatesRooms.me.player.transform.position) < 5f){
            rb.AddForce((((Vector2)(transform.position - TemplatesRooms.me.player.transform.position).normalized) / 5f) * -1f, ForceMode2D.Impulse);
        }
    }

    public void Collect(){
        g.transform.parent = null;
        g.GetComponent<TrailRenderer>().autodestruct = true;
    }
}
