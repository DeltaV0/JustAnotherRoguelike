using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

public class Lightning : MonoBehaviour
{
    public List<GameObject> prevHit = new List<GameObject>();

    public List<Vector3> prevHitPos = new List<Vector3>();

    public LineRenderer lr;

    public float Damage;

    public Vector3 Origin;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(zap()); 
    }

    IEnumerator zap()
    {
        Origin = transform.position;
        lr.positionCount = 1;
        lr.SetPosition(0, transform.position);
        bool HitSomething;
        for(int i = 1; i < 5; i++){
            Collider2D[] cols;
            cols = Physics2D.OverlapCircleAll((Vector2)transform.position, 5f, LayerMask.GetMask("Enemy"));
            cols = cols.OrderBy((d) => (d.transform.position - transform.position).sqrMagnitude).ToArray();
            HitSomething = false;
            for(int q = 0; q < cols.Length; q++){
                RaycastHit2D rHit = Physics2D.Linecast(transform.position, cols[q].transform.position, LayerMask.GetMask("Wall"));
                if(!rHit && !prevHit.Contains(cols[q].transform.root.gameObject)){
                    lr.positionCount = lr.positionCount + 1;
                    HitSomething = true;
                    transform.position = cols[q].transform.root.position;
                   // Origin = transform.position;
                    lr.SetPosition(i, transform.position);
                    prevHit.Add(cols[q].transform.root.gameObject);
                    prevHitPos.Add(transform.position);
                    if(cols[q].transform.root.gameObject != null && cols[q].transform.root.gameObject.GetComponent<EnemyIdentifier>() != null){
                        cols[q].transform.root.gameObject.GetComponent<EnemyIdentifier>().AI.Stun(PlayerStats.me.StunTimer);
                        cols[q].transform.root.gameObject.GetComponent<EnemyIdentifier>().AI.Slow(PlayerStats.me.BulletSlow);
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
            if(!HitSomething){
                lr.positionCount = lr.positionCount + 1;
                Vector3 moveDir = Vector3.zero;
                transform.position += ((Vector3)UnityEngine.Random.insideUnitCircle * 1f);
                //RaycastHit2D rHit = Physics2D.Linecast(transform.position, (Vector3)(UnityEngine.Random.insideUnitCircle * 0.25f) + transform.position, LayerMask.GetMask("Wall"));
                //if(!rHit){
                
                //} else {
                //    transform.position = rHit.point;
                //}
                for(int p = 0; p < prevHitPos.Count; p++){
                    moveDir += (transform.position - prevHitPos[p]).normalized;
                }
                moveDir += (transform.position - Origin);
                moveDir = moveDir.normalized * 2.5f;
                RaycastHit2D rHit = Physics2D.Linecast(transform.position, (Vector3)moveDir + transform.position, LayerMask.GetMask("Wall"));
                
                if(!rHit){
                    transform.position += moveDir;
                } else {
                    transform.position = rHit.point;
                }
                lr.SetPosition(i, transform.position);
                prevHitPos.Add(transform.position);
                yield return new WaitForSeconds(0.02f);
            }
            
        }
        for(int i = 0; i < 10; i++){
            lr.widthMultiplier = 0.1f - (((float)i / 25f) * 0.1f);
            yield return new WaitForSeconds(0.02f);
        }
        Destroy(gameObject);
    }
}
