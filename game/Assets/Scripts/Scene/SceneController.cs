using UnityEngine;

namespace Scene
{
    public class SceneController : MonoBehaviour
    {
        private Transform _player;

        private bool _followPlayer;

        private void Start()
        {
            var p = GameObject.FindWithTag("Player");
            if (p == null)
                return;

            _player = p.transform;
            _followPlayer = true;
        }

        private void LateUpdate()
        {
            if (_followPlayer)
                transform.position = _player.position;
        }
    }
}