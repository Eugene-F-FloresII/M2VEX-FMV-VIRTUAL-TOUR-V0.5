using UnityEngine;
using UnityEngine.UI;
using PrimeTween;
using UnityEngine.EventSystems;
using Controllers;

namespace Gameplay
{
    public class Card : MonoBehaviour, IPointerDownHandler
    {
        [SerializeField] private Image _iconImage;
        [SerializeField] private Sprite _hiddenIconSprite;
        [SerializeField] public Sprite _iconSprite;
        
        private bool _isSelected;
        private CardController _cardController;
    
        
        public void OnPointerDown(PointerEventData eventData)
        {
            _cardController.SetSelected(this);
            _cardController.IsCardSelected(_isSelected);
        }
        
        public void Show()
        {
            Tween.Rotation(transform, //the target
                new Vector3(0f, 180f, 0f), // 180 degrees in y axis
                0.5f); // in 2 seconds
    
            Tween.Delay(0.1f, ()=> _iconImage.sprite = _iconSprite);
          
            _isSelected = true;
        }
    
        public void Hide()
        {
            Tween.Rotation(transform, //the target
                new Vector3(0f, 0f, 0f), // 180 degrees in y axis
                0.5f); // in 2 seconds
    
            Tween.Delay(0.1f, () =>
            {
                _iconImage.sprite = _hiddenIconSprite;
                _isSelected = false;
            });
        }
        public void SetIconSprite(Sprite sp)
            {
                _iconSprite = sp;
            }
    
        public void Initialize(CardController cardController)
        {
            _cardController =  cardController;
        }
        
    }

}
