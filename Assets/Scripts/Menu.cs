using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public GameObject confirmPopup;
    public GameObject menu;
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

        menu.SetActive(true);
    }

    public void OnClickReturnToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void OnClickOpenConfirmPopup()
    {
        confirmPopup.SetActive(true);
    }

    public void OnClickCancel()
    {
        confirmPopup.SetActive(false);
    }

    public void OnClickBackToGame()
    {
        menu.SetActive(false);
    }

    public void OnRestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
