using System.Collections;
using Collection;
using UnityEngine;


namespace Managers
{
    public class AudioManager : MonoBehaviour
    {
        private AudioClip _audioClip;
        
        private float _audioClipLength;
        

        private void Awake()
        {
            ServiceLocator.Register(this);
        }

        private void OnDestroy()
        {
            ServiceLocator.Unregister<AudioManager>();
        }

        public AudioSource CreateAudioSource(AudioClip audioClip)
        { 
            GameObject audioObject = new GameObject("AudioSource");
            AudioSource audioSource = audioObject.AddComponent<AudioSource>();
            
            _audioClip = audioClip;
           audioSource.clip = _audioClip;
          _audioClipLength = _audioClip.length;
           
           audioSource.Play();
           
           StartCoroutine(EndAudio(_audioClipLength, audioSource));
           
           return audioSource;
        }

        private IEnumerator EndAudio(float time, AudioSource audioSource)
        {
            yield return new WaitForSeconds(time);
            if (audioSource != null)
            {
                Destroy(audioSource.gameObject);
            }
        }
    }

}
