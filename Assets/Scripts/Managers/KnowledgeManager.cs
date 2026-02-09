using System.Collections.Generic;
using Collection;
using Obvious.Soap;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
        [SerializeField] private FloatVariable _experienceGained;
        [SerializeField] private IntVariable _currentLevel;

        [Header("References")]
        [SerializeField] private TextMeshProUGUI _experienceText;
        [SerializeField] private TextMeshProUGUI _playerMoodText;
        [SerializeField] private Image _playerMoodImage;
        [SerializeField] private List<Sprite> _playerMoodSprites;

        private PlayerMood _playerMood;
        private float _highestExperience;

        private void Awake()
        {
            ServiceLocator.Register(this);
        }

        private void OnEnable()
        {
            _currentExperience.OnValueChanged += ExperienceValueChanged;
            _experienceGained.OnValueChanged += ExperienceGainedValueChanged;
        }


        private void OnDisable()
        {
            _currentExperience.OnValueChanged -= ExperienceValueChanged;
            _experienceGained.OnValueChanged -= ExperienceGainedValueChanged;
        }

        private void OnDestroy()
        {
            ServiceLocator.Unregister<KnowledgeManager>();
        }


        private void Update()
        { 
            UpdateExperienceText();
        }

        public void GainExperience(float value)
        {
            _experienceGained.Value = value;
            _currentExperience.Value += _experienceGained.Value;
            
        }

        private void ExperienceValueChanged(float value)
        {
            
            _experienceText.text = _currentExperience.Value + "/" + _maxExperiencePerLevel.Value;
            
            if (value < 0)
            {
                _currentExperience.Value = 0;
            }

            if (value >= _maxExperiencePerLevel.Value)
            {
                _currentExperience.Value = 0;
                _currentLevel.Value++;
            }

        }

        private void ExperienceGainedValueChanged(float value)
        {
            UpdatePlayerMood();
        }

        private void UpdateExperienceText()
        {
            _experienceText.text = _currentExperience.Value + "/" + _maxExperiencePerLevel.Value;
        }

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
        
    }

}
