using UnityEngine;
using System.Collections;
using UnityEngine.AI;
using System.Collections.Generic;
using NavMeshPlus.Components;

public class Generator : MonoBehaviour
{
    public List<Room> Rooms;

    public Room CurrentRoom;

    public Vector2 OldPos;

    public Vector2 CurrentPos;

    public GameObject RoomPF;

    public GameObject InitRoomPF;

    public Transform roomHolder;

    public NavMeshSurface surface;

    public Transform ExitLocation;

    public static Generator me;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        me = this;
        Rooms.Clear();
        StartCoroutine(Generate());
        
    }

    public void RemakeRooms(){
        transform.position = new Vector3(0, 0, 0);
        Rooms.Clear();
        foreach (Transform child in roomHolder)
        {
            Destroy(child.gameObject);
        }
        TemplatesRooms.me.itemsInWorld.Clear();
        TemplatesRooms.me.shopping = false;
        PlayerStats.me.Level++;
        GoerCalculate.me.Reset();
        ExitLocation = null;
        TemplatesRooms.me.player = null;
        StartCoroutine(Generate());
    }

    IEnumerator Generate(){
        yield return new WaitForSecondsRealtime(0.1f);
        GameObject newRoom1 = Instantiate(InitRoomPF, transform.position, Quaternion.identity);
        Room NRReoom = newRoom1.GetComponent<Room>();
        NRReoom.RoomType = 0;
        Rooms.Add(NRReoom);
        CurrentRoom = Rooms[0];
        Debug.Log("init");

        for(int i = 0; i < 20; i++){
            
            OldPos = (Vector2)transform.position;
            
            bool Overlap = false;
            int R = UnityEngine.Random.Range(0, 4);
            if(R == 0){
                transform.position += new Vector3(0, 20, 0); //going up
            } else if(R == 1){
                transform.position += new Vector3(0, -20, 0); //going down
            } else if(R == 2){
                transform.position += new Vector3(20, 0, 0); //going right
            } else if(R == 3){
                transform.position += new Vector3(-20, 0, 0); //going left
            }
            CurrentPos = (Vector2)transform.position;
            foreach(Room r in Rooms){
                if((Vector2)r.transform.position == CurrentPos){
                    Debug.Log("Touched Old Room");
                    Overlap = true;
                    if(R == 0){
                        r.Walls[3].SetActive(false);
                        if(CurrentRoom != null){
                        CurrentRoom.Walls[2].SetActive(false);
                        }
                    } else if(R == 1){
                        r.Walls[2].SetActive(false);
                        if(CurrentRoom != null){
                        CurrentRoom.Walls[3].SetActive(false);
                        }
                    } else if(R == 2){
                        r.Walls[0].SetActive(false);
                        if(CurrentRoom != null){
                        CurrentRoom.Walls[1].SetActive(false);
                        }
                    } else if(R == 3){
                        r.Walls[1].SetActive(false);
                        if(CurrentRoom != null){
                        CurrentRoom.Walls[0].SetActive(false);
                        }
                    }
                    CurrentRoom = r;
                    //CurrentRoom = null;
                    //transform.position = new Vector3(UnityEngine.Random.Range(-2, 2) * 20f, UnityEngine.Random.Range(-2, 2) * 20f, 0);
                    if(Rooms.Count < Mathf.Pow(7, 2)){
                        i--;
                    }
                    
                    //do this if meets back
                }
            }
            if(Mathf.Abs(CurrentPos.x) / 20 >= 4 || Mathf.Abs(CurrentPos.y) / 20 >= 4 ){
                Debug.Log("Out Of Bounds");
                Overlap = true;
                transform.position = (Vector3)OldPos;
                if(Rooms.Count < Mathf.Pow(7, 2)){
                        i--;
                    }
                if(CurrentRoom != null){
                if(R == 0){
                    CurrentRoom.Walls[2].SetActive(true);
                } else if(R == 1){
                    CurrentRoom.Walls[3].SetActive(true);
                } else if(R == 2){
                    CurrentRoom.Walls[1].SetActive(true);
                } else if(R == 3){
                    CurrentRoom.Walls[0].SetActive(true);
                }
                }
            }
            if(!Overlap){
                yield return new WaitForSecondsRealtime(0.1f);
                GameObject newRoom = Instantiate(RoomPF, transform.position, Quaternion.identity);
                Room NRRoom = newRoom.GetComponent<Room>();
                Rooms.Add(NRRoom);
                Debug.Log("New Room");
                if(R == 0){
                    if(CurrentRoom != null){
                        CurrentRoom.Walls[2].SetActive(false);
                    }
                    NRRoom.Walls[3].SetActive(false);
                    
                } else if(R == 1){
                    if(CurrentRoom != null){
                        CurrentRoom.Walls[3].SetActive(false);
                    }
                    NRRoom.Walls[2].SetActive(false);
                    
                } else if(R == 2){
                    if(CurrentRoom != null){
                        CurrentRoom.Walls[1].SetActive(false);
                    }
                    NRRoom.Walls[0].SetActive(false);
                    
                } else if(R == 3){
                    if(CurrentRoom != null){
                        CurrentRoom.Walls[0].SetActive(false);
                    }
                    NRRoom.Walls[1].SetActive(false);
                    
                }
                CurrentRoom = NRRoom;
            }
            //if(Rooms.Count >= Mathf.Pow(5, 2)){
                //i = 10000;
            //}
            Debug.Log(i + "");

        }
        yield return new WaitForSecondsRealtime(0.1f);
        //when done
        for(int i = 0; i < Rooms.Count; i++){
            Rooms[i].transform.parent = roomHolder;
            
        }
        yield return new WaitForSecondsRealtime(0.1f);
        //TemplatesRooms.me.rooms = Rooms;
        surface.BuildNavMesh();
        ExitLocation = GetFarthestRoom().transform;
        GoerCalculate.me.CalcPath();
        Rooms[0].SpawnPlayer();
        //int rand = UnityEngine.Random.Range((int)(rooms.Count * 0.75f), rooms.Count - 1);
        GetFarthestRoom().SpawnExit();
        GetFarthestRoom().RoomType = 0;
        //ExitLocation = Rooms[Rooms.Count - 1].transform;
        //TemplatesRooms.me.ExitLocation = ExitLocation;
        //done = true;
        
        Debug.Log("Done");
    }

    public Room GetFarthestRoom(){
        Room q = null;
        float d = 0;
        for(int i = 0; i < Rooms.Count; i++){
            if(Vector3.Distance(Rooms[i].transform.position, Rooms[0].transform.position) >= d){
                q = Rooms[i];
                d = Vector3.Distance(Rooms[i].transform.position, Rooms[0].transform.position);
            }
        }
        return q;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
