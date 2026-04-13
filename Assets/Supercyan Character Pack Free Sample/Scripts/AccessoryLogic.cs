using UnityEngine; // Importa a biblioteca principal do Unity

namespace Supercyan.FreeSample
{
    // Este script controla acessórios (AccessoryLogic).
    // Ele usa um SkinnedMeshRenderer para renderizar o acessório
    // e destrói o objeto "rig" ao iniciar.
    public class AccessoryLogic : MonoBehaviour
    {
        [SerializeField] private SkinnedMeshRenderer m_renderer = null;
        // Referência ao componente SkinnedMeshRenderer (responsável por renderizar o acessório).
        // O [SerializeField] permite que você arraste o componente no Inspector.

        public SkinnedMeshRenderer Renderer { get { return m_renderer; } }
        // Propriedade pública que retorna o renderer.
        // Isso permite que outros scripts acessem o m_renderer sem alterar diretamente a variável.

        [SerializeField] private GameObject m_rig = null;
        // Referência ao objeto "rig" (estrutura de ossos usada para animação).
        // Também pode ser atribuída pelo Inspector.

        private void Awake()
        {
            Destroy(m_rig);
            // Quando o objeto é inicializado (Awake é chamado antes do Start),
            // o "rig" é destruído. Isso remove a hierarquia de ossos associada,
            // deixando apenas o acessório renderizado.
        }
    }
}