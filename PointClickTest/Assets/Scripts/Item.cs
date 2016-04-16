using UnityEngine;
using System.Collections;

//The item is a scriptable object so we can make them as assets, we can assign them to object responses to go in our inventory
[System.Serializable]
public class Item : ScriptableObject
{
    //VARIABLES
    //name
    public string itemName;
    //texture
    public Texture2D itemImage;

}
