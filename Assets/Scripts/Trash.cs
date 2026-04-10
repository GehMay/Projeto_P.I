using UnityEngine;
using UnityEngine.UI;

public class Trash : MonoBehaviour
{
    public string tipoLixeira;   // Exemplo: "Papel", "Plástico", "Vidro"
    public Text mensagemUI;      // Texto na tela para avisos
    public float distanciaInteracao = 2f; // distância máxima para interagir

    private Transform player;

    void Start()
    {
        // Encontrar o player pela tag (certifique-se de que o Player tem a tag "Player")
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        // Verifica se o player está perto da lixeira
        float distancia = Vector3.Distance(player.position, transform.position);

        if (distancia <= distanciaInteracao)
        {
            // Mostra instrução na tela
            mensagemUI.text = "Pressione E para jogar lixo";

            // Se o jogador apertar E
            if (Input.GetKeyDown(KeyCode.E))
            {
                // Aqui você precisa passar o lixo selecionado no inventário
                string lixoSelecionado = "Papel"; // exemplo, depois você liga com a hotbar
                JogarNaLixeira(lixoSelecionado);
            }
        }
        else
        {
            // Limpa mensagem quando está longe
            mensagemUI.text = "";
        }
    }

    void JogarNaLixeira(string lixo)
    {
        // Compara tag da lixeira com o tipo do lixo
        if (lixo == tipoLixeira)
        {
            mensagemUI.text = "Lixo jogado corretamente!";
            Debug.Log("Jogou fora: " + lixo);
            // Aqui você pode remover do inventário
        }
        else
        {
            mensagemUI.text = "Lixeira errada!";
            Debug.Log("Tentou jogar lixo errado!");
        }
    }
}
