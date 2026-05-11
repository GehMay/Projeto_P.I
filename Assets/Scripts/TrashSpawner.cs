using UnityEngine;

[System.Serializable]
public class SpawnArea
{
    public string nomeComodo;
    public Vector3 center;
    public Vector3 size;
    public int quantidade;
}

public class TrashSpawner : MonoBehaviour
{
    [Header("Prefabs dos lixos")]
    public GameObject[] trashPrefabs;

    [Header("Áreas de Spawn")]
    public SpawnArea[] areas;

    [Header("Configuração")]
    public float alturaMaxima = 1.5f;

    void Start()
    {
        foreach (SpawnArea area in areas)
        {
            SpawnTrashNaArea(area);
        }
    }

    void SpawnTrashNaArea(SpawnArea area)
    {
        int spawnados = 0;
        int tentativas = 0;
        int maxTentativas = area.quantidade * 10;

        while (spawnados < area.quantidade && tentativas < maxTentativas)
        {
            tentativas++;

            Vector3 randomPos = area.center + new Vector3(
                Random.Range(-area.size.x / 2, area.size.x / 2),
                0,
                Random.Range(-area.size.z / 2, area.size.z / 2)
            );

            RaycastHit hit;
            Vector3 rayOrigin = randomPos + Vector3.up * 10f;

            if (Physics.Raycast(rayOrigin, Vector3.down, out hit, 20f))
            {
                Debug.Log("Acertou: " + hit.collider.name + " | Tag: " + hit.collider.tag + " | Y: " + hit.point.y);
                if (hit.point.y > alturaMaxima) continue;
                if (!hit.collider.CompareTag("Ground")) continue;

                Vector3 spawnPos = hit.point + Vector3.up * 0.3f;

                Collider[] colliders = Physics.OverlapSphere(spawnPos, 0.3f);
                bool posicaoLivre = true;

                foreach (Collider col in colliders)
                {
                    if (!col.CompareTag("Ground"))
                    {
                        posicaoLivre = false;
                        break;
                    }
                }

                if (posicaoLivre)
                {
                    GameObject prefab = trashPrefabs[Random.Range(0, trashPrefabs.Length)];
                    Instantiate(prefab, spawnPos, Quaternion.identity);
                    GameManager.instance.RegistrarLixo(); // registra o lixo
                    spawnados++;
                }
            }
        }

        Debug.Log(area.nomeComodo + ": Spawnados " + spawnados + " de " + area.quantidade);
    }

    void OnDrawGizmosSelected()
    {
        if (areas == null) return;
        Gizmos.color = Color.yellow;
        foreach (SpawnArea area in areas)
        {
            Gizmos.DrawWireCube(area.center, area.size);
        }
    }
}