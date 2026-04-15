using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void IniciarJogo()
    {
        // Define que a próxima cena será "Fase_1"
        LoadingScreen.cenaParaCarregar = "Fase_1";
        //Chama a cena de Loading, que por sua vez vai carregar a cena definida
        SceneManager.LoadScene("Loading");
    }

    public void AbrirCreditos()
    {
        // Define que a próxima cena será "Creditos"
        LoadingScreen.cenaParaCarregar = "Créditos";
        //Chama a cena de Loading, que por sua vez vai carregar a cena definida
        SceneManager.LoadScene("Loading");
    }

    public void VoltarMainMenu()
    {
        // Define que a próxima cena será "MainMenu"
        LoadingScreen.cenaParaCarregar = "MainMenu";
        //Chama a cena de Loading, que por sua vez vai carregar a cena definida
        SceneManager.LoadScene("Loading");
    }   

    public void SairDoJogo()
    {
        Application.Quit();
    }
}