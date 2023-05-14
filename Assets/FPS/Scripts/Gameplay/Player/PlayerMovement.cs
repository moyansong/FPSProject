
using FPS.Game;
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

        private float m_StepInterval;

        [Header("Effect")]
        [Tooltip("The sound made when jumping up")]
        public AudioClip jumpSound;

        [Tooltip("The sound made when landing")]
        public AudioClip landSound;

        public AudioClip footstepSound;

        [Tooltip("Do you need to make a sound when walking")]
        public bool playFootstepSound = true;
        
        private Vector2 m_Input;
        private Vector3 m_MoveDirection = Vector3.zero;
        
        private CollisionFlags m_collisionFlags;
               
        private float m_StepCycle;
        private float m_NextStep;

        private CharacterController m_CharacterController;
        private AudioSource m_AudioSource;

        public bool isWalking { get; set; }

        // 为真则起跳
        public bool jump { get; set; }

        public bool isInAir { get; private set; }

        public bool isGrounded 
        {
            get
            {
                return m_CharacterController.isGrounded;
            } 
        }

        public bool wasGrounded { get; private set; }

        // Use this for initialization
        private void Start()
        {
            m_CharacterController = GetComponent<CharacterController>();
            m_StepCycle = 0f;
            m_NextStep = m_StepCycle / 2f;
            isInAir = false;
            m_AudioSource = GetComponent<AudioSource>();
        }


        // Update is called once per frame
        private void Update()
        {
            // the jump state needs to read here to make sure it is not missed
            if (!jump)
            {
                jump = inputHandler.GetJumpInputDown();
            }

            if (!wasGrounded && m_CharacterController.isGrounded)
            {
                PlayLandingSound();
                m_MoveDirection.y = 0f;
                isInAir = false;
            }
            if (!m_CharacterController.isGrounded && !isInAir && wasGrounded)
            {
                m_MoveDirection.y = 0f;
            }

            wasGrounded = m_CharacterController.isGrounded;
        }


        private void PlayLandingSound()
        {
            m_AudioSource.clip = landSound;
            m_AudioSource.Play();
            m_NextStep = m_StepCycle + .5f;
        }


        private void FixedUpdate()
        {
            float speed;
            GetInput(out speed);
            // always move along the camera forward as it is the direction that it being aimed at
            Vector3 desiredMove = transform.forward * m_Input.y + transform.right * m_Input.x;

            // get a normal for the surface that is being touched to move along it
            RaycastHit hitInfo;
            Physics.SphereCast(
                transform.position, 
                m_CharacterController.radius, 
                Vector3.down, 
                out hitInfo,
                m_CharacterController.height / 2f, 
                Physics.AllLayers, 
                QueryTriggerInteraction.Ignore
            );

            desiredMove = Vector3.ProjectOnPlane(desiredMove, hitInfo.normal).normalized;

            m_MoveDirection.x = desiredMove.x * speed;
            m_MoveDirection.z = desiredMove.z * speed;


            if (isGrounded)
            {
                m_MoveDirection.y = -stickToGroundForce;

                if (jump)
                {
                    m_MoveDirection.y = jumpSpeed;
                    PlayJumpSound();
                    jump = false;
                    isInAir = true;
                }
            }
            else
            {
                m_MoveDirection += Physics.gravity * gravityMultiplier * Time.fixedDeltaTime;
            }
            m_collisionFlags = m_CharacterController.Move(m_MoveDirection * Time.fixedDeltaTime);

            ProgressStepCycle(speed);
        }


        private void PlayJumpSound()
        {
            m_AudioSource.PlayOneShot(jumpSound);
        }


        private void ProgressStepCycle(float speed)
        {
            if (m_CharacterController.velocity.sqrMagnitude > 0 && (m_Input.x != 0 || m_Input.y != 0))
            {
                m_StepCycle += (m_CharacterController.velocity.magnitude + (speed * (isWalking ? 1f : runstepLenghten))) *
                             Time.fixedDeltaTime;
            }

            if (!(m_StepCycle > m_NextStep))
            {
                return;
            }

            m_NextStep = m_StepCycle + m_StepInterval;

            PlayFootStepAudio();
        }


        private void PlayFootStepAudio()
        {
            if (!m_CharacterController.isGrounded || !playFootstepSound)
            {
                return;
            }

            m_AudioSource.PlayOneShot(footstepSound);
        }


        private void GetInput(out float speed)
        {
            // Read input
            float horizontal = Input.GetAxisRaw(Inputs.k_AxisNameHorizontal);
            float vertical = Input.GetAxisRaw(Inputs.k_AxisNameVertical);

            bool waswalking = isWalking;

#if !MOBILE_INPUT
            // On standalone builds, walk/run speed is modified by a key press.
            // keep track of whether or not the character is walking or running
            isWalking = !inputHandler.GetRunInputHeld();
#endif
            // set the desired speed to be walking or running
            speed = isWalking ? maxWalkSpeed : maxRunSpeed;
            m_Input = new Vector2(horizontal, vertical);

            // normalize input if it exceeds 1 in combined length:
            if (m_Input.sqrMagnitude > 1)
            {
                m_Input.Normalize();
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
            body.AddForceAtPosition(m_CharacterController.velocity * 0.1f, hit.point, ForceMode.Impulse);
        }
    }
}