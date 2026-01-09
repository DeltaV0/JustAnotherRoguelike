using UnityEngine;

public class FancyAddon : MonoBehaviour
{
    public GameObject owner;

    public ParticleSystem parts;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(owner == null){
            parts.Stop();
            parts.transform.parent = null;
            Destroy(transform.root.gameObject);
        }
        if(owner != null){
            transform.root.position = owner.transform.position;
            transform.root.rotation = owner.transform.rotation;
        }
        
    }
}
