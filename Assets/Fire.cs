using UnityEngine;
using System.Collections;

public class Fire : MonoBehaviour
{
    public GameObject Owner;

    public ParticleSystem parts;

    public float Damage;

    public float FireLength;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(Burn()); 
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(Owner != null){
        transform.position = Owner.transform.position;
        }
    }

    IEnumerator Burn()
    {
        for(int i = 0; i < FireLength; i++)
        {
            if(Owner != null && Owner.GetComponent<EnemyIdentifier>() != null)
            {
                Owner.GetComponent<EnemyIdentifier>().AI.Burning = true;
                Owner.GetComponent<EnemyIdentifier>().AI.health -= Damage;
                GameObject a = Instantiate(Prefabs.me.Misc[3], Owner.transform.position, Quaternion.identity);
                Owner.GetComponent<EnemyIdentifier>().AI.aud.PlayOneShot(Owner.GetComponent<EnemyIdentifier>().AI.audio[1], 0.25f);
                a.GetComponent<HealthChangeText>().Activate(-1 * Damage);
                
            }
            yield return new WaitForSeconds(1f);
        }
        if(Owner != null){
        Owner.GetComponent<EnemyIdentifier>().AI.Burning = false;
        }
        parts.Stop();
        this.enabled = false;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy") &&  !other.transform.root.gameObject.GetComponent<EnemyIdentifier>().AI.Burning)
        {
            GameObject f = Instantiate(Prefabs.me.PlaceableThings[2], other.transform.position, Quaternion.identity);
            f.transform.parent = TemplatesRooms.me.roomHolder;
            f.GetComponent<Fire>().Owner = other.transform.root.gameObject;
            f.GetComponent<Fire>().Damage = Damage;
            f.GetComponent<Fire>().FireLength = FireLength;
        }
    }
}
