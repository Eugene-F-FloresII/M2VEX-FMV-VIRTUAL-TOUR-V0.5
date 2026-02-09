using Collection;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Video;


namespace Gameplay
{
    public class SkipVideo : MonoBehaviour, IPointerDownHandler
    {
        private VideoPlayer _videoPlayer;

        private void Start()
        {
           _videoPlayer = ServiceLocator.Get<VideoPlayer>();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            SkipCurrentVideo();
        }
        private void SkipCurrentVideo()
        {
            if (_videoPlayer.isPlaying && _videoPlayer != null)
            {
                _videoPlayer.time = _videoPlayer.length - 0.1f;
                _videoPlayer.Play();
            }
        }
    }

}
