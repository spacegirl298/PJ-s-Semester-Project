using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class UiManager : MonoBehaviour
{
    public Camera mainCamera; // Reference to the camera
    public float rotationSpeed = 1f; // Speed at which the camera rotates
    private bool isRotating = false; // Track if the camera is currently rotating
    public GameObject[] UIElements;
    public GameObject initialButton;

    private bool gameStarted = false; 
    
    private FirstPersonControls firstPersonControls; //player script
    private Controls Controls; //defined controls in acfion 


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

        initialButton.SetActive(false);

        foreach (GameObject UIelement in UIElements)
        {
            UIelement.SetActive(true);
        }
    }
}
