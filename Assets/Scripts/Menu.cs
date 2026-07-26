using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public GameObject confirmPopup;
    public GameObject menu;
    public AudioSource audioSource;
    public AudioClip ButtonClickClip;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        confirmPopup.SetActive(false);
        menu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnMenuOpen()
    {
        audioSource.PlayOneShot(ButtonClickClip);
        menu.SetActive(true);
    }

    public void OnClickReturnToMenu()
    {
        audioSource.PlayOneShot(ButtonClickClip);
        SceneManager.LoadScene("MainMenu");
    }

    public void OnClickOpenConfirmPopup()
    {
        audioSource.PlayOneShot(ButtonClickClip);
        confirmPopup.SetActive(true);
        
    }

    public void OnClickCancel()
    {
        audioSource.PlayOneShot(ButtonClickClip);
        confirmPopup.SetActive(false);
    }

    public void OnClickBackToGame()
    {
        audioSource.PlayOneShot(ButtonClickClip);
        menu.SetActive(false);
    }

    public void OnRestartGame()
    {
        audioSource.PlayOneShot(ButtonClickClip);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
