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

    public bool Visited;

   // public Color[] Colors;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Visited = false;
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
        Visited = true;
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
        if(CameraMover.me.WorldMap && Visited){
            spr.color = rt.Colours[2];
        } else if(summoned && enemyCount <= 0 && RoomType < 10 && RoomType > 0){
            spr.color = rt.Colours[1];
        } else {
            spr.color = rt.Colours[0];
        }
        if(text != null){
            text.text = PlayerStats.me.Level + "";
        }
        if(TemplatesRooms.me.player != null && TemplatesRooms.me.player.currentRoom == this){
            if(enemyCount > 0){
                MusicManager.me.volumes[1] = 0.1f;
                MusicManager.me.volumes[0] = 0f;
                MusicManager.me.volumes[2] = 0f;
            } else if(RoomType < 10){
                MusicManager.me.volumes[0] = 0.1f;
                MusicManager.me.volumes[1] = 0f;
                MusicManager.me.volumes[2] = 0f;
            } else {
                MusicManager.me.volumes[0] = 0f;
                MusicManager.me.volumes[1] = 0f;
                MusicManager.me.volumes[2] = 0.1f;
            }
        }
    }

    void OnMouseDown(){
        if(CameraMover.me.WorldMap && Visited){
            if(TemplatesRooms.me.player != null){
                Prefabs.me.PlaySFX(0, 0.25f);
                TemplatesRooms.me.player.timer = PlayerStats.me.FireSpeed;
                TemplatesRooms.me.player.transform.position = transform.position;
                CameraMover.me.WorldMap = false;
                TemplatesRooms.me.player.ReloadTimer++;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other){
        if(other.CompareTag("Player") && TemplatesRooms.me.player != null){
            TemplatesRooms.me.player.currentRoom = this;
            if(!summoned){
            Visited = true;
            summoned = true;
            if(RoomType < 10 && RoomType > 0){
                for(int i = 0; i < UnityEngine.Random.Range(1, (PlayerStats.me.Level) + 3); i++){
                    bool placed = false;
                    Vector3 RandomPos;
                    while(!placed){
                        RandomPos.x = UnityEngine.Random.Range(-10f, 10f);
                        RandomPos.y = UnityEngine.Random.Range(-10f, 10f);
                        RandomPos.z = 0;
                        if(Physics2D.OverlapCircle(new Vector2(RandomPos.x, RandomPos.y), 0.75f, LayerMask.GetMask("Wall")) == null){
                            placed = true;
                            enemyCount++;
                            if(i == 0){
                                GameObject e = Instantiate(Prefabs.me.EnemyInit[UnityEngine.Random.Range(0, Prefabs.me.EnemyInit.Length)], (Vector3)(UnityEngine.Random.insideUnitCircle * 5f ) + transform.position, Quaternion.identity);
                                e.transform.GetComponent<EnemyIdentifier>().AI.room = this;
                            } else {

                                GameObject e = Instantiate(Prefabs.me.All[UnityEngine.Random.Range(0, Prefabs.me.All.Length)], (Vector3)(UnityEngine.Random.insideUnitCircle * 5f ) + transform.position, Quaternion.identity);
                                e.transform.GetComponent<EnemyIdentifier>().AI.room = this;
                            }
                            
                        }
                    }
                    
                }
            } else if(RoomType == 10){
                for(int i = -1; i < 2; i++){
                    GameObject e = Instantiate(Prefabs.me.Items[UnityEngine.Random.Range(0, Prefabs.me.Items.Length)], transform.position + new Vector3(i * 5, 0, 0), Quaternion.identity);
                    e.transform.parent = TemplatesRooms.me.roomHolder;
                }
            }
            }
            //Spawned = true;
        }
    }
}
