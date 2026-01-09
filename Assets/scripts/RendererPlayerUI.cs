using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RendererPlayerUI : MonoBehaviour
{
    public PlayerStats stats;
    public TMP_Text textMoney;
    public TMP_Text Health;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(TemplatesRooms.me.player != null){
        textMoney.text = stats.Money + " Pesos";
        Health.text = (Mathf.Round(TemplatesRooms.me.player.health * 10f) / 10f) + "/" +( Mathf.Round(PlayerStats.me.MaxHealth * 10f) / 10f) + " Health";
        } else {
            textMoney.text = "";
            Health.text = "";
        }
    }
}
