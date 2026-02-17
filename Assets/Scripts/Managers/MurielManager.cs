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

        private void OnDialogueIndexUpdate(int value)
        {
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

        public void SpawnDialogue(string dialogue)
        {
            _dialogues.Add(dialogue);
            
            _murielController = CreateMurielController(_murielPrefab);
        }
        
        public void SpawnDialogue(string first, string second)
        {
            _dialogues.Add(first);
            _dialogues.Add(second);
            
            _murielController = CreateMurielController(_murielPrefab);
        }
        
        public void SpawnDialogue(string first, string second, string third)
        {
            _dialogues.Add(first);
            _dialogues.Add(second);
            _dialogues.Add(third);

            _murielController = CreateMurielController(_murielPrefab);
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
