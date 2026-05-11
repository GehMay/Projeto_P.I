using UnityEngine;

public class TrashBin : MonoBehaviour
{
    [Header("Tipo aceito pela lixeira")]
    public string acceptedType;

    private bool playerPerto = false;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger ativado por: " + other.name + " | Tag: " + other.tag);
        if (other.CompareTag("Player"))
        {
            playerPerto = true;
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerPerto = false;
        }
    }

    private void Update()
    {
        if (playerPerto && Input.GetKeyDown(KeyCode.F))
        {
            TentarJogarLixo();
        }
    }

    void TentarJogarLixo()
    {
        Item item = Inventory.instance.GetSelectedItem();

        if (item == null)
        {
            Debug.Log("Nenhum item selecionado!");
            return;
        }

        if (item.itemName == acceptedType)
        {
            Debug.Log("Correto! " + item.itemName + " jogado na lixeira certa!");
            int quantidade = item.amount;
            Inventory.instance.RemoveSelectedItem();
            GameManager.instance.LixoDescartado(quantidade);
        }
        else
        {
            Debug.Log("Errado! " + item.itemName + " não pertence a esta lixeira!");
        }
    }
}