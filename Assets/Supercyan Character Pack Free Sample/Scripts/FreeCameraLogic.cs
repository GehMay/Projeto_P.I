using System.Collections.Generic;
using UnityEngine;

namespace Supercyan.FreeSample
{
    public class FreeCameraLogic : MonoBehaviour
    {
        [SerializeField] private List<Transform> m_targets = null; // Lista de alvos possíveis
        private Transform m_currentTarget = null;
        private int m_currentIndex = 0;

        public float mouseSensitivityX = 3f;
        public float mouseSensitivityY = 2f;
        public float minY = -30f;
        public float maxY = 60f;

        [Header("Offset fixo da câmera em relação ao alvo")]
        public Vector3 positionOffset = new Vector3(0f, 1.6f, 0f); // altura dos olhos

        private float rotationX = 0f;
        private float rotationY = 0f;

        private void Start()
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;

            if (m_targets.Count > 0)
            {
                m_currentIndex = 0;
                m_currentTarget = m_targets[m_currentIndex];
            }

            // Inicializa a rotação com a rotação atual da câmera
            rotationX = transform.eulerAngles.y;
            rotationY = transform.eulerAngles.x;
        }

        private void Update()
        {
            if (m_targets.Count == 0) return;

            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivityX;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivityY;

            rotationX += mouseX;
            rotationY -= mouseY;
            rotationY = Mathf.Clamp(rotationY, minY, maxY);
        }

        private void LateUpdate()
        {
            if (m_currentTarget == null) return;

            // Posição fixa no alvo + offset de altura
            transform.position = m_currentTarget.position + positionOffset;

            // Rotação livre pelo mouse
            transform.rotation = Quaternion.Euler(rotationY, rotationX, 0);
        }

        private void SwitchTarget(int step)
        {
            if (m_targets.Count == 0) return;

            m_currentIndex += step;

            if (m_currentIndex > m_targets.Count - 1) m_currentIndex = 0;
            if (m_currentIndex < 0) m_currentIndex = m_targets.Count - 1;

            m_currentTarget = m_targets[m_currentIndex];
        }

        public void NextTarget() { SwitchTarget(1); }
        public void PreviousTarget() { SwitchTarget(-1); }
    }
}