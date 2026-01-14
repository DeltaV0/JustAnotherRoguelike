using UnityEngine;
using TMPro;
using System;

public class Item : MonoBehaviour
{
    [System.Serializable]
    public class Effect{

        public bool HardCoded;

        public int stat;
        public float ADD;
        public float MULT;
    }

    public string name;
    public string desc;

    public int ID;

    public int price;

    public int basePrice;

    public int priceInflation;

    public Sprite sprite;

    public int Requirement;
    public int DisableOnPurchase;

    public bool unique;

    [Tooltip("IMPORTANT:\n0: Damage (float)\n1: Bullet Speed (float)\n2: Shots (Int)\n3: Inaccuracy (float)\n4: Max Ammo (Int)\n5: Max Reload (Int) (Timer)\n6: FireSpeed (Int) (Timer)\n7: Homing Power (float)\n8: Bounces (Int)\n9: Player Max Health (float)\n10: Move Speed (float)\n11: Dash Cooldown (Int) (Timer)\n12: Explosion Level (float)\n13: Acid Level (float)\n14: Explosion Power (float)\n15: Fire Length (int) (Seconds)")]
    public Effect[] effects;

   // public GameObject Interactible;

    public SpriteRenderer spr;

    public TMP_Text[] text;

    //private bool Correct;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       // Correct = false;
        spr.sprite = sprite;

        price = basePrice;

        for(int i = 0; i < PlayerStats.me.Items.Count; i++){
            //if(PlayerStats.me.Items[i] == ID){
                price += (int)(priceInflation * 0.75f);
            //}
        }
        price += (int)(priceInflation * 0.1f * PlayerStats.me.Level);
        if((unique && TemplatesRooms.me.itemsInWorld.Contains(ID))){
            GameObject e = Instantiate(Prefabs.me.Misc[0], transform.position, Quaternion.identity);
            e.transform.parent = TemplatesRooms.me.roomHolder;
            Destroy(gameObject);
        }
        if((Requirement == 0 || PlayerStats.me.Items.Contains(Requirement)) && (DisableOnPurchase == 0 || !PlayerStats.me.Items.Contains(DisableOnPurchase)) ){
            
        } else {
            GameObject e = Instantiate(Prefabs.me.Misc[0], transform.position, Quaternion.identity);
            e.transform.parent = TemplatesRooms.me.roomHolder;
            Destroy(gameObject);
        }
        TemplatesRooms.me.itemsInWorld.Add(ID);
    }

    // Update is called once per frame
    void Update()
    {
        if(TemplatesRooms.me.player != null && Vector3.Distance(transform.position, TemplatesRooms.me.player.transform.position) < 3){
            TemplatesRooms.me.ItemLookingAt = transform;
            TemplatesRooms.me.LookingAtItem = true;
            //Interactible.SetActive(true);
            
        } else {
            if(TemplatesRooms.me.ItemLookingAt == transform){
            TemplatesRooms.me.LookingAtItem = false;
            }
            
        }

        if(TemplatesRooms.me.player != null && Vector3.Distance(transform.position, TemplatesRooms.me.player.transform.position) < 2){
            //Interactible.SetActive(true);
            
            if(PlayerStats.me.Money >= price){
            text[0].text = "Buy? (E)";
            text[1].text = "You'll have " + (PlayerStats.me.Money - price) + " pesos left";
            text[2].text = "Cost: " + price + " pesos";
            if(Input.GetKeyDown("e")){
                Buy();
            }
            } else {
                text[0].text = "You're BROKE!!";
                text[1].text = "You need " + (price - PlayerStats.me.Money) + " more pesos";
                text[2].text = "Cost: " + price + " pesos";
            }
        } else {
            text[0].text = name;
            text[1].text = desc;
            text[2].text = "Cost: " + price + " pesos";
        }
        
    }

    void Buy(){
        TemplatesRooms.me.itemsInWorld.Remove(ID);
        TemplatesRooms.me.LookingAtItem = false;
        PlayerStats.me.Money -= price;
        PlayerStats.me.Items.Add(ID);

        /*switch (ID)
        {
            default:
                break;
            case 1:
                //heavyBullet
                PlayerStats.me.Damage++;
                PlayerStats.me.bulletSpeed *= 0.75f;
                break;
            case 2:
                //multishot
                PlayerStats.me.Damage = (PlayerStats.me.Damage * 0.5f);
                PlayerStats.me.shots *= 2;
                PlayerStats.me.inaccuracy *= 2f;
                break;
            case 3:
                //quickReload
                PlayerStats.me.MaxAmmo--;
                PlayerStats.me.MaxReload = (int)(PlayerStats.me.MaxReload * 0.75f);
                break;
            case 4:
                //extmag
                PlayerStats.me.MaxAmmo = (int)(PlayerStats.me.MaxAmmo * 1.25f);
                PlayerStats.me.MaxReload = (int)(PlayerStats.me.MaxReload * 1.25f);
                break;
            case 5:
                //Quickshot
                PlayerStats.me.bulletSpeed *= 1.25f;
                PlayerStats.me.FireSpeed = (int)(PlayerStats.me.FireSpeed * 0.75f);
                PlayerStats.me.Damage = (PlayerStats.me.Damage * 0.75f);
                break;
            case 6:
                //homing
                PlayerStats.me.inaccuracy *= 1.5f;
                PlayerStats.me.homingPower += 0.01f;
                break;
        }*/
        for(int i = 0; i < effects.Length; i++){
            if(!effects[i].HardCoded){
                PlayerStats.me.ChangeStat(effects[i].stat, effects[i].ADD, effects[i].MULT + 1f);
            }
            //do the hardcoded effect i guess
        }
        Destroy(gameObject);
    }
}
