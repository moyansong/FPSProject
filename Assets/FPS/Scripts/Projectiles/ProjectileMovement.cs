using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UIElements;

namespace FPS.Gameplay
{
    [RequireComponent(typeof(Rigidbody))]
    public class ProjectileMovement : MonoBehaviour
    {
        public GameObject owner { get; set; }

        public WeaponController weaponController { get; set; }

        private Vector3 m_lastFramePosition;
        private Vector3 m_initialVelocity;
        private Vector3 m_initialPosition;
        private Quaternion m_initialRotation;

        public Vector3 initialVelocity
        {
            get
            {
                return m_initialVelocity;
            }
            set
            {
                m_initialVelocity = value;
                velocity = value;
            }
        }

        public Vector3 initialPosition
        {
            get
            {
                return m_initialPosition;
            }
            set
            {
                m_initialPosition = value;
                position = value;
            }
        }

        public Quaternion initialRotation
        {
            get
            {
                return m_initialRotation;
            }
            set
            {
                m_initialRotation = value;
                rotation = value;
            }
        }

        public Vector3 velocity
        {
            get
            {
                return m_rigidbody.velocity;
            }
            set
            {
                m_rigidbody.velocity = value;
            }
        }

        public Vector3 position
        {
            get
            {
                return m_rigidbody.position;
            }
            set
            {
                m_rigidbody.position = value;
            }
        }
        
        public Quaternion rotation
        {
            get
            {
                return m_rigidbody.rotation;
            }
            set
            {
                m_rigidbody.rotation = value;
            }
        }

        [Header("Parameters")]
        [Tooltip("LifeTime of the projectile")]
        public float maxLifeTime = 5f;

        [Header("Movement")]
        [Tooltip("Speed of the projectile (m/s)")]
        public float speed = 500f;

        [Tooltip("Range of the projectile,Non-positive numbers mean unlimited range")]
        public float range = 1000f;

        private Rigidbody m_rigidbody;
        private AudioSource m_audioSource;
        
        // Start is called before the first frame update
        void Start()
        {
            m_rigidbody = GetComponent<Rigidbody>();
            m_audioSource = GetComponent<AudioSource>();
            if (maxLifeTime > 0.0f)
            {
                Destroy(gameObject, maxLifeTime);
            }
            m_lastFramePosition = m_rigidbody.position;
        }

        // Update is called once per frame
        void Update()
        {
            CheckRange();
            Debug.DrawLine(m_lastFramePosition, m_rigidbody.position, Color.red, 10f);
            m_lastFramePosition = m_rigidbody.position;
            if (Input.GetKeyDown(KeyCode.C))
            {
                Vector3 v = new Vector3(1, 1, 1);
                
                Debug.Log($"{(v.normalized * speed).magnitude}");
            }
        }

        private void CheckRange()
        {
            if (range > 0.0f)
            {
                float distance = Vector3.Distance(gameObject.transform.position, m_initialPosition);
                if (distance >= range)
                {
                    Destroy(gameObject);
                }
            }
        }

        public void Fire(Vector3 muzzlePosition, Vector3 hitPosition)
        {
            Vector3 direction = (hitPosition - muzzlePosition).normalized;
            initialPosition = muzzlePosition;
            m_rigidbody.transform.LookAt(hitPosition);
            initialVelocity = direction * speed;
        }

        public void Fire(Vector3 force, ForceMode forceMode = ForceMode.Force)
        {
            m_rigidbody.AddForce(force, forceMode);
        }

        public void Fire(float x, float y, float z, ForceMode forceMode = ForceMode.Force)
        {
            m_rigidbody.AddForce(x, y, z, forceMode);
        }
    }
}