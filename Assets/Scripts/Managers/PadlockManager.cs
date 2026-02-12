using Collection;
using UnityEngine;


namespace Managers
{
    public class PadlockManager : MonoBehaviour
    {
        [SerializeField] private Canvas _canvas;
        [SerializeField] private GameObject _padlockPrefab;

        private GameObject _padlock;

        private void Awake()
        {
            ServiceLocator.Register(this);
        }

        private void OnDestroy()
        {
            ServiceLocator.Unregister<PadlockManager>();
        }
        
    }

}
