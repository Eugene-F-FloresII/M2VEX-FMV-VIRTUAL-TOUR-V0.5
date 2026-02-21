using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Managers
{
    public enum FeatureType
    {
        Objectives,
        Mission,
        Journal,
        Minigame
    }
    
    public class FeaturesManager : MonoBehaviour
    {
        [SerializeField] private List<Button> _featureButtons;
        [SerializeField] private Transform _transform;

        private void OnEnable()
        {
            DiscoveryLevelManager.OnPlayerLevelChanged += AddFeatureButton;
        }
        private void OnDisable()
        {
            DiscoveryLevelManager.OnPlayerLevelChanged -= AddFeatureButton;
        }
        
        private void AddFeatureButton(int value)
        {
            if (value < _featureButtons.Count)
            {
                Instantiate(_featureButtons[value], _transform);
            }
        }
    }
}
