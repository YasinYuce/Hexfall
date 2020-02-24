using UnityEngine;
using System.Collections;
using System;

namespace YasinYuce.HexagonYasin
{
    public class PieceParticle : MonoBehaviour, IResettable
    {
        public Action Release { get; set; }

        public void ShowParticle(Vector3 pos, Color c)
        {
            transform.position = pos;
            var main = GetComponent<ParticleSystem>().main;
            Color forAlphaChange = c;
            forAlphaChange.a = 0.75f;
            main.startColor = forAlphaChange;
            StartCoroutine(DelayedStop());
        }

        private IEnumerator DelayedStop()
        {
            yield return new WaitForSeconds(1f);
            Reset();
        }

        public void Reset()
        {
            gameObject.SetActive(false);
            Release?.Invoke();
        }
    }
}