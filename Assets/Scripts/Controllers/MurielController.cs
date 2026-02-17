using System.Collections;
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
        
        private MurielManager _murielManager;
        private float _time;


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
