using UnityEngine;
using UnityEngine.EventSystems;

public class Joystick : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    [Header("Referências")]
    public RectTransform knob;
    public PlayerController playerController;

    [Header("Configuração")]
    public float raioMaximo = 50f;

    private RectTransform rectTransform;
    private Vector2 inputVector;
    private Vector2 centroJoystick;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();

#if !UNITY_ANDROID && !UNITY_IOS
            gameObject.SetActive(false);
#endif
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        // salva o centro no momento do toque
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            rectTransform.parent as RectTransform,
            eventData.position,
            eventData.pressEventCamera,
            out centroJoystick);
        OnDrag(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 pos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            rectTransform.parent as RectTransform,
            eventData.position,
            eventData.pressEventCamera,
            out pos);

        // calcula a diferença do centro
        pos = pos - centroJoystick;

        if (pos.magnitude > raioMaximo)
            pos = pos.normalized * raioMaximo;

        knob.localPosition = pos;
        inputVector = pos / raioMaximo;
        playerController.joystickInput = inputVector;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        inputVector = Vector2.zero;
        knob.localPosition = Vector2.zero;
        playerController.joystickInput = Vector2.zero;
    }
}