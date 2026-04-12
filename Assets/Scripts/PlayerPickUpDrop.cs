using System.Collections.Generic;
using UnityEngine;

public class PlayerPickUpDrop : MonoBehaviour
{
    public float distanciaMaxima = 2f;
    public LayerMask layer;
    public List<string> tagsObjetosSeguraveis;

    public Transform handPoint; // ponto da mão
    private GameObject itemAtual;

    //________________________________________________________________//
    public Transform cameraTransform;

    void Update()
    {
        Debug.DrawRay(cameraTransform.position, cameraTransform.forward * distanciaMaxima, Color.red);
        if (Input.GetKeyDown(KeyCode.E))
        {
            TentarPegarItem();
        }

    }

    void TentarPegarItem()
    {
        Ray ray = new Ray(cameraTransform.position, cameraTransform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, distanciaMaxima))
        {
            Debug.Log("Acertou: " + hit.collider.name);
            Debug.Log("Tag: " + hit.collider.tag);

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
        // Remove item anterior
        if (itemAtual != null)
        {
            Destroy(itemAtual);
        }

        // Instancia na mão
        itemAtual = Instantiate(item, handPoint);

        itemAtual.transform.localPosition = Vector3.zero;
        itemAtual.transform.localRotation = Quaternion.identity;

        // Desativa o objeto original
        item.SetActive(false);
    }
}
