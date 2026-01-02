using UnityEngine;
using DG.Tweening;

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
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public void Hit(float Damage){
        timer = rechargeTimerMax;
        shieldHealth -= Damage;
        shield.transform.DOShakePosition(0.25f, 1f, 50, 0.5f, false, true);
    }

    public void Heal(){
        
        if(shieldHealth < shieldMaxHealth){
            shield.transform.DOShakePosition(0.25f, 1f, 50, 0.5f, false, true);
                shieldHealth++;
            }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(owner == null){
            Destroy(transform.root.gameObject);
        }
        if(owner != null){
            transform.root.position = owner.transform.position;
            transform.root.rotation = owner.transform.rotation;
        }
        
        //shield.transform.localEulerAngles = new Vector3(0, 0, Mathf.LerpAngle(shield.transform.eulerAngles.z, 0, 0.1f));
        collider.enabled = shieldHealth > 0;
        //shield.SetActive(shieldHealth > 0);
        if(shieldHealth > 0){
            shield.transform.localScale = new Vector3( Mathf.Lerp(shield.transform.localScale.x, 1.5f, 0.1f),0.1f, 1);
        } else {
            shield.transform.localScale = new Vector3( Mathf.Lerp(shield.transform.localScale.x, 0f, 0.1f),0.1f, 1);
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
