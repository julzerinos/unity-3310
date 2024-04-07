using UnityEngine;

namespace Entities
{
    public class BatteryController : MonoBehaviour
    {
        public AudioClip sound;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player"))
                return;

            AudioController.Instance.SetSound(sound, false);
            gameObject.SetActive(false);
        }
    }
}