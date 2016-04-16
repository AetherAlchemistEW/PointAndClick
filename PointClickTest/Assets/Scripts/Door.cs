using UnityEngine;
using System.Collections;

//Interactable derivation
public class Door : Interactable
{
    //Our new interact method allows us to add additional functionality to certain actions
    new public Response Interact(Actions act)
    {
        if (act == Actions.open)
        {

        }
        if (act == Actions.close)
        {

        }
        if(act == Actions.use)
        {

        }

        return responses[(int)act];
    }
}
