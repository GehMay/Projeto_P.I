using UnityEngine;

// Gerencia os 5 slots do inventário
public class Inventory : MonoBehaviour
{
    public InventorySlot[] slots = new InventorySlot[7]; // Array com 7 slots

    // Adiciona item ao inventário
    public void AddItem(Item item)          
    {
        // Procura slot vazio
        for (int i = 0; i < slots.Length; i++)
        {
            // Se o slot não tem ícone ativo → está vazio
            if (!slots[i].icon.enabled)
            {
                slots[i].AddItem(item); // Coloca o item no slot
                return;
            }
        }
        Debug.Log("Inventário cheio!"); // Se não achar slot vazio
    }
}