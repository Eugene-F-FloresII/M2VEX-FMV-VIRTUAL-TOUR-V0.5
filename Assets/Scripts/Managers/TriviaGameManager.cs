using System.Collections.Generic;
using Collection;
using Controllers;
using Data;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Managers
{
    [System.Serializable]
    public struct TriviaEntry
    {
        public string Question;
        public AnswerScriptableObject Answers;
    }

    public class TriviaGameManager : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private TriviaGameController _buttonPrefab;
        [SerializeField] private Transform _buttonContainer;

        [Header("Trivia Data")] 
        [SerializeField] private float _experienceGained = 25f;
        [SerializeField] private List<TriviaEntry> _triviaEntries;
        
        [Header("**Read-Only!**")]
        public bool _isAsking;
        [SerializeField] private List<TriviaGameController> _triviaGameControllers;

        private Button _button;
        private MurielController _murielController;
        private MurielManager _murielManager;
        private TriviaEntry _currentEntry;
        private EventLogsManager _eventLogsManager;
        private KnowledgeManager _knowledgeManager;
        private MinigameManager _minigameManager;

        private void Awake()
        {
            ServiceLocator.Register(this);
             _murielManager = ServiceLocator.Get<MurielManager>();
            _eventLogsManager = ServiceLocator.Get<EventLogsManager>();
            _knowledgeManager = ServiceLocator.Get<KnowledgeManager>();
            _minigameManager = ServiceLocator.Get<MinigameManager>();
        }

        private void Start()
        {
            
            if (_triviaEntries == null || _triviaEntries.Count == 0)
            {
                Debug.LogWarning("No trivia entries assigned.");
                return;
            }

            _currentEntry = _triviaEntries[Random.Range(0, _triviaEntries.Count)];
            
            AskQuestion();
        }

        private void OnDestroy()
        {
            ServiceLocator.Unregister<TriviaGameManager>();
        }

        public void CorrectAnswer(bool right)
        {
            DestroyButtons();
            OnFinishedGame("Trivia Game: ",
                "Win",
                _experienceGained,
                right);
        }

        public void WrongAnswer(bool right)
        {
            DestroyButtons();
            OnFinishedGame("Trivia Game: ",
                "Lose",
                _experienceGained,
                right);
        }
        
        public void ShowAnswers()
        {
            foreach (var answer in _currentEntry.Answers.AnswerData)
            {
                CreateButton(answer.Answers, answer.CorrectAnswer);
            }

            _button = _murielController.gameObject.GetComponentInChildren<Button>();
            
            Destroy(_button.gameObject);
        }

        public void InitializeMurielController(MurielController murielController)
        {
            _murielController = murielController;
        }
        
        private void AskQuestion()
        {
            _murielManager.SpawnDialogue(_currentEntry.Question);
            
            _isAsking = true;
        }
        
        private void OnFinishedGame(string gameName, string status, float experience, bool correct)
        {
            _eventLogsManager.InstantiateEventLogs(gameName, status);
            
            if (correct)
            {
                _knowledgeManager.GainExperience(experience);
            }
            else
            {
                _knowledgeManager.ReduceExperience(experience);
            }

            if (_murielController != null)
            {
                Destroy(_murielController.gameObject);
            }

            _isAsking = false;
            
            _minigameManager.GameFinished();
        }

        private TriviaGameController CreateButton(string answerText, bool correctAnswer)
        {
            var button = Instantiate(_buttonPrefab, _buttonContainer);
            button.Initialize(this, correctAnswer);
            button.ChangeText(answerText);
            
            _triviaGameControllers.Add(button);
            
            return button;
        }

        private void DestroyButtons()
        {
            Destroy(gameObject);
        }
    }

}
