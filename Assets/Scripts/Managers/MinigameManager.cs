using System.Collections.Generic;
using Collection;
using Controllers;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Managers
{
    public class MinigameManager : MonoBehaviour
    {
    
        [Header("References")]
        [SerializeField] private List<GameObject> _gamesPrefabs;
        
        private EventLogsManager _eventLogsManager;
        private AreaController _areaController;
        private GameObject _minigame;
        private string _locationName;
        private int _randomIndex;

        private void Awake()
        {
            ServiceLocator.Register(this);
            
            _eventLogsManager = ServiceLocator.Get<EventLogsManager>();
        }

        private void OnDestroy()
        {
            ServiceLocator.Unregister<MinigameManager>();
        }
        
        public void PickRandomGame()
        {
            _randomIndex = Random.Range(0, _gamesPrefabs.Count);
            
           _minigame =  Instantiate(_gamesPrefabs[_randomIndex], gameObject.transform);
           
           Debug.Log(_minigame.name);
        }

        public void GameFinished()
        {
            _locationName = _areaController._locationName;
            
            _eventLogsManager.InstantiateEventLogs(_locationName + ": ", "Unlocked");
            
            _areaController._lockArea = false;
            _areaController.UpdateAreaPadlock();
            
            Destroy(_minigame);
            
        }

        public void Initialize(AreaController areaController)
        {
            _areaController = areaController;
        }
    
    }

}
