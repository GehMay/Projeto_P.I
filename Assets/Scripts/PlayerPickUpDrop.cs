using System.Collections.Generic;
using UnityEngine;

public class PlayerPickUpDrop : MonoBehaviour
{
    [Header("Configuração do Raycast")]
    public float distanciaMaxima = 2f; // distância máxima para pegar
    public LayerMask layer; // camada dos objetos pegáveis
    public List<string> tagsObjetosSeguraveis; // tags permitidas (ex: "Trash")

    [Header("Ponto da mão do jogador")]
    public Transform handPoint; // onde o item vai ficar na mão
    private GameObject itemAtual; // referência ao item que está sendo segurado

    [Header("Referência do container da câmera")]
    public Transform camContainer; // substitui cameraTransform
    private float verticalRotation = 0f;

    void Update()
    {
        // desenha o raio na cena para debug
        Debug.DrawRay(camContainer.position, camContainer.forward * distanciaMaxima, Color.red);

        // controla rotação vertical da câmera
        camContainer.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);

        // tecla E para pegar
        if (Input.GetKeyDown(KeyCode.E))
        {
            TentarPegarItem();
        }

        // tecla Q para soltar
        if (Input.GetKeyDown(KeyCode.Q))
        {
            SoltarItem();
        }
    }

    void TentarPegarItem()
    {
        Ray ray = new Ray(camContainer.position, camContainer.forward);
        RaycastHit hit;

        Debug.Log("Raycast Disparado");

        if (Physics.Raycast(ray, out hit, distanciaMaxima))
        {
            Debug.Log("Acertou: " + hit.collider.name);

            // verifica se a tag está na lista de objetos pegáveis
            if (tagsObjetosSeguraveis.Contains(hit.collider.tag))
            {
                Debug.Log("PEGOU ITEM!");
                PegarItem(hit.collider.gameObject);
            }
        }
        else
        {
            Debug.Log("Não acertou nada");
        }
    }

    void PegarItem(GameObject item)
    {
        // se já tem um item na mão, destrói
        if (itemAtual != null)
        {
            Destroy(itemAtual);
        }

        // pega referência ao TrashObject para acessar o Item
        TrashObject trash = item.GetComponent<TrashObject>();
        if (trash != null)
        {
            // adiciona ao inventário
            Inventory.instance.AddItem(trash.item);
            Debug.Log("Adicionado ao inventário: " + trash.item.itemName);
        }

        // instancia uma cópia do objeto na mão
        itemAtual = Instantiate(item, handPoint);

        // reseta posição e rotação para alinhar com a mão
        itemAtual.transform.localPosition = Vector3.zero;
        itemAtual.transform.localRotation = Quaternion.identity;

        // desativa o objeto original no chão
        item.SetActive(false);
    }

    void SoltarItem()
    {
        if (itemAtual != null)
        {
            // ativa de novo o objeto original no chão
            itemAtual.SetActive(true);

            // solta na frente do jogador
            itemAtual.transform.parent = null;
            itemAtual.transform.position = transform.position + transform.forward;

            itemAtual = null;
        }
    }
}
