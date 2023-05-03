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

        private PlayerInputHandler m_inputHandler;

        // 纵向旋转角度，随鼠标y轴变化
        private float m_verticalAngle = 0f;
        private float m_rotationSpeed = 200f;
        private float m_rotationMultiplier = 1f;

        // Start is called before the first frame update
        void Start()
        {
            m_inputHandler = GetComponent<PlayerInputHandler>();
        }

        // Update is called once per frame
        void Update()
        {
            UpdateView();
        }

        private void UpdateView()
        {
            m_verticalAngle += m_inputHandler.GetLookInputsVertical() * m_rotationSpeed * m_rotationMultiplier;
            m_verticalAngle = Mathf.Clamp(m_verticalAngle, -89f, 89f);
            playerCamera.transform.localEulerAngles = new Vector3(-m_verticalAngle, 0, 0);
            character.transform.localEulerAngles = new Vector3(-m_verticalAngle, 0, 0);

            transform.Rotate(new Vector3(0f, (m_inputHandler.GetLookInputsHorizontal() * m_rotationSpeed * m_rotationMultiplier), 0f), Space.Self);
        }
    }
}