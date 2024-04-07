using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Entities
{
    public class BoltSpawner : MonoBehaviour
    {
        public int maxBolts;
        public GameObject bolt;

        public bool standalone;

        private bool _spawning = true;

        private List<GameObject> _bolts;

        private void Awake()
        {
            _bolts = new List<GameObject>();

            for (var i = 0; i < maxBolts; i++)
            {
                _bolts.Add(Instantiate(bolt, Vector3.zero, Quaternion.identity));
                _bolts[i].SetActive(false);
            }
        }

        private void Start()
        {
            if (standalone)
                StartCoroutine(Spawning());
        }

        private IEnumerator Spawning()
        {
            while (_spawning)
            {
                Spawn();
                yield return new WaitForSeconds(.5f);
            }
        }

        private void Spawn()
        {
            SpawnAt(transform.position + (Vector3) Random.insideUnitCircle * .3f);
        }

        private GameObject GetBolt()
        {
            for (var i = 0; i < maxBolts; i++)
                if (!_bolts[i].activeSelf)
                    return _bolts[i];

            _bolts.Add(Instantiate(bolt, transform));
            return _bolts[_bolts.Count - 1];
        }

        public void SpawnAt(Vector2 v)
        {
            var b = GetBolt();

            b.SetActive(true);
            b.transform.position = v;
        }
    }
}