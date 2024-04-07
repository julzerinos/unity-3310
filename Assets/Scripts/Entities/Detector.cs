using System;
using UnityEngine;

namespace Entities
{
    public class Detector : MonoBehaviour
    {
        public event Action EnterTriggered;
        public event Action ExitTriggered;

        public string detectTag;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag(detectTag))
                return;

            EnterTriggered?.Invoke();
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (!other.CompareTag(detectTag))
                return;

            ExitTriggered?.Invoke();
        }
    }
}