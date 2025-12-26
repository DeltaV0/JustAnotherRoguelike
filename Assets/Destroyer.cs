using UnityEngine;

public class Destroyer : MonoBehaviour
{
    public bool EmptyRoom;
    void OnTriggerEnter2D(Collider2D other){
        if(other.CompareTag("Point")){
            if(EmptyRoom && other.GetComponent<Destroyer>() != null){
                //oops, something broke and placed an empty room on top of an actual room
                //workaround by just not doing that
                Destroy(transform.parent.parent.gameObject);
            }
            if(other.GetComponent<RoomSpawner>() != null){
            other.GetComponent<RoomSpawner>().Spawned = true;
            } else {
            //Destroy(other.transform.parent.parent.gameObject);
            }
        }
    }

    void Update(){
        if(TemplatesRooms.me.done && GoerCalculate.me.path.status == UnityEngine.AI.NavMeshPathStatus.PathComplete){
            Destroy(gameObject);
        }
    }
}
