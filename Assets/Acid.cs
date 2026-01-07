using UnityEngine;

public class Acid : MonoBehaviour
{
    public float Power;
    public float Radius;

    private bool active;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.localScale = new Vector3(transform.localScale.x - 0.005f, transform.localScale.y - 0.005f, transform.localScale.z - 0.005f);
        if(transform.localScale.x <= 0 && active){
            Destroy(gameObject);
        }
    }

    void OnTriggerStay2D(Collider2D other){
        if(other.CompareTag("Enemy")){
                //col.transform.gameObject.GetComponent<Rigidbody2D>().linearVelocity += (Vector2)(col.transform.root.position - transform.position).normalized * Power;
                other.gameObject.GetComponent<EnemyIdentifier>().AI.health -= Power;
                //col.gameObject.GetComponent<EnemyIdentifier>().AI.aud.PlayOneShot(col.gameObject.GetComponent<EnemyIdentifier>().AI.audio[1], 0.25f);
            }
            if(other.CompareTag("Player")){
                //col.transform.gameObject.GetComponent<Rigidbody2D>().linearVelocity += (Vector2)(col.transform.root.position - transform.position).normalized * Power;
                other.gameObject.GetComponent<Player>().health -= Power;
                //col.gameObject.GetComponent<Player>().aud.PlayOneShot(col.gameObject.GetComponent<Player>().audio[1], 0.25f);
            }
    }

    public void Activate(){
        transform.localScale = new Vector3(Radius * 2f, Radius * 2f, Radius * 2f);
        active = true;
    }


}
