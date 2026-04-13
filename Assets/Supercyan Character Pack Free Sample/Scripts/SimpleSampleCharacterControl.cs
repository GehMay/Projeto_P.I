using System.Collections.Generic; // Biblioteca para listas
using UnityEngine; // Biblioteca principal do Unity

namespace Supercyan.FreeSample
{
    // Script de controle de personagem simples
    public class SimpleSampleCharacterControl : MonoBehaviour
    {
        // Tipos de controle possíveis
        private enum ControlMode
        {
            Tank,   // Movimento estilo "tanque": frente/atrás e rotação gradual
            Direct  // Movimento livre baseado na direção da câmera
        }

        [SerializeField] private float m_moveSpeed = 2; // Velocidade de movimento
        [SerializeField] private float m_turnSpeed = 200; // Velocidade de rotação
        [SerializeField] private float m_jumpForce = 4; // Força do pulo

        [SerializeField] private Animator m_animator = null; // Animator para animações
        [SerializeField] private Rigidbody m_rigidBody = null; // Rigidbody para física

        [SerializeField] private ControlMode m_controlMode = ControlMode.Direct; // Modo de controle atual

        private float m_currentV = 0; // Entrada vertical interpolada
        private float m_currentH = 0; // Entrada horizontal interpolada

        private readonly float m_interpolation = 10; // Fator de suavização
        private readonly float m_walkScale = 0.33f; // Escala para andar
        private readonly float m_backwardsWalkScale = 0.16f; // Escala para andar para trás
        private readonly float m_backwardRunScale = 0.66f; // Escala para correr para trás

        private bool m_wasGrounded; // Flag se estava no chão no frame anterior
        private Vector3 m_currentDirection = Vector3.zero; // Direção atual de movimento

        private float m_jumpTimeStamp = 0; // Momento do último pulo
        private float m_minJumpInterval = 0.25f; // Intervalo mínimo entre pulos
        private bool m_jumpInput = false; // Flag de entrada de pulo

        private bool m_isGrounded; // Flag se está no chão

        private List<Collider> m_collisions = new List<Collider>(); // Lista de colisores em contato

        private void Awake()
        {
            // Garante que Animator e Rigidbody estão atribuídos
            if (!m_animator) { m_animator = gameObject.GetComponent<Animator>(); }
            if (!m_rigidBody) { m_rigidBody = gameObject.GetComponent<Rigidbody>(); }
        }

        // Detecta entrada em colisão
        private void OnCollisionEnter(Collision collision)
        {
            ContactPoint[] contactPoints = collision.contacts;
            for (int i = 0; i < contactPoints.Length; i++)
            {
                // Verifica se a superfície é "chão" (normal apontando para cima)
                if (Vector3.Dot(contactPoints[i].normal, Vector3.up) > 0.5f)
                {
                    if (!m_collisions.Contains(collision.collider))
                    {
                        m_collisions.Add(collision.collider);
                    }
                    m_isGrounded = true; // Marca como no chão
                }
            }
        }

        // Enquanto estiver em colisão
        private void OnCollisionStay(Collision collision)
        {
            ContactPoint[] contactPoints = collision.contacts;
            bool validSurfaceNormal = false;
            for (int i = 0; i < contactPoints.Length; i++)
            {
                if (Vector3.Dot(contactPoints[i].normal, Vector3.up) > 0.5f)
                {
                    validSurfaceNormal = true; break;
                }
            }

            if (validSurfaceNormal)
            {
                m_isGrounded = true;
                if (!m_collisions.Contains(collision.collider))
                {
                    m_collisions.Add(collision.collider);
                }
            }
            else
            {
                if (m_collisions.Contains(collision.collider))
                {
                    m_collisions.Remove(collision.collider);
                }
                if (m_collisions.Count == 0) { m_isGrounded = false; }
            }
        }

