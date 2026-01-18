using UnityEngine;
using System.Collections;

public class LazerOneShot : MonoBehaviour
{
    public LineRenderer[] gos;

    //public GameObject Owner;

    public AudioSource shootSFX;

    public float[] chargeTimes;

    public float BeamWidth;

    public float damage;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(Shoot());
    }

        


    IEnumerator Shoot(){
        RaycastHit2D hit; 
        hit = Physics2D.Raycast(transform.position, transform.up,25f, LayerMask.GetMask("Wall"));
        Debug.Log(hit.point + " " + transform.position);
        gos[1].positionCount = 25;
        gos[0].SetPosition(0, transform.position);
        gos[1].SetPosition(0, transform.position);
        gos[0].SetPosition(1, (Vector3)hit.point);
        gos[1].SetPosition(24, (Vector3)hit.point);
        LerpBetween((Vector3)hit.point);
        gos[0].widthMultiplier = 0;
        gos[1].widthMultiplier = 0;
        gos[1].gameObject.SetActive(false);
        for(int i = 0; i < chargeTimes[0] * 50; i++){
            gos[0].widthMultiplier = Mathf.Pow( (i / (chargeTimes[0] * 50)), 0.25f) * BeamWidth;
            yield return new WaitForSeconds(0.02f);
        }
        gos[1].gameObject.SetActive(true);
        shootSFX.pitch = Random.Range(0.9f, 1.1f);
        shootSFX.Play();
        RaycastHit2D hit2 = Physics2D.BoxCast((Vector2)transform.position, new Vector2(BeamWidth, BeamWidth), transform.eulerAngles.z, transform.up, Vector3.Distance(transform.position, hit.point), LayerMask.GetMask("Player"));
        if(hit2 && TemplatesRooms.me.player.DashTimer[1] > 0){
            TemplatesRooms.me.player.Hurt(damage);
        }
        gos[0].widthMultiplier = BeamWidth;
        gos[1].widthMultiplier = BeamWidth;
        for(int i = 0; i < chargeTimes[1] * 50; i++){
            gos[0].widthMultiplier = Mathf.Pow( (1 - (i / (chargeTimes[1] * 50))), 4f) * BeamWidth;
            gos[1].widthMultiplier = Mathf.Pow( (1 - (i / (chargeTimes[1] * 50))), 4f) * BeamWidth;
            yield return new WaitForSeconds(0.02f);
        }
        Destroy(gameObject);
    }

    public void LerpBetween(Vector3 to){
        for(int i = 1; i < 24; i++){
           gos[1].SetPosition(i, Vector3.Lerp(transform.position, to, ((float)i) / 24));
        }
    }
}
