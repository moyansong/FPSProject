using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace FPS.Gameplay.AI
{

    [RequireComponent(typeof(ZombieController))]
    public class ZombieMovement : MonoBehaviour
    {
        [Header("Movement")]
        public float walkSpeed = 0.15f;

        [Header("Movement")]
        public float followSpeed = 0.15f;

        public ZombieType zombieType => m_ZombieController.zombieType;

        public ZombieState zombieState => m_ZombieController.zombieState;

        //public ZombieState lasZombieState => m_ZombieController.lastZombieState;

        public GameObject target => m_ZombieController.target;

        public TargetInfo targetInfo => m_ZombieController.targetInfo;

        private AnimatorStateInfo m_AnimatorStateInfo => m_Animator.GetCurrentAnimatorStateInfo((int)zombieType + 1);


        private Animator m_Animator;
        private NavMeshAgent m_NavMeshAgent;
        private ZombieController m_ZombieController;

        private void Awake()
        {
            m_Animator = GetComponent<Animator>();
            m_NavMeshAgent = GetComponent<NavMeshAgent>();
            m_ZombieController = GetComponent<ZombieController>();

            m_ZombieController.onZombieStateChanged += OnZombieStateChanged;
        }

        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            UpdateByZombieState();
        }

        void UpdateByZombieState()
        {
            switch (zombieState)
            {
                case ZombieState.Following:
                    UpdateFollowing();
                    break;
                default: 
                    break;
            }
        }

        private void UpdateFollowing()
        {
            //m_NavMeshAgent.SetDestination(target.transform.position);
            MoveTo(m_ZombieController.testTarget.transform.position);
        }

        private void OnZombieStateChanged(ZombieState oldState, ZombieState newState)
        {
            switch (newState)
            {
                case ZombieState.Idle:
                    OnIdle();
                    break;
                case ZombieState.Walking:
                    OnWalking();
                    break;
                case ZombieState.Eating:
                    OnEating();
                    break;
                case ZombieState.Following:
                    OnFollowing();
                    break;
                default:
                    break;
            }
        }

        private void OnIdle()
        {
            m_NavMeshAgent.isStopped = true;
        }

        private void OnWalking()
        {
            m_NavMeshAgent.speed = walkSpeed;
            //MoveTo();
        }

        private void OnEating()
        {
            m_NavMeshAgent.isStopped = true;
        }

        private void OnFollowing()
        {
            m_NavMeshAgent.isStopped = false;
            m_NavMeshAgent.speed = followSpeed;
        }

        private void MoveTo(Vector3 destination)
        {
            m_NavMeshAgent.isStopped = false;
            m_NavMeshAgent.SetDestination(destination);
        }
    }
}