using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FPS.UI
{
    public class UIView : MonoBehaviour
    {
        protected Button[] m_Buttons;

        // Start is called before the first frame update
        protected virtual void Start()
        {
            m_Buttons = transform.GetComponentsInChildren<Button>(true);
        }

        // Update is called once per frame
        void Update()
        {
            
        }

    }
}
