using UnityEngine;

public class Exit : MonoBehaviour
{
    public bool ShopExit; 
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        TemplatesRooms.me.ExitLocation = transform;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other){
        if(other.CompareTag("Player")){
            if(ShopExit){
                Generator.me.RemakeRooms();
            } else {
                TemplatesRooms.me.GoShopping();
            }
            
            Destroy(gameObject);
        }
    }
}
