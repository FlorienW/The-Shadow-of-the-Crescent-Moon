using System.Collections;
using Audio;
using Enemy;
using UnityEngine;

public class DemoDummyVictory : MonoBehaviour
{
    private Knight _knight;
    public AudioClip victoryIntro;
    private bool _isWorked;
    public GameObject victoryScreen;

    //* ABSTRACTION
    private void Start()
    {
        _knight = GetComponent<Knight>();
    }

    private void Update()
    {
        if (!_isWorked)
        {
            CheckIsEnemyDead();
        }
    }

    private void CheckIsEnemyDead()
    {
        if (_knight.enemyHealthPoint <= 0)
        {
            _isWorked = true;
            StartCoroutine(PlayVictoryMusic());
            ActivateTheVictoryScreen();
        }
    }

    private IEnumerator PlayVictoryMusic()
    {
        AudioController.instance.StopMusic();
        AudioController.instance.PlayMusicAtPoint(victoryIntro, transform.position);
        yield return new WaitForSeconds(victoryIntro.length);
        AudioController.instance.ChangeCurrentMusicClips("Victory");
    }

    private void ActivateTheVictoryScreen()
    {
        victoryScreen.SetActive(true);
    }
}
