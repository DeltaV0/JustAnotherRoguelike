using UnityEngine;
using System;

public class RoomSpawner : MonoBehaviour
{
    public int SpawnDir;

    public TemplatesRooms rt;

    public bool Spawned;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Spawned = false;
        rt = TemplatesRooms.me;
        Invoke("Spawn", 0.2f);
    }

    void SpawnEmpty(){
        if(!Spawned){
            GameObject q = Instantiate(TemplatesRooms.me.Empty, transform.position, transform.rotation);
                q.transform.parent = TemplatesRooms.me.roomHolder;
        }
    }

    void Update(){
        if(TemplatesRooms.me.done && GoerCalculate.me.path.status == UnityEngine.AI.NavMeshPathStatus.PathComplete){
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Spawn()
    {
        /*Collider2D[] colliders = Physics2D.OverlapCircleAll((Vector2)transform.position, 2f);
        foreach(Collider2D col in colliders){
            //wtf??
            if(col.CompareTag("Point")){
            
            if(col.GetComponent<Destroyer>() != null){
                Spawned = true;
            }
            if(col.GetComponent<RoomSpawner>() != null && col.GetComponent<RoomSpawner>().Spawned == false && Spawned){
                col.GetComponent<RoomSpawner>().Spawned = true;
            }
            if(col.GetComponent<RoomSpawner>() != null && col.GetComponent<RoomSpawner>().Spawned && !Spawned){
                Spawned = true;
            }
            if(col.GetComponent<RoomSpawner>() != null && col.GetComponent<RoomSpawner>().Spawned == false && Spawned == false){
                
                col.GetComponent<RoomSpawner>().Spawned = true;
                Spawned = true;
                GameObject q = Instantiate(TemplatesRooms.me.Empty, transform.position, transform.rotation);
                q.transform.parent = TemplatesRooms.me.roomHolder;
                
                //Invoke("SpawnEmpty", 0.1f);
                
            }
            Spawned = true; 
            
            }
        }*/
        if(!Spawned){
        if(SpawnDir == 1){
            if((transform.position.y / 20) >= 2){
                Debug.Log("edge");
                Instantiate(rt.bottom[UnityEngine.Random.Range(0, Math.Clamp(4 - ((int)(transform.position.y / 20) - 2), 1, 4))], transform.position, transform.rotation);
            } else {
                Instantiate(rt.bottom[UnityEngine.Random.Range(1, rt.bottom.Length)], transform.position, transform.rotation);
            }
            
            //q.transform.parent = TemplatesRooms.me.roomHolder;
            //bdoor
        } else if(SpawnDir == 2){
            if((transform.position.x / 20) >= 2){
                Debug.Log("edge");
                Instantiate(rt.left[UnityEngine.Random.Range(0, Math.Clamp(4 - ((int)(transform.position.x / 20) - 2), 1, 4))], transform.position, transform.rotation);
            } else {
                Instantiate(rt.left[UnityEngine.Random.Range(1, rt.left.Length)], transform.position, transform.rotation);
            }
            

            //q.transform.parent = TemplatesRooms.me.roomHolder;
            //ldoor
        } else if(SpawnDir == 3){
            if((transform.position.y / 20) <= -2){
                Debug.Log("edge");
                Instantiate(rt.top[UnityEngine.Random.Range(0, Math.Clamp(4 - ((int)(transform.position.y / 20) + 2), 1, 4))], transform.position, transform.rotation);
            } else {
                Instantiate(rt.top[UnityEngine.Random.Range(1, rt.top.Length)], transform.position, transform.rotation);
            }
            
            //q.transform.parent = TemplatesRooms.me.roomHolder;
            //tdoor
        } else if(SpawnDir == 4){
            if((transform.position.x / 20) <= -2){
                Debug.Log("edge");
                Instantiate(rt.right[UnityEngine.Random.Range(0, Math.Clamp(4 - ((int)(transform.position.x / 20) + 2), 1, 4))], transform.position, transform.rotation);
            } else {
                Instantiate(rt.right[UnityEngine.Random.Range(1, rt.right.Length)], transform.position, transform.rotation);
            }
            //q.transform.parent = TemplatesRooms.me.roomHolder;
            //rdoor
        }
        Spawned = true;
        }
    }

    void OnTriggerEnter2D(Collider2D other){
        if(other.CompareTag("Point")){
            
            if(other.GetComponent<Destroyer>() != null){
                Spawned = true;
            }
            if(other.GetComponent<RoomSpawner>() != null && other.GetComponent<RoomSpawner>().Spawned == false && Spawned){
                other.GetComponent<RoomSpawner>().Spawned = true;
            }
            if(other.GetComponent<RoomSpawner>() != null && other.GetComponent<RoomSpawner>().Spawned && !Spawned){
               Spawned = true;
            }
            if(other.GetComponent<RoomSpawner>() != null && other.GetComponent<RoomSpawner>().Spawned == false && Spawned == false){
                
                other.GetComponent<RoomSpawner>().Spawned = true;
                Spawned = true;
                GameObject q = Instantiate(TemplatesRooms.me.Empty, transform.position, transform.rotation);
                q.transform.parent = TemplatesRooms.me.roomHolder;
                
                //Invoke("SpawnEmpty", 0.1f);
                
            }
            Spawned = true; 
            
        }
    }
}
