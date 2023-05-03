
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.Events;
using UnityStandardAssets.CrossPlatformInput;
using UnityStandardAssets.Utility;

namespace FPS.Gameplay
{
    public class PlayerMovement : MonoBehaviour
    {
        [Header("Reference")]
        public PlayerInputHandler inputHandler;

        [Header("Movement")]
        [Tooltip("Maximum speed when walking")]
        public float maxWalkSpeed;

        [Tooltip("Maximum speed when running")]
        public float maxRunSpeed;

        [SerializeField][Range(0f, 1f)] 
        public float runstepLenghten;

        [Tooltip("Jump speed, the faster the speed, the higher the jump")]
        public float jumpSpeed;

        public float stickToGroundForce;

        public float gravityMultiplier;

        private float m_stepInterval;

        [Header("Effect")]
        [Tooltip("The sound made when jumping up")]
        public AudioClip jumpSound;

        [Tooltip("The sound made when landing")]
        public AudioClip landSound;

        public AudioClip footstepSound;

        [Tooltip("Do you need to make a sound when walking")]
        public bool playFootstepSound = true;
        
        private Vector2 m_input;
        private Vector3 m_moveDirection = Vector3.zero;
        
        private CollisionFlags m_collisionFlags;
               
        private float m_stepCycle;
        private float m_nextStep;

        private CharacterController m_characterController;
        private AudioSource m_audioSource;

        public bool isWalking { get; set; }

        // 为真则起跳
        public bool jump { get; set; }

        public bool isInAir { get; private set; }

        public bool isGrounded 
        {
            get
            {
                return m_characterController.isGrounded;
            } 
        }

        public bool wasGrounded { get; private set; }

        // Use this for initialization
        private void Start()
        {
            m_characterController = GetComponent<CharacterController>();
            m_stepCycle = 0f;
            m_nextStep = m_stepCycle / 2f;
            isInAir = false;
            m_audioSource = GetComponent<AudioSource>();
        }


        // Update is called once per frame
        private void Update()
        {
            // the jump state needs to read here to make sure it is not missed
            if (!jump)
            {
                jump = inputHandler.GetJumpInputDown();
            }

            if (!wasGrounded && m_characterController.isGrounded)
            {
                PlayLandingSound();
                m_moveDirection.y = 0f;
                isInAir = false;
            }
            if (!m_characterController.isGrounded && !isInAir && wasGrounded)
            {
                m_moveDirection.y = 0f;
            }

            wasGrounded = m_characterController.isGrounded;
        }


        private void PlayLandingSound()
        {
            m_audioSource.clip = landSound;
            m_audioSource.Play();
            m_nextStep = m_stepCycle + .5f;
        }


        private void FixedUpdate()
        {
            float speed;
            GetInput(out speed);
            // always move along the camera forward as it is the direction that it being aimed at
            Vector3 desiredMove = transform.forward * m_input.y + transform.right * m_input.x;

            // get a normal for the surface that is being touched to move along it
            RaycastHit hitInfo;
            Physics.SphereCast(
                transform.position, 
                m_characterController.radius, 
                Vector3.down, 
                out hitInfo,
                m_characterController.height / 2f, 
                Physics.AllLayers, 
                QueryTriggerInteraction.Ignore
            );

            desiredMove = Vector3.ProjectOnPlane(desiredMove, hitInfo.normal).normalized;

            m_moveDirection.x = desiredMove.x * speed;
            m_moveDirection.z = desiredMove.z * speed;


            if (isGrounded)
            {
                m_moveDirection.y = -stickToGroundForce;

                if (jump)
                {
                    m_moveDirection.y = jumpSpeed;
                    PlayJumpSound();
                    jump = false;
                    isInAir = true;
                }
            }
            else
            {
                m_moveDirection += Physics.gravity * gravityMultiplier * Time.fixedDeltaTime;
            }
            m_collisionFlags = m_characterController.Move(m_moveDirection * Time.fixedDeltaTime);

            ProgressStepCycle(speed);
        }


        private void PlayJumpSound()
        {
            m_audioSource.PlayOneShot(jumpSound);
        }


        private void ProgressStepCycle(float speed)
        {
            if (m_characterController.velocity.sqrMagnitude > 0 && (m_input.x != 0 || m_input.y != 0))
            {
                m_stepCycle += (m_characterController.velocity.magnitude + (speed * (isWalking ? 1f : runstepLenghten))) *
                             Time.fixedDeltaTime;
            }

            if (!(m_stepCycle > m_nextStep))
            {
                return;
            }

            m_nextStep = m_stepCycle + m_stepInterval;

            PlayFootStepAudio();
        }


        private void PlayFootStepAudio()
        {
            if (!m_characterController.isGrounded || !playFootstepSound)
            {
                return;
            }

            m_audioSource.PlayOneShot(footstepSound);
        }


        private void GetInput(out float speed)
        {
            // Read input
            float horizontal = Input.GetAxisRaw(GameConstants.k_AxisNameHorizontal);
            float vertical = Input.GetAxisRaw(GameConstants.k_AxisNameVertical);

            bool waswalking = isWalking;

#if !MOBILE_INPUT
            // On standalone builds, walk/run speed is modified by a key press.
            // keep track of whether or not the character is walking or running
            isWalking = !inputHandler.GetRunInputHeld();
#endif
            // set the desired speed to be walking or running
            speed = isWalking ? maxWalkSpeed : maxRunSpeed;
            m_input = new Vector2(horizontal, vertical);

            // normalize input if it exceeds 1 in combined length:
            if (m_input.sqrMagnitude > 1)
            {
                m_input.Normalize();
            }
        }

        private void OnControllerColliderHit(ControllerColliderHit hit)
        {
            Rigidbody body = hit.collider.attachedRigidbody;
            //dont move the rigidbody if the character is on top of it
            if (m_collisionFlags == CollisionFlags.Below)
            {
                return;
            }

            if (body == null || body.isKinematic)
            {
                return;
            }
            body.AddForceAtPosition(m_characterController.velocity * 0.1f, hit.point, ForceMode.Impulse);
        }
    }
}