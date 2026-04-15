using UnityEngine;

public class TrashBin : MonoBehaviour
{
    [Header("Tipo aceito pela lixeira")]
    public string acceptedType; // ← precisa ser public ou [SerializeField]

    private void OnTriggerEnter(Collider other)
    {
        TrashObject trash = other.GetComponent<TrashObject>();
        if (trash != null)
        {
            if (trash.item.itemName == acceptedType)
            {
                Debug.Log("Correto: " + trash.item.itemName + " foi jogado na lixeira certa!");
                Destroy(other.gameObject); // remove o lixo da cena
            }
            else
            {
                Debug.Log("Errado: " + trash.item.itemName + " não pertence a esta lixeira!");
            }
        }
    }
}
