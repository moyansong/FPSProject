using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace FPS.Gameplay.AI
{
    public enum ZombieType
    {
        Shuffle,
        Dizzy,
        Alert
    }

    public enum ZombieState
    {
        Idle,
        Walking,
        Eating
    }

    [RequireComponent(typeof(Health), typeof(NavMeshAgent), typeof(Animator))]
    public class ZombieController : MonoBehaviour
    {
        [Header("Paramerters")]
        [Tooltip("Type of zombie")]
        public ZombieType zombieType;

        [Tooltip("The starting state of the zombie")]
        public ZombieState startingZombieState;

        public ZombieState zombieState
        { 
            get
            {
                return m_ZombieState;
            }
            set
            {
                m_LastZombieState = m_ZombieState;
                m_ZombieState = value;
                OnZombieStateChanged();
            }
        }

        private ZombieState m_ZombieState;
        private ZombieState m_LastZombieState = ZombieState.Idle;

        private Health m_Health;
        private Animator m_Animator;
        private NavMeshAgent m_NavMeshAgent;
        private ZombieMovement m_ZombieMovement;

        private void Awake()
        {
            m_Health = GetComponent<Health>();
            m_Animator = GetComponent<Animator>();
            m_NavMeshAgent = GetComponent<NavMeshAgent>();
            m_ZombieMovement = GetComponent<ZombieMovement>();
        }

        // Start is called before the first frame update
        void Start()
        {
            m_Animator.SetLayerWeight(((int)zombieType + 1), 1);
            zombieState = startingZombieState;
            
            //InvokeRepeating("SetRandomZombieState", 10f, 10f);
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void OnZombieStateChanged()
        {
            m_Animator.SetTrigger(zombieState.ToString());
        }
    }
}