using UnityEngine;

public class MobileUI : MonoBehaviour
{
    public GameObject botaoPegar;
    public GameObject botaoJogar;

    void Start()
    {
#if !UNITY_ANDROID && !UNITY_IOS
        botaoPegar.SetActive(false);
        botaoJogar.SetActive(false);
#endif
    }
}