using System.Collections.Generic; // Importa a biblioteca para usar listas
using UnityEngine; // Importa a biblioteca principal do Unity

namespace Supercyan.FreeSample
{
    // Script que controla a câmera para seguir diferentes alvos
    public class FreeCameraLogic : MonoBehaviour
    {
        private Transform m_currentTarget = null; // Alvo atual que a câmera está seguindo
        private float m_distance = 2f; // Distância da câmera em relação ao alvo
        private float m_height = 1; // Altura da câmera em relação ao alvo
        private float m_lookAtAroundAngle = 180; // Ângulo de rotação em torno do alvo

        [SerializeField] private List<Transform> m_targets = null; // Lista de alvos possíveis
        private int m_currentIndex = 0; // Índice do alvo atual na lista

        private void Start()
        {
            // --- BLOQUEIO DO MOUSE ---
            Cursor.visible = false; // Esconde o cursor da tela
            Cursor.lockState = CursorLockMode.Locked; // Trava o cursor no centro da tela

            // --- INICIALIZAÇÃO DA CÂMERA ---
            if (m_targets.Count > 0) // Se houver alvos na lista
            {
                m_currentIndex = 0; // Começa pelo primeiro alvo
                m_currentTarget = m_targets[m_currentIndex]; // Define o alvo atual
            }
        }

        // Função para trocar de alvo
        private void SwitchTarget(int step)
        {
            if (m_targets.Count == 0) { return; } // Se não houver alvos, sai da função

            m_currentIndex += step; // Avança ou retrocede na lista

            if (m_currentIndex > m_targets.Count - 1) { m_currentIndex = 0; } // Se passar do último, volta ao primeiro
            if (m_currentIndex < 0) { m_currentIndex = m_targets.Count - 1; } // Se passar antes do primeiro, vai para o último

            m_currentTarget = m_targets[m_currentIndex]; // Atualiza o alvo atual
        }

        public void NextTarget() { SwitchTarget(1); } // Função pública para ir ao próximo alvo
        public void PreviousTarget() { SwitchTarget(-1); } // Função pública para ir ao alvo anterior

        private void Update()
        {
            if (m_targets.Count == 0) { return; } // Se não houver alvos, não faz nada
        }

        private void LateUpdate()
        {
            if (m_currentTarget == null) { return; } // Se não houver alvo atual, não faz nada

            float targetHeight = m_currentTarget.position.y + m_height; // Calcula a altura da câmera em relação ao alvo
            float currentRotationAngle = m_lookAtAroundAngle; // Define o ângulo de rotação

            Quaternion currentRotation = Quaternion.Euler(0, currentRotationAngle, 0); // Cria a rotação em torno do alvo

            Vector3 position = m_currentTarget.position; // Pega a posição do alvo
            position -= currentRotation * Vector3.forward * m_distance; // Move a câmera para trás do alvo
            position.y = targetHeight; // Ajusta a altura da câmera

            transform.position = position; // Atualiza a posição da câmera
            transform.LookAt(m_currentTarget.position + new Vector3(0, m_height, 0)); // Faz a câmera olhar para o alvo
        }
    }
}
