using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPS.Gameplay
{
    [RequireComponent(typeof(Collider))]
    public class ProjectileCollider : MonoBehaviour
    {
        //[Header("Effects")]

        [Header("Damage")]
        [Tooltip("Maxdamage of the projectile,the final damage is related to speed")]
        public float damage = 40f;

        [Tooltip("The projectile might penetrate something")]
        public float penetrationPower = 40f;

        [Tooltip("If it is true, it means that the projectile will be destroyed after stopping, otherwise it will hang on the person")]
        public bool shouldDestory = true;

        [Tooltip("Will set the isTrigger of the collider to the same value")]
        public bool isTigger = true;

        private Collider m_collider;
        private AudioSource m_audioSource;
        private ProjectileMovement m_projectileMovement;

        public Vector3 velocity
        {
            get 
            {
                return m_projectileMovement.velocity;
            }
            set
            {
                m_projectileMovement.velocity = value;
            }
        }

        // Start is called before the first frame update
        void Start()
        {
            m_collider = GetComponent<Collider>();
            m_audioSource = GetComponent<AudioSource>();
            m_collider.isTrigger = isTigger;
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void OnDestroy()
        {

        }

        private void OnCollisionEnter(Collision collision)
        {
            
        }

        private void OnTriggerEnter(Collider collision)
        {
            if (collision.GetComponent<Collider>() != null)
            {
                Debug.Log($"{collision.GetComponent<Collider>().ToString()}");
            }
            else
            {
                Debug.Log("fuck");
            }
        }

        private void OnTriggerStay(Collider collision)
        {
            // 穿过物体时，衰减子弹速度
            
        }

        private void OnTriggerExit(Collider collision)
        {
            
        }
    }
}