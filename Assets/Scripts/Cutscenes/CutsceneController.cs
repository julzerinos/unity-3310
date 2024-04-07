using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Cutscenes
{
    public class CutsceneController : MonoBehaviour
    {
        public AudioClip sceneMusic;

        public string FollowingScene;

        private Transform _transitionTransform;
        private SpriteMask _sm;
        private readonly Vector3 _transitionLeft = new Vector3(-.8f, 0f, 0f);
        private readonly Vector3 _transitionRight = new Vector3(.7f, 0f, 0f);
        private Coroutine _transition;
        private bool _transitioning;

        private readonly List<GameObject> _scenes = new List<GameObject>();
        private int _sceneDepth;
        private int _sceneMaxDepth;

        private void Awake()
        {
            var transition = transform.Find("Transition");
            _transitionTransform = transition;
            _sm = _transitionTransform.GetComponent<SpriteMask>();
            ResetTransition();

            var scenes = transform.Find("Scenes");
            for (var i = 0; i < scenes.childCount; i++)
                _scenes.Add(scenes.GetChild(i).gameObject);

            SetCutScene();
        }

        private void Start()
        {
            AudioController.Instance.SetSound(sceneMusic, true, 100);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.RightArrow))
                Next(1);
            if (Input.GetKeyDown(KeyCode.LeftArrow))
                Next(-1);
        }

        private void SetCutScene()
        {
            _sceneDepth = 0;
            _sceneMaxDepth = _scenes.Count;
            _sm.backSortingOrder = _sm.frontSortingOrder - 1;
        }

        private void Next(int direction)
        {
            if (_transitioning)
            {
                StopCoroutine(_transition);
                AfterTransition(direction);

                return;
            }

            if (direction < 0 && _sm.backSortingOrder == _sm.frontSortingOrder - 1)
                return;

            _transition = StartCoroutine(RunTransition(direction));
        }

        private void BeforeTransition(int direction)
        {
            _transitioning = true;

            if (direction < 0)
                UpdateLayers(direction);
        }

        private void AfterTransition(int direction)
        {
            if (direction > 0)
                UpdateLayers(direction);
            _transitioning = false;
            ResetTransition();
        }

        private void UpdateLayers(int direction)
        {
            if ((_sceneDepth += direction) >= _sceneMaxDepth)
                End();

            UpdateMaskLayers();

            if (direction > 0)
                DisableDisplayed();
            else
                EnableDisplayed();
        }

        private void End()
        {
            SceneManager.LoadScene(FollowingScene);
        }

        private void EnableDisplayed()
        {
            _scenes[_sceneDepth].SetActive(true);
        }

        private void DisableDisplayed()
        {
            _scenes[_sceneDepth - 1].SetActive(false);
        }

        private void UpdateMaskLayers()
        {
            _sm.backSortingOrder = _sm.frontSortingOrder - _sceneDepth - 1;
        }

        private void ResetTransition()
        {
            _transitionTransform.position = _transitionRight;
        }

        private IEnumerator RunTransition(int direction)
        {
            BeforeTransition(direction);

            var a = direction > 0 ? _transitionRight : _transitionLeft;
            var b = direction > 0 ? _transitionLeft : _transitionRight;

            for (float i = 0; i < 1; i += 0.05f)
            {
                _transitionTransform.position = Vector3.Lerp(a, b, i);
                yield return new WaitForSeconds(0.05f);
            }

            AfterTransition(direction);
        }
    }
}