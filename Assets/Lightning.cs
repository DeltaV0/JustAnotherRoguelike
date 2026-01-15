using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

public class Lightning : MonoBehaviour
{
    public List<GameObject> prevHit = new List<GameObject>();

    public LineRenderer lr;

    public float Damage;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(zap()); 
    }

    IEnumerator zap()
    {
        lr.positionCount = 1;
        lr.SetPosition(0, transform.position);
        for(int i = 1; i < 5; i++){
            Collider2D[] cols;
            cols = Physics2D.OverlapCircleAll((Vector2)transform.position, 5f, LayerMask.GetMask("Enemy"));
            cols = cols.OrderBy((d) => (d.transform.position - transform.position).sqrMagnitude).ToArray();
            for(int q = 0; q < cols.Length; q++){
                RaycastHit2D rHit = Physics2D.Linecast(transform.position, cols[q].transform.position, LayerMask.GetMask("Wall"));
                if(!rHit && !prevHit.Contains(cols[q].transform.root.gameObject)){
                    lr.positionCount = lr.positionCount + 1;
                    
                    transform.position = cols[q].transform.root.position;
                    lr.SetPosition(i, transform.position);
                    prevHit.Add(cols[q].transform.root.gameObject);
                    if(cols[q].transform.root.gameObject != null && cols[q].transform.root.gameObject.GetComponent<EnemyIdentifier>() != null){
                        cols[q].transform.root.gameObject.GetComponent<EnemyIdentifier>().AI.health -= Damage;
                        cols[q].transform.root.gameObject.GetComponent<EnemyIdentifier>().AI.aud.PlayOneShot( cols[q].transform.root.gameObject.GetComponent<EnemyIdentifier>().AI.audio[1], 0.25f);
                        GameObject a = Instantiate(Prefabs.me.Misc[3], cols[q].transform.root.position, Quaternion.identity);
                        a.GetComponent<HealthChangeText>().Activate(-1f * Damage);
                    } else if(cols[q].transform.gameObject != null && cols[q].transform.root.gameObject.GetComponent<Shield>() != null){
                        cols[q].transform.root.gameObject.GetComponent<Shield>().Hit(Damage);
                        GameObject a = Instantiate(Prefabs.me.Misc[3], cols[q].transform.position, Quaternion.identity);
                        a.GetComponent<HealthChangeText>().Activate(-1f * Damage);
                    }
                    yield return new WaitForSeconds(0.02f);
                    break;
                }
            }
            
        }
        for(int i = 0; i < 10; i++){
            lr.widthMultiplier = 0.1f - (((float)i / 25f) * 0.1f);
            yield return new WaitForSeconds(0.02f);
        }
        Destroy(gameObject);
    }
}
