using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
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

    public AudioSource aud;

    public AudioClip[] audio;

    public Image Black;
    public Image ImpactFrame;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        stats = PlayerStats.me;
        health = stats.MaxHealth;
        moveSpeed = stats.MoveSpeed;
        TemplatesRooms.me.player = this;
        currentRoom = Generator.me.Rooms[0];
    }

    public void UpdateStats(){
        shots = stats.shots;
        fireForce = stats.bulletSpeed;
        innacuracy = stats.inaccuracy;
    }

    void Update(){
        UpdateStats();
        if(!CameraMover.me.WorldMap){
        
        if(ammo <= 0){
            ammo = stats.MaxAmmo;
            ReloadTimer = stats.MaxReload;
            //AmmoCircle.fillAmount = 0f;
        }
        if (Input.GetMouseButton(0) && timer <= 0 && ammo > 0 && ReloadTimer <= 0)
        {
            aud.pitch = Random.Range(0.8f, 1.2f);
            aud.PlayOneShot(audio[0], 0.3f);
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
            DashTimer[0] = stats.DashCooldown;
            DashTimer[1] = 10;
            rb.linearVelocity += moveDir * moveSpeed * 7.5f;
        }
        if(DashTimer[1] > 0){
            collider.excludeLayers = LayerMask.GetMask("Enemy","IgnoreBullet");
        } else {
            collider.excludeLayers = LayerMask.GetMask();
        }
        
        }
    }

    public void Hurt(float d){
        health -= d;
        StartCoroutine(ouch(d));
        

    }

    IEnumerator ouch(float d){
        Time.timeScale = 0;
       // Black.color = new Color(1, 1, 1, 1);
        ImpactFrame.color = new Color(1, 0.25f, 0.25f, 1);
        yield return new WaitForSecondsRealtime(0.125f * d);
        ImpactFrame.color = new Color(1, 0.25f, 0.25f, 0);
        //Black.color = new Color(0, 0, 0, 0);
        Time.timeScale = 1;
    }

    IEnumerator Death(){
        Time.timeScale = 0.1f;
        ImpactFrame.color = new Color(1, 0.25f, 0.25f, 1);
        for(int i = 0; i < 10; i++){
            Black.color = new Color(0, 0, 0, ((float)i / 10));
            yield return new WaitForSecondsRealtime(0.1f);
        }
        ImpactFrame.color = new Color(1, 0.25f, 0.25f, 0);
        Time.timeScale = 1;
        SceneManager.LoadSceneAsync("Death");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(health <= 0){
            StartCoroutine(Death());
            Time.timeScale = 0;
            //Destroy(gameObject);
        }
        //Debug.Log((float)(ammo / stats.MaxAmmo));
        if(ReloadTimer <= 0){
            AmmoCircle.fillMethod = Image.FillMethod.Vertical;
            AmmoCircle.fillAmount = Mathf.Lerp(AmmoCircle.fillAmount, (float)((float)ammo / (float)stats.MaxAmmo), 0.1f);
        } else {
            AmmoCircle.fillMethod = Image.FillMethod.Radial360;
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
        if(!CameraMover.me.WorldMap){
            Vector2 aimDirectionB = new Vector2(transform.position.x, transform.position.y) - (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
            float aimAngleB = Mathf.Atan2(aimDirectionB.y, aimDirectionB.x) * Mathf.Rad2Deg + 90f;
            transform.eulerAngles = new Vector3(0, 0, aimAngleB);
            rb.linearVelocity = Vector3.Lerp((Vector3)rb.linearVelocity, moveDir * moveSpeed, 0.2f);
        } else{ 
            rb.linearVelocity = Vector3.Lerp((Vector3)rb.linearVelocity, new Vector3(0, 0, 0), 0.2f);
        }
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
