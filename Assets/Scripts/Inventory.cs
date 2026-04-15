using UnityEngine;

// Gerencia os slots do inventário
public class Inventory : MonoBehaviour
{
    public static Inventory instance; // Singleton

    public InventorySlot[] slots = new InventorySlot[7]; // Array com 7 slots

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject); // evita duplicados
        }
    }

    // Adiciona item ao inventário
    public void AddItem(Item item)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (!slots[i].icon.enabled) // slot vazio
            {
                slots[i].AddItem(item);
                return;
            }
        }
        Debug.Log("Inventário cheio!");
    }
}
