using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory instance;
    public RectTransform selectorImage; // arraste a imagem de seleção aqui no Inspector

    public InventorySlot[] slots = new InventorySlot[7];
    public Transform painelSlots;
    public int selectedSlot = 0;  // NOVO

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

        foreach (InventorySlot slot in slots)
        {
            slot.icon.enabled = false;
            slot.amountText.text = "";
        }
    }

    void Update()
    {
        for (int i = 0; i < 7; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                selectedSlot = i;
                Debug.Log("Slot selecionado: " + (i + 1));
                MoverSeletor(i);
            }
        }
    }

    void MoverSeletor(int index)
    {
        if (selectorImage != null && slots[index] != null)
        {
            selectorImage.position = slots[index].GetComponent<RectTransform>().position;
        }
    }

    public void AddItem(Item item)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i] == null) continue;

            if (slots[i].icon.enabled && slots[i].GetItemName() == item.itemName)
            {
                slots[i].AddAmount(1);
                return;
            }
        }

        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i] == null) continue;

            if (!slots[i].icon.enabled)
            {
                slots[i].AddItem(item);
                return;
            }
        }

        Debug.Log("Inventário cheio!");
    }

    public Item GetSelectedItem()  // NOVO
    {
        if (slots[selectedSlot] != null && slots[selectedSlot].icon.enabled)
        {
            return slots[selectedSlot].GetItem();
        }
        return null;
    }

    public void RemoveSelectedItem()  // NOVO
    {
        if (slots[selectedSlot] != null)
        {
            slots[selectedSlot].ClearSlot();
        }
    }
}