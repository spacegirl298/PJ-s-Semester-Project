using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class UiManager : MonoBehaviour
{
    public Camera mainCamera; // Reference to the camera
    public float rotationSpeed = 1f; // Speed at which the camera rotates
    private bool isRotating = false; // Track if the camera is currently rotating
    public GameObject[] UIElements;
    //public GameObject initialButton;

    public GameObject ContollerPage;
    public GameObject KeyboardPage;
    public GameObject CreditsPage;

    public GameObject CreditsClose;
    public GameObject OptionsClose;
    public GameObject RightOptions;
    public GameObject LeftOptions;

   
   public GameObject firstSelectedButton;
    public GameObject OptionsButton;
    public GameObject CreditsButton;

    private bool gameStarted = false; 
    
    private FirstPersonControls firstPersonControls; //player script
    private Controls Controls; //defined controls in acfion 

    public void Start()
    {

        foreach (GameObject UIelement in UIElements)
        {
            UIelement.SetActive(false);
        } 

    ContollerPage.SetActive(false);
    KeyboardPage.SetActive(false);
    CreditsPage.SetActive(false);

     CreditsClose.SetActive(false);
     OptionsClose.SetActive(false);
     RightOptions.SetActive(false);
     LeftOptions.SetActive(false);
}
    private void Awake()
    {
        
        Controls = new Controls(); // initialize the input actions
    }

    private void OnEnable()
    {
        
        Controls.Enable();

       
        Controls.UI.AnyButton.performed += AnyButton;
    }

    private void OnDisable()
    {
        
        Controls.Disable();
    }

 
    private void AnyButton(InputAction.CallbackContext context) //any button on keyboard will rotate cam
    {
        if (!gameStarted)
        {
            RotateCameraLeftBy90Degrees();
            gameStarted = true;
        }
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void Options()
    {
        KeyboardPage.SetActive(true);
        OptionsClose.SetActive(true);
        RightOptions.SetActive(true);

        EventSystem.current.SetSelectedGameObject(OptionsClose);

    }

    public void OptionsRight()
    {
        KeyboardPage.SetActive(false);
        ContollerPage.SetActive(true);
        LeftOptions.SetActive(true);
        RightOptions.SetActive(false);

        EventSystem.current.SetSelectedGameObject(LeftOptions);
    }

    public void OptionsLeft()
    {
        KeyboardPage.SetActive(true);
        ContollerPage.SetActive(false);
        LeftOptions.SetActive(false);
        RightOptions.SetActive(true);

        EventSystem.current.SetSelectedGameObject(RightOptions);
    }

    public void CloseOptions()
    {
        KeyboardPage.SetActive(false);
        ContollerPage.SetActive(false);
        LeftOptions.SetActive(false);
        RightOptions.SetActive(false);
        OptionsClose.SetActive(false);

        EventSystem.current.SetSelectedGameObject(OptionsButton);
    }
    public void Credits()
    {
        CreditsPage.SetActive(true);
        CreditsClose.SetActive(true);

        EventSystem.current.SetSelectedGameObject(CreditsClose);
    }

    public void CloseCredits()
    {
        CreditsPage.SetActive(false);
        CreditsClose.SetActive(false);

        EventSystem.current.SetSelectedGameObject(CreditsButton);
    }
  
    public void RotateCameraLeftBy90Degrees()
    {
        if (!isRotating) // Prevent triggering multiple rotations simultaneously
        {
            StartCoroutine(RotateCameraCoroutine(90f));
        }
    }

    // Coroutine to smoothly rotate the camera
    private IEnumerator RotateCameraCoroutine(float angle)
    {
        isRotating = true;

        Quaternion startRotation = mainCamera.transform.rotation; // Initial rotation
        Quaternion endRotation = startRotation * Quaternion.Euler(0, angle, 0); // Target rotation

        float rotationProgress = 0f;
        while (rotationProgress < 1f)
        {
            rotationProgress += Time.deltaTime * (rotationSpeed / angle); // Normalize the rotation speed based on angle
            mainCamera.transform.rotation = Quaternion.Lerp(startRotation, endRotation, rotationProgress); // Smoothly interpolate rotation
            yield return null;
        }

        mainCamera.transform.rotation = endRotation; // Ensure exact final rotation
        isRotating = false;

      // initialButton.SetActive(false);

        foreach (GameObject UIelement in UIElements)
        {
            UIelement.SetActive(true);
        }
        EventSystem.current.SetSelectedGameObject(firstSelectedButton);
    }
}
