using FPS.Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FPS.UI
{
    public class PlayerAttributeUIView : UIView
    {
        // Start is called before the first frame update
        [Tooltip("Image component displaying current health")]
        public Image healthFillImage;

        protected override void Start()
        {
            base.Start();
        }

        private void Awake()
        {
            EventManager.AddListener<PlayerHealthChangedEvent>(OnPlayerHealthChanged);
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void OnPlayerHealthChanged(PlayerHealthChangedEvent evt)
        {
            healthFillImage.fillAmount = evt.newHealth / evt.maxHealth;
        }
    }
}