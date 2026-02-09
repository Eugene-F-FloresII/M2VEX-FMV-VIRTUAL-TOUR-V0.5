using System.Collections;
using Collection;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Managers
{
    public class LocationManager : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Image _locationImage;
        [SerializeField] private TextMeshProUGUI _locationText;
        [SerializeField] private Animator _animator;

        private readonly string _animationName = "IsAppearing";
        private AnimationClip _animationClip;
        private void Awake()
        {
            ServiceLocator.Register(this);
        }
        
        private void OnDestroy()
        {
            ServiceLocator.Unregister<LocationManager>();
        }

        /// <summary>
        /// Updates the location texts
        /// </summary>
        /// <param name="locationText"></param>
        public void UpdateLocationText(string locationText)
        {
            _locationText.text = locationText;
        }
        
        /// <summary>
        /// Show Location
        /// </summary>
        public void ShowLocationText()
        {
            _animator.SetBool(_animationName, true);
            
            _locationText.gameObject.SetActive(true);
            _locationImage.gameObject.SetActive(true);
        }
        
        /// <summary>
        /// Ends the animation and temporarily deactivates the gameobjects
        /// </summary>
        public void DisappearLocationText()
        {
            _animator.SetBool(_animationName, false);
           
            StartCoroutine(EndAnimation());

        }

        private IEnumerator EndAnimation()
        {
            yield return new WaitForSeconds(0.5f);
            _locationText.gameObject.SetActive(false);
            _locationImage.gameObject.SetActive(false);
        }
    }
}

