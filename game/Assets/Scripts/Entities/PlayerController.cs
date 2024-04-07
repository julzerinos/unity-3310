using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Entities
{
    public class PlayerController : MonoBehaviour
    {
        public AudioClip sound;
        public AudioClip deathSound;

        public static event Action<int> LoadChanged;

        private Animator _am;
        private Rigidbody2D _rb;

        private Vector2 _move;
        private const float MaxSpeed = 0.45f;

        private bool _isOn;
        private int _load = 10;

        private bool _died;

        private static readonly int Switched = Animator.StringToHash("Switched");

        private void Awake()
        {
            _am = GetComponentInChildren<Animator>();
            _rb = GetComponent<Rigidbody2D>();
        }

        private void Start()
        {
            _load = Stats.Instance.PlayerHealth;
            LoadChanged?.Invoke(_load);
        }

        private void Update()
        {
            if (_died)
                return;

            if (Input.GetButtonDown("Jump"))
                Switch();

            if (!_isOn)
                return;

            _move.x = Input.GetAxisRaw("Horizontal");
            _move.y = Input.GetAxisRaw("Vertical");
        }

        private void FixedUpdate()
        {
            if (!_isOn)
                return;

            _rb.velocity = Vector3.ClampMagnitude(_rb.velocity, MaxSpeed);
            _rb.AddForce(_move, ForceMode2D.Force);
        }

        private void Switch()
        {
            if (_isOn)
                TurnOff();
            else
                TurnOn();

            _am.SetTrigger(Switched);
        }

        private void TurnOn()
        {
            _isOn = true;

            _rb.gravityScale = 0f;
            _rb.velocity = Vector2.zero;
        }

        private void TurnOff()
        {
            _isOn = false;

            _rb.gravityScale = 1f;
            _move = Vector2.zero;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Portal"))
                EndLevel();

            if (other.CompareTag("Pickup"))
                Heal();

            if (!_isOn)
                return;

            if (other.CompareTag("Damage"))
                Hit();
        }

        private void EndLevel()
        {
            if (!Stats.Instance.SaveStats(_load))
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        private void Heal()
        {
            _load = Mathf.Clamp(++_load, 0, 10);

            LoadChanged?.Invoke(_load);
        }

        private void Hit()
        {
            AudioController.Instance.SetSound(sound, false);

            LoadChanged?.Invoke(--_load);

            if (_load > 0) return;

            Die();
        }

        private void Die()
        {
            _died = true;
            Switch();

            AudioController.Instance.SetSound(deathSound, false, 10);

            StartCoroutine(FallToDeath());
        }

        private IEnumerator FallToDeath()
        {
            yield return new WaitForSeconds(6.0f);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}