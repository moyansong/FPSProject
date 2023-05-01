using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FPS.UI
{
    public class View : MonoBehaviour
    {
        public bool visibility = false;

        protected Button[] buttons;

        // Start is called before the first frame update
        protected virtual void Start()
        {
            buttons = transform.GetComponentsInChildren<Button>(true);
        }

        // Update is called once per frame
        void Update()
        {
            
        }

    }
}
