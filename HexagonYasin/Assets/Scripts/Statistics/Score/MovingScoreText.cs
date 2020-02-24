using System;
using UnityEngine;

namespace YasinYuce.HexagonYasin
{
    public class MovingScoreText : MonoBehaviour, IResettable
    {
        private Vector3 targetPos;
        private TextMesh textMesh;

        public TextMesh TextMesh
        {
            get
            {
                if (!textMesh)
                {
                    textMesh = GetComponent<TextMesh>();
                }

                return textMesh;
            }
        }

        public Action Release { get; set; }

        public void Move(int addedScore, Vector3 groupsMiddlePos)
        {
            transform.position = groupsMiddlePos + Vector3.back;
            targetPos = transform.position + Vector3.up * 0.8f;
            TextMesh.text = addedScore.ToString();
            gameObject.SetActive(true);
        }

        public void Reset()
        {
            gameObject.SetActive(false);
            Release?.Invoke();
        }

        private void FixedUpdate()
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, 0.013f);
            if (Vector3.Distance(transform.position, targetPos) < 0.013f)
            {
                Reset();
            }
        }
    }
}