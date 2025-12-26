using UnityEngine;
using UnityEngine.UI;

public class ShootingEnemy : BasicEnemy
{
    public int timer;

    public Image healthBar;

    public int AItimer;

    public float innacuracy;

    public float fireForce;

    public GameObject bulletPrefab;

    public int shots;

    public int AIType;

    public int averageMaxShootTimer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        AIType = Random.Range(0, 3);
        TemplatesRooms.me.player.enemiesInRoom.Add(this);
        maxHealth *= Random.Range(0.95f, 1f + (((float)PlayerStats.me.Level - 1f )/ 5f));
        averageMaxShootTimer = (int)((float)averageMaxShootTimer * Random.Range((1f / 1f + (((float)PlayerStats.me.Level - 1f )/ 5f)), 1.1f));
        agent.speed *= Random.Range(0.95f, 1f + (((float)PlayerStats.me.Level - 1f )/ 5f));
        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {   
        if(AItimer <= 0){
            AItimer = Random.Range(25, 201);
            AIType = Random.Range(0, 3);
            if(AIType == 1){
                agent.SetDestination((Vector3)(UnityEngine.Random.insideUnitCircle * 5f ) + room.transform.position);
            }
        }
        healthBar.fillAmount = (float)health / (float)maxHealth;
        transform.rotation = Quaternion.Euler(0, 0, transform.rotation.z);
        Vector3 curreul = new Vector3(0, 0, rb.rotation);//Mathf.LerpAngle(transform.rotation.z, aimAngleP, 1f));
        
        transform.rotation = Quaternion.Euler(curreul) ;
       
        if(health <= 0){
            room.enemyCount--;
            for(int i = 0; i < Random.Range(1, 5); i++){
                GameObject bullet = (GameObject)Instantiate(Coin, transform.position, transform.rotation);
                bullet.transform.parent = TemplatesRooms.me.roomHolder;
                bullet.GetComponent<Rigidbody2D>().rotation = Random.Range(0, 360f);
                bullet.GetComponent<Rigidbody2D>().AddForce(new Vector2(Mathf.Cos((bullet.GetComponent<Rigidbody2D>().rotation + 90f) * Mathf.Deg2Rad) * 1f * PlayerStats.me.Damage, Mathf.Sin((bullet.GetComponent<Rigidbody2D>().rotation + 90f) * Mathf.Deg2Rad) * 1f * PlayerStats.me.Damage), ForceMode2D.Impulse);
            }
            TemplatesRooms.me.player.enemiesInRoom.Remove(this);
            Destroy(gameObject);
        }

        if (timer <= 0)
        {
            timer = averageMaxShootTimer + Random.Range(-10, 10);

            for (int i = 0; i < shots; i++) {
            GameObject bullet = (GameObject)Instantiate(bulletPrefab, transform.position, transform.rotation);

            //Vector2 aimDirection = new Vector2(transform.position.x, transform.position.y) - (Vector2)TemplatesRooms.me.player.transform.position - (Vector2)(TemplatesRooms.me.player.rb.linearVelocity * 0.25f);

            //float aimAngle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg + 90f + Random.Range(-1f * (innacuracy * 0.5f), (innacuracy * 0.5f));

            bullet.GetComponent<Rigidbody2D>().rotation = rb.rotation + Random.Range(-1f * (innacuracy * 0.5f), (innacuracy * 0.5f));


            bullet.GetComponent<Rigidbody2D>().AddForce(new Vector2(Mathf.Cos((bullet.GetComponent<Rigidbody2D>().rotation + 90f) * Mathf.Deg2Rad) * fireForce, Mathf.Sin((bullet.GetComponent<Rigidbody2D>().rotation + 90f) * Mathf.Deg2Rad) * fireForce), ForceMode2D.Impulse);
            bullet.GetComponent<Bullet>().player = false;
            bullet.GetComponent<Bullet>().damage = 1;
            bullet.GetComponent<Bullet>().Owner = gameObject;
            //bullet.GetComponent<Bullet>().flaming = burningBullets;
            //bullet.GetComponent<Bullet>().enemytype = gameObject;
            }

           
        }
    }

    void FixedUpdate(){
        Vector3 aimDirectionP = new Vector3(transform.root.position.x, transform.root.position.y, 0) - TemplatesRooms.me.player.transform.position - (Vector3)(TemplatesRooms.me.player.rb.linearVelocity * 0.75f * (Vector3.Distance(transform.position, TemplatesRooms.me.player.transform.position) * 0.1f) );

        float aimAngleP = Mathf.Atan2(aimDirectionP.y, aimDirectionP.x) * Mathf.Rad2Deg + 90f;

        if(AIType == 0){
        agent.SetDestination(TemplatesRooms.me.player.transform.position);
        } else if(AIType == 2){
            agent.SetDestination(((TemplatesRooms.me.player.transform.position - room.transform.position) * -1) + room.transform.position);
        }
        //Debug.Log(transform.rotation.z + ", " + aimAngleP + ", " + Mathf.LerpAngle(transform.rotation.z, aimAngleP, 0.5f));

        //Vector3 curreul = new Vector3(0, 0, rb.rotation);//Mathf.LerpAngle(transform.rotation.z, aimAngleP, 1f));

        //transform.rotation = Quaternion.Euler(curreul) ;
        rb.angularVelocity = (rb.rotation - (Mathf.LerpAngle(rb.rotation+ 180f, aimAngleP + 180f, 0.1f) - 180f)) * -30f;
        //rb.rotation = Mathf.LerpAngle(rb.rotation, aimAngleP, 0.1f);
        
        //agent.nextPosition = transform.position;
        if (timer > 0) {
            timer--;
        }
        if (AItimer > 0) {
            AItimer--;
        }
        rb.linearVelocity = Vector3.Lerp((Vector3)rb.linearVelocity, agent.desiredVelocity, 0.05f);
    }
}
