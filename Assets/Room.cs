using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using TMPro;

public class Room : MonoBehaviour
{

    public TemplatesRooms rt;

    public GameObject exit;
    public GameObject player;

    public bool CanBeItemRoom;
    public bool CanBeEnemyRoom;

    public Transform[] enemySpawnPoints;

    public bool summoned;

    public int RoomType;

    public int enemyCount;

    public SpriteRenderer spr;

    public GameObject Blocker;

    public TMP_Text text;

   // public Color[] Colors;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(RoomType > 0){
            // 0 = spawn room, - = preset for some reason
            
            if(CanBeItemRoom){
                if(CanBeEnemyRoom){
                    RoomType = UnityEngine.Random.Range(1, 11);
                } else {
                    RoomType = 10;
                }
            } else {
                if(CanBeEnemyRoom){
                    RoomType = 1;
                } else {
                    RoomType = 0;
                }
            }
        }

        RoomType = Math.Abs(RoomType);
        rt = TemplatesRooms.me;
        TemplatesRooms.me.Timer = 50;
        TemplatesRooms.me.rooms.Add(this);
    }

    public void SpawnPlayer(){
        GameObject q = Instantiate(player, transform.position, transform.rotation);
        q.transform.parent = TemplatesRooms.me.roomHolder;
        
    }

    public void SpawnExit(){
        //TemplatesRooms
        GameObject q = Instantiate(exit, transform.position, transform.rotation);
        q.transform.parent = TemplatesRooms.me.roomHolder;

    }

    // Update is called once per frame
    void Update()
    {
        Blocker.SetActive(enemyCount > 0);
        if(summoned && enemyCount <= 0 && RoomType < 10 && RoomType > 0){
            spr.color = rt.Colours[1];
        } else {
            spr.color = rt.Colours[0];
        }
        if(text != null){
            text.text = PlayerStats.me.Level + "";
        }
    }

    void OnTriggerEnter2D(Collider2D other){
        if(other.CompareTag("Player") && TemplatesRooms.me.player != null){
            TemplatesRooms.me.player.currentRoom = this;
            if(!summoned){
            summoned = true;
            if(RoomType < 10 && RoomType > 0){
                for(int i = 0; i < UnityEngine.Random.Range(1, (PlayerStats.me.Level / 2) + 3); i++){
                    bool placed = false;
                    Vector3 RandomPos;
                    while(!placed){
                        RandomPos.x = UnityEngine.Random.Range(-15f/2f, 15f/2f);
                        RandomPos.y = UnityEngine.Random.Range(-15f/2f, 15f/2f);
                        RandomPos.z = 0;
                        if(Physics2D.OverlapCircle(new Vector2(RandomPos.x, RandomPos.y), 0.75f, LayerMask.GetMask("Wall")) == null){
                            placed = true;
                            enemyCount++;
                            GameObject e = Instantiate(Prefabs.me.Enemy[UnityEngine.Random.Range(0, Prefabs.me.Enemy.Length)], (Vector3)(UnityEngine.Random.insideUnitCircle * 5f ) + transform.position, Quaternion.identity);
                            e.transform.GetComponent<EnemyIdentifier>().AI.room = this;
                        }
                    }
                    
                }
            } else if(RoomType == 10){
                for(int i = -1; i < 2; i++){
                    GameObject e = Instantiate(Prefabs.me.Items[UnityEngine.Random.Range(0, Prefabs.me.Items.Length)], transform.position + new Vector3(i * 4, 0, 0), Quaternion.identity);
                    e.transform.parent = TemplatesRooms.me.roomHolder;
                }
            }
            }
            //Spawned = true;
        }
    }
}
