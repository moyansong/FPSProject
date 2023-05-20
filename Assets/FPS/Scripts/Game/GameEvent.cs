using FPS.Gameplay;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPS.Game
{
    public static class Events
    {
        public static DamageEvent damageEvent = new DamageEvent();
        public static PlayerHealthChangedEvent playerHealthChangedEvent = new PlayerHealthChangedEvent();
        public static WeaponChangedEvent weaponChangedEvent = new WeaponChangedEvent();
    }

    public class GameEvent
    {
        
    }

    public class DamageEvent : GameEvent
    {
        public float damage;
        public GameObject damageCauser;
        public GameObject instigator;

        public DamageEvent() { }

        public DamageEvent(float damage, GameObject damageCauser, GameObject instigator)
        {
            this.damage = damage;
            this.damageCauser = damageCauser;
            this.instigator= instigator;
        }
    }

    public class PlayerHealthChangedEvent : GameEvent
    {
        public float oldHealth;
        public float newHealth;
        public float maxHealth;
        public GameObject instigator;

        public PlayerHealthChangedEvent() { }

        public PlayerHealthChangedEvent(float oldHealth, float newHealth, float maxHealth, GameObject instigator)
        {
            this.oldHealth = oldHealth;
            this.newHealth = newHealth;
            this.maxHealth = maxHealth;
            this.instigator = instigator;
        }
    }

    public class WeaponChangedEvent : GameEvent
    {
        public WeaponController oldWeaponController;
        public WeaponController newWeaponController;

        public WeaponChangedEvent() { }

        public WeaponChangedEvent(WeaponController oldWeaponController, WeaponController newWeaponController)
        {
            this.oldWeaponController = oldWeaponController;
            this.newWeaponController = newWeaponController;
        }
    }

}