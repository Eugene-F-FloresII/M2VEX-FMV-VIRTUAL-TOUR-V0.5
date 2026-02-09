using System.Collections.Generic;
using Collection;
using Obvious.Soap;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Controllers
{
    /// <summary>
    /// Mainly communicates with Area Manager through _areaActive
    /// </summary>
    public class MinimapController : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private float _speed = 1f;
        
        [Header("References")]
        [SerializeField] private Image _dotImage; 
        [SerializeField] private Canvas _minimapCanvas;
        [SerializeField] private IntVariable _areaActive;
        [SerializeField] private PlayerInput _playerInput;
        [SerializeField] private List<Transform> _minimapPoints;
        
        private bool _isMinimapOpen;
        private InputAction _inputAction;
        private float _move;


        /// <summary>
        /// Register MinimapController to Service Locator
        /// </summary>
        private void Awake()
        {
            ServiceLocator.Register(this);
            _inputAction = _playerInput.actions["Minimap"];
        }

        private void Start()
        {
            _minimapCanvas.gameObject.SetActive(false);
        }

        private void OnEnable()
        {
            _inputAction.performed += ShowMinimap;
        }

        private void OnDisable()
        {
            _inputAction.performed -= ShowMinimap;
        }

        private void Update()
        {
            if (_isMinimapOpen)
            {
                UpdateDotLocation(_areaActive.Value);
            }
        }

        /// <summary>
        /// Unregister MinimapController to Service Locator
        /// </summary>
        private void OnDestroy()
        {
            ServiceLocator.Unregister<MinimapController>();
        }

        /// <summary>
        /// Updates the dot location of the minimap
        /// </summary>
        public void UpdateDotLocation(int value)
        {
            _move = _speed * Time.deltaTime;

            _dotImage.gameObject.transform.position = Vector2.MoveTowards(_dotImage.gameObject.transform.position,
                _minimapPoints[value].transform.position,
                _move);
        }

        /// <summary>
        /// For mobile implementation
        /// </summary>
        public void ShowMinimapButton()
        {
            _isMinimapOpen = !_isMinimapOpen;
            _minimapCanvas.gameObject.SetActive(_isMinimapOpen ? true : false);
        }
        
        /// <summary>
        /// For Keyboard implementation
        /// </summary>
        private void ShowMinimap(InputAction.CallbackContext context) {
        {
            _isMinimapOpen = !_isMinimapOpen;
            _minimapCanvas.gameObject.SetActive(_isMinimapOpen ? true : false);

        }}
    }

}
