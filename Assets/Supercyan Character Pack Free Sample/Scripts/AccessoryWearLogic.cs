using UnityEngine; // Biblioteca principal do Unity

namespace Supercyan.FreeSample
{
    // Este script controla como acessórios (AccessoryLogic) são "vestidos" em um personagem.
    // Ele conecta os ossos do acessório aos ossos do personagem para que se movam juntos.
    public class AccessoryWearLogic : MonoBehaviour
    {
        [SerializeField] private SkinnedMeshRenderer m_characterRenderer;
        // Renderer do personagem (responsável por renderizar a malha com ossos).
        // Pode ser atribuído pelo Inspector.

        [SerializeField] private AccessoryLogic[] m_accessoriesToAttach = null;
        // Lista de acessórios que serão anexados ao personagem.

        private Transform[] m_characterBones;
        // Array que guarda todos os ossos do personagem.

        private bool m_initialized = false;
        // Flag para saber se o script já foi inicializado corretamente.

        // Função que inicializa o personagem e pega os ossos
        private void Initialize(GameObject character)
        {
            if (m_characterRenderer == null) // Se o renderer não foi atribuído
            {
                m_characterRenderer = GetComponentInChildren<SkinnedMeshRenderer>(); // Procura no objeto

                if (m_characterRenderer == null) // Se não encontrar
                {
                    Debug.LogWarning("Missing character components."); // Mostra aviso no console
                    return; // Sai da função
                }
            }

            if (m_characterRenderer.rootBone == null) // Se não houver osso raiz
            {
                Debug.LogWarning("Missing character root bone."); // Mostra aviso
                return; // Sai da função
            }

            m_characterBones = m_characterRenderer.bones; // Guarda todos os ossos do personagem
            m_initialized = true; // Marca como inicializado
        }

        private void Awake()
        {
            Initialize(gameObject); // Inicializa o personagem ao acordar
            foreach (AccessoryLogic a in m_accessoriesToAttach) { Attach(a); }
            // Para cada acessório na lista, chama a função Attach
        }

        // Função que conecta um acessório ao personagem
        public void Attach(AccessoryLogic accessory)
        {
            if (!m_initialized) // Se não estiver inicializado
            {
                Initialize(gameObject); // Tenta inicializar
                if (!m_initialized) // Se ainda falhar
                {
                    Debug.LogWarning("AccessoryWearLogic not initialized correctly, can't attach accessories.");
                    return; // Sai da função
                }
            }
            else if (accessory == null) // Se o acessório for nulo
            {
                Debug.LogWarning("Trying to attach null accessory.");
                return;
            }
            else if (accessory.Renderer == null) // Se o acessório não tiver renderer
            {
                Debug.LogWarning("Trying to attach accessory with missing renderer.");
                return;
            }
            else if (accessory.Renderer.rootBone == null) // Se o acessório não tiver osso raiz
            {
                Debug.LogWarning("Trying to attach accessory with missing root bone.");
                return;
            }

            // Tenta mapear os ossos do acessório para os ossos do personagem
            Transform[] newBones = GetBones(accessory.Renderer.bones, m_characterBones);
            if (newBones == null) // Se não conseguir mapear
            {
                Debug.LogWarning("Trying to attach accessory with incompatible rig.");
                return;
            }

            // Atualiza os ossos do acessório para os ossos do personagem
            accessory.Renderer.bones = newBones;
            accessory.Renderer.rootBone = m_characterRenderer.rootBone; // Define o osso raiz igual ao do personagem
        }

        // Função que mapeia ossos do acessório para ossos do personagem
        private Transform[] GetBones(Transform[] accessoryBones, Transform[] characterBones)
        {
            Transform[] newBones = new Transform[accessoryBones.Length]; // Cria array do mesmo tamanho

            for (int i = 0; i < accessoryBones.Length; i++) // Para cada osso do acessório
            {
                Transform bone = FindBone(m_characterRenderer.rootBone, accessoryBones[i].name);
                // Procura osso correspondente no personagem pelo nome

                if (bone == null) { return null; } // Se não encontrar, retorna null
                newBones[i] = bone; // Se encontrar, adiciona ao array
            }

            return newBones; // Retorna ossos mapeados
        }

        // Função recursiva que procura um osso pelo nome dentro da hierarquia
        private Transform FindBone(Transform rootBone, string name)
        {
            if (rootBone.name == name) { return rootBone; } // Se o nome bater, retorna o osso
            else
            {
                Transform found = null;
                for (int i = 0; i < rootBone.childCount; i++) // Percorre filhos do osso
                {
                    found = FindBone(rootBone.GetChild(i), name); // Procura recursivamente
                    if (found != null) { return found; } // Se encontrar, retorna
                }
                return null; // Se não encontrar em nenhum filho, retorna null
            }
        }
    }
}
