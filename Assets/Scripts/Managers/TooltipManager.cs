using System.Collections;
using Collection;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem; 

namespace Managers
{
    public class TooltipManager : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private TextMeshProUGUI _tooltipText;
        [SerializeField] private Image _image;
        [SerializeField] private RectTransform _toolTipPanel;
        [SerializeField] private Canvas _canvas;
        
        [Header("Settings")]
        [SerializeField] private float _textSpeed = 0.05f;
        [SerializeField] private Vector2 _offset = new Vector2(10f, 40f);
        [SerializeField] private float _followSpeed = 10f;
        [SerializeField] private bool _smoothFollow = true;
        
        private Coroutine _textCoroutine;
        
        private void Awake()
        {
            if (_toolTipPanel ==  null)
            {
                _toolTipPanel = _image.GetComponent<RectTransform>();
            }
            
            ServiceLocator.Register(this);
        }
        
        private void OnDisable()
        {

            if (_textCoroutine != null)
            {
                StopCoroutine(_textCoroutine);
                _textCoroutine = null;
            }
        }

        private void OnDestroy()
        {
            ServiceLocator.Unregister<TooltipManager>();
        }
        
        private void Update()
        {
            if (_image.gameObject.activeSelf)
            {
                FollowMouse();
            }
        }
        
        public void ToolTipShow(string obj)
        {
            _image.gameObject.SetActive(true);
            StopAllCoroutines();
            _textCoroutine = StartCoroutine(IEToolTipShow(obj));
        }
        
        public void HideTooltip()
        {
            if (_textCoroutine != null)
            {
                StopCoroutine(_textCoroutine);
                _textCoroutine = null;
            }
            
            StopAllCoroutines();
            _image.gameObject.SetActive(false);
            _tooltipText.text = string.Empty;
        }

        private void FollowMouse()
        {
            // Use new Input System
            Vector2 mousePosition = Mouse.current.position.ReadValue();
            
            Vector2 localPoint;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                _canvas.transform as RectTransform,
                mousePosition,
                _canvas.worldCamera,
                out localPoint
            );

            Vector2 targetPosition = localPoint + _offset;

            if (_smoothFollow)
            {
                _toolTipPanel.localPosition = Vector2.Lerp(
                    _toolTipPanel.localPosition, 
                    targetPosition, 
                    Time.deltaTime * _followSpeed
                );
            }
            else
            {
                _toolTipPanel.localPosition = targetPosition;
            }
        }
        
        //TypeWriter text effect
        private IEnumerator IEToolTipShow(string tooltipText)
        {
            _tooltipText.text = "";

            foreach (char letters in tooltipText)
            {
                _tooltipText.text += letters;
                yield return new WaitForSeconds(_textSpeed);
            }
            
            _textCoroutine = null;
        }
        
    }
}