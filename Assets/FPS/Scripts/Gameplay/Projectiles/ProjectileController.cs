using FPS.Game;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UIElements;

namespace FPS.Gameplay
{
    [RequireComponent(typeof(Rigidbody), typeof(Collider))]
    public class ProjectileController : MonoBehaviour
    {
        [Header("Damage")]
        [Tooltip("Maxdamage of the projectile,the final damage is related to speed")]
        public float damage = 40f;

        [Header("Parameters")]
        [Tooltip("LifeTime of the projectile")]
        public float maxLifeTime = 5f;

        [Header("Movement")]
        [Tooltip("Speed of the projectile (m/s)")]
        public float speed = 500f;

        [Tooltip("Range of the projectile, Non-positive numbers mean unlimited range")]
        public float range = 1000f;

        [Tooltip("The projectile might penetrate something")]
        public float penetrationPower = 40f;

        [Tooltip("If it is true, it means that the projectile will be destroyed after stopping, otherwise it will hang on the person")]
        public bool shouldDestory = true;

        [Tooltip("Will set the isTrigger of the collider to the same value")]
        public bool isTigger = true;
  
        public GameObject owner { get; set; }

        public GameObject instigator { get; set; }

        public WeaponController weaponController { get; set; }
      
        public Vector3 initialVelocity
        {
            get
            {
                return m_InitialVelocity;
            }
            set
            {
                m_InitialVelocity = value;
                velocity = value;
            }
        }

        public Vector3 initialPosition
        {
            get
            {
                return m_InitialPosition;
            }
            set
            {
                m_InitialPosition = value;
                position = value;
            }
        }

        public Quaternion initialRotation
        {
            get
            {
                return m_InitialRotation;
            }
            set
            {
                m_InitialRotation = value;
                rotation = value;
            }
        }

        public Vector3 velocity
        {
            get
            {
                return m_Rigidbody.velocity;
            }
            set
            {
                m_Rigidbody.velocity = value;
            }
        }

        public Vector3 position
        {
            get
            {
                return m_Rigidbody.position;
            }
            set
            {
                m_Rigidbody.position = value;
            }
        }
        
        public Quaternion rotation
        {
            get
            {
                return m_Rigidbody.rotation;
            }
            set
            {
                m_Rigidbody.rotation = value;
            }
        }

        private Collider m_Collider; 
        private Rigidbody m_Rigidbody;
        private AudioSource m_AudioSource;

        private Vector3 m_LastFramePosition;
        private Vector3 m_InitialVelocity;
        private Vector3 m_InitialPosition;
        private Quaternion m_InitialRotation;

        private void Awake()
        {
            m_Collider = GetComponent<Collider>();
            m_Collider.isTrigger = isTigger;
            m_Rigidbody = GetComponent<Rigidbody>();
            m_AudioSource = GetComponent<AudioSource>();
            
            if (maxLifeTime > 0.0f)
            {
                Destroy(gameObject, maxLifeTime);
            }
            m_LastFramePosition = position;
        }

        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            CheckRange();
            
            m_LastFramePosition = position;
        }

        private void CheckRange()
        {
            if (range > 0.0f)
            {
                float distance = Vector3.Distance(gameObject.transform.position, m_InitialPosition);
                if (distance >= range)
                {
                    Destroy(gameObject);
                }
            }
        }

        public void Shoot(Vector3 muzzlePosition, Vector3 hitPosition)
        {
            Vector3 direction = (hitPosition - muzzlePosition).normalized;
            initialPosition = muzzlePosition;
            //m_Rigidbody.transform.LookAt(hitPosition);
            m_Rigidbody.transform.forward = direction;
            initialVelocity = direction * speed;
        }

        public void Shoot(Vector3 vecloity)
        {
            this.velocity = vecloity;
        }

        public void Shoot(Vector3 force, ForceMode forceMode = ForceMode.Force)
        {
            m_Rigidbody.AddForce(force, forceMode);
        }

        public void Shoot(float x, float y, float z, ForceMode forceMode = ForceMode.Force)
        {
            m_Rigidbody.AddForce(x, y, z, forceMode);
        }

        private void OnDestroy()
        {

        }

        private void OnCollisionEnter(Collision collider)
        {

        }

        private void OnTriggerEnter(Collider collider)
        {
            ApplyDamage(collider, damage);
            Destroy(gameObject);
        }

        private void OnTriggerStay(Collider collider)
        {
            // 穿过物体时，衰减子弹速度

        }

        private void OnTriggerExit(Collider collider)
        {

        }

        private void ApplyDamage(Collider collider, float damageAmount)
        {
            Health health = collider.GetComponent<Health>();
            if (health != null)
            {
                DamageEvent damageEvent = new DamageEvent(damageAmount, owner, instigator);
                health.TakeDamage(damageEvent);
            }
        }
    }
}