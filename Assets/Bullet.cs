using UnityEngine;

public class Bullet : MonoBehaviour
{
    public bool player;

    public float damage;

    public float HomingStrength;

    public Rigidbody2D rb;

    public float initVel;

    public int bounces;

    public Collider2D col;

    private int InvFrames;

    private bool bnce;

    public GameObject Owner;

    public ParticleSystem trail;

    public AudioSource aud;

    public AudioClip[] audio;

    public GameObject explosionPF;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        bnce = false;
        //bounces = 2;
        initVel = rb.linearVelocity.magnitude;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(bnce){
            if(PlayerStats.me.explosionLevel > 0 && player){
                SpawnExplosion();
            }
            if(PlayerStats.me.acidLevel > 0 && player){
                SpawnAcid();
            }
            aud.pitch = Random.Range(0.8f, 1.2f);
            aud.PlayOneShot(audio[0], 0.1f);
            bounces--;
            bnce = false;
        }
        
        if(InvFrames > 0){
            col.enabled = false;
            InvFrames--;
        } else {
            col.enabled = true;
        }
        //Vector3 curreul = new Vector3(0, 0, rb.rotation);
        
        //transform.rotation = Quaternion.Euler(curreul);

        Vector2 aimDirectionP = new Vector2(transform.root.position.x, transform.root.position.y) - (new Vector2(transform.root.position.x, transform.root.position.y) - rb.linearVelocity * 4);

        float aimAngleP = Mathf.Atan2(aimDirectionP.y, aimDirectionP.x) * Mathf.Rad2Deg + 90f;

        transform.rotation = Quaternion.Euler(0, 0, aimAngleP);
        rb.angularVelocity = 0;
        if(Owner == null){
            trail.Stop();
                    trail.transform.parent = null;
                    trail.transform.localScale = new Vector3(1, 1, 1);
            Destroy(gameObject);
        }
        if(player){
            if(HomingStrength > 0 && TemplatesRooms.me.player.enemiesInRoom.Count > 0){
                rb.linearVelocity = Vector2.LerpUnclamped(rb.linearVelocity, ((Vector2)(transform.position) - (Vector2)(FindNearestEnemy().transform.position)).normalized * initVel * -1, HomingStrength);
            }
        } else {
            if(TemplatesRooms.me.player != null){
        rb.linearVelocity = Vector2.LerpUnclamped(rb.linearVelocity, ((Vector2)(transform.position) - (Vector2)(TemplatesRooms.me.player.transform.position)).normalized * initVel * -1, HomingStrength);
            }
        }
        
    }

    public BasicEnemy FindNearestEnemy(){
        float dist;
        dist = 10000;
        BasicEnemy selected = TemplatesRooms.me.player.enemiesInRoom[0];
        foreach(BasicEnemy e in TemplatesRooms.me.player.enemiesInRoom){
            if(Vector3.Distance(e.transform.position, transform.position) < dist){
                selected = e;
                dist = Vector3.Distance(e.transform.position, transform.position);
            }
        }
        return selected;

        
    }

    void OnDestroy(){
            if(PlayerStats.me.explosionLevel > 0 && player){
                SpawnExplosion();
            }
            if(PlayerStats.me.acidLevel > 0 && player){
                SpawnAcid();
            }
    }

    public void SpawnExplosion(){
        GameObject g = Instantiate(Prefabs.me.PlaceableThings[0], transform.position, Quaternion.identity);
        g.transform.parent = TemplatesRooms.me.roomHolder;
        g.GetComponent<Explosion>().Power = damage * PlayerStats.me.explosionPower;
        g.GetComponent<Explosion>().Radius = damage * 0.1f + PlayerStats.me.explosionLevel;
        g.GetComponent<Explosion>().Boom();
    }

    public void SpawnAcid(){
        GameObject g = Instantiate(Prefabs.me.PlaceableThings[1], transform.position, Quaternion.identity);
        g.transform.parent = TemplatesRooms.me.roomHolder;
        g.GetComponent<Acid>().Power = damage * 0.025f;
        g.GetComponent<Acid>().Radius = damage * 0.1f + PlayerStats.me.acidLevel;
        g.GetComponent<Acid>().Activate();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        InvFrames = 1;
        if (collision.collider.CompareTag("Wall") || collision.collider.CompareTag("Respawn") || collision.collider.CompareTag("Coin"))
        {
            
            if(bounces <= 0){
                trail.Stop();
                    trail.transform.parent = TemplatesRooms.me.roomHolder;
                    trail.transform.localScale = new Vector3(1, 1, 1);
                Destroy(gameObject);
            } else {
                
                bnce = true;
                rb.linearVelocity = Vector2.Reflect(collision.relativeVelocity.normalized, collision.GetContact(0).normal.normalized) * initVel * -1;
                InvFrames = 1;
            }
            
        }
        else if (player)
        {
            
            if (collision.collider.CompareTag("Enemy"))
            {
                collision.transform.root.gameObject.GetComponent<Rigidbody2D>().linearVelocity += gameObject.GetComponent<Rigidbody2D>().linearVelocity * 0.1f;
                collision.gameObject.GetComponent<EnemyIdentifier>().AI.health -= damage;
                collision.gameObject.GetComponent<EnemyIdentifier>().AI.aud.PlayOneShot(collision.gameObject.GetComponent<EnemyIdentifier>().AI.audio[1], 0.25f);
                if(bounces <= 0){
                    trail.Stop();
                    trail.transform.parent = TemplatesRooms.me.roomHolder;
                    trail.transform.localScale = new Vector3(1, 1, 1);

                Destroy(gameObject);
                } else {
                bnce = true;
                rb.linearVelocity = Vector2.Reflect(collision.relativeVelocity.normalized, collision.GetContact(0).normal.normalized) * initVel * -1;
                InvFrames = 1;
                }
            } else if(collision.collider.CompareTag("Shield") && !collision.gameObject.GetComponent<Shield>().player){
                //collision.transform.root.gameObject.GetComponent<Rigidbody2D>().linearVelocity += gameObject.GetComponent<Rigidbody2D>().linearVelocity * 0.1f;
                
                collision.gameObject.GetComponent<Shield>().Hit(damage);
                if(bounces <= 0){
                    trail.Stop();
                    trail.transform.parent = TemplatesRooms.me.roomHolder;
                    trail.transform.localScale = new Vector3(1, 1, 1);

                    Destroy(gameObject);
                } else {
                bnce = true;
                rb.linearVelocity = Vector2.Reflect(collision.relativeVelocity.normalized, collision.GetContact(0).normal.normalized) * initVel * -1;
                InvFrames = 1;
                }
            }
        }
        else {
            if (collision.collider.CompareTag("Player"))
            {
                collision.gameObject.GetComponent<Rigidbody2D>().linearVelocity += gameObject.GetComponent<Rigidbody2D>().linearVelocity * 0.1f;
                collision.gameObject.GetComponent<Player>().Hurt(damage);
                collision.gameObject.GetComponent<Player>().aud.PlayOneShot(collision.gameObject.GetComponent<Player>().audio[1], 0.25f);
                //collision.gameObject.GetComponent<Player>().animator.SetTrigger("Hit");
                if(bounces <= 0){
                    trail.Stop();
                    trail.transform.parent = TemplatesRooms.me.roomHolder;
                    trail.transform.localScale = new Vector3(1, 1, 1);
                Destroy(gameObject);
                } else {
                bnce = true;
                rb.linearVelocity = Vector2.Reflect(collision.relativeVelocity.normalized, collision.GetContact(0).normal.normalized) * initVel * -1;
                InvFrames = 1;
                }
            }
        }
    }
}
