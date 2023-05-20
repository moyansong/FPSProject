using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FPS.UI
{
    public class StoreUIView : UIView
    {
        [Header("Commodity Detail")]
        [Tooltip("Commodity Detail UI")]
        public GameObject commodityDetail;

        [Tooltip("The image of the commodity will be displayed on the right side of the product list")]
        public Image commodityImage;

        [Tooltip("Commodity Name")]
        public Text commodityNameText;

        [Tooltip("Describe the functionality of the commodity")]
        public Text commodityDescriptionText;

        [Header("Effect")]
        [Tooltip("The sound emitted when clicking a normal button")]
        public AudioClip clickSound;
        [Tooltip("The sound emitted when clicking a buy button")]
        public AudioClip buySound;

        protected AudioSource m_AudioSource;

        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();
            commodityDetail.SetActive(false);
            m_AudioSource = GetComponent<AudioSource>();
        }

        // Update is called once per frame
        void Update()
        {

        }

        /// <summary>
        /// 显示商品详情
        /// </summary>
        /// <param name="ID"><每个物体都有一个ID，全游戏通用/param>
        /// <param name="isVisible"></param>
        public void SetCommodityDetailVisibility(int ID, bool isVisible)
        {
            commodityDetail.SetActive(isVisible);
        }

        public void BuyButtonOnClick()
        {
            m_AudioSource.PlayOneShot(buySound);
        }
    }
}