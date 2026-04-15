using System.Collections.Generic;
using UnityEngine;

namespace Supercyan.FreeSample
{
    public class FreeCameraLogic : MonoBehaviour
    {
        [SerializeField] private List<Transform> m_targets = null; // Lista de alvos possíveis
        private Transform m_currentTarget = null;

        private int m_currentIndex = 0;

        public float mouseSensitivityX = 3f; // sensibilidade horizontal
        public float mouseSensitivityY = 2f; // sensibilidade vertical
        public float distance = 3f;          // distância da câmera em relação ao alvo
        public float minY = -30f;            // limite mínimo vertical
        public float maxY = 60f;             // limite máximo vertical

        private float rotationX = 0f; // acumula rotação horizontal
        private float rotationY = 0f; // acumula rotação vertical

        private void Start()
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;

            if (m_targets.Count > 0)
            {
                m_currentIndex = 0;
                m_currentTarget = m_targets[m_currentIndex];
            }
        }

        private void Update()
        {
            if (m_targets.Count == 0) return;

            // Lê movimento do mouse
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivityX;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivityY;

            rotationX += mouseX;
            rotationY -= mouseY;
            rotationY = Mathf.Clamp(rotationY, minY, maxY);
        }

        private void LateUpdate()
        {
            if (m_currentTarget == null) return;

            // Calcula rotação da câmera com base no mouse
            Quaternion rotation = Quaternion.Euler(rotationY, rotationX, 0);

            // Define posição da câmera em relação ao alvo
            Vector3 offset = rotation * new Vector3(0, 0, -distance);
            transform.position = m_currentTarget.position + offset;

            // Faz a câmera olhar para o alvo
            transform.LookAt(m_currentTarget.position);
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