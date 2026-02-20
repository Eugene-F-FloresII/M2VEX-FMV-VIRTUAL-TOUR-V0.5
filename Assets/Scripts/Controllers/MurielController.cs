using System;
using System.Collections;
using Collection;
using Managers;
using Obvious.Soap;
using TMPro;
using UnityEngine;

namespace Controllers
{
    public class MurielController : MonoBehaviour
    {
        [Header("References & Setting")]
        [SerializeField] private TextMeshProUGUI _murielText;
        [SerializeField] private IntVariable _dialogueIndex;
        [SerializeField] private float _typingSpeed = 1f;

        private TriviaGameManager _triviaGameManager;
        private MurielManager _murielManager;
        private float _time;

        private void Start()
        { 
            _triviaGameManager = ServiceLocator.Get<TriviaGameManager>();
        }

        public void MurielMessage(string message)
        {
            StartCoroutine(TypeText(message));
        }

        public void InitializeMuriel(MurielManager manager)
        {
            _murielManager = manager;
        }

        public void OnButtonClicked()
        {
            _murielManager.NextDialogue = true;
            _dialogueIndex.Value--;

            if (_triviaGameManager != null && _triviaGameManager._isAsking)
            {
                _triviaGameManager.InitializeMurielController(this);
                
                StartCoroutine(IEShowAnswers());
            }
            
        }

        private IEnumerator IEShowAnswers()
        {
            yield return new WaitForSeconds(0.1f);
            _triviaGameManager.ShowAnswers();
            
        }
        private IEnumerator TypeText(string text)
        {
            _murielText.text = "";

            foreach (char t in  text)
            {
                _murielText.text += t;
                yield return new WaitForSeconds(_typingSpeed);
            }
        }
    }

}
