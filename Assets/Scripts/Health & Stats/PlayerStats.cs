using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerStats : MonoBehaviour
{
    private FirstPersonController firstPersonController;
    private Controls Controls; 
    
    public HealthBar healthBar;
    public CheckpointManagers checkpointManager;
    
    public float maxHealth = 100f;
    private float currentHealth;
    

    private Coroutine healthDrainCoroutine;
    private bool isGlueActive = false;

    // for Glue
    public float glueDuration = 10f; //how long glue lazt
    private float currentGlueTime;
    public GlueBar glueBar; 

    private bool hasGlue = false;

    void Awake()
    {
        // Initialize the input controls
        Controls = new Controls();
        Controls.Player.Enable();
        Controls.Player.Glue.performed += ctx => UseGluePowerUp(); //ref to input actions/action maps whatever its called
    }
    
    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetSliderMax(maxHealth);
        healthBar.SetSlider(currentHealth);

        glueBar.SetSliderMax(glueDuration);
        glueBar.gameObject.SetActive(false); 
        StartHealthDrain();
    }

   
    IEnumerator HealthDrainRoutine()
    {
        while (currentHealth > 0)
        {
            yield return new WaitForSeconds(20f); // remove health every 2 seconds
            if (!isGlueActive) //  drain health only when glue is not active
            {
                DecreaseHealth(10); // remove health by 10
            }
        }
        /*
        if (!isGlueActive)
        {
            DecreaseHealth(10);
            Debug.Log("healt draining... Glue is NOT active");
        }
        else
        {
            Debug.Log("glue is active, health is not draining");
        } */
    }

    void DecreaseHealth(float amount)
    {
        currentHealth -= amount;

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            
            //player dies (respawn back to lasy checjpoint
            
            StartCoroutine(RespawnPlayerAndReset()); // respawn player and reset timer after delay
            currentHealth = maxHealth; 
        }

        healthBar.SetSlider(currentHealth); 
    }
    
    private IEnumerator RespawnPlayerAndReset()
    {
        yield return new WaitForSeconds(0.4f); // wait 3secs before spawning back

   
        checkpointManager.StartRespawn(); // respawn player at the last checkpoint
        currentHealth = maxHealth; 
        healthBar.SetSlider(currentHealth); 
        StartHealthDrain(); //restart draining


    }
    
    public void CollectGlue()
    {
        hasGlue = true;
        Debug.Log("glue collect");
        
    }

   /* 
    void Update()
    {
        var playerInput = new Controls();
        playerInput.Player.Enable();
        
       
        if (hasGlue && playerInput.Player.Glue.performed += ctx => UseGluePowerUp(); 
        {
            UseGluePowerUp();
            Debug.Log("glue is on");
        }
    }

  */ 
    public void UseGluePowerUp()
    {
        if (hasGlue && !isGlueActive)
        {
            StartCoroutine(GluePowerUpRoutine());
            hasGlue = false; // use the glue & set bool to true
        }
        else
        {
        }
    }

  
    IEnumerator GluePowerUpRoutine()
    {
        isGlueActive = true; //glue is now active
        currentGlueTime = glueDuration; 
        glueBar.gameObject.SetActive(true); 

        
        while (currentGlueTime > 0)
        {
            glueBar.SetSlider(currentGlueTime); 
            yield return new WaitForSeconds(1f); 
            currentGlueTime -= 1f; 
        }

        //when glue finishes
        isGlueActive = false; //drain health again
        glueBar.SetSlider(0);
        glueBar.gameObject.SetActive(false); 
    }

   
    private void StartHealthDrain()
    {
        if (healthDrainCoroutine != null)
        {
            StopCoroutine(healthDrainCoroutine);
        }
        healthDrainCoroutine = StartCoroutine(HealthDrainRoutine());
    }
}
