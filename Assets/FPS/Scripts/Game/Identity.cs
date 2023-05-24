using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPS.Game
{
    public enum CharacterType
    {
        Player,
        Enemy,
        NPC
    }

    public class Identity : MonoBehaviour
    {
        public CharacterType characterType;

        // Start is called before the first frame update
        void Start()
        {

        }

    }
}