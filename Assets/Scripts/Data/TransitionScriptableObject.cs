using System.Collections.Generic;
using UnityEngine;


namespace Data
{
    [CreateAssetMenu(fileName = "TransitionClipsData", menuName = "Data/Transition Clips")]
    public class TransitionScriptableObject : ScriptableObject
    {
        [Header("Forward Transition")]
        public List<UnityEngine.Video.VideoClip> _transitionClips;
        
        [Header("Reverse Transition")]
        public List<UnityEngine.Video.VideoClip> _transitionReverseClips;
    }
}


