using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class VictoryAndDefeatCallback : MonoBehaviour
    {
        private bool _isTimeOver;
        public GameObject victoryAndDefeatText;
        
        //* ABSTRACTION
        void Start()
        {
            StartCoroutine(Cooldown());
        }

        void Update()
        {
            if (_isTimeOver && Input.anyKeyDown)
            {
                SceneManager.LoadScene(0);
            }
        }

        private IEnumerator Cooldown()
        {
            yield return new WaitForSeconds(2f);
            _isTimeOver = true;
            victoryAndDefeatText.SetActive(true);
        }
    }
}
