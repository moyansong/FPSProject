using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPS.Gameplay
{
    public class PlayerController : MonoBehaviour
    {
        [Header("References")]
        [Tooltip("Reference to the main camera used for the player")]
        public Camera playerCamera;

        [Tooltip("Audio source for footsteps, jump, etc...")]
        public AudioSource audioSource;

        [Tooltip("Player controlled character.")]
        public GameObject character;

        private PlayerInputHandler m_InputHandler;

        // 纵向旋转角度，随鼠标y轴变化
        private float m_VerticalAngle = 0f;
        private float m_RotationSpeed = 200f;
        private float m_RotationMultiplier = 1f;

        // Start is called before the first frame update
        void Start()
        {
            m_InputHandler = GetComponent<PlayerInputHandler>();
        }

        // Update is called once per frame
        void Update()
        {
            UpdateView();
        }

        private void UpdateView()
        {
            m_VerticalAngle += m_InputHandler.GetLookInputsVertical() * m_RotationSpeed * m_RotationMultiplier;
            m_VerticalAngle = Mathf.Clamp(m_VerticalAngle, -89f, 89f);
            playerCamera.transform.localEulerAngles = new Vector3(-m_VerticalAngle, 0, 0);
            character.transform.localEulerAngles = new Vector3(-m_VerticalAngle, 0, 0);

            transform.Rotate(new Vector3(0f, (m_InputHandler.GetLookInputsHorizontal() * m_RotationSpeed * m_RotationMultiplier), 0f), Space.Self);
        }
    }
}