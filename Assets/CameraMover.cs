using UnityEngine;

public class CameraMover : MonoBehaviour
{
    public Camera cam;

    public bool Overview;

    public static CameraMover me;

    public bool WorldMap;

    public Vector3 campos;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        me = this;   
    }

    void Update(){
        if(Input.GetKeyDown("tab") && TemplatesRooms.me.player.currentRoom.enemyCount <= 0){
            WorldMap = !WorldMap;
        }
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        
        if(WorldMap){
            float moveX = Input.GetAxisRaw ("Horizontal");
		    float moveY = Input.GetAxisRaw ("Vertical");
		
		    Vector2 moveDir = new Vector2 (moveX, moveY).normalized;

            campos += (Vector3)moveDir;
            transform.position = Vector3.Lerp(transform.position, campos, 0.1f);
            cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, 20f, 0.1f);
        } else if(TemplatesRooms.me.shopping){
            
            if(TemplatesRooms.me.LookingAtItem){
                transform.position = Vector3.Lerp(transform.position, Vector3.Lerp(TemplatesRooms.me.ItemLookingAt.position, Vector3.Lerp(new Vector3(TemplatesRooms.me.player.transform.position.x, TemplatesRooms.me.player.transform.position.y, -10f), (Vector3)Camera.main.ScreenToWorldPoint(Input.mousePosition), 0.25f), 0.25f), 0.1f);
                cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, 5f, 0.1f);
            } else {
                transform.position = Vector3.Lerp(transform.position, Vector3.Lerp(new Vector3(0, 0, -10f), Vector3.Lerp(new Vector3(TemplatesRooms.me.player.transform.position.x, TemplatesRooms.me.player.transform.position.y, -10f), (Vector3)Camera.main.ScreenToWorldPoint(Input.mousePosition), 0.25f), 0.5f), 0.1f);
                cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, 8f, 0.1f);
            }
            campos = transform.position;
            
        } else if(TemplatesRooms.me.player != null){
            if(TemplatesRooms.me.LookingAtItem){
                transform.position = Vector3.Lerp(transform.position, Vector3.Lerp(TemplatesRooms.me.ItemLookingAt.position, Vector3.Lerp(new Vector3(TemplatesRooms.me.player.transform.position.x, TemplatesRooms.me.player.transform.position.y, -10f), (Vector3)Camera.main.ScreenToWorldPoint(Input.mousePosition), 0.25f), 0.25f), 0.1f);
                cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, 5f, 0.1f);
            } else {
            transform.position = Vector3.Lerp(transform.position, Vector3.Lerp(new Vector3(TemplatesRooms.me.player.currentRoom.transform.position.x, TemplatesRooms.me.player.currentRoom.transform.position.y, -10f), Vector3.Lerp(new Vector3(TemplatesRooms.me.player.transform.position.x, TemplatesRooms.me.player.transform.position.y, -10f), (Vector3)Camera.main.ScreenToWorldPoint(Input.mousePosition), 0.25f), 0.5f), 0.1f);
            cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, 7f, 0.1f);
            }
            campos = transform.position;
        } else {
            transform.position = Vector3.Lerp(transform.position, new Vector3(0, 0, -10), 0.1f);
            cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, GetFarthestRoom() + 8f, 0.1f);
            campos = transform.position;
        }
    }

    public float GetFarthestRoom(){
        float q = 0;
        for(int i = 0; i < TemplatesRooms.me.rooms.Count; i++){
            if(Vector3.Distance(TemplatesRooms.me.rooms[i].transform.position, TemplatesRooms.me.rooms[0].transform.position) > q){
                q = Vector3.Distance(TemplatesRooms.me.rooms[i].transform.position, TemplatesRooms.me.rooms[0].transform.position);
            }
        }
        return q;
    }
}
