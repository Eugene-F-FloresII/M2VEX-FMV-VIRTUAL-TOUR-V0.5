using System.Collections.Generic;
using Collection;
using Controllers;
using UnityEngine;

namespace Managers
{
    public class EventLogsManager : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private EventLogsController _eventLogPrefab;
        [SerializeField] private Transform _eventLogHolder;
        
        [Header("Don't add anything here")]
        [SerializeField] private List<EventLogsController> _eventLogControllers;

        private EventLogsController _eventLogsController;
        private EventLogsController _eventLogs;
        private readonly int _eventLogLimit = 3;
        
        private void Awake()
        {
            ServiceLocator.Register(this);
        }

        private void OnDestroy()
        {
            ServiceLocator.Unregister<EventLogsManager>();
        }

        public void InstantiateEventLogs(string eventLog, string pointOfInterest)
        {
            _eventLogsController = Instantiate(_eventLogPrefab, _eventLogHolder.transform);
            _eventLogsController.Initialize(this);
            _eventLogsController.EventLogText(eventLog, pointOfInterest);
            
            _eventLogControllers.Add(_eventLogsController);
            
            UpdateEventLogList();
            
        }

        private void UpdateEventLogList()
        {
            if (_eventLogControllers.Count > _eventLogLimit)
            {
                // Store reference before removing from list
                _eventLogs = _eventLogControllers[0];
        
                // Remove from list
                _eventLogControllers.RemoveAt(0);
        
                // Destroy the GameObject
                _eventLogs.DestroyEventLog();
              
            }
        }
    }

}
