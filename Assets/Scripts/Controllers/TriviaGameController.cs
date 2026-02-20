using Managers;
using TMPro;
using UnityEngine;

namespace Controllers
{
    public class TriviaGameController : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _textMeshProUGUI;

        private bool _isCorrectAnswer;
        private TriviaGameManager _triviaGameManager;
        public void ChangeText(string text)
        {
            _textMeshProUGUI.text = text;
        }

        public void Initialize(TriviaGameManager triviaGameManager, bool isCorrectAnswer)
        {
            _triviaGameManager = triviaGameManager;
            _isCorrectAnswer = isCorrectAnswer;
        }

        public void OnButtonClick()
        {
            if (_isCorrectAnswer)
            {
                _triviaGameManager.CorrectAnswer(_isCorrectAnswer); 
            }
            else
            {
                _triviaGameManager.WrongAnswer(_isCorrectAnswer);
            }
        }
        
    }
}

