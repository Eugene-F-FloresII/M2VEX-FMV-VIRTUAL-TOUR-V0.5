using System.Collections;
using Managers;
using TMPro;
using UnityEngine;

namespace Controllers
{
    public class EventLogsController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private TextMeshProUGUI _eventLogText;
        [SerializeField] private Animator _animator;
        
        
        private readonly string _eventLogAnimationName = "IsPoppedUp";
        
        private EventLogsManager _eventLogsManager;
        
        
        private void OnEnable()
        {
            _animator.SetBool(_eventLogAnimationName, true);
        }
        
        public void EventLogText(string text,  string pointOfInterest)
        {
           _eventLogText.text = text + " [" + pointOfInterest + "]";
        }

        public void Initialize(EventLogsManager eventLogsManager)
        {
            _eventLogsManager = eventLogsManager;
        }

        public void DestroyEventLog()
        {
            _animator.SetBool(_eventLogAnimationName, false);
            StartCoroutine(IEPlayAnimation());
        }
        

        private IEnumerator IEPlayAnimation()
        {
            yield return new WaitForSeconds(1f);
            Destroy(gameObject, .5f);
        }
    }

}
