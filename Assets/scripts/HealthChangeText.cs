using UnityEngine;
using TMPro;

public class HealthChangeText : MonoBehaviour
{
    public TMP_Text text;

    //public int change;

    public Rigidbody2D rb;
    
    public void Activate(float i){
        if(i == 0){
            text.text = "<color=White>" + (Mathf.Ceil(i * 10) / 10) + "</color>";
        } else if(i < 0){
            text.text = "<color=Red>" + (Mathf.Ceil(i * 10) / 10) + "</color>";
        } else if(i > 0){
            text.text = "<color=Green>+" + (Mathf.Ceil(i * 10) / 10) + "</color>";
        }
        rb.AddForce(new Vector2(Mathf.Cos((Random.Range(0, 360f) + 90f) * Mathf.Deg2Rad) * 2f, Mathf.Sin((Random.Range(0, 360f) + 90f) * Mathf.Deg2Rad) * 2f), ForceMode2D.Impulse);
        Invoke("Kill", 1f);
    }

    public void Kill(){
        Destroy(gameObject);
    }
}
