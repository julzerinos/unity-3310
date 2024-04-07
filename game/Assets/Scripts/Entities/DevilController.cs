using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Entities
{
    public class DevilController : MonoBehaviour
    {
        private Transform _player;
        private bool _isTracking;

        private Rigidbody2D _rg;
        private Animator _am;
        private BoltSpawner _bs;

        private static readonly int Cast = Animator.StringToHash("Cast");
        private static readonly int Move = Animator.StringToHash("Move");

        private Coroutine _attack;

        private bool _shouldWalk = true;
        private bool _canAttack;

        private void Awake()
        {
            _am = GetComponentInChildren<Animator>();
            _bs = GetComponentInChildren<BoltSpawner>();
            _rg = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            var velocity = _rg.velocity;
            _am.SetFloat(Move, velocity.sqrMagnitude * Mathf.Sign(velocity.x));

            _canAttack = _rg.velocity.sqrMagnitude <= 0f;
        }

        private void Start()
        {
            StartCoroutine(WalkLoop());
        }

        private IEnumerator WalkLoop()
        {
            while (_shouldWalk)
            {
                if (Random.value < .3f)
                    Walk();


                yield return new WaitForSeconds(Random.Range(10f, 20f));
            }
        }

        private void Walk()
        {
            _rg.AddForce(new Vector2(Random.value - 1f, .5f * Random.value), ForceMode2D.Impulse);
        }

        private void StopWalk()
        {
            _rg.velocity = new Vector2(0, 0);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (_isTracking)
                return;

            if (!other.CompareTag("Player"))
                return;

            _player = other.transform;
            _isTracking = true;
            StartAttacking();
        }

        private void StartAttacking()
        {
            // StopWalk();
            _attack = StartCoroutine(AttackLoop());
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (!other.CompareTag("Player"))
                return;

            _player = null;
            _isTracking = false;

            StopAttacking();
        }

        private void StopAttacking()
        {
            StopCoroutine(_attack);
        }

        private IEnumerator AttackLoop()
        {
            while (_isTracking)
            {
                if (_canAttack)
                    Attack();

                yield return new WaitForSeconds(Random.Range(2f, 4f));
            }
        }

        private void Attack()
        {
            _am.SetTrigger(Cast);
            _bs.SpawnAt(_player.position);
        }
    }
}