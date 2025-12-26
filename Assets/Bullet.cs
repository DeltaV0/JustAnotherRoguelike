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
            Destroy(gameObject);
        }
        if(player){
            if(HomingStrength > 0 && TemplatesRooms.me.player.enemiesInRoom.Count > 0){
                rb.linearVelocity = Vector2.LerpUnclamped(rb.linearVelocity, ((Vector2)(transform.position) - (Vector2)(FindNearestEnemy().transform.position)).normalized * initVel * -1, HomingStrength);
            }
        } else {
        rb.linearVelocity = Vector2.LerpUnclamped(rb.linearVelocity, ((Vector2)(transform.position) - (Vector2)(TemplatesRooms.me.player.transform.position)).normalized * initVel * -1, HomingStrength);
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

    void OnCollisionEnter2D(Collision2D collision)
    {
        InvFrames = 1;
        if (collision.collider.CompareTag("Wall") || collision.collider.CompareTag("Respawn") || collision.collider.CompareTag("Coin"))
        {
            if(bounces <= 0){
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
                if(bounces <= 0){
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
                collision.gameObject.GetComponent<Player>().health -= damage;
                //collision.gameObject.GetComponent<Player>().animator.SetTrigger("Hit");
                if(bounces <= 0){
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
