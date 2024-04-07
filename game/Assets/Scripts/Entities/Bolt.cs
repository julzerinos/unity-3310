using UnityEngine;

namespace Entities
{
    public class Bolt : MonoBehaviour
    {
        public AudioClip sound;

        private Animator _am;
        private Collider2D _cl;

        private void Awake()
        {
            _cl = GetComponent<Collider2D>();
            _am = GetComponent<Animator>();
        }

        public void Finished()
        {
            _cl.enabled = false;
            gameObject.SetActive(false);
        }

        public void Damaging()
        {
            AudioController.Instance.SetSound(sound, false, 0);

            _cl.enabled = true;
        }

        private void OnEnable()
        {
            _am.Rebind();
        }
    }
}