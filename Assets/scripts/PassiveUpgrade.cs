using UnityEngine;

public class PassiveUpgrade : MonoBehaviour
{

    public SpriteRenderer spr;

   // public TMP_Text[] text;

    public string name;
    public string desc;

    public int ID;

    public int price;

    public int basePrice;

    public int priceInflation;

    public Sprite sprite;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //price += (int)(priceInflation * 0.1f * PlayerStats.me.Level);
        //if((unique && TemplatesRooms.me.itemsInWorld.Contains(ID))){
           // GameObject e = Instantiate(Prefabs.me.Misc[0], transform.position, Quaternion.identity);
           // e.transform.parent = TemplatesRooms.me.roomHolder;
           // Destroy(gameObject);
        //}
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
