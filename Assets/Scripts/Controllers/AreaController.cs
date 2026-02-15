using System.Collections.Generic;
using Collection;
using Data;
using UnityEngine;
using Managers;

namespace Controllers
{
    
    public class AreaController : MonoBehaviour
    {
        [Header("Idle Video from VideoClipsManager")]
        [SerializeField] private string _areaClipName;
        
        [Header("Reverse Images Present")]
        [SerializeField] private string _areaRightClipName;
        [SerializeField] private string _areaLeftClipName;
        [SerializeField] private string _areaUpClipName;
        
        [Header("Prefab References")]
        [SerializeField] private LookController _lookRightButtonPrefab;
        [SerializeField] private LookController _lookLeftButtonPrefab;
        [SerializeField] private LookController _lookUpButtonPrefab;
        [SerializeField] private TransitionController _forwardButtonPrefab;
        [SerializeField] private TransitionController _backwardButtonPrefab;
        [SerializeField] private PadLockController _padLockPrefab;
        
        [Header("Scriptable Objects References")] 
        [SerializeField] private VideoClipsScriptableObject _areaScriptableObject;
        
        [Header("Canvas Reference")]
        [SerializeField] private Canvas _areaCanvas;
        [SerializeField] private Canvas _padlockCanvas;
        
        [Header("List of Buttons in Area (Don't add anything here)")]
        [SerializeField] private List<GameObject> _buttonControllers;
        
        [Header("Areas to look at?")]
        [SerializeField] private bool _lookRight;
        [SerializeField] private bool _lookLeft;
        [SerializeField] private bool _lookUp;
        [SerializeField] private bool _back;
        [SerializeField] private bool _forward;
        
        [Header("Is Transition reversed?")]
        [SerializeField] private bool _forwardTransition;
        
        [Header("Tooltip")]
        [SerializeField] private string _areaRightToolTip;
        [SerializeField] private string _areaLeftToolTip;
        [SerializeField] private string _areaUpToolTip;
        [SerializeField] private string _areaForwardToolTip;
        [SerializeField] private string _areaBackwardToolTip;
        
        [Header("Location Name")]
        [SerializeField] private string _locationName;

        [Header("Does area have minigame?")] 
        [SerializeField] public bool _lockArea;
        
        private PadLockController _padLockController;
        private LocationManager _locationManager;
        private EventLogsManager _eventLogsManager;
        private LookController _lookRightController;
        private LookController _lookLeftController;
        private LookController _lookUpController;
        private TransitionController _forwardTransitionController;
        private TransitionController _backwardTransitionController;
        
        private void Awake()
        {
            _locationManager = ServiceLocator.Get<LocationManager>();
        }

        private void OnEnable()
        {
            UpdateAreaPadlock();
            
            SpawnButtons();
            
            VideoClipManager.OnPickedIdleClipName?.Invoke(_areaClipName);
            
            _locationManager.UpdateLocationText(_locationName);

        }

        private void OnDisable()
        {
            if (_buttonControllers.Count != 0)
            {
                DestroyButtons();
            }
        }

        public void DestroyButtons()
                {
                    foreach (var controller in _buttonControllers)
                    {
                        if (controller != null)
                        {
                            Destroy(controller.gameObject);
                        }
                        else
                        {
                            Debug.LogWarning("You're trying to destroy nothing");
                        }
                    }
    
                    _buttonControllers.Clear();
                }

        public void UpdateAreaPadlock()
        {
            if (_lockArea && _padlockCanvas != null)
            {
                _padLockController = Instantiate(_padLockPrefab,_padlockCanvas.transform);
                _padLockController.Initialize(this);
            }
            else
            {
                if (_padLockController != null && _padlockCanvas != null)
                {
                    Destroy(_padLockController.gameObject);
                }
                
                Debug.Log("Not Locked");
            }
        }
        
       public void SpawnButtons()
        {
            _locationManager.ShowLocationText();
            
            if (_lookRight && _areaScriptableObject._lookRightClip != null)
            {
                _lookRightController = CreateLookButton(
                    _lookRightButtonPrefab,
                    _areaRightToolTip,
                    _areaScriptableObject._lookRightClip,
                    _areaScriptableObject._lookRightClipReversed,
                    _areaClipName);
                
                _buttonControllers.Add(_lookRightController.gameObject);
            }
            
            if (_lookLeft && _areaScriptableObject._lookLeftClip != null)
            {
                _lookLeftController = CreateLookButton(
                    _lookLeftButtonPrefab,
                    _areaLeftToolTip,
                    _areaScriptableObject._lookLeftClip,
                    _areaScriptableObject._lookLeftClipReversed,
                    _areaClipName);
                
                _buttonControllers.Add(_lookLeftController.gameObject);
            }
            
            if (_lookUp && _areaScriptableObject._lookUpClip != null)
            {
                _lookUpController = CreateLookButton(
                    _lookUpButtonPrefab,
                    _areaUpToolTip,
                    _areaScriptableObject._lookUpClip,
                    _areaScriptableObject._lookUpClipReversed,
                    _areaClipName);
                
                _buttonControllers.Add(_lookUpController.gameObject);
            }

            if (_forward && _forwardButtonPrefab != null)
            {
                _forwardTransitionController = CreateTransitionController(_forwardButtonPrefab,
                    _areaForwardToolTip,
                    _forwardTransition);

                _buttonControllers.Add(_forwardTransitionController.gameObject);
            }

            if (_back && _backwardButtonPrefab != null)
            {
                _backwardTransitionController = CreateTransitionController(_backwardButtonPrefab,
                    _areaBackwardToolTip,
                    _forwardTransition);
                
                _buttonControllers.Add(_backwardTransitionController.gameObject);
            }
        }
       
        private LookController CreateLookButton(LookController lookControllerPrefab,
            string toolTip,
            UnityEngine.Video.VideoClip mainClip,
            UnityEngine.Video.VideoClip reversedClip,
            string clipName)
        {
            
            var lookController = Instantiate(lookControllerPrefab, _areaCanvas.transform);
            lookController.Initialize(this, toolTip);
            lookController.AreaVideoClip(mainClip, clipName);

               
            if (reversedClip != null)
            {
                lookController.AreaReverseClip(reversedClip, clipName);
            }
            
            return lookController;
        }

        private TransitionController CreateTransitionController(TransitionController transitionControllerPrefab,
            string toolTip,
            bool transition
        )
        {
            var transitionController = Instantiate(transitionControllerPrefab, _areaCanvas.transform);
            transitionController.Initialize(this);
            transitionController.PlaceToolTipMessage(toolTip);
            transitionController.SetBoolVariable(transition);
            
            return transitionController;
        }


    }
    
    
}
