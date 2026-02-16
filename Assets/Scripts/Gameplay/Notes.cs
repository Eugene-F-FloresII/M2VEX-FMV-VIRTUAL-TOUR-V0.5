using System.Collections;
using UnityEngine;
using Controllers;

public class Notes : MonoBehaviour
{
    private RectTransform _rectTransform;
    private RectTransform _canvasRect;
    private Animator _animator;
    private RhythmGameController _rhythmGameController;

    void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _canvasRect = GetComponentInParent<Canvas>().GetComponent<RectTransform>();
        _animator = GetComponent<Animator>();
    }

    void OnEnable()
    {
        _animator.Play("Note");
        SetRandomPosition();
    }


    public void PointReceived()
    {
        _rhythmGameController.PlayerScore();
        Destroy(gameObject);
    }

    public void Initialize(RhythmGameController rhythmGameController)
    {
        _rhythmGameController = rhythmGameController;
    }

    void SetRandomPosition()
    {
        // Get canvas bounds
        float canvasWidth = _canvasRect.rect.width;
        float canvasHeight = _canvasRect.rect.height;

        // Get image size
        float imageWidth = _rectTransform.rect.width;
        float imageHeight = _rectTransform.rect.height;

        // Pick random position inside canvas
        float randomX = Random.Range(-canvasWidth / 2f + imageWidth / 2f, canvasWidth / 2f - imageWidth / 2f);
        float randomY = Random.Range(-canvasHeight / 2f + imageHeight / 2f, canvasHeight / 2f - imageHeight / 2f);

        _rectTransform.anchoredPosition = new Vector2(randomX, randomY);

        StartCoroutine(IEDisappear());
    }

    IEnumerator IEDisappear()
    {
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
        _rhythmGameController.MissedScore();
    }
}
