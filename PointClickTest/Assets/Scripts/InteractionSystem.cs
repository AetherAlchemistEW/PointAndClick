using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;

//Handles most core gameplay, all clicking, UI interaction, and inventory
public class InteractionSystem : MonoBehaviour
{
    //VARIABLES
    //Path Related
    public Vector3 pathPoint;
    public Vector3 clickPoint;
    public AvatarMovement puppet;

    //Interaction Related
    private Interactable curTarget;
    public Response lastResponse;
    public Actions action;

    //UI Related
    public Texture2D basePointer;
    public Text targetUI;
    public Text messageUI;
    public GameObject canvasUI;
    public GameObject messagePanel;
    public GameObject inventoryPanel;
    public List<Image> inventoryUI;

    //Inventory related
    public List<Item> inventory;
    public Item selectedItem;
	
	// Update is called once per frame
	void Update ()
    {
        //if we have an item selected, draw its texture on the pointer, otherwise set it to the base pointer
        if(selectedItem != null)
        {
            Cursor.SetCursor(selectedItem.itemImage, Vector2.zero, CursorMode.Auto);
        }
        else
        {
            Cursor.SetCursor(basePointer, Vector2.zero, CursorMode.Auto);
        }

        //Click to ray system, has to make sure the mouse isn't over the UI first
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            Ray ray = GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            Clear();

            if (Physics.Raycast(ray, out hit))
            {
                //Hit the backboard which handles our pathing, this way we can obscure it with interactables
                if (hit.transform.GetComponent<PathSystem>())
                {
                    PathSystem p = hit.transform.GetComponent<PathSystem>();
                    //Find the point we want to path to from our pathing system based on the ray's impact point
                    pathPoint = p.PathPoint(hit.point.x);
                    selectedItem = null;
                    puppet.SetPath(p.path, pathPoint);
                }
                //Object is (or derives from) interactable
                else if (hit.transform.GetComponent<Interactable>())
                {
                    curTarget = hit.transform.GetComponent<Interactable>();
                    targetUI.text = curTarget.gameObject.name;
                    //if we have an item selected already go straight to 'use', if that fails run 'give'
                    if (selectedItem != null)
                    {
                        Use();
                        if(!lastResponse.success)
                        {
                            Give();
                        }
                        selectedItem = null;
                    }
                }
                clickPoint = hit.point;
            }
            //Make sure we update the UI with any new information
            UpdateUI();
        }
	}

    //Toggle on/off the relevant UI features
    void UpdateUI()
    {
        bool UiOn = curTarget != null ? true : false;
        canvasUI.SetActive(UiOn);

        bool messageDisplay = lastResponse.success;
        messagePanel.SetActive(messageDisplay);
        inventoryPanel.SetActive(!messageDisplay);

        //Draw sprites into our inventory if we have items in that slot
        for(int i = 0; i < inventory.Count; i++)
        {
            if (inventory[i] != null)
            {
                Rect rec = new Rect(0, 0, inventory[i].itemImage.width, inventory[i].itemImage.height);

                Sprite s = Sprite.Create(inventory[i].itemImage, rec, new Vector2(0.5f, 0.5f), 100);
                inventoryUI[i].sprite = s;
            }
        }
    }

    //Clear everything, this way all actions will be fresh and work correctly.
    void Clear()
    {
        pathPoint = Vector3.zero;
        clickPoint = Vector3.zero;
        curTarget = null;
        targetUI.text = "";
        messageUI.text = "";
        lastResponse = new Response();
    }

    //Our main interaction method, if we have a target then send our chosen action through and collect the response.
    void Interact()
    {
        if(curTarget != null)
        {
            lastResponse = curTarget.Interact(action);
            messageUI.text = lastResponse.message;
            //If the response involves an item (and we have inventory space) then take it.
            if(lastResponse.item != null)
            {
                if (inventory.Count < 7)
                {
                    inventory.Add(lastResponse.item);
                    Debug.Log(inventory[0].name);
                    curTarget.responses[(int)action].item = null;
                }
                else
                {
                    Debug.Log("Inventory is full");
                }
            }
        }
        //Update the UI with the outcome
        UpdateUI();
    }

    //----------ALL THE UI CALL METHODS GO HERE, They assign our action and call interact on our target -----------
    public void Give()
    {
        action = Actions.give;
        Interact();
    }
    public void PickUp()
    {
        action = Actions.pickUp;
        Interact();
    }
    public void Use()
    {
        action = Actions.use;
        Interact();
    }
    public void Open()
    {
        action = Actions.open;
        Interact();
    }
    public void LookAt()
    {
        action = Actions.lookAt;
        Interact();
    }
    public void Push()
    {
        action = Actions.push;
        Interact();
    }
    public void Close()
    {
        action = Actions.close;
        Interact();
    }
    public void TalkTo()
    {
        action = Actions.talkTo;
        Interact();
    }
    public void Pull()
    {
        action = Actions.pull;
        Interact();
    }

    //Inventory selection call from the buttons
    public void SelectItem(int slot)
    {
        if (inventory.Count >= slot)
        {
            //shift index, makes the editor side easier to work in 1 index
            slot--;

            if (inventory[slot] != null)
            {
                selectedItem = inventory[slot];
            }
        }
    }

    //Temp gizmo test drawing goes here.
    void OnDrawGizmos()
    {
        if(pathPoint != Vector3.zero)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(pathPoint, 0.2f);
        }
        if (clickPoint != Vector3.zero)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawCube(clickPoint, (Vector3.one * 0.2f));
        }
    }
}
