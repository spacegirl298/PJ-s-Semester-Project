using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class InventoryManager : MonoBehaviour
{
    private FirstPersonController firstPersonController; //player script
    private Controls Controls; //defined controls in acfion 
    
    public GameObject inventoryPanel;    
    public List<Image> collectibleSlots;   
    public List<Image> glueSlots;       

    [Header("BOOK PAGES/PANELS")]
    [Space(5)]
    public GameObject collectiblePage;     
    public GameObject gluePage;    
    
    [Header("JAIDEN'S ANIMATIONS")]
    [Space(5)]
    public Animator openInventory;
    public Animator flipPage;      

    [Header("ACTUAL SPRITES")]
    [Space(5)]
    public Sprite needleSprite;
    public Sprite yarnSprite;
    public Sprite stuffingSprite;
    public Sprite glueSprite;
    
    
    [Header("GRAYED OUT VERSIONS OF STUFF")]
    [Space(5)]
    //these would be the initial/grayed out versions of the stuff.
    public Sprite needleGray;
    public Sprite yarnGray;
    public Sprite stuffingGray;
    public Sprite glueGray; 
    
    private int currentPage = 0;           // page 0 for collectibles and page 1 for glue
    private bool isInventoryOpen = false;  

    
    void Awake()
    {
        // initialize the input controls
        Controls = new Controls();
        Controls.Player.Enable();
        
        Controls.Player.OpenInventory.performed += ctx => OpenInventory(); //ref to input actions/action maps whatever its called
        Controls.Player.NavigateLeft.performed += ctx => Navigate(-1); 
        Controls.Player.NavigateRight.performed += ctx => Navigate(1); 
    }
    
    private void Start()
    {
        CloseInventoryInstantly();  

        // initializd slots with grayed-out versions
        collectibleSlots[0].sprite = needleGray;
        collectibleSlots[1].sprite = yarnGray;
        collectibleSlots[2].sprite = stuffingGray;
        
        foreach (var slot in collectibleSlots)
        {
            slot.enabled = true;  //since we showing grayed out versions slots are on
        }
        foreach (var slot in glueSlots)
        {
            slot.enabled = true;
            slot.sprite = glueGray; //gray out glue too
        }
    }

   /* private void Update()
    {
        OpenInventory();
        PageNavigate();
    } */
   
   

   /* private void OpenInventory()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (isInventoryOpen)
            {
               // StartCoroutine(CloseInventoryWithAnimation());
            }
            else
            {
                OpenInventoryWithAnimation();
            }
        }
    } */
   
   private void OpenInventory()
   {
       if (isInventoryOpen)
       {
           StartCoroutine(CloseInventoryWithAnimation());
       }
       else
       {
           OpenInventoryWithAnimation();
       }
   }

   private void Navigate(int direction)
   {
       if (isInventoryOpen)
       {
           if (direction == 1 && currentPage == 0) // switch to glue page
           {
               FlipToGluePage();
           }
           else if (direction == -1 && currentPage == 1) // switch to collectibles page
           {
               FlipToCollectiblePage();
           }
       }
   }
   

    /*private void PageNavigate() //jaiden animate to flip between pages :))
    {
        if (isInventoryOpen)
        {
            if (Input.GetKeyDown(KeyCode.RightArrow) && currentPage == 0)
            {
                FlipToGluePage();
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow) && currentPage == 1)
            {
                FlipToCollectiblePage();
            }
        }
    } */

    public void AddItem(string itemName, Sprite itemSprite) 
    {
        switch (itemName)
        {
            case "Needle":
                AddToSlot(collectibleSlots[0], itemSprite);  //slot 1
                break;

            case "Yarn":
                AddToSlot(collectibleSlots[1], itemSprite); //2nd slot
                break;

            case "Stuffing":
                AddToSlot(collectibleSlots[2], itemSprite); //3 slot
                break;

            case "Glue":
                AddToNextAvailableSlot(glueSlots, itemSprite);
                break;

            default:
                Debug.Log("not wrking");
                break;
        }
    }

    private void AddToSlot(Image slot, Sprite sprite) //so wef can add collectible to specific slot
    {
       /* if (!slot.enabled) */  if (slot.sprite == null || slot.sprite == needleGray || slot.sprite == yarnGray || slot.sprite == stuffingGray) //check if slot is empty
        {
            slot.sprite = sprite;
            slot.enabled = true;
        }
        else
        {
           
        }
    }
    
    // this supoosed  to chec if all the collectibles are collected bt player and arw in the slots
    public bool AreAllCollectiblesCollected()
    {
        return collectibleSlots[0].sprite == needleSprite && //the final/coloured sprites of collectibles
               collectibleSlots[1].sprite == yarnSprite && 
               collectibleSlots[2].sprite == stuffingSprite;
    }

    private void AddToNextAvailableSlot(List<Image> slots, Sprite sprite) 
    {
        foreach (var slot in slots)
        {
            if (!slot.enabled) //check for avail slot
            {
                slot.sprite = sprite;
                slot.enabled = true;
                return;
            }
        }
    }

    public void RemoveItem(string itemName) // Remove item from inventory
    {
        if (itemName == "Glue")
        {
            foreach (var slot in glueSlots)
            {
                if (slot.sprite == glueSprite)
                {
                    slot.sprite = null;
                    slot.enabled = false;
                    break;
                }
            }
        }
    }

    
    private void OpenInventoryWithAnimation() //maboiiiii ???
    {
        isInventoryOpen = true;
        inventoryPanel.SetActive(true); 
        collectiblePage.SetActive(true); 
        gluePage.SetActive(false);
        
        //  play animation to open inventory #jaiden
        
       
       
    }

    
    private IEnumerator CloseInventoryWithAnimation()
    {
       // jaiden add closing animation for close inventory
       

        yield return new WaitForSeconds(0.2f); 

        inventoryPanel.SetActive(false); 
        isInventoryOpen = false;
    }

    
    private void CloseInventoryInstantly() // for  instantly close the inventory without animation 
    {
        isInventoryOpen = false;
        inventoryPanel.SetActive(false); 
        collectiblePage.SetActive(false);
        gluePage.SetActive(false);
    }

    private void FlipToGluePage()
    {
        
        
        currentPage = 1;
    //plage animation for flip 
        collectiblePage.SetActive(false);
        gluePage.SetActive(true);
    }

    private void FlipToCollectiblePage()
    {
        
        
        currentPage = 0;
        //page animation for flip
    
        collectiblePage.SetActive(true);
        gluePage.SetActive(false);
    }
}
