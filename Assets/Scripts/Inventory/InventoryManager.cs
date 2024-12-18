using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class InventoryManager : MonoBehaviour
{
    private FirstPersonControls firstPersonControls; //player script
    private Controls Controls; //defined controls in acfion 
    private IntroDialogue introDialogue;
    
    public GameObject inventoryPanel;
    public GameObject InventoryIcon;
    public List<Image> collectibleSlots;   
    public List<Image> glueSlots;
    

    [Header("Slots")]
    public GameObject CollectibleSlots;
    public GameObject GlueSlots;

  /*  [Header("BOOK PAGES/PANELS")]
    [Space(5)]
    public GameObject collectiblePage;     
    public GameObject gluePage; */


    [Header("JAIDEN'S ANIMATIONS")]
    [Space(5)]
    public Animator bookIn;
    public Animator bookOut;
    public Animator openInventory;
    public Animator closeInventory;
    public Animator flipPageLeft;
    public Animator flipPageRight;
    
    [Header("ACTUAL SPRITES")]
    [Space(5)]
    public Sprite needleSprite;
    public Sprite threadSprite;
    public Sprite stuffingSprite;
    public Sprite scissorsSprite; 
    public Sprite glueSprite;
   
    
    
    [Header("GRAYED OUT VERSIONS OF STUFF")]
    [Space(5)]
    //these would be the initial/grayed out versions of the stuff.
    public Sprite needleGray;
    public Sprite threadGray;
    public Sprite stuffingGray;
    public Sprite scissorsGray;
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
        //CloseInventoryInstantly();  

        // initializd slots with grayed-out versions
        collectibleSlots[0].sprite = needleGray;
        collectibleSlots[1].sprite = threadGray;
        collectibleSlots[2].sprite = stuffingGray;
        collectibleSlots[3].sprite = scissorsGray; 
        
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
                AddToSlot(collectibleSlots[0], itemSprite);  // Add to 1st slot
                break;

            case "Yarn":
                AddToSlot(collectibleSlots[1], itemSprite);  // Add to 2nd slot
                break;

            case "Stuffing":
                AddToSlot(collectibleSlots[2], itemSprite);  // Add to 3rd slot
                break;

            case "Scissors":
                AddToSlot(collectibleSlots[3], itemSprite);  // Add to 4th slot
                break;

            case "Glue":
                AddToNextAvailableSlot(glueSlots, itemSprite);  // Add glue to glue slots
                break;

            default:
                Debug.Log("idk");
                break;
        }
        CheckForWinCondition();
    }
    
    public void CheckForWinCondition()
    {
        if (AreAllCollectiblesCollected())
        {
            Debug.Log("you have all collectibles");
            
        }
        else
        {
            Debug.Log("missing more collectibles");
        }
    }
    private void AddToSlot(Image slot, Sprite sprite) //so wef can add collectible to specific slot
    {
        if (slot.sprite == needleGray || slot.sprite == threadGray || slot.sprite == stuffingGray || slot.sprite == scissorsGray)
        {
            slot.sprite = sprite;
            slot.enabled = true;
        } 
    }
    
    // this supoosed  to chec if all the collectibles are collected bt player and arw in the slots
    public bool AreAllCollectiblesCollected()
    {
        return collectibleSlots[0].sprite == needleSprite &&    
               collectibleSlots[1].sprite == threadSprite &&    
               collectibleSlots[2].sprite == stuffingSprite &&  
               collectibleSlots[3].sprite == scissorsSprite;   
    }
    

    private void AddToNextAvailableSlot(List<Image> slots, Sprite sprite)
    {
        foreach (var slot in slots)
        {
            if (slot.sprite == glueGray)  // checka if theres gray version of glue
            {
                slot.sprite = sprite;
                slot.enabled = true;
                return;
            }
        }
    }

    public void RemoveItem(string itemName) // remove glue from inventory
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
        if (firstPersonControls == null)
        {
            firstPersonControls = GetComponent<FirstPersonControls>();
        }

        if (firstPersonControls != null)
        {
            firstPersonControls.enabled = false; //disable movement when inventoy is open
        }
        
        isInventoryOpen = true;

        bookIn.SetBool("ComingUp", true);
        bookOut.SetBool("GoingDown", false);
        openInventory.SetBool("InventoryOpen", true);
        closeInventory.SetBool("InventoryClosed", false);
        CollectibleSlots.SetActive(true);

        InventoryIcon.SetActive(false);
        //inventoryPanel.SetActive(true); 
        //collectiblePage.SetActive(true); 
        //gluePage.SetActive(false);

        //  play animation to open inventory #jaiden



    }

    
    private IEnumerator CloseInventoryWithAnimation()
    {
        if (firstPersonControls == null)
        {
            firstPersonControls = GetComponent<FirstPersonControls>();
        }

        if (firstPersonControls != null)
        {
            firstPersonControls.enabled = true; //enable movement when inventoy is open
        } 
        
       // jaiden add closing animation for close inventory
       

        yield return new WaitForSeconds(0f);

        bookIn.SetBool("ComingUp", false);
        bookOut.SetBool("GoingDown", true);
        openInventory.SetBool("InventoryOpen", false);
        closeInventory.SetBool("InventoryClosed", true);
        flipPageLeft.SetBool("PageFlippedLeft", false);
        flipPageRight.SetBool("PageFlippedRight", true);

        inventoryPanel.SetActive(false); 
        isInventoryOpen = false;
        CollectibleSlots.SetActive(false);
        GlueSlots.SetActive(false);

        StartCoroutine(IconClose());
    }

    private IEnumerator CollectSlots()
    {
        yield return new WaitForSeconds(1f);
        CollectibleSlots.SetActive(true);
    }

    private IEnumerator CollectiblesSlots()
    {
        yield return new WaitForSeconds(1.6f);
        CollectibleSlots.SetActive(true);
    }

    private IEnumerator ClosingCollect()
    {
        yield return new WaitForSeconds(0.5f);
        CollectibleSlots.SetActive(false);
    }


    private IEnumerator ClosingSlots()
    {
        yield return new WaitForSeconds(0.4f);
        CollectibleSlots.SetActive(false);
    }

    private IEnumerator PatchSlots()
    {
        yield return new WaitForSeconds(1f);
        GlueSlots.SetActive(true);
    }

    private IEnumerator PatchClose()
    {
        yield return new WaitForSeconds(1f);
        GlueSlots.SetActive(false);
    }

    private IEnumerator CloseFromGLue()
    {
        yield return new WaitForSeconds(0.3f);
        GlueSlots.SetActive(false);
    }

    private IEnumerator IconClose()
    {
        yield return new WaitForSeconds(1f);
        InventoryIcon.SetActive(true);
    }
    /*private void CloseInventoryInstantly() // for  instantly close the inventory without animation 
    {
        isInventoryOpen = false;
        inventoryPanel.SetActive(false); 
        collectiblePage.SetActive(false);
        gluePage.SetActive(false);
    }*/

    private void FlipToGluePage()
    {
        flipPageLeft.SetBool("PageFlippedLeft", false);
        flipPageRight.SetBool("PageFlippedRight", true);

        CollectibleSlots.SetActive(true);
        GlueSlots.SetActive(false);

        currentPage = 1;
    //plage animation for flip 
        //collectiblePage.SetActive(false);
       //gluePage.SetActive(true);
    }

    private void FlipToCollectiblePage()
    {

        flipPageLeft.SetBool("PageFlippedLeft", true);
        flipPageRight.SetBool("PageFlippedRight", false);

        CollectibleSlots.SetActive(false);
        GlueSlots.SetActive(true);

        currentPage = 0;
        //page animation for flip
    
        //collectiblePage.SetActive(true);
        //gluePage.SetActive(false);
    }
}
