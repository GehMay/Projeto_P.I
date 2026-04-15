using UnityEngine; // Biblioteca principal do Unity
using UnityEngine.SceneManagement; // Necessário para carregar cenas
using UnityEngine.UI; // Necessário para usar UI (como Slider)
using System.Collections; // Necessário para usar corrotinas (IEnumerator)

public class LoadingScreen : MonoBehaviour
{
    public Slider barraProgresso; // Referência ao Slider da tela de loading
    public static string cenaParaCarregar; // Variável estática que guarda o nome da próxima cena

    void Start()
    {
        // Quando a cena de Loading iniciar, começa a corrotina que carrega a cena definida
        StartCoroutine(CarregarCenaAsync(cenaParaCarregar));
    }

    // Corrotina que faz o carregamento assíncrono da cena
    IEnumerator CarregarCenaAsync(string nomeCena)
    {
        // Inicia o carregamento da cena em segundo plano
        AsyncOperation operacao = SceneManager.LoadSceneAsync(nomeCena);

        // Impede que a cena seja ativada automaticamente quando terminar de carregar
        operacao.allowSceneActivation = false;

        // Enquanto a cena não terminar de carregar
        while (!operacao.isDone)
        {
            // Calcula o progresso (vai de 0 até 0.9, por isso divide por 0.9)
            float progresso = Mathf.Clamp01(operacao.progress / 0.9f);

            // Atualiza o valor do Slider com o progresso
            barraProgresso.value = progresso;

            // Quando o progresso chegar em 90% (0.9), libera a ativação da cena
            if (operacao.progress >= 0.9f)
            {
                operacao.allowSceneActivation = true;
            }

            // Espera um frame antes de continuar o loop
            yield return null;
        }
    }
}