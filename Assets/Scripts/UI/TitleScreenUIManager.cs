using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    //* INHERITANCE
    public class TitleScreenUIManager : UIManager
    {

        //* ABSTRACTION
        public void LoadTheGame()
        {
            SceneManager.LoadScene(1);
        }
        
        public void ExitGame()
        {
            #if UNITY_EDITOR
            UnityEditor.EditorApplication.ExitPlaymode();
            #else
            Application.Quit();
            #endif
        }
    
    }
}
