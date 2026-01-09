using UnityEngine;
using DG.Tweening;
using System.Collections;

public class Shield : MonoBehaviour
{
    public float shieldHealth;
    public float shieldMaxHealth;

    public int rechargeTimerMax;

    private int timer;

    public Collider2D collider;

    public GameObject shield;

    public GameObject owner;

    public bool player;

    private bool shaking;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
       // initpos = transform.position;
    }

    public void Hit(float Damage){
        timer = rechargeTimerMax;
        shieldHealth -= Damage;
        StartCoroutine(Shake());
    }

    public void Heal(){
        
        if(shieldHealth < shieldMaxHealth){
            shield.transform.localScale = new Vector3(shield.transform.localScale.x, 0.25f, 1 );
            //StartCoroutine(Shake());
                shieldHealth++;
            }
    }

    IEnumerator Shake(){
        shaking = true;
        yield return new WaitForSeconds(0.25f);
        shaking = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(owner == null){
            Destroy(transform.root.gameObject);
        }
        if(owner != null){
            if(shaking){
                transform.root.position = owner.transform.position + ((Vector3)UnityEngine.Random.insideUnitCircle * 0.25f);
            } else {
                transform.root.position = owner.transform.position;
            }
            
            transform.root.rotation = owner.transform.rotation;
        }
        
        //shield.transform.localEulerAngles = new Vector3(0, 0, Mathf.LerpAngle(shield.transform.eulerAngles.z, 0, 0.1f));
        collider.enabled = shieldHealth > 0;
        //shield.SetActive(shieldHealth > 0);
        if(shieldHealth > 0){
            shield.transform.localScale = new Vector3( Mathf.Lerp(shield.transform.localScale.x, 1.5f, 0.1f),Mathf.Lerp(shield.transform.localScale.y, 0.1f, 0.1f), 1);
        } else {
            shield.transform.localScale = new Vector3( Mathf.Lerp(shield.transform.localScale.x, 0f, 0.1f),Mathf.Lerp(shield.transform.localScale.y, 0.1f, 0.1f), 1);
        }

        
        
        if(shieldHealth < 0){
            shieldHealth = 0;
        }
        if(timer > 0){
            timer--;
        } else {
            timer = rechargeTimerMax;
            Heal();
        }
    }
}
