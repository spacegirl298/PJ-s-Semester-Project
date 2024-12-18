using System.Collections;
using System.Collections.Generic;
//using UnityEditor.ShaderGraph;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class FirstPersonControls : MonoBehaviour
{

    public GameObject hallwayLight; 
    
    [Header("MOVEMENT SETTINGS")]
    [Space(5)]
    // Public variables to set movement and look speed, and the player camera
    public float moveSpeed; // Speed at which the player moves
    public float lookSpeed; // Sensitivity of the camera movement
    public float gravity = -9.81f; // Gravity value
    public float jumpHeight = 1.0f; // Height of the jump
    public Transform playerCamera; // Reference to the player's camera
                                   // Private variables to store input values and the character controller

    private Vector2 moveInput; // Stores the movement input from the player
    private Vector2 lookInput; // Stores the look input from the player
    private float verticalLookRotation = 0f; // Keeps track of vertical camera rotation for clamping
    private Vector3 velocity; // Velocity of the player
    private CharacterController characterController; // Reference to the CharacterController component

    [Header("SHOOTING SETTINGS")]
    [Space(5)]
    public GameObject projectilePrefab; // Projectile prefab for shooting
    public Transform firePoint; // Point from which the projectile is fired
    public float projectileSpeed = 20f; // Speed at which the projectile is fired
    

    [Header("PICKING UP SETTINGS")]
    [Space(5)]
    public Transform holdPosition; // Position where the picked-up object will be held
    private GameObject heldObject; // Reference to the currently held object
    public float pickUpRange = 6f; // Range within which objects can be picked up
    private bool holdingGun = false;

    [Header("CROUCH SETTINGS")]
    [Space(5)]

    public float crouchHeight = 1f; //make player short
    public float standingHeight = 2f; //make player normal
    public float crouchSpeed = 0.5f; //make player slow when crouched
    private bool isCrouching = false; //check if crouch 

    [Header("INTERACT SETTINGS")] 
    [Space(5)]
    
    private Collectibles Collectibles;
    public ParticleSystem collectionSystem; //collectible particle
    public Material switchMaterial; // Material to apply when switch is activated
    public GameObject[] objectsToChangeColor; // Array of objects to change color

    [Header("Particle System")] [Space(5)] 
    public ParticleSystem collectibleParticles;
    

    [Header("Audio")] [Space(5)] 
    public AudioSource cyrusDoorSound;
    public AudioClip cyrusOpenDoor;
    
    public AudioSource apolloDoorSound;
    public AudioClip apolloDoorClip;
    
    public AudioSource collectSound;

    [Header("Character Sounds")] [Space(5)]
    //public AudioSource walkingSound;
   // public AudioClip walkingClip;
    
  //  public AudioSource breathingSound;
   // public AudioClip breathingClip;
    
    public AudioSource jumpSound;
    public AudioClip jumpClip;
    
    [Header("Animations")]
    [Space(5)]
    public bool IsWalking;
    public bool IsLeftWalking;
    public bool IsRightWalking;
    public Animator PlayerAnimator;
    public Animator doorAnimator;
    public bool isCollected;
    
    [Header("Lore UI")]
    public GameObject lorePanel; 
    public Image loreImage; 
    
    public Sprite[] loreImages; // array to hold lore images
    public string[] loreObjectNames; //  array to hold the names n tags of the GameObjects that will trigger the lore images
    private bool isLorePanelVisible = false;

    private FirstPersonControls firstPersonControls;
    
    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked; // lock the cursor 
        Cursor.visible = false; 
        
        // Get and store the CharacterController component attached to this GameObject
        characterController = GetComponent<CharacterController>();
        isCollected = false;
        lorePanel.SetActive(false); //lore panel initially off
        
        if (firstPersonControls == null)
        {
            firstPersonControls = FindObjectOfType<FirstPersonControls>(); // Look for FirstPersonControls on any object in the scene
        }
    }
    
    private void Start()
    {
       
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        hallwayLight.SetActive(true);
    }


    private void OnEnable()
    {
        // Create a new instance of the input actions
        var playerInput = new Controls();

        // Enable the input actions
        playerInput.Player.Enable();

        // Subscribe to the movement input events
        playerInput.Player.Movement.performed += ctx => moveInput = ctx.ReadValue<Vector2>(); // Update moveInput when movement input is performed
        playerInput.Player.Movement.canceled += ctx => moveInput = Vector2.zero; // Reset moveInput when movement input is canceled

        // Subscribe to the look input events
        playerInput.Player.LookAround.performed += ctx => lookInput = ctx.ReadValue<Vector2>(); // Update lookInput when look input is performed
        playerInput.Player.LookAround.canceled += ctx => lookInput = Vector2.zero; // Reset lookInput when look input is canceled

        // Subscribe to the jump input event
        playerInput.Player.Jump.performed += ctx => Jump(); // Call the Jump method when jump input is performed

        // Subscribe to the shoot input event
        playerInput.Player.Shoot.performed += ctx => Shoot(); // Call the Shoot method when shoot input is performed

        // Subscribe to the pick-up input event
        playerInput.Player.PickUp.performed += ctx => PickUpObject(); // Call the PickUpObject method when pick-up input is performed

        //Subscribe to the crouch event
        playerInput.Player.Crouch.performed += ctx => ToggleCrouch(); //Call the crouch method when crouch input is performed

        // Subscribe to the interact input event
        playerInput.Player.Interact.performed += ctx => Interact(); // Interact with switch

    }
    
   


    private void Update()
    {
        // Call Move and LookAround methods every frame to handle player movement and camera rotation
        Move();
        LookAround();
        ApplyGravity();
        WalkAnim();
        
      
    }
    public void Move()
    {
        // Create a movement vector based on the input
        Vector3 move = new Vector3(moveInput.x, 0, moveInput.y);

        // Transform direction from local to world space
        move = transform.TransformDirection(move);

        // Move the character controller based on the movement vector and speed
        characterController.Move(move * moveSpeed * Time.deltaTime);

        // Reset walking states when there's no input
        if (moveInput.y == 0 && moveInput.x == 0)
        {
            IsWalking = false;
            IsLeftWalking = false;
            IsRightWalking = false;
            // Stop walking sound, breathing sound, etc.
        }
        else
        {
            IsWalking = true;

            // Determine if walking left or right
            IsLeftWalking = moveInput.x < 0; // True if moving left
            IsRightWalking = moveInput.x > 0; // True if moving right

            // Optional: Play walking sounds, animations, etc.
        }

        // Handle crouch-specific speed
        float currentSpeed = isCrouching ? crouchSpeed : moveSpeed;
    }
    /*
    public void Move()
    {
        // Create a movement vector based on the input
        Vector3 move = new Vector3(moveInput.x, 0, moveInput.y);

        // Transform direction from local to world space
        move = transform.TransformDirection(move);

        // Move the character controller based on the movement vector and speed
        characterController.Move(move * moveSpeed * Time.deltaTime);

        if (moveInput.y == 0 && moveInput.x == 0)
        {
            IsWalking = false;
            IsLeftWalking = false;
            IsRightWalking = false;
            //walkingSound.Stop();
            //breathingSound.Stop();
        }

        else 
        {
        IsWalking = true;
        IsLeftWalking = false;
        IsRightWalking = false;
            //walkingSound.Play();
            //  breathingSound.Play();
        }


       

        float currentSpeed;
        if (isCrouching)
        {
            currentSpeed = crouchSpeed;
        }
        else
        {
            currentSpeed = moveSpeed;
        }

    }*/

    public void WalkAnim() 
    {
        if (IsWalking == true) 
        {
            PlayerAnimator.SetBool("IsWalking", true);
        }

        if (IsWalking == false) 
        {
            PlayerAnimator.SetBool("IsWalking", false);
        }
    }

    public void LookAround()
    {
        // Get horizontal and vertical look inputs and adjust based on sensitivity
        float LookX = lookInput.x * lookSpeed;
        float LookY = lookInput.y * lookSpeed;

        // Horizontal rotation: Rotate the player object around the y-axis
        transform.Rotate(0, LookX, 0);

        // Vertical rotation: Adjust the vertical look rotation and clamp it to prevent flipping
        verticalLookRotation -= LookY;
        verticalLookRotation = Mathf.Clamp(verticalLookRotation, -90f, 90f);

        // Apply the clamped vertical rotation to the player camera
        playerCamera.localEulerAngles = new Vector3(verticalLookRotation, 0, 0);
    }

    public void ApplyGravity()
    {
        if (characterController.isGrounded && velocity.y < 0)
        {
            velocity.y = -0.5f; // Small value to keep the player grounded
        }

        velocity.y += gravity * Time.deltaTime; // Apply gravity to the velocity
        characterController.Move(velocity * Time.deltaTime); // Apply the velocity to the character
    }

    public void Jump()
    {
        if (characterController.isGrounded)
        {
            // Calculate the jump velocity
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            jumpSound.Play();
        }
        
        
    }

   

    public void Shoot()
    {
        if (holdingGun == true)
        {
            // Instantiate the projectile at the fire point
            GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);

            // Get the Rigidbody component of the projectile and set its velocity
            Rigidbody rb = projectile.GetComponent<Rigidbody>();
            rb.velocity = firePoint.forward * projectileSpeed;

            // Destroy the projectile after 3 seconds
            Destroy(projectile, 3f);
        }
    }

    public void PickUpObject()
    {
        // Check if we are already holding an object
        if (heldObject != null)
        {
            DropObject();
        }

        // Perform a raycast from the camera's position forward
        Ray ray = new Ray(playerCamera.position, playerCamera.forward);
        RaycastHit hit;

        // Debugging: Draw the ray in the Scene view
        Debug.DrawRay(playerCamera.position, playerCamera.forward * pickUpRange, Color.red, 2f);

        if (Physics.Raycast(ray, out hit, pickUpRange))
        {
            // Check if the hit object has the tag "PickUp"
            if (hit.collider.CompareTag("PickUp"))
            {
                // Pick up the object
                heldObject = hit.collider.gameObject;
                heldObject.GetComponent<Rigidbody>().isKinematic = true; // Disable physics

                // Attach the object to the hold position
                heldObject.transform.position = holdPosition.position;
                heldObject.transform.rotation = holdPosition.rotation;
                heldObject.transform.parent = holdPosition;
            }
            else if (hit.collider.CompareTag("Gun"))
            {
                // Pick up the object
                heldObject = hit.collider.gameObject;
                heldObject.GetComponent<Rigidbody>().isKinematic = true; // Disable physics

                // Attach the object to the hold position
                heldObject.transform.position = holdPosition.position;
                heldObject.transform.rotation = holdPosition.rotation;
                heldObject.transform.parent = holdPosition;

                holdingGun = true;
            }
        }
    }

    private void DropObject()
    {

        Rigidbody heldObjectRb = heldObject.GetComponent<Rigidbody>();
        heldObjectRb.isKinematic = false;
        heldObjectRb.useGravity = true;


        heldObject.transform.parent = null;

        heldObjectRb.AddForce(Vector3.down * 2f, ForceMode.Impulse);


        heldObject = null;
        holdingGun = false;
    }
    public void ToggleCrouch()
    {
        if (isCrouching)
        {
            //Player is standing up
            characterController.height = standingHeight;
            isCrouching = false;
        }
        else
        {
            //Player is crouched down
            characterController.height = crouchHeight;
            isCrouching = true;
        }
    }

    public void Interact()
    {
        // Perform a raycast to detect interactable objects
        Ray ray = new Ray(playerCamera.position, playerCamera.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, pickUpRange))
        {
            // Check if the object is a switch
            if (hit.collider.CompareTag("Switch")) 
            {
                // Change the material color of the objects in the array
                foreach (GameObject obj in objectsToChangeColor)
                {
                    Renderer renderer = obj.GetComponent<Renderer>();
                    if (renderer != null)
                    {
                        renderer.material.color = switchMaterial.color; // Set the color to match the switch material color
                    }
                }
            }
            // Check if the object is a door
            else if (hit.collider.CompareTag("Door")) 
            {
                // Start moving the door upwards
                StartCoroutine(RaiseDoor(hit.collider.gameObject));
            }


            else if (hit.collider.CompareTag("Collectible") || hit.collider.CompareTag("Powerup"))
            {
                Collectibles collectible = hit.collider.GetComponent<Collectibles>();

                if (collectible != null)
                {

                    InventoryManager playerInventory = GetComponent<InventoryManager>();
                    if (playerInventory != null)
                    {

                        playerInventory.AddItem(collectible.collectibleName, collectible.collectibleSprite);


                        AudioSource collectibleSound = hit.collider.GetComponent<AudioSource>();
                        if (collectibleSound != null)
                        {
                            collectibleSound.Play();
                        }


                        ParticleSystem collectibleParticles = hit.collider.GetComponentInChildren<ParticleSystem>();
                        if (collectibleParticles != null)
                        {
                            collectibleParticles.transform.SetParent(null);
                            collectibleParticles.Play();
                            
                        }


                        hit.collider.gameObject.SetActive(false);
                    }
                }
            }
            else if (hit.collider.CompareTag("NPC"))
            {
                NpcDialogue dialogueComponent = hit.collider.GetComponent<NpcDialogue>();

                if (dialogueComponent != null)
                {
                    dialogueComponent.TriggerDialogue(); // Trigger dialogue 
                }
            }
            else if (hit.collider.CompareTag("Lore"))
            {
                
                if (isLorePanelVisible)
                {
                    HideLorePanel();
                    EnablePlayerMovement();
                }
                else
                {
                    ShowLorePanel(hit.collider.gameObject);
                    DisablePlayerMovement();
                }
            }
        }
    }


    private void ShowLorePanel(GameObject loreObject)
    {
        string loreIndx = loreObject.name;

        
        int index = System.Array.IndexOf(loreObjectNames, loreIndx);

        if (index >= 0 && index < loreImages.Length)
        {
            loreImage.sprite = loreImages[index];
            lorePanel.SetActive(true); 
            isLorePanelVisible = true;
        }
    }

