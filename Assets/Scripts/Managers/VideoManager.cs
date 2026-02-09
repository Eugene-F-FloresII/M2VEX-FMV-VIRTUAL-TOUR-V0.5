using System;
using Collection;
using Controllers;
using UnityEngine;
using UnityEngine.Video;

namespace Managers
{
    /// <summary>
    /// I know(2x) "Why didn't you just use Service Locators?" its cuz I realized late
    /// I'll change it (someday lmao) 
    /// </summary>
    public class VideoManager : MonoBehaviour
    {
        private VideoPlayer _videoPlayer;
        private AreaController _areaController;

        private string _currentActiveArea;
        
        public static Action<UnityEngine.Video.VideoClip, bool, bool>  OnPickedVideo { get; set; }
        public static Action OnExitedHover {get; set;}
        
        public static Action<string> OnReversedFinished {get; set;}
        
        public static Action<AreaController> OnVideoReversedFinished {get; set;}

        private void Awake()
        {
            _videoPlayer = GetComponent<VideoPlayer>();
            
            
            ServiceLocator.Register(_videoPlayer);
        }

        private void OnDestroy()
        {
            ServiceLocator.Unregister<VideoPlayer>();
        }

        private void OnEnable()
        {
            VideoClipManager.OnVideoClipPicked += PlayVideo;
            OnVideoReversedFinished += AreaController;
            OnReversedFinished += Inject;
            OnPickedVideo += PlayVideo;
            OnExitedHover += UnsubscribeVideoEvent;
        }

        private void OnDisable()
        {
            VideoClipManager.OnVideoClipPicked -= PlayVideo;
            OnPickedVideo -= PlayVideo;
            OnExitedHover -= UnsubscribeVideoEvent;
            OnReversedFinished -= Inject;
            OnVideoReversedFinished -= AreaController;
        }

        public void PlayVideo(UnityEngine.Video.VideoClip videoClip, bool loop, bool reverse)
        {
            _videoPlayer.clip = videoClip;
            _videoPlayer.isLooping = loop;

            if (!_videoPlayer.isLooping && !reverse)
            {
                _videoPlayer.loopPointReached += VideoPlayed;
            }
            else
            {
                Debug.Log("reverse is true");
            }

            if (!_videoPlayer.isLooping && reverse)
            {
                _videoPlayer.loopPointReached += ReversePlayed;
            }
        }

        private void VideoPlayed(VideoPlayer source)
        {
            source.Pause();
        }

        private void ReversePlayed(VideoPlayer source)
        {
            Debug.Log("Video Reversed");
            VideoClipManager.OnPickedIdleClipName?.Invoke(_currentActiveArea);
            ImagesManager.OnPickedImage?.Invoke(_currentActiveArea);
            _areaController.SpawnButtons();
            UnsubscribeVideoEvent();
        }

        private void Inject(string currentClipName)
        {
            _currentActiveArea = currentClipName;
        }

        private void AreaController(AreaController areaController)
        {
            _areaController = areaController;
        }
        
        private void UnsubscribeVideoEvent()
        {
            _videoPlayer.loopPointReached -= VideoPlayed;
            _videoPlayer.loopPointReached -= ReversePlayed;
        }
    }
}