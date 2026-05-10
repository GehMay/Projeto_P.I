using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory instance;

    public InventorySlot[] slots = new InventorySlot[7];
    public Transform painelSlots; // arraste o "Panel" aqui no Inspector

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        slots = painelSlots.GetComponentsInChildren<InventorySlot>();
        Debug.Log("Slots encontrados: " + slots.Length);

        // inicializa todos os slots como vazios
        foreach (InventorySlot slot in slots)
        {
            slot.icon.enabled = false;
            slot.amountText.text = "";
        }
    }

    public void AddItem(Item item)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i] == null)
            {
                Debug.Log("Slot " + i + " está nulo!");
                continue;
            }

            if (!slots[i].icon.enabled)
            {
                slots[i].AddItem(item);
                return;
            }
        }
        Debug.Log("Inventário cheio!");
    }
}