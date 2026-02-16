using Collection;
using System.Collections;
using System.Collections.Generic;
using Managers;
using Obvious.Soap;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Controllers
{
    public class RhythmGameController : MonoBehaviour
    {
        [Header("Audio Settings")]
        [SerializeField] private AudioSource _musicSource;
        [SerializeField] private float _bpm = 120f;          // beats per minute of your song
    
        [Header("Note Settings")]
        [SerializeField] private Notes _notePrefab;     // assign a cube or prefab in Inspector
        [SerializeField] Transform _spawnPoint;
        [SerializeField] private GameObject _results;
        [SerializeField] private GameObject _currentStatus;
        [SerializeField] private List<AudioClip> _audioClip;
        [SerializeField] private float _experienceGained = 25f;
        [SerializeField] private int _maxNotes = 30;
    
        [Header("Scoring References")] 
        [SerializeField] private TextMeshProUGUI _scoreText;
        [SerializeField] private IntVariable _currentScore;
        [SerializeField] private IntVariable _missedNotes;
        
        private EventLogsManager _eventLogsManager;
        private MinigameManager _minigameManager;
        private KnowledgeManager _knowledgeManager;
        private AudioManager _audioManager;
        private Notes _notes;
        private float _beatInterval;       // seconds per beat
        private float _nextBeatTime;       // when the next beat happens
        private float _songStartTime;
        private int _spawnedNotes;
        private int _randomIndex;
        private double _totalNotes;
        private bool _resultsShown = false;

        private void Awake()
        {
            _eventLogsManager = ServiceLocator.Get<EventLogsManager>();
            _minigameManager = ServiceLocator.Get<MinigameManager>();
            _knowledgeManager = ServiceLocator.Get<KnowledgeManager>();
            _audioManager = ServiceLocator.Get<AudioManager>();
        }

        void Start()
        {
            
            _randomIndex = Random.Range(0, _audioClip.Count);
            _currentStatus.SetActive(false);
            _results.SetActive(false);
         
            _beatInterval = 60f / _bpm;
            _songStartTime = Time.time;
            
            _musicSource = _audioManager.CreateAudioSource(_audioClip[_randomIndex]);
            
            if (_musicSource == null)
            { 
                Debug.LogError("No AudioSource assigned!"); 
                return;
            }
            _musicSource.Play(); // start music
            _nextBeatTime = _songStartTime + _beatInterval; // schedule first beat
            
        }
    
        private void OnEnable()
        {
            _currentScore.OnValueChanged += RhythmGameScore;
        }
    
        private void OnDisable()
        {
            _currentScore.OnValueChanged -= RhythmGameScore;
        }
    
        private void Update()
        {
            float songTime = Time.time - _songStartTime;
    
            if (songTime >= _nextBeatTime - _songStartTime)
            {
                if (_spawnedNotes < _maxNotes)
                {
                    SpawnNote();
                    
                    _nextBeatTime += _beatInterval;
                }
                else if (!_resultsShown)
                {
                    _musicSource.Stop();
                    Results();
                    _resultsShown = true;
                }
            }
        }
        
        public void PlayerScore()
        {
            _currentScore.Value++;
        }
    
        public void MissedScore()
        {
            _missedNotes.Value++;
        }
    
        private void RhythmGameScore(int value)
        {
            _scoreText.text = "Score: " + value;
        }
        
        private void SpawnNote()
            {
                if (_notePrefab != null && _spawnPoint != null)
                {
                    _notes = Instantiate(_notePrefab, _spawnPoint);
                    _notes.Initialize(this);
                    _spawnedNotes++;
                }
            }
        
        private void Results()
        {
            _results.SetActive(true);
    
            _totalNotes = _spawnedNotes * 0.5;
            double overall = _currentScore.Value - _missedNotes.Value;
            
            StartCoroutine(IESetStatus(overall));
    
        }
    
        private void OnFinishedGame(string gameName, string status, float experience)
        {
            _eventLogsManager.InstantiateEventLogs(gameName, status);
            _knowledgeManager.GainExperience(experience);
            _minigameManager.GameFinished();
        }
    
        private IEnumerator IESetStatus(double stats)
        {
            _currentStatus.SetActive(true);
            yield return new WaitForSeconds(1f);
    
            if (stats > _totalNotes)
            {
                StartCoroutine(EndGame());
                OnFinishedGame("Rhythm Game" + ": ", 
                    "Excellent!",
                    _experienceGained);
                
            } else if (stats < _totalNotes)
            {
                StartCoroutine(EndGame());
                OnFinishedGame("Rhythm Game" + ": ",
                    "Decent!", 
                    _experienceGained * 0.5f);
            } else 
            {
                StartCoroutine(EndGame());
                OnFinishedGame("Rhythm Game" + ": ",
                    "Meh.", 
                    _experienceGained * 0.25f);
           
            }
        }
    
        private IEnumerator EndGame()
        {
            yield return new WaitForSeconds(2f);
            _resultsShown = false;
            Destroy(gameObject);
           // GlobalManager.instance.Padlock(padlockstatus.name, false);
        }
    }

}
