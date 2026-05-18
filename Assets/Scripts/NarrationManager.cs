using UnityEngine;
using TMPro;
using System.Collections;

public class NarrationManager : MonoBehaviour
{
    public static NarrationManager Instance;

    [Header("UI")]
    public GameObject narrationPanel;
    public TextMeshProUGUI narrationText;

    [Header("Audio")]
    public AudioSource audioSource;

    void Awake()
    {
        Instance = this;
        narrationPanel.SetActive(false);
    }

    public void Play(NarrationData data)
    {
        if (data == null) return;
        StopAllCoroutines();
        StartCoroutine(PlayNarration(data));
    }

    private IEnumerator PlayNarration(NarrationData data)
    {
        narrationPanel.SetActive(true);
        narrationText.text = data.text;

        if (data.audioClip != null)
        {
            audioSource.clip = data.audioClip;
            audioSource.Play();
            yield return new WaitWhile(() => audioSource.isPlaying);
        }
        else
        {
            yield return new WaitForSeconds(data.displayDuration);
        }

        narrationPanel.SetActive(false);
    }
}