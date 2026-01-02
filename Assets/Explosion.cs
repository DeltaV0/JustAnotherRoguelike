using UnityEngine;

public class Explosion : MonoBehaviour
{
    public float Power;
    public float Radius;

    public ParticleSystem partsmain;
    public ParticleSystem partseff;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        
    }

    public void Boom(){
        partsmain.transform.localScale = new Vector3(Radius * 2, Radius * 2, 1);
        partseff.transform.localScale = new Vector3(Radius / 3, Radius / 3, 1);

        Collider2D[] hit = Physics2D.OverlapCircleAll((Vector2)transform.position, Radius, LayerMask.GetMask("Enemy", "Player"));
        for(int i = 0; i < hit.Length; i++){
            Collider2D col = hit[i];
            if(col.CompareTag("Enemy")){
                col.transform.gameObject.GetComponent<Rigidbody2D>().linearVelocity += (Vector2)(col.transform.root.position - transform.position).normalized * Power;
                col.gameObject.GetComponent<EnemyIdentifier>().AI.health -= Power;
                col.gameObject.GetComponent<EnemyIdentifier>().AI.aud.PlayOneShot(col.gameObject.GetComponent<EnemyIdentifier>().AI.audio[1], 0.25f);
            }
            if(col.CompareTag("Shield")){
                col.gameObject.GetComponent<Shield>().Hit(Power);
            }
            if(col.CompareTag("Player")){
                col.transform.gameObject.GetComponent<Rigidbody2D>().linearVelocity += (Vector2)(col.transform.root.position - transform.position).normalized * Power;
                col.gameObject.GetComponent<Player>().health -= Power;
                col.gameObject.GetComponent<Player>().aud.PlayOneShot(col.gameObject.GetComponent<Player>().audio[1], 0.25f);
            }
        }
        Invoke("d", 5f);
    }

    void d(){
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
