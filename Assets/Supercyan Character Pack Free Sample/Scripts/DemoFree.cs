using UnityEngine; // Biblioteca principal do Unity

namespace Supercyan.FreeSample
{
    // Este script demonstra como controlar personagens e câmera,
    // permitindo trocar de alvo e disparar animações via teclado ou botões na tela.
    public class DemoFree : MonoBehaviour
    {
        private readonly string[] m_animations = { "Pickup", "Wave", "Win" };
        // Lista de nomes das animações que podem ser disparadas

        private Animator[] m_animators = null;
        // Array que guarda todos os animators encontrados na cena

        [SerializeField] private FreeCameraLogic m_cameraLogic = null;
        // Referência ao script de lógica da câmera (para trocar de alvo)

        private void Start()
        {
            m_animators = FindObjectsOfType<Animator>();
            // Busca automaticamente todos os componentes Animator presentes na cena
        }

        private void Update()
        {
            // Se apertar Q → troca para o personagem anterior
            if (Input.GetKeyDown(KeyCode.Q))
            {
                m_cameraLogic.PreviousTarget();
            }

            // Se apertar E → troca para o próximo personagem
            if (Input.GetKeyDown(KeyCode.E))
            {
                m_cameraLogic.NextTarget();
            }
        }

        private void OnGUI()
        {
            // Cria uma área vertical para os botões e textos
            GUILayout.BeginVertical(GUILayout.Width(Screen.width));

            // Linha horizontal para botões de troca de personagem
            GUILayout.BeginHorizontal();

            // Botão para personagem anterior
            if (GUILayout.Button("Previous character (Q)"))
            {
                m_cameraLogic.PreviousTarget();
            }

            // Botão para próximo personagem
            if (GUILayout.Button("Next character (E)"))
            {
                m_cameraLogic.NextTarget();
            }

            GUILayout.EndHorizontal(); // Fecha linha horizontal

            GUILayout.Space(16); // Espaço entre seções

            // Cria botões para cada animação da lista
            for (int i = 0; i < m_animations.Length; i++)
            {
                if (i == 0) { GUILayout.BeginHorizontal(); } // Abre linha horizontal no primeiro botão

                if (GUILayout.Button(m_animations[i])) // Cria botão com nome da animação
                {
                    // Quando clicado, dispara a animação em todos os animators da cena
                    for (int j = 0; j < m_animators.Length; j++)
                    {
                        m_animators[j].SetTrigger(m_animations[i]);
                    }
                }

                if (i == m_animations.Length - 1) { GUILayout.EndHorizontal(); } // Fecha linha no último botão
            }

            GUILayout.Space(16); // Espaço entre seções

            // Muda cor do texto para preto temporariamente
            Color oldColor = GUI.color;
            GUI.color = Color.black;

            // Mostra instruções de controle
        }
    }
}