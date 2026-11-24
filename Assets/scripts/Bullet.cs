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

    public float fireLevel;

    private EnemyIdentifier hitEnemy;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        hitEnemy = null;
        bnce = false;
        //bounces = 2;
        initVel = rb.linearVelocity.magnitude;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(bnce){
            if(hitEnemy != null){
                hitEnemy.AI.health -= damage;
                hitEnemy.AI.Stun(PlayerStats.me.StunTimer);
                hitEnemy.AI.Slow(PlayerStats.me.BulletSlow);
                hitEnemy.AI.aud.PlayOneShot(hitEnemy.AI.audio[1], 0.25f);
                GameObject a = Instantiate(Prefabs.me.Misc[3], hitEnemy.gameObject.transform.position, Quaternion.identity);
                a.GetComponent<HealthChangeText>().Activate(-1 * damage);
                hitEnemy = null;
            }
            ColParts();
            if(player && PlayerStats.me.Zaplevel > 0){
            Zap();
        }
            if(explosionLevel > 0){
                SpawnExplosion();
            }
            if(acidLevel > 0){
                SpawnAcid();
            }
            if (fireLevel > 0)
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
        if(hitEnemy != null){
                hitEnemy.AI.health -= damage;
                hitEnemy.AI.Stun(PlayerStats.me.StunTimer);
                hitEnemy.AI.Slow(PlayerStats.me.BulletSlow);
                hitEnemy.AI.aud.PlayOneShot(hitEnemy.AI.audio[1], 0.25f);
                GameObject a = Instantiate(Prefabs.me.Misc[3], hitEnemy.gameObject.transform.position, Quaternion.identity);
                a.GetComponent<HealthChangeText>().Activate(-1 * damage);
                hitEnemy = null;
            }
        if(player && PlayerStats.me.Zaplevel > 0){
            Zap();
        }
            if(explosionLevel > 0){
                SpawnExplosion();
            }
            if(acidLevel > 0){
                SpawnAcid();
            }
        if (fireLevel > 0)
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
            f.GetComponent<Fire>().Damage = damage * 0.1f;
            f.GetComponent<Fire>().FireLength = fireLevel;
            //f.GetComponent<Fire>().Owner = Target;
        
    }

    public void Zap(){
        GameObject g = Instantiate(Prefabs.me.PlaceableThings[3], transform.position, Quaternion.identity);
        g.GetComponent<Lightning>().Damage = damage * 0.25f;
       // aud.pitch = Random.Range(0.8f, 1.2f);
        //    aud.PlayOneShot(audio[1], 0.1f);
    }

    public void SpawnExplosion(){
        GameObject g = Instantiate(Prefabs.me.PlaceableThings[0], transform.position, Quaternion.identity);
        
        g.transform.parent = TemplatesRooms.me.roomHolder;
        g.GetComponent<Explosion>().Power = damage * explosionPower;
        g.GetComponent<Explosion>().Radius = damage * 0.1f + explosionLevel;
        g.GetComponent<Explosion>().PlayerActivated = player;
        CameraMover.me.shake += g.GetComponent<Explosion>().Power * 0.25f;
        g.GetComponent<Explosion>().Boom();
       // aud.pitch = Random.Range(0.8f, 1.2f);
        //    aud.PlayOneShot(audio[1], 0.1f);
    }

    public void SpawnAcid(){
        GameObject g = Instantiate(Prefabs.me.PlaceableThings[1], transform.position, Quaternion.identity);
        g.transform.parent = TemplatesRooms.me.roomHolder;
        g.GetComponent<Acid>().Power = damage * 0.25f;
        g.GetComponent<Acid>().Radius = damage * 0.1f + acidLevel;
        g.GetComponent<Acid>().PlayerActivated = player;
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
                    if (player)
                {
                    TemplatesRooms.me.player.PlayerBullets.Remove(this);
                }
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
                hitEnemy = collision.transform.root.gameObject.GetComponent<EnemyIdentifier>();
                collision.transform.root.gameObject.GetComponent<Rigidbody2D>().linearVelocity += gameObject.GetComponent<Rigidbody2D>().linearVelocity * 0.1f;
                
                
                if(bounces <= 0){
                    trail.Stop();
                    trail.transform.parent = TemplatesRooms.me.roomHolder;
                    trail.transform.localScale = new Vector3(1, 1, 1);
                TemplatesRooms.me.player.PlayerBullets.Remove(this);
                Destroy(gameObject);
                } else {
                bnce = true;
                rb.linearVelocity = Vector2.Reflect(collision.relativeVelocity.normalized, collision.GetContact(0).normal.normalized) * initVel * -1;
                InvFrames = 1;
                }
            } else if(collision.collider.CompareTag("Shield") && !collision.gameObject.GetComponent<Shield>().player){
                //collision.transform.root.gameObject.GetComponent<Rigidbody2D>().linearVelocity += gameObject.GetComponent<Rigidbody2D>().linearVelocity * 0.1f;
                
                collision.transform.root.gameObject.GetComponent<Shield>().Hit(damage);
                if(bounces <= 0){
                    trail.Stop();
                    trail.transform.parent = TemplatesRooms.me.roomHolder;
                    trail.transform.localScale = new Vector3(1, 1, 1);
                    TemplatesRooms.me.player.PlayerBullets.Remove(this);
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
