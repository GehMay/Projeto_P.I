using UnityEngine;
using UnityEngine.UI;

// Each inventory slot stores one item
public class InventorySlot : MonoBehaviour
{
    public Image icon;        // Icon of the item in the slot
    public TMPro.TextMeshProUGUI amountText;   // Text showing the quantity
    private Item storedItem;  // Reference to the item stored in this slot

    // Add an item to the slot
    public void AddItem(Item newItem)
    {
        storedItem = newItem; // Save the item reference
        icon.sprite = newItem.icon; // Show the icon
        icon.enabled = true; // Enable the image
        amountText.text = newItem.amount.ToString(); // Show the quantity
    }

    // Clear the slot (when the item is removed)
    public void ClearSlot()
    {
        storedItem = null; // Remove reference
        icon.sprite = null; // Remove icon
        icon.enabled = false; // Disable image
        amountText.text = ""; // Clear quantity text
    }
}