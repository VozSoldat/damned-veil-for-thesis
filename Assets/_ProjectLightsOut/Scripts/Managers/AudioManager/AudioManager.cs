using System;
using System.Collections;
using System.Collections.Generic;
using ProjectLightsOut.DevUtils;
using UnityEngine;

namespace ProjectLightsOut.Managers
{
    [Serializable] public class AudioData
    {
        public string name;
        public AudioClip clip;
        public AudioSource audioSource;
    }

    public class AudioManager : Singleton<AudioManager>
    {
        [SerializeField] private List<AudioData> audioData = new List<AudioData>();
        private Action OnFadeOutComplete;
        public static bool IsBGMPlaying { get; private set; }
        private Coroutine BGMCoroutine;

        protected override void Awake()
        {
            base.Awake(); 

            DontDestroyOnLoad(gameObject);
        }

        private void OnEnable()
        {
            EventManager.AddListener<OnPlaySFX>(OnPlayAudio);
            EventManager.AddListener<OnPlayBGM>(OnPlayBGM);
            EventManager.AddListener<OnStopBGM>(OnStopBGM);
        }

        private void OnDisable()
        {
            EventManager.RemoveListener<OnPlaySFX>(OnPlayAudio);
            EventManager.RemoveListener<OnPlayBGM>(OnPlayBGM);
            EventManager.RemoveListener<OnStopBGM>(OnStopBGM);
        }

        private void OnPlayAudio(OnPlaySFX evt)
        {
            var data = audioData.Find(x => x.name.ToLower() == evt.AudioName.ToLower());

            if (data == null)
            {
                Debug.LogError($"AudioManager: {evt.AudioName} not found.");
                return;
            }

            //AudioClip clip = data.clip;

            // if (data.clip != null)
            // {
            //     data.audioSource.clip = data.clip;
            // }

            // else
            // {
            //     //Debug.Log("AudioManager: AudioClip is null. Using default AudioClip.");
            // }

            data.audioSource.Play();
            //data.audioSource.clip = clip;
        }

        private void OnStopBGM(OnStopBGM evt)
        {
            IsBGMPlaying = false;

            AudioData data = audioData.Find(x => x.name.ToLower() == evt.AudioName.ToLower());

            if (data == null)
            {
                Debug.LogError($"AudioManager: {evt.AudioName} not found.");
                return;
            }

            if (evt.FadeOut > 0)
            {
                if (BGMCoroutine != null)
                {
                    StopCoroutine(BGMCoroutine);
                }

                BGMCoroutine = StartCoroutine(FadeOut(data.audioSource, evt.FadeOut));
            }

            else
            {
                data.audioSource.Stop();
            }
        }

        private void OnPlayBGM(OnPlayBGM evt)
        {
            IsBGMPlaying = true;
            
            AudioData data = audioData.Find(x => x.name.ToLower() == evt.AudioName.ToLower());

            if (data == null)
            {
                Debug.LogError($"AudioManager: {evt.AudioName} not found.");
                return;
            }

            if (data.audioSource.isPlaying)
            {
                if (BGMCoroutine != null)
                {
                    StopCoroutine(BGMCoroutine);
                }
                
                BGMCoroutine = StartCoroutine(FadeOut(data.audioSource, 3f));

                OnFadeOutComplete = () => PlayBGM(data, evt.FadeIn);
            }

            else
            {
                PlayBGM(data, evt.FadeIn);
            }
        }

        private void PlayBGM(AudioData data, float fadeIn)
        {
            OnFadeOutComplete = null;
            
            if (data.clip != null)
            {
                data.audioSource.clip = data.clip;
            }

            else
            {
                //Debug.Log("AudioManager: AudioClip is null. Using default AudioClip.");
            }
                
            if (fadeIn > 0)
            {
                if (BGMCoroutine != null)
                {
                    StopCoroutine(BGMCoroutine);
                }
                BGMCoroutine = StartCoroutine(FadeIn(data.audioSource, fadeIn));
            }

            else
            {
                data.audioSource.Play();
            }
        }

        private IEnumerator FadeOut(AudioSource audioSource, float fadeTime)
        {
            float startVolume = audioSource.volume;

            while (audioSource.volume > 0)
            {
                audioSource.volume -= startVolume * Time.deltaTime / fadeTime;

                yield return null;
            }

            audioSource.Stop();
            audioSource.volume = startVolume;
            OnFadeOutComplete?.Invoke();
        }

        private IEnumerator FadeIn(AudioSource audioSource, float fadeTime)
        {
            float startVolume = 1;
            audioSource.volume = 0;
            audioSource.Play();

            while (audioSource.volume < startVolume)
            {
                audioSource.volume += startVolume * Time.deltaTime / fadeTime;

                yield return null;
            }

            audioSource.volume = startVolume;
        }
    }
}