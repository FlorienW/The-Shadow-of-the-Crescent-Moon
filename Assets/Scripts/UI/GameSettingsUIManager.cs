using UnityEngine;

namespace UI
{
    //* INHERITANCE
    public class GameSettingsUIManager : UIManager
    {
        //* ABSTRACTION
        //* POLYMORPHISM
        public override void ReturnToTitleScreen()
        {
            Time.timeScale = 1f;
            base.ReturnToTitleScreen();
        }

        //* POLYMORPHISM
        public override void ReturnToSettingsScreen()
        {
            Time.timeScale = 0f;
            base.ReturnToSettingsScreen();
        }
    }
}
