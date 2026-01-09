using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.AI;
using NavMeshPlus.Components;

public class TemplatesRooms : MonoBehaviour
{
    public GameObject[] top;
    public GameObject[] bottom;
    public GameObject[] left;
    public GameObject[] right;

    public GameObject Empty;

    public static TemplatesRooms me;

    public List<Room> rooms;

    public Player player;

    public int Timer;

    public bool done;

    public NavMeshSurface surface;

    public Transform ExitLocation;

    public Color[] Colours;

    public Transform roomHolder;

    public GameObject spawnRoom;

    public bool shopping;

    public GameObject Shop;

    public bool LookingAtItem;

    public Transform ItemLookingAt;

    public List<int> itemsInWorld;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Timer = 50;
        done = false;
        me = this;
        //Instantiate(spawnRoom, transform.position, transform.rotation);
        PlayerStats.me.Level = 1;
        //q.transform.parent = roomHolder;
    }

    public void GoShopping(){
        transform.position = new Vector3(0, 0, 0);
        itemsInWorld.Clear();
        shopping = true;
        foreach (Transform child in roomHolder)
        {
            Destroy(child.gameObject);
        }
        player = null;
        GameObject q = Instantiate(Shop, transform.position, transform.rotation);
        q.transform.parent = roomHolder;
        GoerCalculate.me.Reset();
    }

    public void RemakeRooms(){
        itemsInWorld.Clear();
        shopping = false;
        PlayerStats.me.Level++;
        //foreach (Transform child in roomHolder)
        //{
           // Destroy(child.gameObject);
        //}
        GoerCalculate.me.Reset();
        ExitLocation = null;
        player = null;
        Timer = 50;
        done = false;
        rooms.Clear();
        //Instantiate(spawnRoom, transform.position, transform.rotation);
        //q.transform.parent = roomHolder;
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        /*if(Timer > 0){
            Timer--;
        } else {
            if(!done){
                for(int i = 0; i < rooms.Count; i++){
                    rooms[i].transform.parent = roomHolder;
                }
                surface.BuildNavMesh();
                rooms[0].SpawnPlayer();
                int rand = UnityEngine.Random.Range((int)(rooms.Count * 0.75f), rooms.Count - 1);
                rooms[rand].SpawnExit();
                rooms[rand].RoomType = 0;
                ExitLocation = rooms[rand].transform;
                done = true;
                GoerCalculate.me.CalcPath();
            }
            
        }*/
    }
}
