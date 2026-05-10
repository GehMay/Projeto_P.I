using UnityEngine;

public class TrashSpawner : MonoBehaviour
{
    [Header("Prefabs dos lixos")]
    public GameObject[] trashPrefabs; // Papel, Vidro, Plástico, Lata, Orgânico

    [Header("Área de spawn")]
    public Vector3 areaCenter; // centro do campo
    public Vector3 areaSize;   // largura, profundidade, altura

    [Header("Configuração")]
    public int trashCount = 20; // quantidade de lixos que vão aparecer

    void Start()
    {
        SpawnTrash();
    }

    void SpawnTrash()
    {
        for (int i = 0; i < trashCount; i++)
        {
            // posição aleatória dentro da área
            Vector3 randomPos = areaCenter + new Vector3(
                Random.Range(-areaSize.x / 2, areaSize.x / 2),
                0, // sempre no chão
                Random.Range(-areaSize.z / 2, areaSize.z / 2)
            );

            // escolhe um lixo aleatório
            GameObject prefab = trashPrefabs[Random.Range(0, trashPrefabs.Length)];
            Debug.Log("Spawnado: " + prefab.name + " | Tag: " + prefab.tag);
            Collider col = prefab.GetComponentInChildren<Collider>();
            Debug.Log("Spawnado: " + prefab.name +
                      " | Tag: " + prefab.tag +
                      " | Collider: " + (col != null ? col.name : "NENHUM"));
            // instancia no cenário
            Instantiate(prefab, randomPos, Quaternion.identity);
        }
    }

    // desenha a área no editor para visualizar
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(areaCenter, areaSize);
    }
}