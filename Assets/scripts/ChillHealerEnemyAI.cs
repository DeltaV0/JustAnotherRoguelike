using UnityEngine;
using UnityEngine.UI;

public class ChillHealerEnemyAI : BasicEnemy
{
    public int maxHealTimer;

    public Image healthBar;

    public int healtimer;

    public int AItimer;
    public int[] AISpeed;
    public int AIType;

    public float turnspeed;

    public ParticleSystem parts;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        AIType = Random.Range(0, 3);
        TemplatesRooms.me.player.enemiesInRoom.Add(this);
        maxHealth *= Random.Range(0.95f, 1f + (((float)PlayerStats.me.Level - 1f )/ 3f));
        maxHealTimer = (int)((float)maxHealTimer * Random.Range((1f / 1f + (((float)PlayerStats.me.Level - 1f )/ 3f)), 1.1f));
        agent.speed *= Random.Range(0.95f, 1f + (((float)PlayerStats.me.Level - 1f )/ 3f));
        
        if(Random.Range(0, 11) == 0 && PlayerStats.me.Level > 1){
            GameObject bullet = (GameObject)Instantiate(Prefabs.me.additions[0], transform.position, transform.rotation);
            //bullet.transform.parent = transform;
            bullet.transform.GetChild(0).gameObject.GetComponent<Shield>().owner = gameObject;
            cost += (int)(cost * 0.25f);
        }
        if(Random.Range(0, 13) == 0 && PlayerStats.me.Level > 2){
            GameObject bullet = (GameObject)Instantiate(Prefabs.me.additions[1], transform.position, transform.rotation);
            bullet.transform.gameObject.GetComponent<FancyAddon>().owner = gameObject;
            maxHealth *= 2;
            maxHealTimer = (int)(maxHealTimer * 0.5f);
            agent.speed *= 1.5f;
            cost += (int)(cost * 0.5f);
        }
        health = maxHealth;
        if(room.budget - cost < 0){
            TemplatesRooms.me.player.enemiesInRoom.Remove(this);
            GameObject e = Instantiate(Prefabs.me.Misc[1], transform.position, Quaternion.identity);
            e.transform.GetComponent<ItemSpawner>().room = room;
            Destroy(gameObject);
        } else {
            room.enemyCount++;
            room.budget -= cost;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(AItimer <= 0){
            AItimer = Random.Range(AISpeed[0], AISpeed[1]);
            AIType = Random.Range(0, 3);
            if(AIType == 1){
                agent.SetDestination(new Vector3(Random.Range(-10f, 10f), Random.Range(-10f, 10f), 0) + room.transform.position);
            }
        }
        healthBar.fillAmount = (float)health / (float)maxHealth;
        transform.rotation = Quaternion.Euler(0, 0, transform.rotation.z);
        Vector3 curreul = new Vector3(0, 0, rb.rotation);//Mathf.LerpAngle(transform.rotation.z, aimAngleP, 1f));
        
        transform.rotation = Quaternion.Euler(curreul) ;

        if(health <= 0){
            Instantiate(Prefabs.me.enemyDeath[0], transform.position, transform.rotation);
            room.enemyCount--;
            for(int i = 0; i < Random.Range(1, 5); i++){
                GameObject bullet = (GameObject)Instantiate(Coin, transform.position, transform.rotation);
                bullet.transform.parent = TemplatesRooms.me.roomHolder;
                bullet.GetComponent<Rigidbody2D>().rotation = Random.Range(0, 360f);
                bullet.GetComponent<Rigidbody2D>().AddForce(new Vector2(Mathf.Cos((bullet.GetComponent<Rigidbody2D>().rotation + 90f) * Mathf.Deg2Rad) * 1f * PlayerStats.me.Damage, Mathf.Sin((bullet.GetComponent<Rigidbody2D>().rotation + 90f) * Mathf.Deg2Rad) * 1f * PlayerStats.me.Damage), ForceMode2D.Impulse);
            }
            TemplatesRooms.me.player.enemiesInRoom.Remove(this);
            if(TemplatesRooms.me.player.enemiesInRoom.Count == 0){
                TemplatesRooms.me.player.ammo = PlayerStats.me.MaxAmmo;
            }
            Destroy(gameObject);
        }

        if (healtimer <= 0)
        {
            aud.pitch = Random.Range(0.8f, 1.2f);
            aud.PlayOneShot(audio[0], 0.25f);
            parts.Play();
            healtimer = maxHealTimer;
            foreach(BasicEnemy e in TemplatesRooms.me.player.enemiesInRoom){
                e.heal(1f);
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
        rb.angularVelocity = (rb.rotation - (Mathf.LerpAngle(rb.rotation+ 180f, aimAngleP + 180f, turnspeed) - 180f)) * -30f;

        if (healtimer > 0) {
            healtimer--;
        }
        if (AItimer > 0) {
            AItimer--;
        }
        rb.linearVelocity = Vector3.Lerp((Vector3)rb.linearVelocity, agent.desiredVelocity, 0.05f);
    }
}
