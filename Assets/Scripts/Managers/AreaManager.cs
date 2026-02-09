using System.Collections.Generic;
using Collection;
using Controllers;
using Obvious.Soap;
using UnityEngine;

namespace Managers
{
    public class AreaManager : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] public List<AreaController> _areas;
        [SerializeField] private IntVariable _areaActive;
        
        
        private void Awake()
        {
            ServiceLocator.Register(this);
        }

        private void Start()
        {
            
            SetActiveArea();
        }

        private void OnDestroy()
        {
            ServiceLocator.Unregister<AreaManager>();
        }

        public void SetActiveArea()
        {
            foreach (var area in _areas)
            {
               area.gameObject.SetActive(false);
            }
            
            _areas[_areaActive.Value].gameObject.SetActive(true);
            
            
        }
    }
}