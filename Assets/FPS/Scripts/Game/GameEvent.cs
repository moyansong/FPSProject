using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPS.Game
{
    public static class Events
    {
        public static DamageEvent damageEvent = new DamageEvent();
        public static PlayerHealthChangeEvent healthChangeEvent = new PlayerHealthChangeEvent();
    }

    public class GameEvent
    {
        
    }

    public class DamageEvent : GameEvent
    {
        public float damage;
        public GameObject damageSource;

        public DamageEvent() { }

        public DamageEvent(float damage, GameObject damageSource)
        {
            this.damage = damage;
            this.damageSource = damageSource;
        }
    }

    public class PlayerHealthChangeEvent : GameEvent
    {
        public float oldHealth;
        public float newHealth;
        public GameObject instigator;

        public PlayerHealthChangeEvent() { }

        public PlayerHealthChangeEvent(float oldHealth, float newHealth, GameObject instigator)
        {
            this.oldHealth = oldHealth;
            this.newHealth = newHealth;
            this.instigator = instigator;
        }
    }

}