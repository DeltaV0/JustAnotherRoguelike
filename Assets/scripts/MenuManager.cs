using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MainMenu(){
        SceneManager.LoadSceneAsync("Main");
    }
    public void Gaming(){
        SceneManager.LoadSceneAsync("SampleScene");
    }
}
