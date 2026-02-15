using System.Collections;
using System.Collections.Generic;
using Collection;
using Managers;
using Gameplay;
using TMPro;
using UnityEngine;
using PrimeTween;
using Random = UnityEngine.Random;

namespace Controllers
{
    public class CardController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Card _cardPrefab;
        [SerializeField] private Transform _gridTransfrom;
        [SerializeField] private TextMeshProUGUI _timerText;
        
        [Header("Settings")]
        [SerializeField] private List<Sprite> _sprites;
        [SerializeField] private float _experienceGained = 25f;
        public float _timeLimit = 60f;
    
        private EventLogsManager _eventLogsManager;
        private KnowledgeManager _knowledgeManager;
        private MinigameManager _minigameManager;
        private List<Sprite> _spritePairs;
        private Sprite _temp;
        private Card _instantiatedCard;
        private Card _firstSelected;
        private Card _secondSelected;
        private float _remainingTime;
        private bool _gameOver;
        private bool _isSelected;
        private int _matchCounts;


        private void Awake()
        {
            _eventLogsManager = ServiceLocator.Get<EventLogsManager>();
            _minigameManager = ServiceLocator.Get<MinigameManager>();
            _knowledgeManager = ServiceLocator.Get<KnowledgeManager>();
        }
    
        private void OnEnable()
        {
            PrepareSprites();
                
            CreateCards();
        
            _remainingTime = _timeLimit;
                
            UpdateTimerUI(); // set initial text
                
            StartCoroutine(IECountdownTimer());
        }
    
        /// <summary>
        /// Card Script calls this and gets the first card and second card, then after second card is selected then it starts coroutine CheckMatching
        /// </summary>
        public void SetSelected(Card card)
            {
                if (_isSelected == false)
                {
                    card.Show();
        
                    if (_firstSelected == null)
                    {
                        _firstSelected = card;
                        return;
                    }
                    if (_secondSelected == null)
                    {
                        _secondSelected = card;
                        StartCoroutine(CheckMatching(_firstSelected, _secondSelected));
                        _firstSelected = null;
                        _secondSelected = null;
                    }
                }
            }
    
    
        public void IsCardSelected(bool isSelected)
        {
            _isSelected = isSelected;
        }
        
    
        private void PrepareSprites()
        {
            _spritePairs = new List<Sprite>();   
            
            for (int i = 0; i < _sprites.Count; i++)
            {
                _spritePairs.Add(_sprites[i]);
                _spritePairs.Add(_sprites[i]);
            }
            
            ShuffleSprites(_spritePairs);
        }
    
    
        private void CreateCards()
        {
            for (int i = 0; i < _spritePairs.Count; i++)
            {
                _instantiatedCard = Instantiate(_cardPrefab, _gridTransfrom);
                _instantiatedCard.SetIconSprite(_spritePairs[i]);
                _instantiatedCard.Initialize(this);
            }
        }
    
        private void ShuffleSprites(List<Sprite> spriteList)
            {
                // Starts the last index of the list
                for (int i = spriteList.Count - 1; i > 0; i--)
                {
                    //randomize 
                    int randomIndex = Random.Range(0, i + 1);
        
                    _temp = spriteList[i];
                    
                    spriteList[i] = spriteList[randomIndex];
                    
                    spriteList[randomIndex] = _temp;
                }
            }
        private void UpdateTimerUI()
        {
            int minutes = Mathf.FloorToInt(_remainingTime / 60);
            int seconds = Mathf.FloorToInt(_remainingTime % 60);
        
            _timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
            
        }
        /// <summary>
        /// Try and Match both cards
        /// </summary>
        /// <param name="cardA"></param>
        /// <param name="cardB"></param>
        /// <returns></returns>
        IEnumerator CheckMatching(Card cardA, Card cardB)
        {
            yield return new WaitForSeconds(0.5f);
    
            if (cardA._iconSprite == cardB._iconSprite)
            {
                _matchCounts++;
                if (_matchCounts >= _spritePairs.Count / 2)
                {
                    //level Completed
                    Sequence.Create()
                        .Chain(Tween.Scale(_gridTransfrom, Vector3.one * 1.2f, 0.2f, ease: Ease.OutBack))
                        .Chain(Tween.Scale(_gridTransfrom, Vector3.one, 0.1f));
                    StartCoroutine(IEFinishGame(true));
                }
            }
    
            else
            {
                cardA.Hide();
                cardB.Hide();
            }
        }
        
        
        
        private IEnumerator IEFinishGame(bool won)
        {
            yield return new WaitForSeconds(2f);
    
            Destroy(gameObject);
            if (won)
            {
                //Win
                _eventLogsManager.InstantiateEventLogs("Memory Game: ", "Win");
                _knowledgeManager.GainExperience(_experienceGained);
                _minigameManager.GameFinished();
                
            }
            else
            {
                _eventLogsManager.InstantiateEventLogs("Memory Game: ", "Lost");
               _knowledgeManager.ReduceExperience(_experienceGained);
                _minigameManager.GameFinished();
                //Lose
            }
        }
    
        
        private IEnumerator IECountdownTimer()
        {
            while (_remainingTime > 0 && !_gameOver)
            {
                yield return new WaitForSeconds(1f);
                _remainingTime--;
    
                UpdateTimerUI();
            }
    
            if (!_gameOver) // ran out of time
            {
                _gameOver = true;
                
                StartCoroutine(IEFinishGame(false));
            }
        }
        
    }

}
