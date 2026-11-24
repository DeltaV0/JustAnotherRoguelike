using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ShootingEnemy : BasicEnemy
{
    [Header("Actual Enemy Variables")]
    public int timer;

    public Image healthBar;

    public int AItimer;

    

    public bool smart;

    

    

    public int AIType;

    public int averageMaxShootTimer;

    public int[] AISpeed;

    public float turnspeed;

    public bool CanSeePlayer;

    

    public int waitaftershooting;

    

    

    
    

    public float constantRot;

    [System.Serializable]
    public class BulletPattern{
        public float fireForce;
        public float innacuracy;
        public GameObject bulletPrefab;
        public int shots;
        public bool LazerOneShotType;
        public int burstSize;
        public float BurstSpeed;
        public float[] BurstMinMaxAngle;
        public float[] ShotMinMaxAngle;
    }
    [Header("Bullet Patterns")]

    public BulletPattern[] Patterns;

    public int currentPattern;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        speedMult = 1f;
        currentPattern = 0;
        AIType = UnityEngine.Random.Range(0, 3);
        TemplatesRooms.me.player.enemiesInRoom.Add(this);
        maxHealth *= UnityEngine.Random.Range(0.95f, 1f + (((float)PlayerStats.me.Level - 1f )/ 3f));
        averageMaxShootTimer = (int)((float)averageMaxShootTimer * UnityEngine.Random.Range((1f / 1f + (((float)PlayerStats.me.Level - 1f )/ 3f)), 1.1f));
        agent.speed *= UnityEngine.Random.Range(0.95f, 1f + (((float)PlayerStats.me.Level - 1f )/ 3f));
        
        if(UnityEngine.Random.Range(0, 11) == 0 && PlayerStats.me.Level > 1){
            GameObject bullet = (GameObject)Instantiate(Prefabs.me.additions[0], transform.position, transform.rotation);
            bullet.transform.GetChild(0).gameObject.GetComponent<Shield>().owner = gameObject;
            cost += (int)(cost * 0.25f);
        }
        if(UnityEngine.Random.Range(0, 13) == 0 && PlayerStats.me.Level > 2){
            GameObject bullet = (GameObject)Instantiate(Prefabs.me.additions[1], transform.position, transform.rotation);
            bullet.transform.gameObject.GetComponent<FancyAddon>().owner = gameObject;
            maxHealth *= 2;
            averageMaxShootTimer = (int)(averageMaxShootTimer * 0.5f);
            agent.speed *= 1.5f;
            cost += (int)(cost * 0.5f);
        }
        health = maxHealth;
        if(room.budget - cost < 0){
            if(room.budget > 0){
                TemplatesRooms.me.player.enemiesInRoom.Remove(this);
                GameObject e = Instantiate(Prefabs.me.Misc[1], transform.position, Quaternion.identity);
                e.transform.GetComponent<ItemSpawner>().room = room;
                Destroy(gameObject);
            } else {
                TemplatesRooms.me.player.enemiesInRoom.Remove(this);
                Destroy(gameObject);
            }
            
        } else {
            room.enemyCount++;
            room.budget -= cost;
        }
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        agent.updatePosition = false;
        InitVectorDrawers();
        
    }

    // Update is called once per frame
    void Update()
    {   
        if(health <= 0){
            Instantiate(Prefabs.me.enemyDeath[0], transform.position, transform.rotation);
            room.enemyCount--;
            for(int i = 0; i < Random.Range(1, cost * 2); i++){
                GameObject bullet = (GameObject)Instantiate(Coin, transform.position, transform.rotation);
                bullet.transform.parent = TemplatesRooms.me.roomHolder;
                bullet.GetComponent<Rigidbody2D>().rotation = UnityEngine.Random.Range(0, 360f);
                bullet.GetComponent<Rigidbody2D>().AddForce(new Vector2(Mathf.Cos((bullet.GetComponent<Rigidbody2D>().rotation + 90f) * Mathf.Deg2Rad) * 1f * PlayerStats.me.Damage, Mathf.Sin((bullet.GetComponent<Rigidbody2D>().rotation + 90f) * Mathf.Deg2Rad) * 1f * PlayerStats.me.Damage), ForceMode2D.Impulse);
            }
            TemplatesRooms.me.player.enemiesInRoom.Remove(this);
            if(TemplatesRooms.me.player.enemiesInRoom.Count == 0){
                TemplatesRooms.me.player.ammo = PlayerStats.me.MaxAmmo;
            }
            Destroy(gameObject);
        }
        healthBar.fillAmount = (float)health / (float)maxHealth;
        
        if(!Stunned){
        if(AItimer <= 0){
            AItimer = Random.Range(AISpeed[0], AISpeed[1]);
            AIType = UnityEngine.Random.Range(0, 3);
            if(AIType == 1){
                agent.SetDestination(new Vector3(UnityEngine.Random.Range(-10f, 10f), UnityEngine.Random.Range(-10f, 10f), 0) + room.transform.position);
            }
        }
        
        transform.rotation = Quaternion.Euler(0, 0, transform.rotation.z);
        Vector3 curreul = new Vector3(0, 0, rb.rotation);//Mathf.LerpAngle(transform.rotation.z, aimAngleP, 1f));
        
        transform.rotation = Quaternion.Euler(curreul);
       
        

        if (timer <= 0)
        {
            
            StartCoroutine(Fire(currentPattern));
            if(currentPattern < Patterns.Length - 1)
                {
                    currentPattern++;
                } else
                {
                    currentPattern = 0;
                }
           
        }
        }
    }

    IEnumerator Fire(int pattern){
        timer = (int)((averageMaxShootTimer + UnityEngine.Random.Range(-10, 10)) / speedMult);
        for(int q = 0; q < Patterns[pattern].burstSize; q++){
            
            aud.pitch = UnityEngine.Random.Range(0.8f, 1.2f);
            aud.PlayOneShot(audio[0], 0.25f);
            

            for (int i = 0; i < Patterns[pattern].shots; i++) {
            GameObject bullet = (GameObject)Instantiate(Patterns[pattern].bulletPrefab, transform.position, transform.rotation);

            //Vector2 aimDirection = new Vector2(transform.position.x, transform.position.y) - (Vector2)TemplatesRooms.me.player.transform.position - (Vector2)(TemplatesRooms.me.player.rb.linearVelocity * 0.25f);

            //float aimAngle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg + 90f + Random.Range(-1f * (innacuracy * 0.5f), (innacuracy * 0.5f));
            if(!Patterns[pattern].LazerOneShotType){
            bullet.GetComponent<Rigidbody2D>().rotation = rb.rotation + UnityEngine.Random.Range(-1f * (Patterns[pattern].innacuracy * 0.5f), (Patterns[pattern].innacuracy * 0.5f))  + (Mathf.Lerp(Patterns[pattern].BurstMinMaxAngle[0], Patterns[pattern].BurstMinMaxAngle[1], (float)q / ((float)Patterns[pattern].burstSize - 0.99f)) + Mathf.Lerp(Patterns[pattern].ShotMinMaxAngle[0], Patterns[pattern].ShotMinMaxAngle[1], (float)i / ((float)Patterns[pattern].shots - 0.99f)));


            bullet.GetComponent<Rigidbody2D>().AddForce(new Vector2(Mathf.Cos((bullet.GetComponent<Rigidbody2D>().rotation + 90f) * Mathf.Deg2Rad) * Patterns[pattern].fireForce, Mathf.Sin((bullet.GetComponent<Rigidbody2D>().rotation + 90f) * Mathf.Deg2Rad) * Patterns[pattern].fireForce), ForceMode2D.Impulse);
            bullet.GetComponent<Bullet>().player = false;
            //bullet.GetComponent<Bullet>().damage = 1;
            bullet.GetComponent<Bullet>().Owner = gameObject;
            //bullet.GetComponent<Bullet>().flaming = burningBullets;
            //bullet.GetComponent<Bullet>().enemytype = gameObject;
            } else {
                //Debug.Log(((float)q / ((float)burstSize - 0f)) + "");
                bullet.transform.eulerAngles = new Vector3(0, 0, bullet.transform.eulerAngles.z + UnityEngine.Random.Range(-1f * (Patterns[pattern].innacuracy * 0.5f), (Patterns[pattern].innacuracy * 0.5f)) + (Mathf.Lerp(Patterns[pattern].BurstMinMaxAngle[0], Patterns[pattern].BurstMinMaxAngle[1], (float)q / ((float)Patterns[pattern].burstSize - 0.99f)) + Mathf.Lerp(Patterns[pattern].ShotMinMaxAngle[0], Patterns[pattern].ShotMinMaxAngle[1], (float)i / ((float)Patterns[pattern].shots - 0.99f))));
                if(bullet.GetComponent<ConstantLazer>() != null){
                    bullet.GetComponent<ConstantLazer>().Owner = gameObject;
                    bullet.GetComponent<ConstantLazer>().angleOffset = UnityEngine.Random.Range(-1f * (Patterns[pattern].innacuracy * 0.5f), (Patterns[pattern].innacuracy * 0.5f)) + (Mathf.Lerp(Patterns[pattern].BurstMinMaxAngle[0], Patterns[pattern].BurstMinMaxAngle[1], (float)q / ((float)Patterns[pattern].burstSize - 0.99f)) + Mathf.Lerp(Patterns[pattern].ShotMinMaxAngle[0], Patterns[pattern].ShotMinMaxAngle[1], (float)i / ((float)Patterns[pattern].shots - 0.99f)));
                }
                //waitaftershooting = 75;
                //rb.linearVelocity = new Vector2(0, 0);
            }
            }
            yield return new WaitForSeconds(Patterns[pattern].BurstSpeed / speedMult);
        }
    }

    void FixedUpdate(){
        RenderLines();
        if(!Stunned){
        //speedMult = Mathf.Lerp(speedMult, 1f, 0.005f);
        if(AIType == 0){
        agent.SetDestination(TemplatesRooms.me.player.transform.position);
        } else if(AIType == 2){
            agent.SetDestination(((TemplatesRooms.me.player.transform.position - room.transform.position) * -1) + room.transform.position);
        }
        RaycastHit2D hit; 
        hit = Physics2D.Linecast(transform.position, TemplatesRooms.me.player.transform.position, LayerMask.GetMask("Wall"));
        CanSeePlayer = hit;
        Vector3 aimDirectionP = Vector3.zero;

        if(!CanSeePlayer){
            aimDirectionP = new Vector3(transform.root.position.x, transform.root.position.y, 0) - TemplatesRooms.me.player.transform.position - (Vector3)(TemplatesRooms.me.player.rb.linearVelocity * 0.75f * (Vector3.Distance(transform.position, TemplatesRooms.me.player.transform.position) * 0.1f) );

        } else {
           // Debug.Log("blind");
            if(agent.path.corners.Length > 1){
                aimDirectionP = new Vector3(transform.root.position.x, transform.root.position.y, 0) - (Vector3)agent.path.corners[1];
            }
            AIType = 0;

        }
        float aimAngleP = Mathf.Atan2(aimDirectionP.y, aimDirectionP.x) * Mathf.Rad2Deg + 90f;
        if(constantRot == 0){
            rb.angularVelocity = (rb.rotation - (Mathf.LerpAngle(rb.rotation+ 180f, aimAngleP + 180f, turnspeed * speedMult) - 180f)) * -30f;
        } else {
           // rb.rotation += constantRot;
            rb.angularVelocity = constantRot;
        }

        if(waitaftershooting > 0){
            rb.linearVelocity = new Vector2(0, 0);
            waitaftershooting--;
            agent.SetDestination(transform.position);
            rb.angularVelocity = 0;
        }
        
        //Debug.Log(transform.rotation.z + ", " + aimAngleP + ", " + Mathf.LerpAngle(transform.rotation.z, aimAngleP, 0.5f));

        //Vector3 curreul = new Vector3(0, 0, rb.rotation);//Mathf.LerpAngle(transform.rotation.z, aimAngleP, 1f));

        //transform.rotation = Quaternion.Euler(curreul) ;
        
        //rb.rotation = Mathf.LerpAngle(rb.rotation, aimAngleP, 0.1f);
        
        //agent.nextPosition = transform.position;
        if (timer > 0 && !CanSeePlayer) {
            timer--;
        }
        if (AItimer > 0) {
            AItimer--;
        }
        }
        Vector3 GoalVelocity = agent.desiredVelocity;
        if (true)
        {
            Vector3 closP = TemplatesRooms.me.player.LOScol.ClosestPoint(transform.position);
            Vector3 p = (transform.position - closP).normalized / ((transform.position - closP).sqrMagnitude + 1);
            GoalVelocity += p * 5f;

            for(int i = 0; i <TemplatesRooms.me.player.PlayerBullets.Count; i++)
            {
                Vector3 closP2 = TemplatesRooms.me.player.PlayerBullets[i].transform.position;
                Vector3 p2 = (transform.position - closP2).normalized / ((transform.position - closP2).sqrMagnitude + 0.5f);
                GoalVelocity += p2 * 5f;
            }
        }
        rb.linearVelocity = Vector3.Lerp((Vector3)rb.linearVelocity, GoalVelocity * speedMult * (Stunned ? 0f : 1f) * (health/maxHealth), 0.05f);
    }
}
