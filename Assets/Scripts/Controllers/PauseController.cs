using UnityEngine;
using UnityEngine.InputSystem;


namespace Controllers
{
    public class PauseController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private PlayerInput _playerInput;
        [SerializeField] private Canvas _pauseCanvas;

        private InputAction _inputAction;
        private bool _isPaused;


        private void Awake()
        {
            _inputAction = _playerInput.actions["Pause"];
            
            _pauseCanvas.gameObject.SetActive(false);
            
            _isPaused = false;
        }

        private void OnEnable()
        {
            _inputAction.performed += PauseGame;
        }

        private void OnDisable()
        {
            _inputAction.performed -= PauseGame;
        }


        private void PauseGame(InputAction.CallbackContext context)
        {
            _isPaused = !_isPaused;
            Time.timeScale = _isPaused ? 0 : 1;
            _pauseCanvas.gameObject.SetActive(_isPaused ? true : false);
        }
    }

}
