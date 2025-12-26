using UnityEngine;

public class CameraMover : MonoBehaviour
{
    public Camera cam;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(TemplatesRooms.me.player != null){
            transform.position = Vector3.Lerp(transform.position, Vector3.Lerp(new Vector3(TemplatesRooms.me.player.currentRoom.transform.position.x, TemplatesRooms.me.player.currentRoom.transform.position.y, -10f), new Vector3(TemplatesRooms.me.player.transform.position.x, TemplatesRooms.me.player.transform.position.y, -10f), 0.5f), 0.1f);
            cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, 6f, 0.1f);
        } else {
            transform.position = Vector3.Lerp(transform.position, new Vector3(0, 0, -10), 0.1f);
            cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, GetFarthestRoom() + 6f, 0.1f);
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
