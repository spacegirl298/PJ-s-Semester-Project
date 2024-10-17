using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    private FirstPersonControls firstPersonControls;
    private Controls Controls;

    public GameObject pauseMenu;
    public GameObject Player;

    public bool isPaused = false;
    // Start is called before the first frame update

    void Awake()
    {
        firstPersonControls = Player.GetComponent<FirstPersonControls>();

        Controls = new Controls();
        Controls.Player.Enable();

        Controls.Player.Enable();

        Controls.Player.PauseScene.performed += ctx => PauseMenuScreen(); //ref to input actions/action maps whatever its called

    }
    void Start()
    {
        pauseMenu.SetActive(false);

    }

    // Update is called once per frame

    private void PauseMenuScreen()
    {
        if (isPaused)
        {
            ResumeGame();

        }
        else
        {
            PauseGame();
        }

    }

    public void PauseGame()
    {
        if (firstPersonControls == null)
        {
            firstPersonControls = GetComponent<FirstPersonControls>();
        }

        if (firstPersonControls != null)
        {
            firstPersonControls.enabled = false; //disable movement�
        }

        isPaused = true;
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
    }
    public void ResumeGame()
    {
        if (firstPersonControls == null)
        {
            firstPersonControls = GetComponent<FirstPersonControls>();
        }

        if (firstPersonControls != null)
        {
            firstPersonControls.enabled = true; //enable movement when resume is clicked
        }

        isPaused = false;
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;

    }

    public void Restart(string sceneName)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(sceneName);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
