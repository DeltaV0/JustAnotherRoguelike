using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
//using TMPro;

public class Player : MonoBehaviour
{
    public Rigidbody2D rb;

    public PlayerStats stats;

    public float moveSpeed;

    Vector2 moveDir;
	Vector2 mousePos;

    public float health;

    public int timer;

    public float innacuracy;

    public GameObject bulletPrefab;
    public float fireForce = 20f;
    public int shots;


    //public int Money;

    public Room currentRoom;

    public int ammo;

    public int ReloadTimer;

    public int[] DashTimer;

    public Collider2D collider;

    public Image AmmoCircle;

    public List<BasicEnemy> enemiesInRoom;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        stats = PlayerStats.me;
        health = stats.MaxHealth;
        TemplatesRooms.me.player = this;
        currentRoom = TemplatesRooms.me.rooms[0];
    }

    public void UpdateStats(){
        shots = stats.shots;
        fireForce = stats.bulletSpeed;
        innacuracy = stats.inaccuracy;
    }

    void Update(){
        UpdateStats();
        Vector2 aimDirectionB = new Vector2(transform.position.x, transform.position.y) - (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float aimAngleB = Mathf.Atan2(aimDirectionB.y, aimDirectionB.x) * Mathf.Rad2Deg + 90f;
        transform.eulerAngles = new Vector3(0, 0, aimAngleB);
        if(ammo <= 0){
            ammo = stats.MaxAmmo;
            ReloadTimer = stats.MaxReload;
        }
        if (Input.GetMouseButton(0) && timer <= 0 && ammo > 0 && ReloadTimer <= 0)
        {
            
            timer = stats.FireSpeed;

            for (int i = 0; i < shots; i++) {
            ammo--;
            GameObject bullet = (GameObject)Instantiate(bulletPrefab, transform.position, transform.rotation);

            Vector2 aimDirection = new Vector2(transform.position.x, transform.position.y) - (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);

            float aimAngle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg + 90f + Random.Range(-1f * (innacuracy * 0.5f), (innacuracy * 0.5f));

            bullet.GetComponent<Rigidbody2D>().rotation = aimAngle;


            bullet.GetComponent<Rigidbody2D>().AddForce(new Vector2(Mathf.Cos((bullet.GetComponent<Rigidbody2D>().rotation + 90f) * Mathf.Deg2Rad) * fireForce * Random.Range(0.9f, 1.1f), Mathf.Sin((bullet.GetComponent<Rigidbody2D>().rotation + 90f) * Mathf.Deg2Rad) * fireForce * Random.Range(0.9f, 1.1f)), ForceMode2D.Impulse);
            bullet.GetComponent<Bullet>().player = true;
            bullet.GetComponent<Bullet>().damage = stats.Damage;
            bullet.GetComponent<Bullet>().HomingStrength = stats.homingPower;
            bullet.GetComponent<Bullet>().bounces = stats.bounces;
            bullet.GetComponent<Bullet>().Owner = gameObject;
            //bullet.GetComponent<Bullet>().flaming = burningBullets;
            //bullet.GetComponent<Bullet>().enemytype = gameObject;
            }

           
        }
        if(Input.GetKeyDown("space") && DashTimer[0] <= 0){
            DashTimer[0] = 50;
            DashTimer[1] = 10;
            rb.linearVelocity += moveDir * moveSpeed * 7.5f;
        }
        if(DashTimer[1] > 0){
            collider.excludeLayers = LayerMask.GetMask("Enemy","IgnoreBullet");
        } else {
            collider.excludeLayers = LayerMask.GetMask();
        }
        if(health <= 0){
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Debug.Log((float)(ammo / stats.MaxAmmo));
        if(ReloadTimer <= 0){
        AmmoCircle.fillAmount = Mathf.Lerp(AmmoCircle.fillAmount, (float)((float)ammo / (float)stats.MaxAmmo), 0.1f);
        } else {
            AmmoCircle.fillAmount = Mathf.Lerp(AmmoCircle.fillAmount, 1f - (float)((float)ReloadTimer / (float)stats.MaxReload), 0.25f);
        }
        if (timer > 0) {
            timer--;
        }
        if (DashTimer[0] > 0) {
            DashTimer[0]--;
        }
        if (DashTimer[1] > 0) {
            DashTimer[1]--;
        }
        if (ReloadTimer > 0) {
            ReloadTimer--;
        }
        float moveX = Input.GetAxisRaw ("Horizontal");
		float moveY = Input.GetAxisRaw ("Vertical");
		
		moveDir = new Vector2 (moveX, moveY).normalized;
		mousePos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
        rb.linearVelocity = rb.linearVelocity = Vector3.Lerp((Vector3)rb.linearVelocity, moveDir * moveSpeed, 0.2f);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Coin"))
        {
            stats.Money++;
            Destroy(collision.gameObject);
        }
    }
}
