using UnityEngine;

[CreateAssetMenu(fileName = "Narration", menuName = "RPG/Narration Data")]
public class NarrationData : ScriptableObject
{
    [TextArea(2, 5)]
    public string text;
    public AudioClip audioClip;
    public float displayDuration = 3f;
}