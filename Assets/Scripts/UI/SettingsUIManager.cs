using Audio;

namespace UI
{
    //* INHERITANCE
    public class SettingsUIManager : UIManager
    {

        //* ABSTRACTION
        public void SetMusicVolume(float volume)
        {
            AudioController.instance.MusicVolume = volume;
            AudioController.instance.ApplyMusicVolume();
        }

        public void SetEffectVolume(float volume)
        {
            AudioController.instance.EffectVolume = volume;
        }
        
    }
}
