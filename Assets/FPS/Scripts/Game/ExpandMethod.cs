using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace FPS.Game
{
    public static class ExpandMethod
    {

        public static Coroutine StartCoroutine(this MonoBehaviour monoBehaviour, float delay, UnityAction action)
        {
            return monoBehaviour.StartCoroutine(DelayAction(delay, action));
        }

        public static IEnumerator DelayAction(float delay, UnityAction action)
        {
            yield return new WaitForSeconds(delay);
            action();
        }
    }
}