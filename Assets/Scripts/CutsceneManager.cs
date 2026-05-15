using UnityEngine;
using UnityEngine.Playables;

public class CutsceneManager : MonoBehaviour
{
    public GameObject botaoMenu;
    private PlayableDirector director;

    void Start()
    {
        director = GetComponent<PlayableDirector>();
        botaoMenu.SetActive(false);
    }

    void Update()
    {
        // quando a timeline terminar, mostra o botão
        if (director.state != PlayState.Playing && director.time > 1f)
        {
            botaoMenu.SetActive(true);
        }
    }
}