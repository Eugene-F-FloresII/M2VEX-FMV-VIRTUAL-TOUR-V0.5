using UnityEngine;

namespace Data
{
    
    [CreateAssetMenu(fileName = "VideoClipsData", menuName = "Data/Video Clips")]
    public class VideoClipsScriptableObject : ScriptableObject
    {
        [Header("Look right Video Clip")]
        [SerializeField] public UnityEngine.Video.VideoClip _lookRightClip;
        [SerializeField] public UnityEngine.Video.VideoClip _lookRightClipReversed;
        
        [Header("Look left Video Clip")]
        [SerializeField] public UnityEngine.Video.VideoClip _lookLeftClip;
        [SerializeField] public UnityEngine.Video.VideoClip _lookLeftClipReversed;
        
        [Header("Look up Video Clip")]
        [SerializeField] public UnityEngine.Video.VideoClip _lookUpClip;
        [SerializeField] public UnityEngine.Video.VideoClip _lookUpClipReversed;
        
    }
}