using FPS.Game;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

namespace FPS.Gameplay
{
    [RequireComponent(typeof(Collider))]
    public class MeleeWeaponController : WeaponController
    {
        [Header("Weapon Parameters")]
        [Tooltip("Damage of light strike")]
        public float lightStrikeDamage = 50f;

        [Tooltip("The interval between two light strike, unit(s)")]
        public float lightStrikeInterval = 0.5f;

        [Tooltip("Damage of heavy strike")]
        public float heavyStrikeDamage = 100f;

        [Tooltip("The interval between two heavy strike, unit(s)")]
        public float heavyStrikeInterval = 1f;

        private Collider m_Collider;

        private float m_damage;

        private float m_lastStrikeTime = -10f;

        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();
            m_Collider = GetComponent<Collider>();
        }

        // Update is called once per frame
        void Update()
        {

        }
 
        public override void Fire1()
        {
            base.Fire1(); 
            LightStrike();
        }

        private bool CanLightStrike()
        {
            return Time.time - m_lastStrikeTime >= lightStrikeInterval;
        }

        private void LightStrike()
        {
            if (!CanLightStrike()) return;

            m_damage = lightStrikeDamage; 
        }

        private void HeavyStrike()
        {
            m_damage = heavyStrikeDamage;
        }

        private void OnTriggerEnter(Collider collider)
        {
            ApplyDamage(collider, m_damage);
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
