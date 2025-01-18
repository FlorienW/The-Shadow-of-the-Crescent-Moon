using System;
using UnityEngine;

namespace UI
{
    public class UIManager : MonoBehaviour
    {
        private bool _isSettingsOpen;

        public GameObject settingsUI;
        public GameObject titleScreenUI;
        
        //* ABSTRACTION
        public virtual void ReturnToTitleScreen()
        {
            settingsUI.SetActive(false);
            titleScreenUI.SetActive(true);
            _isSettingsOpen = false;
        }


        public virtual void ReturnToSettingsScreen()
        {
            settingsUI.SetActive(true);
            titleScreenUI.SetActive(false);
            _isSettingsOpen = true;
        }
        
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (_isSettingsOpen)
                {
                    ReturnToTitleScreen();
                }
                else
                {
                    ReturnToSettingsScreen();
                }
            }
        }
    }
}
