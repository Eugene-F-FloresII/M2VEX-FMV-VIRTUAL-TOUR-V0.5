using System.Collections.Generic;
using Collection;
using Controllers;
using Obvious.Soap;
using UnityEngine;

namespace Managers
{
    public class MurielManager : MonoBehaviour
    {
        
        [Header("**Dont Touch**")]
        public bool NextDialogue;
        
        [Header("References")]
        [SerializeField] private MurielController _murielPrefab;
        [SerializeField] private IntVariable _dialogueIndex;
        
        [Header("**Read Only**")]
        [SerializeField] private List<string> _dialogues;
        private MurielController _murielController;
        
        private void Awake()
        {
            ServiceLocator.Register(this);
        }

        private void OnEnable()
        {
            _dialogueIndex.OnValueChanged += OnDialogueIndexUpdate;
        }

        private void OnDisable()
        {
            _dialogueIndex.OnValueChanged -= OnDialogueIndexUpdate;
        }

        private void OnDestroy()
        {
            ServiceLocator.Unregister<MurielManager>();
        }
        public void SpawnDialogue(params string[] dialogue)
            {
                _dialogues.AddRange(dialogue);
                _murielController = CreateMurielController(_murielPrefab);
            }
        
        private void OnDialogueIndexUpdate(int value)
        {
            //if confused, go to MurielController.cs OnButtonClicked() method
            
            if (value == 0)
            {
                _dialogues.Clear();
                Destroy(_murielController.gameObject);
            }
            else if (NextDialogue)
            {
                _murielController.MurielMessage(_dialogues[_dialogues.Count - value]);
                NextDialogue = false;
            }
        }
        
        

        private MurielController CreateMurielController(MurielController murielControllerPrefab)
        {
            var muriel = Instantiate(murielControllerPrefab, transform);
            muriel.InitializeMuriel(this);

            _dialogueIndex.Value = _dialogues.Count + 1;
            NextDialogue = false;

            return muriel;
        }
    }

}
