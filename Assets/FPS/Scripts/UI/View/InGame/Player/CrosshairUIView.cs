using FPS.Game;
using FPS.Gameplay;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FPS.UI
{
    public class CrosshairUIView : UIView
    {
        public Image centerImage;
        public Image leftImage;
        public Image rightImage;
        public Image topImage;
        public Image bottomImage;

        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();
            
        }

        private void Awake()
        {
            EventManager.AddListener<WeaponChangedEvent>(OnWeaponChanged);
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void OnWeaponChanged(WeaponChangedEvent evt)
        {
            //Debug.Log($"Change Weapon, old weapon:{weaponChangeEvent.oldWeaponController.gameObject}, new weapon:{weaponChangeEvent.newWeaponController.gameObject}");
            CrosshairData crosshairData = evt.newWeaponController.crosshairData;
            if (crosshairData.IsVaild())
            {
                gameObject.SetActive(true);

                centerImage.sprite = crosshairData.crosshairCenterSprite;
                centerImage.color = crosshairData.crosshairColor;

                leftImage.sprite = crosshairData.crosshairLeftSprite;
                leftImage.color = crosshairData.crosshairColor;

                rightImage.sprite = crosshairData.crosshairRightSprite;
                rightImage.color = crosshairData.crosshairColor;

                topImage.sprite = crosshairData.crosshairTopSprite;
                topImage.color = crosshairData.crosshairColor;

                bottomImage.sprite = crosshairData.crosshairBottomSprite;
                bottomImage.color = crosshairData.crosshairColor; 
            }
            else
            {
                gameObject.SetActive(false);
                Debug.Log("CrosshairData is not vaild, maybe this weapon don't have crosshair, or you forget set the crosshairData");
            }
        }
    }
}