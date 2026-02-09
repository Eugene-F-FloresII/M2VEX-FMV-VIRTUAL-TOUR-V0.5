using Collection;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Video;
using Managers;

namespace Controllers
{
    public class LookController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
 
        private VideoPlayer _videoPlayer;
        private TooltipManager _tooltipManager;
        private AreaController _areaController;
        private UnityEngine.Video.VideoClip _videoClip;
        private UnityEngine.Video.VideoClip _videoReverseClip;


        private string _currentClipName;
        private string _reverseClipName;
        private string _toolTipText;

        private void Start()
        {
            //Service Locator
            _videoPlayer = ServiceLocator.Get<VideoPlayer>();
            _tooltipManager = ServiceLocator.Get<TooltipManager>();
     
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (_videoPlayer.isLooping)
            {
                VideoManager.OnPickedVideo?.Invoke(_videoClip, false, false);
            }
            
            //ToolTip Show
            _tooltipManager.ToolTipShow(_toolTipText);

        }

        public void OnPointerExit(PointerEventData eventData)
        {
            
            if (_videoReverseClip != null && _videoPlayer.isPaused)
            {
                VideoManager.OnPickedVideo?.Invoke(_videoReverseClip, false, true);
                VideoManager.OnReversedFinished?.Invoke(_currentClipName);
                ImagesManager.OnPickedImage?.Invoke(_reverseClipName);
                VideoManager.OnVideoReversedFinished?.Invoke(_areaController);
                
                _areaController.DestroyButtons();
                
            }
            else
            {
                VideoClipManager.OnPickedIdleClipName?.Invoke(_currentClipName);
                VideoManager.OnExitedHover?.Invoke();
            }
            
            //Tooltip Hide
            _tooltipManager.HideTooltip();
        }

        public void AreaVideoClip(UnityEngine.Video.VideoClip videoClip, string areaName)
        {
            _videoClip = videoClip;
            _currentClipName = areaName;
        }

        public void AreaReverseClip(UnityEngine.Video.VideoClip videoClip, string reverseClipName)
        {
            _videoReverseClip = videoClip;
            _reverseClipName = reverseClipName;
        }

        public void Initialize(AreaController areaController, string tooltipText)
        {
            _areaController = areaController;
            _toolTipText = tooltipText;
        }
        
    }
}