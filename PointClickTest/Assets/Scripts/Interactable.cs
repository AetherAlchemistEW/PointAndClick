using UnityEngine;
using System.Collections;

//Good a place as any to keep the different actions
public enum Actions {give, pickUp, use, open, lookAt, push, close, talkTo, pull};

//Struct for packaging up the responses from objects
[System.Serializable]
public struct Response
{
    [SerializeField]
    public bool success;
    [SerializeField]
    public Item item;
    [SerializeField]
    public string message;
}

//Interactable base class, an array of responses corresponding to our possible actions and a basic return class
public class Interactable : MonoBehaviour
{
    public Response[] responses;

    public Response Interact (Actions act)
    {
        return responses[(int)act];
    }
}
