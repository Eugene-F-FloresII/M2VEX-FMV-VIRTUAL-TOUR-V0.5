using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Managers
{
    public class MinigameManager : MonoBehaviour
    {
    
        [Header("References")]
        [SerializeField] private List<GameObject> _gamesPrefabs;
        
        private int _randomIndex;
    
    
        private void Start()
        {
            PickRandomGame();
        }
    
        private void PickRandomGame()
        {
            _randomIndex = Random.Range(0, _gamesPrefabs.Count);
            
           var obj =  Instantiate(_gamesPrefabs[_randomIndex], gameObject.transform);
           
        }
    
    }

}
