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
        
        [Header("List of Buttons in Area (**READ ONLY**)")]
        [SerializeField] private List<GameObject> _buttonControllers;
        
        [Header("Areas to look at?")]
        [SerializeField] private bool _lookRight;
        [SerializeField] private bool _lookLeft;
        [SerializeField] private bool _lookUp;
        [SerializeField] private bool _back;
        [SerializeField] private bool _forward;

        [Header("Are buttons Interactable (Left and Right areas)")] 
        [SerializeField] private bool _lookRightInteractable;
        [SerializeField] private bool _lookLeftInteractable;
        
        [Header("Is Transition reversed?")]
        [SerializeField] private bool _forwardTransition;
        [SerializeField] private bool _backwardTransition;
        [SerializeField] private bool _rightTransition;
        [SerializeField] private bool _leftTransition;
        
        
        [Header("Tooltip")]
        [SerializeField] private string _areaRightToolTip;
        [SerializeField] private string _areaLeftToolTip;
        [SerializeField] private string _areaUpToolTip;
        [SerializeField] private string _areaForwardToolTip;
        [SerializeField] private string _areaBackwardToolTip;
        
        [Header("Location Name")]
        [SerializeField] public string _locationName;

        [Header("Does area have minigame?")] 
        [SerializeField] public bool _lockArea;

        [Header("Transition Destination(Use elements in AreaManager)")] 
        [SerializeField] private int _rightDestination;
        [SerializeField] private int _leftDestination;
        [SerializeField] private int _forwardDestination;
        [SerializeField] private int _backDestination;
        
        private PadLockController _padLockController;
        private LocationManager _locationManager;
        private EventLogsManager _eventLogsManager;
        private LookController _lookRightController;
        private LookController _lookLeftController;
        private LookController _lookUpController;
        private TransitionController _forwardTransitionController;
        private TransitionController _backwardTransitionController;
        private TransitionController _rightTransitionController;
        private TransitionController _leftTransitionController;
        
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

                GetTransitionController(_lookRightController,
                    _rightDestination,
                    _rightTransition,
                    _lookRightInteractable,
                    _areaRightToolTip
                );
                
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
                
                GetTransitionController(_lookLeftController,
                    _leftDestination,
                    _leftTransition,
                    _lookLeftInteractable,
                    _areaLeftToolTip
                );
                
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
                    _forwardTransition,
                    _forwardDestination);

                _buttonControllers.Add(_forwardTransitionController.gameObject);
            }

            if (_back && _backwardButtonPrefab != null)
            {
                _backwardTransitionController = CreateTransitionController(_backwardButtonPrefab,
                    _areaBackwardToolTip,
                    _backwardTransition,
                    _backDestination);
                
                _buttonControllers.Add(_backwardTransitionController.gameObject);
            }
        }

        private LookController GetTransitionController(LookController lookController, int roomTargetIndex, bool transition,
            bool isInteractable, string tooltip)
        {
            var controller = lookController;
            var transitionController = controller.GetComponent<TransitionController>();
            transitionController.Initialize(this);
            transitionController.SetBoolVariable(transition);
            transitionController._toolTipMessage = tooltip;
            transitionController._isInteractable = isInteractable;
            transitionController._roomTargetIndex = roomTargetIndex;
            return controller;
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
            bool transition,
            int destination
        )
        {
            var transitionController = Instantiate(transitionControllerPrefab, _areaCanvas.transform);
            transitionController.Initialize(this);
            transitionController.PlaceToolTipMessage(toolTip);
            transitionController.SetBoolVariable(transition);
            transitionController._roomTargetIndex = destination;
            
            return transitionController;
        }
        

    }
    
    
}
