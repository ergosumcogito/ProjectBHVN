using System.Collections;
using UnityEngine;

namespace Core
{
    public class MusicManager : MonoBehaviour
    {
        public static MusicManager Instance { get; private set; }

        [SerializeField] private AudioSource source;

        private Coroutine _fadeRoutine;
        private bool _isFadingOut;

        private void Awake()
        {
            if (Instance && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);

            if (!source) source = GetComponent<AudioSource>();
            if (!source) source = gameObject.AddComponent<AudioSource>();

            source.playOnAwake = false;
            source.loop = true;
        }

        public void PlayLevelMusic(AudioClip clip, float volume = 1f, bool loop = true, float fadeIn = 0.5f,
            float fadeOut = 0.5f)
        {
            if (source.clip == clip && source.isPlaying && !_isFadingOut)
            {
                source.volume = volume;
                source.loop = loop;
                return;
            }

            _isFadingOut = false;

            if (_fadeRoutine != null) StopCoroutine(_fadeRoutine);

            source.Stop();
            source.clip = null;

            _fadeRoutine = StartCoroutine(SwapMusicRoutine(clip, volume, loop, fadeIn, fadeOut));
        }

        private IEnumerator SwapMusicRoutine(AudioClip newClip, float volume, bool loop, float fadeIn, float fadeOut)
        {
            if (source.isPlaying && fadeOut > 0f)
            {
                var startVol = source.volume;
                for (float t = 0; t < fadeOut; t += Time.unscaledDeltaTime)
                {
                    source.volume = Mathf.Lerp(startVol, 0f, t / fadeOut);
                    yield return null;
                }
            }

            source.Stop();
            source.clip = newClip;
            source.loop = loop;

            if (!newClip)
            {
                source.volume = 0f;
                yield break;
            }

            source.volume = 0f;
            source.Play();

            if (fadeIn > 0f)
            {
                for (float t = 0; t < fadeIn; t += Time.unscaledDeltaTime)
                {
                    source.volume = Mathf.Lerp(0f, volume, t / fadeIn);
                    yield return null;
                }
            }

            source.volume = volume;
        }

        public void StopMusic(float fadeOut = 0.25f)
        {
            if (_fadeRoutine != null) StopCoroutine(_fadeRoutine);

            _isFadingOut = true;

            if (fadeOut <= 0f)
            {
                source.Stop();
                source.clip = null;
                source.volume = 0f;
                _isFadingOut = false;
                return;
            }

            _fadeRoutine = StartCoroutine(FadeOutAndStop(fadeOut));
        }

        private IEnumerator FadeOutAndStop(float fadeOut)
        {
            if (!source.isPlaying)
            {
                source.clip = null;
                source.volume = 0f;
                _isFadingOut = false;
                yield break;
            }

            var startVol = source.volume;

            for (float t = 0; t < fadeOut; t += Time.unscaledDeltaTime)
            {
                source.volume = Mathf.Lerp(startVol, 0f, t / fadeOut);
                yield return null;
            }

            source.Stop();
            source.clip = null;
            source.volume = 0f;
            _isFadingOut = false;
        }
    }
}