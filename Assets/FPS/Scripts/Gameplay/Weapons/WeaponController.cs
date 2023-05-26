using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPS.Gameplay
{
    public enum WeaponState
    {
        Idle,
        Firing,
        Reloading,
        Equipping
    }

    [System.Serializable]
    public struct CrosshairData
    {
        [Tooltip("The image that will be used for this weapon's crosshair center")]
        public Sprite crosshairCenterSprite;

        [Tooltip("The image that will be used for this weapon's crosshair Left")]
        public Sprite crosshairLeftSprite;

        [Tooltip("The image that will be used for this weapon's crosshair right")]
        public Sprite crosshairRightSprite;
        
        [Tooltip("The image that will be used for this weapon's crosshair top")]
        public Sprite crosshairTopSprite;
        
        [Tooltip("The image that will be used for this weapon's crosshair bottom")]
        public Sprite crosshairBottomSprite;

        [Tooltip("The color of the crosshair image")]
        public Color crosshairColor;

        public bool IsVaild()
        {
            return crosshairCenterSprite && crosshairLeftSprite && crosshairRightSprite && crosshairTopSprite && crosshairBottomSprite;
        }
    }

    public class WeaponController : MonoBehaviour
    {
        [Header("Parameters")]
        [Tooltip("Each item in the game has a unique ID, and the ID of the weapon usually ranges from 0 to 29999")]
        public int weaponID;

        [Tooltip("The name that will be displayed in the UI for this weapon")]
        public string weaponName;

        [Tooltip("Holding this weapon with the right hand ?")]
        public bool isRightHolding = true;

        [Tooltip("SubWeapon or DualWeapon")]
        public GameObject subWeapon = null;

        [Tooltip("Default data for the crosshair")]
        public CrosshairData crosshairData;

        [Tooltip("Number of layers in the animator")]
        public int animatorLayer = 0;


        public GameObject owner { get; set; }

        public GameObject instigator { get; set; }

        public Camera ownerCamera { get; set; }

        public GameObject sourcePrefab { get; set; }

        public Animator animator { get; set; }

        public WeaponState weaponState 
        { 
            get
            {
                return m_WeaponState;
            }
            set
            {
                m_LastWeaponState = m_WeaponState;
                m_WeaponState = value;
                OnWeaponStateChanged();
            }
        }

        public bool isIdle => weaponState == WeaponState.Idle;
        public bool isFiring => weaponState == WeaponState.Firing;
        public bool isReloading => weaponState == WeaponState.Reloading;
        public bool isEquipping => weaponState == WeaponState.Equipping;

        protected AudioSource m_AudioSource;

        protected WeaponState m_WeaponState = WeaponState.Idle;
        protected WeaponState m_LastWeaponState = WeaponState.Idle;

        protected virtual void Awake()
        {
            m_AudioSource = GetComponent<AudioSource>();
        }

        protected virtual void Start()
        {
            
        }

        void Update()
        {
            
        }

        public virtual void Equip()
        {
            gameObject.SetActive(true);
            if (subWeapon != null)
            {
                subWeapon.SetActive(true);
            }
        }

        public virtual void Unequip()
        {
            gameObject.SetActive(false);
            if (subWeapon != null)
            {
                subWeapon.SetActive(false);
            }
        }

        public virtual bool HandleFire1Input(bool inputDown, bool inputHeld, bool inputUp)
        {
            return false;
        }

        public virtual bool Reload()
        {
            return true;
        }

        protected virtual void OnWeaponStateChanged()
        {

        }

        public void OnDestroy()
        {
            Destroy(subWeapon);
        }
    }

}