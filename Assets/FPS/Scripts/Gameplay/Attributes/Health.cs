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

        public UnityAction<DamageEvent> OnDamaged;

        public float health
        {
            get
            {
                return m_Health;
            }
            private set
            {
                m_Health = Mathf.Clamp(value, 0f, maxHealth);
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

            if (isPlayer)
            {
                EventManager.Broadcast(new PlayerHealthChangedEvent(health, health, maxHealth, gameObject));
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.B))
            {
                TakeDamage(new DamageEvent(10f, gameObject, gameObject));
            }
        }

        public void TakeDamage(DamageEvent damageEvent)
        {
            float oldHealth = health;
            health -= damageEvent.damage;
            float trueDamage = oldHealth - health;

            Debug.Log($"{gameObject} take {trueDamage} damage from {damageEvent.damageCauser} and {damageEvent.instigator}, oldHealth: {oldHealth}, newHealth: {health}");

            OnDamaged?.Invoke(damageEvent);// Action可能没有初始化

            if (isPlayer)
            {
                EventManager.Broadcast(new PlayerHealthChangedEvent(oldHealth, health, maxHealth, damageEvent.damageCauser));
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