// Hide the lore panel
    private void HideLorePanel()
    {
        lorePanel.SetActive(false); // Hide the lore panel
        isLorePanelVisible = false;
    }

    /*public IEnumerator ParticalPlay()
    {
        //partical system plays 
        yield return new WaitForSeconds(1f);
        //turn off particals
    }*/

    private IEnumerator RaiseDoor(GameObject door)
    {
        float raiseAmount = 5f; // The total distance the door will be raised
        float raiseSpeed = 2f; // The speed at which the door will be raised
        Vector3 startPosition = door.transform.position; // Store the initial position of the door
        Vector3 endPosition = startPosition + Vector3.up * raiseAmount; // Calculate the final position of the door after raising

        // Continue raising the door until it reaches the target height
        while (door.transform.position.y < endPosition.y)
        {
            // Move the door towards the target position at the specified speed
            door.transform.position = Vector3.MoveTowards(door.transform.position, endPosition, raiseSpeed * Time.deltaTime);
            yield return null; // Wait until the next frame before continuing the loop
        }
    }




private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "DoorCollider")
        {
            isCollected = true;
            doorAnimator.SetBool("isCollected", isCollected);
            
            if (cyrusDoorSound != null)
            {
                cyrusDoorSound.Play();
            }
            
            hallwayLight.SetActive(false);
        }

        if (other.gameObject.tag == "CloseDoor")
        {
            isCollected = false;
            doorAnimator.SetBool("isCollected", isCollected);
        }

    }

    public void JumpPad(float boostForce)
    {
        velocity.y = Mathf.Sqrt(boostForce * -2f * gravity);
        jumpSound.Play();
    }
    
    private void DisablePlayerMovement()
    {
        if (firstPersonControls != null)
        {
            firstPersonControls.enabled = false;
            Debug.Log("Player movement disabled.");
        }
    }

    private void EnablePlayerMovement()
    { if (firstPersonControls != null)
        {
            firstPersonControls.enabled = true;
            Debug.Log("Player movement enabled.");
        }
    }
}
