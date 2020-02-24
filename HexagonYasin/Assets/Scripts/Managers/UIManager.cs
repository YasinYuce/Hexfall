using UnityEngine;
using System.Collections;

namespace YasinYuce.HexagonYasin
{
    public class UIManager : MonoBehaviour
    {
        public GameObject GameOverPanel;

        public void StartGame()
        {
            GameOverPanel.SetActive(false);
        }

        public void GameOver()
        {
            StartCoroutine(DelayedGameOverPanel());
        }

        private IEnumerator DelayedGameOverPanel()
        {
            yield return new WaitForSeconds(1f);
            GameOverPanel.SetActive(true);
        }
    }
}