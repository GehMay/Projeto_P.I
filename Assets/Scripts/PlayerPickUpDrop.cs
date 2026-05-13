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
        Debug.DrawRay(handPoint.position, Vector3.up * distanciaMaxima, Color.green);

#if !UNITY_ANDROID && !UNITY_IOS
        if (Input.GetKeyDown(KeyCode.E))
        {
            TentarPegarItem();
        }

        // tecla Q para soltar
        if (Input.GetKeyDown(KeyCode.Q))
        {
            SoltarItem();
        }
#endif
    }

    // chamado pelo botão na tela no celular
    public void BotaoPegarPressionado()
    {
        TentarPegarItem();
    }

    // chamado pelo botão na tela no celular
    public void BotaoJogarPressionado()
    {
        TrashBin[] lixeiras = FindObjectsOfType<TrashBin>();
        foreach (TrashBin lixeira in lixeiras)
        {
            float distancia = Vector3.Distance(transform.position, lixeira.transform.position);
            if (distancia < 3f)
            {
                lixeira.TentarJogarLixo();
                return;
            }
        }
        Debug.Log("Nenhuma lixeira próxima!");
    }

    void TentarPegarItem()
    {
        // detecta todos os colliders próximos à mão
        Collider[] colliders = Physics.OverlapSphere(handPoint.position, distanciaMaxima);

        Debug.Log("Total de objetos próximos: " + colliders.Length);

        foreach (Collider col in colliders)
        {
            Debug.Log("Próximo: " + col.name + " | Tag: " + col.tag);

            if (tagsObjetosSeguraveis.Contains(col.tag))
            {
                Debug.Log("PEGOU ITEM!");
                PegarItem(col.gameObject);
                return; // pega só um por vez
            }
        }

        Debug.Log("Nenhum item pegável por perto");
    }

    void PegarItem(GameObject item)
    {
        // pega referência ao TrashObject para acessar o Item
        TrashObject trash = item.GetComponent<TrashObject>();
        if (trash != null)
        {
            // adiciona ao inventário
            Inventory.instance.AddItem(trash.item);
            Debug.Log("Adicionado ao inventário: " + trash.item.itemName);
        }

        // faz o objeto sumir da cena
        Destroy(item);

        // limpa referência do item na mão
        itemAtual = null;
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