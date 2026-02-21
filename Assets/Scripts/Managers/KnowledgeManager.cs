using System;
using System.Collections;
using System.Collections.Generic;
using Collection;
using Obvious.Soap;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using NaughtyAttributes;

namespace Managers
{
    public enum PlayerMood
    {
        Dissatisfied,
        Neutral,
        Excited
    }
    public class KnowledgeManager : MonoBehaviour
    {
        
        [Header("SOAP References")]
        [SerializeField] private FloatVariable _maxExperiencePerLevel;
        [SerializeField] private FloatVariable _currentExperience;
        [SerializeField] private FloatVariable _knowledgeGained;
        [SerializeField] private FloatVariable _maxKnowledgeGained;
        [SerializeField] private IntVariable _currentLevel;

        [Header("References")]
        [SerializeField] private TextMeshProUGUI _experienceText;
        [SerializeField] private TextMeshProUGUI _playerMoodText;
        [SerializeField] private Image _playerMoodImage;
        [SerializeField] private List<Sprite> _playerMoodSprites;

        private readonly string _gainedExperience = "Experience Gained: ";
        private EventLogsManager _eventLogsManager;
        private PlayerMood _playerMood;
        private float _xpValueGained;
        private float _highestExperience;
        private float _gainedExperienceValue;
        private float _subtractedValue;
        
        public static Action OnLevelChanged { get; set; }

        private void Awake()
        {
            ServiceLocator.Register(this);
            _eventLogsManager = ServiceLocator.Get<EventLogsManager>();
        }
        
        private void Start()
        {
            UpdateExperienceText();
            UpdatePlayerMood();
        }
        
        private void OnEnable()
        {
            _currentExperience.OnValueChanged += ExperienceValueChanged;
            _knowledgeGained.OnValueChanged += UpdatedKnowledgeValue;
        }

        private void OnDisable()
        {
            _currentExperience.OnValueChanged -= ExperienceValueChanged;
            _knowledgeGained.OnValueChanged -= UpdatedKnowledgeValue;
        }

        private void OnDestroy()
        {
            ServiceLocator.Unregister<KnowledgeManager>();
        }

        private void Update()
        {
            
            if (_currentExperience.Value <= 0 && _xpValueGained <= 0)
            {
                _currentExperience.Value = 0;
                _xpValueGained = 0;
                
            } 
        }

        public void GainExperience(float value)
        {
            StopAllCoroutines();
            
            _knowledgeGained.Value++;

            _xpValueGained += value;
            
            StartCoroutine(LerpExperience(_xpValueGained));
            
            _eventLogsManager.InstantiateEventLogs(_gainedExperience, "+" + value);
        }
        
        public void ReduceExperience(float value)
        {
            StopAllCoroutines();
            
            _knowledgeGained.Value--;

            if (_xpValueGained >= 0)
            {
                _xpValueGained -= value;
                StartCoroutine(LerpExperience(_xpValueGained));
            }
            
            if (_knowledgeGained.Value < 0)
            {
                _knowledgeGained.Value = 0;
            }
            
            _eventLogsManager.InstantiateEventLogs(_gainedExperience, "-" + value);
        }

        [Button("Test(+) Experience")]
        private void TestGainExperience()
        {
            GainExperience(25f);
        }
        
        [Button("Test(-) Experience")]
        private void TestReduceExperience()
        {
            ReduceExperience(25f);
        }

        private void ExperienceValueChanged(float value)
        {
            UpdateExperienceText();
            
            _experienceText.text = _currentExperience.Value + "/" + _maxExperiencePerLevel.Value;
            
            if (value < 0)
            {
                _currentExperience.Value = 0;
            }

            if (value >= _maxExperiencePerLevel.Value)
            {
                StopAllCoroutines();
                
                OnLevelChanged?.Invoke();
                
                _currentExperience.Value = 0;
                
                _xpValueGained = 0;
                
                _currentLevel.Value++;
                
                StartCoroutine(LerpExperience(_xpValueGained));
                
            }

        }

        private void UpdatedKnowledgeValue(float value)
        {
            //33.3% of the max Knowledge gained
            float firstCalcPercentage = 0.333f * _maxKnowledgeGained.Value;
            //66.6% of the max Knowledge gained
            float secondCalcPercentage = 0.666f * _maxKnowledgeGained.Value;

            if (_knowledgeGained.Value < firstCalcPercentage)
            {
                _playerMood = PlayerMood.Dissatisfied;
                _playerMoodText.text = "Not Interested";

            } else if (_knowledgeGained.Value < secondCalcPercentage && _knowledgeGained.Value > firstCalcPercentage)
            {
                _playerMood = PlayerMood.Neutral;
                _playerMoodText.text = "Neutral";
            }
            else if (_knowledgeGained.Value >= secondCalcPercentage)
            {
                _playerMood = PlayerMood.Excited;
                _playerMoodText.text = "Satisfied";
            }
            
            UpdatePlayerMood();
        }

        private void UpdateExperienceText()
        {
            _experienceText.text = _currentExperience.Value + "/" + _maxExperiencePerLevel.Value;
        }

        // Will be implemented in UpdateChangedExperienceValue
        private void UpdatePlayerMood()
        {
            switch (_playerMood)
            {
                case PlayerMood.Dissatisfied:
                    
                    _playerMoodImage.sprite = _playerMoodSprites[0];
                    
                    break;
                case PlayerMood.Neutral:
                    
                    _playerMoodImage.sprite = _playerMoodSprites[1];
                    
                    break;
                case PlayerMood.Excited:
                    
                    _playerMoodImage.sprite = _playerMoodSprites[2];
                    
                    break;
            }
        }
        
        private IEnumerator LerpExperience(float target)
        {
            while (!Mathf.Approximately(_currentExperience.Value, target))
            {
                _currentExperience.Value = Mathf.RoundToInt(Mathf.MoveTowards(_currentExperience.Value, target, Time.deltaTime * 200f));
                yield return null;
            }
            _currentExperience.Value = target;
            
        }
        
    }

}
