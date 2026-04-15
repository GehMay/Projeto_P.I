using UnityEngine;

// Class that represents a trash item in the inventory
[System.Serializable] // Allows the class to appear in the Unity Inspector
public class Item
{
    public string itemName;   // Name of the trash (e.g., "Bottle", "Paper")
    public Sprite icon;       // Icon to show in the inventory slot
    public int amount;        // Quantity of the same trash item
}