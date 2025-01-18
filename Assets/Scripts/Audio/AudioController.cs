using UnityEngine;

namespace Audio
{
    public class AudioController : MonoBehaviour
    {
        //* ENCAPSULATION
        private int _audioSourcesIndex;
        private double _goalTime;
        private double _musicDuration;
        public float MusicVolume { get; set; }
        public float EffectVolume { get; set; }
        
        private AudioSource[] _audioSources;
        public AudioClip[] gameMusicClips;
        public AudioClip[] menuMusicClips;
        public AudioClip[] victoryMusicClips;
        private AudioClip[] _currentMusicClips;
        
        
        
        public static AudioController instance;

        //* ABSTRACTION
        private void Awake()
        {
            if (instance != null)
            {
                Destroy(gameObject);
                return;
            }
            instance = this;
            DontDestroyOnLoad(gameObject);
            EffectVolume = 1f;
            MusicVolume = 1f;
        }
        
        private void Start()
        {
            MusicVolume = 1f;
            EffectVolume = 1f;
            _audioSources = new AudioSource[2];
            _audioSources[0] = transform.GetChild(0).gameObject.GetComponent<AudioSource>();
            _audioSources[1] = transform.GetChild(1).gameObject.GetComponent<AudioSource>();
            _currentMusicClips = menuMusicClips;
            OnPlayMusic();
        }

        public void OnPlayMusic()
        {
            _audioSources[_audioSourcesIndex].clip = _currentMusicClips[_audioSourcesIndex];
            _audioSources[_audioSourcesIndex].Play();
            _musicDuration = _currentMusicClips[_audioSourcesIndex].length;
            _goalTime = AudioSettings.dspTime + _musicDuration;
            
            _audioSourcesIndex = 1 - _audioSourcesIndex;
        }
        
        private void Update()
        {
            if (AudioSettings.dspTime >= _goalTime - 1)
            {
                PlayScheculedClip();
            }
        }

        private void PlayScheculedClip()
        {
            _audioSources[_audioSourcesIndex].clip = _currentMusicClips[_audioSourcesIndex];
            _audioSources[_audioSourcesIndex].PlayScheduled(_goalTime);
            
            _musicDuration = _currentMusicClips[_audioSourcesIndex].length;
            _goalTime += _musicDuration;
            
            _audioSourcesIndex = 1 - _audioSourcesIndex;
        }

        public void PlayMusicAtPoint(AudioClip clip,Vector2 position)
        {
            AudioSource.PlayClipAtPoint(clip, position, MusicVolume);
        }

        public void PlayEffectAtPoint(AudioClip clip, Vector2 position)
        {
            AudioSource.PlayClipAtPoint(clip, position, EffectVolume * 10);
        }
        
        public void PlayEffectAtPoint(AudioClip clip, Vector2 position, float volume)
        {
            AudioSource.PlayClipAtPoint(clip, position, volume * EffectVolume);
        }

        public void ChangeCurrentMusicClips(AudioClip[] clips)
        {
            _currentMusicClips = clips;
        }

        public void ChangeCurrentMusicClips(string menuOrGameOrVictory)
        {
            switch (menuOrGameOrVictory)
            {
                case "Menu":
                    _currentMusicClips = menuMusicClips;
                    StopMusic();
                    _audioSourcesIndex = 0;
                    OnPlayMusic();
                    break;
                case "Game":
                    _currentMusicClips = gameMusicClips;
                    StopMusic();
                    _audioSourcesIndex = 0;
                    OnPlayMusic();
                    break;
                case "Victory":
                    _currentMusicClips = victoryMusicClips;
                    StopMusic();
                    _audioSourcesIndex = 0;
                    OnPlayMusic();
                    break;
            }
        }

        public bool CheckCurrentMusicClips(string menuOrGame)
        {
            switch (menuOrGame)
            {
                case "Menu":
                    return _currentMusicClips == menuMusicClips;
                case "Game":
                    return _currentMusicClips == gameMusicClips;
                default:
                    return true;
            }
        }

        public void ApplyMusicVolume()
        {
            _audioSources[0].volume = MusicVolume;
            _audioSources[1].volume = MusicVolume;
        }

        public void StopMusic()
        {
            _audioSources[0].Stop();
            _audioSources[1].Stop();
        }

        


        
        
        
    }
}
