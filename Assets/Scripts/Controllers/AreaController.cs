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
        
        //private readonly string _eventLogText = "Looking at: ";
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
               _lookRightController = Instantiate(_lookRightButtonPrefab, _areaCanvas.transform);
               _lookRightController.Initialize(this, _areaRightToolTip);
               _lookRightController.AreaVideoClip(_areaScriptableObject._lookRightClip, _areaClipName);

               
               if (_areaScriptableObject._lookRightClipReversed != null)
               {
                   _lookRightController.AreaReverseClip(_areaScriptableObject._lookRightClipReversed, _areaRightClipName);
               }
               
               _buttonControllers.Add(_lookRightController.gameObject);
            }
            
            if (_lookLeft && _areaScriptableObject._lookLeftClip != null)
            {
                _lookLeftController = Instantiate(_lookLeftButtonPrefab, _areaCanvas.transform);
                _lookLeftController.Initialize(this, _areaLeftToolTip);
                _lookLeftController.AreaVideoClip(_areaScriptableObject._lookLeftClip, _areaClipName);


                
                if (_areaScriptableObject._lookLeftClipReversed != null)
                {
                    _lookLeftController.AreaReverseClip(_areaScriptableObject._lookLeftClipReversed, _areaLeftClipName);
                }
               
                _buttonControllers.Add(_lookLeftController.gameObject);
            }
            
            if (_lookUp && _areaScriptableObject._lookUpClip != null)
            {
                _lookUpController = Instantiate(_lookUpButtonPrefab, _areaCanvas.transform);
                _lookUpController.Initialize(this, _areaUpToolTip);
                _lookUpController.AreaVideoClip(_areaScriptableObject._lookUpClip, _areaClipName);
         
                
                if (_areaScriptableObject._lookUpClipReversed != null)
                {
                    _lookUpController.AreaReverseClip(_areaScriptableObject._lookUpClipReversed, _areaUpClipName);
                }
               
                _buttonControllers.Add(_lookUpController.gameObject);
            }

            if (_forward && _forwardButtonPrefab != null)
            {
                _forwardTransitionController = Instantiate(_forwardButtonPrefab, _areaCanvas.transform);
                _forwardTransitionController.Initialize(this);
                _forwardTransitionController.PlaceToolTipMessage(_areaForwardToolTip);
                _forwardTransitionController.SetBoolVariable(_forwardTransition);
  
                
                _buttonControllers.Add(_forwardTransitionController.gameObject);

            }

            if (_back && _backwardButtonPrefab != null)
            {
                _backwardTransitionController = Instantiate(_backwardButtonPrefab, _areaCanvas.transform);
                _backwardTransitionController.Initialize(this);
                _backwardTransitionController.PlaceToolTipMessage(_areaBackwardToolTip);
                _backwardTransitionController.SetBoolVariable(_forwardTransition);
                
                _buttonControllers.Add(_backwardTransitionController.gameObject);
            }
            
            
            
            
            
            
        }

        
    }
    
    
}
