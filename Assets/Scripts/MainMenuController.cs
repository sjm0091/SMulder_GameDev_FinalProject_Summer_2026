using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip ButtonClickClip;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClickStart()
    {
        audioSource.PlayOneShot(ButtonClickClip);
        SceneManager.LoadScene("MainScene");
    }

    public void OnClickQuit()
    {
        Debug.Log("Quit");
         audioSource.PlayOneShot(ButtonClickClip);
        Application.Quit();
       
    }
}
