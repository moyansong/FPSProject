using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPS.Gameplay
{
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

        public GameObject owner { get; set; }

        public GameObject instigator { get; set; }

        public Camera ownerCamera { get; set; }

        public GameObject sourcePrefab { get; set; }

        protected AudioSource m_AudioSource;

        // Start is called before the first frame update
        protected virtual void Start()
        {
            m_AudioSource = GetComponent<AudioSource>();
        }

        // Update is called once per frame
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

        public virtual void Fire1()
        {
            
        }

        public virtual void Reload()
        {

        }

        public void OnDestroy()
        {
            Destroy(subWeapon);
        }
    }

}