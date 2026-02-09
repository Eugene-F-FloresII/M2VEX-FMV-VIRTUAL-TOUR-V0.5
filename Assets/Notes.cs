using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class Notes : MonoBehaviour
{
    private RectTransform rectTransform;
    private RectTransform canvasRect;
    private Animator animator;

    RhythmGame rhythmGame;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasRect = GetComponentInParent<Canvas>().GetComponent<RectTransform>();
        animator = GetComponent<Animator>();
    }
    void Start()
    {
        rhythmGame = GameObject.FindGameObjectWithTag("RhythmGame").GetComponent<RhythmGame>();
    }

    void OnEnable()
    {
        animator.Play("Note");
        SetRandomPosition();
    }


    public void Pointreceived()
    {
        rhythmGame.playerScore(1);
        Destroy(gameObject);
    }

    void SetRandomPosition()
    {
        // Get canvas bounds
        float canvasWidth = canvasRect.rect.width;
        float canvasHeight = canvasRect.rect.height;

        // Get image size
        float imageWidth = rectTransform.rect.width;
        float imageHeight = rectTransform.rect.height;

        // Pick random position inside canvas
        float randomX = Random.Range(-canvasWidth / 2f + imageWidth / 2f, canvasWidth / 2f - imageWidth / 2f);
        float randomY = Random.Range(-canvasHeight / 2f + imageHeight / 2f, canvasHeight / 2f - imageHeight / 2f);

        rectTransform.anchoredPosition = new Vector2(randomX, randomY);

        StartCoroutine(disappear());


        
    }

    IEnumerator disappear()
    {
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
        rhythmGame.missedScore();
    }
}
