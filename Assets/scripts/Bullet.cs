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

    public float explosionLevel;

    public float explosionPower;

    public float acidLevel;
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
            ColParts();
            if(explosionLevel > 0){
                SpawnExplosion();
            }
            if(acidLevel > 0){
                SpawnAcid();
            }
            if (player)
        {
            Burn();
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
            if(TemplatesRooms.me.player != null && HomingStrength > 0 && TemplatesRooms.me.player.enemiesInRoom.Count > 0){
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

    void OnDisable(){
            if(explosionLevel > 0){
                SpawnExplosion();
            }
            if(acidLevel > 0){
                SpawnAcid();
            }
        if (player)
        {
            Burn();
        }
            if(Owner != null){
            ColParts();
            }
    }

    public void Burn()
    {
            GameObject f = Instantiate(Prefabs.me.PlaceableThings[2], transform.position, Quaternion.identity);
            f.transform.parent = TemplatesRooms.me.roomHolder;
            //f.GetComponent<Fire>().Owner = Target;
        
    }

    public void SpawnExplosion(){
        GameObject g = Instantiate(Prefabs.me.PlaceableThings[0], transform.position, Quaternion.identity);
        g.transform.parent = TemplatesRooms.me.roomHolder;
        g.GetComponent<Explosion>().Power = damage * explosionPower;
        g.GetComponent<Explosion>().Radius = damage * 0.1f + explosionLevel;
        g.GetComponent<Explosion>().Boom();
       // aud.pitch = Random.Range(0.8f, 1.2f);
        //    aud.PlayOneShot(audio[1], 0.1f);
    }

    public void SpawnAcid(){
        GameObject g = Instantiate(Prefabs.me.PlaceableThings[1], transform.position, Quaternion.identity);
        g.transform.parent = TemplatesRooms.me.roomHolder;
        g.GetComponent<Acid>().Power = damage * 0.25f;
        g.GetComponent<Acid>().Radius = damage * 0.1f + acidLevel;
        g.GetComponent<Acid>().Activate();
        //aud.pitch = Random.Range(0.8f, 1.2f);
         //   aud.PlayOneShot(audio[2], 0.1f);
    }

    public void ColParts(){
        GameObject g = Instantiate(Prefabs.me.Misc[2], transform.position, transform.rotation);
        g.transform.parent = TemplatesRooms.me.roomHolder;
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
                collision.transform.root.gameObject.GetComponent<EnemyIdentifier>().AI.health -= damage;
                collision.transform.root.gameObject.GetComponent<EnemyIdentifier>().AI.aud.PlayOneShot(collision.gameObject.GetComponent<EnemyIdentifier>().AI.audio[1], 0.25f);
                GameObject a = Instantiate(Prefabs.me.Misc[3], collision.transform.position, Quaternion.identity);
                a.GetComponent<HealthChangeText>().Activate(-1 * damage);
                
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
