using UnityEngine;

public class CameraTouch : MonoBehaviour
{
    [Header("Sensibilidade")]
    public float sensibilidadeX = 0.2f;
    public float sensibilidadeY = 0.2f;

    [Header("Limite vertical")]
    public float minY = -30f;
    public float maxY = 60f;

    [Header("Referência")]
    public PlayerController playerController;
    public RectTransform joystickArea;

    private float rotacaoX = 0f;
    private int touchId = -1;

    void Update()
    {
#if UNITY_ANDROID || UNITY_IOS
        HandleTouch();
#else
            HandleMouse();
#endif
    }

    void HandleTouch()
    {
        for (int i = 0; i < Input.touchCount; i++)
        {
            Touch touch = Input.GetTouch(i);

            if (joystickArea != null &&
                RectTransformUtility.RectangleContainsScreenPoint(joystickArea, touch.position, null))
                continue;

            if (touch.phase == TouchPhase.Began && touchId == -1)
            {
                touchId = touch.fingerId;
            }

            if (touch.fingerId != touchId) continue;

            if (touch.phase == TouchPhase.Moved)
            {
                float rotY = touch.deltaPosition.x * sensibilidadeX;
                playerController.transform.Rotate(0f, rotY, 0f);

                rotacaoX -= touch.deltaPosition.y * sensibilidadeY;
                rotacaoX = Mathf.Clamp(rotacaoX, minY, maxY);
                transform.localRotation = Quaternion.Euler(rotacaoX, 0f, 0f);
            }

            if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
            {
                touchId = -1;
            }
        }
    }

    void HandleMouse()
    {
        float rotY = Input.GetAxis("Mouse X") * sensibilidadeX * 10f;
        playerController.transform.Rotate(0f, rotY, 0f);

        rotacaoX -= Input.GetAxis("Mouse Y") * sensibilidadeY * 10f;
        rotacaoX = Mathf.Clamp(rotacaoX, minY, maxY);
        transform.localRotation = Quaternion.Euler(rotacaoX, 0f, 0f);
    }
}