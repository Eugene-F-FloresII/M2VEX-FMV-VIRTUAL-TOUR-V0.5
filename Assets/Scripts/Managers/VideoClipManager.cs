using System;
using System.Collections.Generic;
using UnityEngine;

namespace Managers
{

    public class VideoClipManager : MonoBehaviour
    {
        
        [Header("VideoClip Dictionary")]
        [SerializeField] private UnityEngine.Video.VideoClip[]  _idleClips;
        [SerializeField] private List<string> _clipNamesID;
        
        private Dictionary<string, UnityEngine.Video.VideoClip> _videoClips;
        /// <summary>
        /// Events
        /// </summary>
        public static Action<UnityEngine.Video.VideoClip, bool, bool> OnVideoClipPicked { get; set;}
        public static Action<string> OnPickedIdleClipName { get; set;}
        private void Awake()
        {
          
            InitializeVideoClipDictionary();
            
        }

        private void OnEnable()
        {
            OnPickedIdleClipName += GetClipName;
        }

        private void OnDisable()
        {
            OnPickedIdleClipName -= GetClipName;
        }

        
        
        /// <summary>
        /// Gives a name to a Video Clip with the same element as the _clipsNameID
        /// </summary>
        private void InitializeVideoClipDictionary()
        {
            
            _videoClips = new Dictionary<string, UnityEngine.Video.VideoClip>();

            if (_idleClips.Length != _clipNamesID.Count)
            {
                Debug.LogWarning("Error! Both Lists must have the same amount");
                return;
            }

            for (int i = 0; i < _idleClips.Length; i++)
            {
                if (!string.IsNullOrEmpty(_clipNamesID[i]) && _idleClips[i] != null)
                {
                    _videoClips.Add(_clipNamesID[i], _idleClips[i]);
                }
            }
        }
        
        
        /// <summary>
        ///  Gets the video clip
        /// </summary>
        private UnityEngine.Video.VideoClip GetVideoClip(string clipName)
        {
            if (_videoClips.TryGetValue(clipName, out UnityEngine.Video.VideoClip clip))
            {
                PlayVideoClip(clip);
                return clip;
            }
            
            Debug.LogWarning("VideoClip: " + clipName + " not found in dictionary!");
            return null;
        }

        private void PlayVideoClip(UnityEngine.Video.VideoClip videoClip)
        {
            OnVideoClipPicked?.Invoke(videoClip, true, false);
        }

        private void GetClipName(string clipName)
        {
            GetVideoClip(clipName);
        }
    }
}