        // Ao sair da colisão
        private void OnCollisionExit(Collision collision)
        {
            if (m_collisions.Contains(collision.collider))
            {
                m_collisions.Remove(collision.collider);
            }
            if (m_collisions.Count == 0) { m_isGrounded = false; }
        }

        private void Update()
        {
            // Detecta entrada de pulo
            if (!m_jumpInput && Input.GetKey(KeyCode.Space))
            {
                m_jumpInput = true;
            }
        }

        private void FixedUpdate()
        {
            m_animator.SetBool("Grounded", m_isGrounded); // Atualiza animação de estar no chão

            // Escolhe modo de controle
            switch (m_controlMode)
            {
                case ControlMode.Direct:
                    DirectUpdate();
                    break;

                case ControlMode.Tank:
                    TankUpdate();
                    break;

                default:
                    Debug.LogError("Unsupported state");
                    break;
            }

            m_wasGrounded = m_isGrounded; // Atualiza flag
            m_jumpInput = false; // Reseta entrada de pulo
        }

        // Controle estilo tanque
        private void TankUpdate()
        {
            float v = Input.GetAxis("Vertical"); // Entrada vertical
            float h = Input.GetAxis("Horizontal"); // Entrada horizontal

            bool walk = Input.GetKey(KeyCode.LeftShift); // Shift = andar

            if (v < 0)
            {
                if (walk) { v *= m_backwardsWalkScale; }
                else { v *= m_backwardRunScale; }
            }
            else if (walk)
            {
                v *= m_walkScale;
            }

            // Suaviza entradas
            m_currentV = Mathf.Lerp(m_currentV, v, Time.deltaTime * m_interpolation);
            m_currentH = Mathf.Lerp(m_currentH, h, Time.deltaTime * m_interpolation);

            // Move e rotaciona personagem
            transform.position += transform.forward * m_currentV * m_moveSpeed * Time.deltaTime;
            transform.Rotate(0, m_currentH * m_turnSpeed * Time.deltaTime, 0);

            // Atualiza animação
            m_animator.SetFloat("MoveSpeed", m_currentV);

            JumpingAndLanding(); // Verifica pulo
        }

        // Controle direto baseado na câmera
        private void DirectUpdate()
        {
            float v = Input.GetAxis("Vertical");
            float h = Input.GetAxis("Horizontal");

            Transform camera = Camera.main.transform; // Pega câmera principal

            if (Input.GetKey(KeyCode.LeftShift))
            {
                v *= m_walkScale;
                h *= m_walkScale;
            }

            // Suaviza entradas
            m_currentV = Mathf.Lerp(m_currentV, v, Time.deltaTime * m_interpolation);
            m_currentH = Mathf.Lerp(m_currentH, h, Time.deltaTime * m_interpolation);

            // Calcula direção baseada na câmera
            Vector3 direction = camera.forward * m_currentV + camera.right * m_currentH;

            float directionLength = direction.magnitude;
            direction.y = 0; // Ignora inclinação vertical
            direction = direction.normalized * directionLength;

            if (direction != Vector3.zero)
            {
                // Suaviza rotação
                m_currentDirection = Vector3.Slerp(m_currentDirection, direction, Time.deltaTime * m_interpolation);

                transform.rotation = Quaternion.LookRotation(m_currentDirection); // Rotaciona personagem
                transform.position += m_currentDirection * m_moveSpeed * Time.deltaTime; // Move personagem

                m_animator.SetFloat("MoveSpeed", direction.magnitude); // Atualiza animação
            }

            JumpingAndLanding(); // Verifica pulo
        }

        // Lógica de pulo
        private void JumpingAndLanding()
        {
            bool jumpCooldownOver = (Time.time - m_jumpTimeStamp) >= m_minJumpInterval;

            if (jumpCooldownOver && m_isGrounded && m_jumpInput)
            {
                m_jumpTimeStamp = Time.time; // Atualiza tempo do pulo
                m_rigidBody.AddForce(Vector3.up * m_jumpForce, ForceMode.Impulse); // Aplica força de pulo
            }
        }
    }
}