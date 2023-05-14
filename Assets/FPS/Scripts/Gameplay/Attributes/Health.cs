using FPS;
using FPS.Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace FPS.Gameplay
{
    public class Health : MonoBehaviour
    {
        [Header("Attributes")]
        [Tooltip("Maximum amount of health")]
        public float maxHealth = 100f;

        public bool isPlayer = true;

        public float health
        {
            get
            {
                return m_Health;
            }
            private set
            {
                m_Health = Mathf.Clamp(m_Health, 0f, maxHealth);
            }
        }

        public bool isDead
        {
            get
            {
                return m_Health <= 0f;
            }
        }

        private float m_Health = 100f;

        // Start is called before the first frame update
        void Start()
        {
            health = maxHealth;
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void TakeDamage(float damage, GameObject damageSource)
        {
            float oldHealth = health;
            health = health - damage;
            float trueDamage = health - oldHealth;

            if (isPlayer)
            {
                EventManager.Broadcast(new PlayerHealthChangeEvent(oldHealth, health, damageSource));
            }

            if (isDead)
            {
                Die();
            }
        }

        private void Die()
        {
            if (!isDead) return;

        }
    }
}