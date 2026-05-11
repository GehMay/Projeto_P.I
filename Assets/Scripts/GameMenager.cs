using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private int totalLixos = 0;
    private int lixosDescartados = 0;

    public string cenaVitoria = "Passou"; // nome da cena de vitória

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void RegistrarLixo()
    {
        totalLixos++;
    }

    public void LixoDescartado(int quantidade = 1)
    {
        lixosDescartados += quantidade;
        Debug.Log("Descartados: " + lixosDescartados + " de " + totalLixos);

        if (lixosDescartados >= totalLixos && totalLixos > 0)
        {
            Debug.Log("TODOS OS LIXOS DESCARTADOS! Indo para: " + cenaVitoria);
            SceneManager.LoadScene(cenaVitoria);
        }
    }
}