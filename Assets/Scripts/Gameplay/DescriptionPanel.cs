using System;
using System.Collections;
using Collection;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

namespace Gameplay
{
    public class DescriptionPanel : MonoBehaviour,IAreaController
    {
        [Header("Description")]
        [SerializeField] private string _areaDescription;
        
        [Header("Reference")]
        [SerializeField] private TextMeshProUGUI _description;
        
        [Header("Settings")]
        [SerializeField] private float _typingSpeed = 0.5f;

        public void EnterArea()
        {
            StartCoroutine(IETypeDescription(_areaDescription));
        }

        public void ExitArea()
        {
            StopAllCoroutines();
            _description.text = "";
        }

        private IEnumerator IETypeDescription(string description)
        {
            _description.text = "";

            foreach (char t in description)
            {
                _description.text += t;
                yield return new WaitForSeconds(_typingSpeed);
            }
        }
        
    }

}
