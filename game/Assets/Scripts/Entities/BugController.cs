using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Entities
{
    public class BugController : MonoBehaviour
    {
        public AudioClip sound;
        public AudioClip attackSound;

        private Animator _am;
        private AnimationEvents _ae;
        private static readonly int AttackTrigger = Animator.StringToHash("Attack");
        private static readonly int Move = Animator.StringToHash("Move");

        private Rigidbody2D _rg;
        private Collider2D _attack;

        private Dictionary<int, Action> _animEventsHandler;

        private bool _shouldFly = true;

        private void Awake()
        {
            _animEventsHandler = new Dictionary<int, Action>()
            {
                {0, AttackStarted},
                {1, EndAttack}
            };

            _am = GetComponentInChildren<Animator>();
            _ae = GetComponentInChildren<AnimationEvents>();
            _ae.EventID += i => _animEventsHandler[i]();

            _attack = transform.Find("Attack").GetComponent<Collider2D>();
            _rg = GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate()
        {
            _am.SetFloat(Move, Mathf.Sign(_rg.velocity.x));
        }

        private void Start()
        {
            StartCoroutine(FlyLoop());
        }

        private void EndAttack()
        {
            _attack.enabled = false;
        }

        private void AttackStarted()
        {
            AudioController.Instance.SetSound(attackSound, false);
            _attack.enabled = true;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player"))
                return;

            Attack();
        }

        private void Attack()
        {
            AudioController.Instance.SetSound(sound, false);
            _am.SetTrigger(AttackTrigger);
        }

        private IEnumerator FlyLoop()
        {
            while (_shouldFly)
            {
                _rg.AddForce(10f * Random.insideUnitCircle);
                yield return new WaitForSeconds(7.0f);
            }
        }
    }
}