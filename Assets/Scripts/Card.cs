using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum CardState { FaceDown, Flipping, FaceUp, Matched, Processing }

[RequireComponent(typeof(CanvasGroup))]
public class Card : MonoBehaviour, IPointerClickHandler
{
    public int id; 
    public Image frontImage;
    public Image backImage;
    public float flipDuration = 0.22f;

    public CardState State { get; private set; } = CardState.FaceUp;
    public Action<Card> OnFlipComplete;

    private Coroutine flipCoroutine;

    private void Start()
    {
        StartCoroutine(PlaySpawn());
    }

    public void SetCard(Sprite front, Sprite back, int id)
    {
        this.id = id;
        frontImage.sprite = front;
        backImage.sprite = back;
    }


    public void OnPointerClick(PointerEventData e)
    {
        if (State == CardState.FaceDown)
            Flip();
    }

    public void Flip()
    {
        if (flipCoroutine != null) 
            return;
        flipCoroutine = StartCoroutine(FlipRoutine());
    }
    public void FlipInvert()
    {
        if (flipCoroutine != null)
            return;
        flipCoroutine = StartCoroutine(FlipInvertRoutine());
    }
    public IEnumerator FlipInvertRoutine()
    {
        State = CardState.Flipping;
        float half = flipDuration / 2f;
        // rotate Y from 0 to 90
        yield return RotateY(0, 90, half);
        // swap visuals
        frontImage.gameObject.SetActive(false);
        backImage.gameObject.SetActive(true);
        // rotate 90 to 180 (final)
        yield return RotateY(90, 180, half);
        State = CardState.FaceDown;
        flipCoroutine = null;
        transform.localEulerAngles = Vector3.zero;
    }

    public IEnumerator FlipRoutine()
    {
        State = CardState.Flipping;
        float half = flipDuration / 2f;
        // rotate Y from 0 to 90
        yield return RotateY(0, 90, half);
        // swap visuals
        frontImage.gameObject.SetActive(true);
        backImage.gameObject.SetActive(false);
        // rotate 90 to 180 (final)
        yield return RotateY(90, 180, half);
        State = CardState.FaceUp;
        flipCoroutine = null;
        OnFlipComplete?.Invoke(this);
    }

    private IEnumerator RotateY(float from, float to, float time)
    {
        float t = 0f;
        while (t < time)
        {
            t += Time.deltaTime;
            float val = Mathf.Lerp(from, to, t / time);
            transform.localEulerAngles = new Vector3(0, val, 0);
            yield return null;
        }
        transform.localEulerAngles = new Vector3(0, to, 0);
    }
    public void FlipBackInstant()
    {
        StopAllCoroutines();
        frontImage.gameObject.SetActive(false);
        backImage.gameObject.SetActive(true);
        transform.localEulerAngles = Vector3.zero;
        State = CardState.FaceDown;
    }
    public void SetMatched()
    {
        State = CardState.Matched;
        GetComponent<CanvasGroup>().blocksRaycasts = false;
        StartCoroutine(PlayDespawn());
    }
    private IEnumerator PlaySpawn()
    {
        yield return new WaitForSeconds(1f);
        FlipInvert();
    }
    private IEnumerator PlayDespawn()
    {
        yield return null;
    }

    public void MarkProcessing()
    {
        if (State == CardState.FaceUp) State = CardState.Processing;
    }
}
