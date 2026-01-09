using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats me;

    public int MaxReload;

    public float MaxHealth;

    public float MoveSpeed;

    public int FireSpeed;

    public int DashCooldown;

    public int Money;

    //public int MaxDash;

    public float homingPower;

    public int shots;

    public float inaccuracy;

    public float bulletSpeed;

    public int MaxAmmo;

    public float Damage;

    public int bounces;

    public List<int> Items;

    public int Level;

    public float explosionLevel;
    public float explosionPower;

    public float acidLevel;

    public void ChangeStat(int ID, float add, float mult){
        if(ID == 0){
            Damage = (Damage * mult) + add;
        } else if(ID == 1){
            bulletSpeed = (bulletSpeed * mult) + add;
        } else if(ID == 2){
            shots = (int)((shots * mult) + add);
        } else if(ID == 3){
            inaccuracy = (inaccuracy * mult) + add;
        } else if(ID == 4){
            MaxAmmo = (int)((MaxAmmo * mult) + add);
        } else if(ID == 5){
            MaxReload = (int)((MaxReload * mult) + add);
        } else if(ID == 6){
            FireSpeed = (int)((FireSpeed * mult) + add);
        } else if(ID == 7){
            homingPower = (homingPower * mult) + add;
        } else if(ID == 8){
            bounces = (int)((bounces * mult) + add);
        } else if(ID == 9){
            MaxHealth = (MaxHealth * mult) + add;
        } else if(ID == 10){
            MoveSpeed = (MoveSpeed * mult) + add;
        } else if(ID == 11){
            DashCooldown = (int)((DashCooldown * mult) + add);
        } else if(ID == 12){
            explosionLevel = ((explosionLevel * mult) + add);
        } else if(ID == 13){
            acidLevel = ((acidLevel * mult) + add);
        } else if(ID == 14){
            explosionPower = ((explosionPower * mult) + add);
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        me = this;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(Damage <= 0){
            Damage = 0;
        }
        if(MaxAmmo < 1){
            MaxAmmo = 1;
        }
    }
}
