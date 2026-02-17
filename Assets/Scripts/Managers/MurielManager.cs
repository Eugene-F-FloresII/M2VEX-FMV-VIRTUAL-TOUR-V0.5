using System.Collections.Generic;
using Collection;
using Controllers;
using Obvious.Soap;
using UnityEngine;

namespace Managers
{
    public class MurielManager : MonoBehaviour
    {
        [SerializeField] private MurielController _murielPrefab;
        [Header("**Read Only**")]
        [SerializeField] private List<string> _dialogues;
        [SerializeField] private IntVariable _dialogueIndex;
        private MurielController _murielController;
        
        [Header("**Dont Touch**")]
        public bool _nextDialogue;

        private void Awake()
        {
            ServiceLocator.Register(this);
        }

        private void Start()
        {
            SpawnDialogue("Hello",
                "Name",
                "Me");
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
            else if (_nextDialogue)
            {
                _murielController.MurielMessage(_dialogues[_dialogues.Count - value]);
                _nextDialogue = false;
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
            _nextDialogue = false;

            return muriel;
        }
    }

}
