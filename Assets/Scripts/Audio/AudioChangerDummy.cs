using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Audio
{
    public class AudioChangerDummy : MonoBehaviour
    {
        private void Start()
        {
            StartCoroutine(ChangeMusic());
        }

        //* ABSTRACTION
        private IEnumerator ChangeMusic()
        {
            yield return new WaitForSeconds(0.5f);
            int buildIndex = SceneManager.GetActiveScene().buildIndex;
            Debug.Log(!AudioController.instance.CheckCurrentMusicClips("Menu"));
            Debug.Log(!AudioController.instance.CheckCurrentMusicClips("Game"));
            switch (buildIndex)
            {
                case 0:
                    if (!AudioController.instance.CheckCurrentMusicClips("Menu"))
                    {
                        AudioController.instance.ChangeCurrentMusicClips("Menu");
                    }
                    break;
                case 1:
                    if (!AudioController.instance.CheckCurrentMusicClips("Game"))
                    {
                        AudioController.instance.ChangeCurrentMusicClips("Game");
                    }
                    break;
            }
        }
    }
}
