using System;
using Data;
using Obvious.Soap;
using TMPro;
using UnityEngine;


namespace Managers
{
    public class DiscoveryLevelManager : MonoBehaviour
    {
        [Header("SOAP References")]
        [SerializeField] private IntVariable _currentLevel;
        [SerializeField] private FloatVariable _maxExperiencePerLevel;
        [SerializeField] private PlayerNameScriptableObject _playerNameScriptableObject;

        [Header("References")] 
        [SerializeField] private TextMeshProUGUI _playerName;
        [SerializeField] private TextMeshProUGUI _playerLevel;

        private int _featureValue;
        private float _experiencePerLevel;
        private readonly float _multiplier = 2;
        
        public static Action OnLevelChanged { get; set; }
        public static Action<int> OnPlayerLevelChanged { get; set;}
        
        
        private void Start()
        {
            UpdatePlayerLevel();
        }

        private void OnEnable()
        {
            _currentLevel.OnValueChanged += LevelValueChanged;
            OnLevelChanged += ChangeMaxExperience;
        }

        private void OnDisable()
        {
            _currentLevel.OnValueChanged -= LevelValueChanged;
            OnLevelChanged -= ChangeMaxExperience;
        }

        private void LevelValueChanged(int level)
        {
            UpdatePlayerLevel();
            Debug.Log("leveled up: " + level);
        }

        private void ChangeMaxExperience()
        {
            _experiencePerLevel = _maxExperiencePerLevel.Value / _multiplier;
            _maxExperiencePerLevel.Value = Mathf.RoundToInt(_maxExperiencePerLevel + _experiencePerLevel);
        }

        private void UpdatePlayerLevel()
        {
            _playerLevel.text = _currentLevel.Value.ToString();
            _playerLevel.color = Color.darkViolet;
            
            OnPlayerLevelChanged?.Invoke(_featureValue);
            
            _featureValue++;
        }
    }

}
