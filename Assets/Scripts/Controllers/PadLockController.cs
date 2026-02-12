using Collection;
using UnityEngine;
using Managers;



namespace Controllers
{
    public class PadLockController : MonoBehaviour
    {
        private MinigameManager _minigameManager;
        private AreaController _areaController;

        private void Start()
        {
            _minigameManager = ServiceLocator.Get<MinigameManager>();
        }

        public void OnButtonClick()
        {
            _minigameManager.PickRandomGame();
            _minigameManager.Initialize(_areaController);
            
        }

        public void Initialize(AreaController areaController)
        {
            _areaController = areaController;
        }
    }

}
