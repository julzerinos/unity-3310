using Entities;
using UnityEngine;

namespace Scene
{
    public class Battery : MonoBehaviour
    {
        
        private Animator _am;

        private int _load = 10;

        private static readonly int Switch = Animator.StringToHash("Switch");
        private static readonly int Load = Animator.StringToHash("Load");

        private void Awake()
        {
            _am = GetComponentInChildren<Animator>();
        }

        private void Start()
        {
            PlayerController.LoadChanged += SetLoad;
        }

        private void OnDestroy()
        {
            PlayerController.LoadChanged -= SetLoad;
        }

        private void SetLoad(int load)
        {
            _load = Mathf.Clamp(load, 0, 10);

            _am.SetTrigger(Switch);
            _am.SetInteger(Load, _load);
        }
    }
}