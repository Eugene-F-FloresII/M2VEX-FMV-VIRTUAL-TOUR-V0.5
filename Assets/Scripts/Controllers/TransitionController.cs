using Collection;
using Data;
using Managers;
using Obvious.Soap;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Video;

namespace Controllers
{
    public class TransitionController : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
    {
        public int _roomTargetIndex;
        public bool _isInteractable;
        
        [Header("References")]
        [SerializeField] private TransitionScriptableObject _transitionScriptableObject;
        
        [Header("SOAP Information")]
        [SerializeField] private IntVariable _roomIndex;
        [SerializeField] private bool _isReversed;
        
        [Header("Tooltip Message")]
        [SerializeField] public string _toolTipMessage;

        private readonly string _transitionMessage = "Transitioning to: ";
        private string _eventLogMessage;
        
        private LocationManager _locationManager;
        private AreaManager _areaManager;
        private VideoPlayer _videoPlayer;
        private TooltipManager _tooltipManager;
        private AreaController _areaController;
        private EventLogsManager _eventLogsManager;
        private TransitionManager _transitionManager;

        private void Start()
        {
            _locationManager = ServiceLocator.Get<LocationManager>();
            _videoPlayer = ServiceLocator.Get<VideoPlayer>();
            _tooltipManager = ServiceLocator.Get<TooltipManager>();
            _areaManager = ServiceLocator.Get<AreaManager>();
            _eventLogsManager = ServiceLocator.Get<EventLogsManager>();
            _transitionManager = ServiceLocator.Get<TransitionManager>();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (_isInteractable)
            {
                PlayTransition();

                _tooltipManager.HideTooltip();

                _locationManager.DisappearLocationText();

                if (_isReversed && _transitionMessage != null)
                {
                    _eventLogsManager.InstantiateEventLogs(_transitionMessage,
                        _areaManager._areas[_roomIndex.Value - 1].gameObject.name);

                }
                else if (!_isReversed && _transitionMessage != null)
                {
                    _eventLogsManager.InstantiateEventLogs(_transitionMessage,
                        _areaManager._areas[_roomIndex.Value + 1].gameObject.name);

                }
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            _tooltipManager.ToolTipShow(_toolTipMessage);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _tooltipManager.HideTooltip();
        }

        private void PlayTransition()
        {
            _areaController.DestroyButtons();
            
            _transitionManager.InstantiateSkipVideo();
            
            if (!_isReversed && _roomIndex != null && _transitionScriptableObject != null)
            {
                _videoPlayer.clip = _transitionScriptableObject._transitionClips[_roomIndex.Value];
                
            } else if (_isReversed && _roomIndex != null && _transitionScriptableObject != null)
            {
                _videoPlayer.clip = _transitionScriptableObject._transitionReverseClips[_roomIndex.Value - 1];
            }
            
            _videoPlayer.Play();
            
            _videoPlayer.loopPointReached += TransitionFinished;
        }

        private void TransitionFinished(VideoPlayer source)
        {
            _roomIndex.Value = _roomTargetIndex;
            
            _transitionManager.DestroySkipVideo();
            
            _areaManager.SetActiveArea();
            
            UnsubscribeVideoEvents();
        }
        
        private void UnsubscribeVideoEvents()
        {
            _videoPlayer.loopPointReached -= TransitionFinished;
        }

        public void PlaceToolTipMessage(string message)
        {
            _toolTipMessage =  message;
        }

        public void SetBoolVariable(bool value)
        {
            _isReversed = value;
        }

        public void Initialize(AreaController areaController)
        {
            _areaController = areaController;
        }
        
        
        
    }

}
