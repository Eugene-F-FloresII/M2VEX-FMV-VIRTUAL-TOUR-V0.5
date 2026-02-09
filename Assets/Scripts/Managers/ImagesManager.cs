using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Managers
{
    public class ImagesManager : MonoBehaviour
    {
        [Header("Image Reference")]
        [SerializeField] private Image _backgroundImage;
        
        [Header("Testing")]
        [SerializeField] private string _imageID;
        
        [Header("Image Settings")]
        [SerializeField] private List<Sprite> _images;
        [SerializeField] private List<string>  _imageNamesID;
        
        private Dictionary<string, Sprite> _imageDict;

        public static Action<string> OnPickedImage {get; set;}

        private void Awake()
        {
            InitializeImageDictionary();
        }

        private void OnEnable()
        {
            OnPickedImage += SetBackgroundImage;
        }

        private void OnDisable()
        {
            OnPickedImage -= SetBackgroundImage;
        }

        private void InitializeImageDictionary()
        {
            
            _imageDict = new Dictionary<string, Sprite>();
            
            // Check if both Lists have the same amount
            if (_images.Count != _imageNamesID.Count)
            {
                Debug.LogError("Error! Both Lists must have the same amount");
                return;
            }

            //Attaching Image and Image name to dictionary
            for (int i = 0; i < _images.Count; i++)
            {
                if (!string.IsNullOrEmpty(_imageNamesID[i]) && _images[i] != null)
                {
                    _imageDict.Add(_imageNamesID[i], _images[i]);
                }
            }
        }

        public Sprite GetImage(string imageName)
        {
            // Gets the image from list
            if (_imageDict.TryGetValue(imageName, out Sprite image))
            {
                return image;
            }
            
            //If no Image name found on the list 
            Debug.LogWarning("Image: " + imageName + " not found in dictionary!");
            return null;
        }
        
        public void SetBackgroundImage(string imageName)
        {
            
            //Gets the image from GetImage method
            Sprite image = GetImage(imageName);
            
            
            //Attaching image to background image sprite
            if (image != null)
            {
                _backgroundImage.sprite = image;
            }
        }

        [ContextMenu("TestSetBackgroundImage")]
        private void SetBackgroundImages()
        {
            SetBackgroundImage(_imageID);
        }
        
        
    }
}