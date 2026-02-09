using Collection;
using Gameplay;
using UnityEngine;

namespace Managers
{
    public class TransitionManager : MonoBehaviour
    {
        [SerializeField] private SkipVideo _skipVideoPrefab;
        [SerializeField] private Canvas _canvas;

        private SkipVideo _skipVideo;
        private EventLogsManager _eventLogsManager;

        private void Awake()
        {
            ServiceLocator.Register(this);
        }

        private void Start()
        {
            _eventLogsManager = ServiceLocator.Get<EventLogsManager>();
        }

        private void OnDestroy()
        {
            ServiceLocator.Unregister<TransitionManager>();
        }

        public void InstantiateSkipVideo()
        {
            _skipVideo = Instantiate(_skipVideoPrefab, _canvas.transform);
        }

        public void DestroySkipVideo()
        {
            _eventLogsManager.InstantiateEventLogs("Transition: ", "Skipped");
            Destroy(_skipVideo.gameObject);
        }
    }
}

