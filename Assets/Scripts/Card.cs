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
        AudioManager.Instance.PlayFlip();
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
        AudioManager.Instance.PlayFlip();
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
    public void SetMatched(bool force = false)
    {
        if (force)
        {
            StopAllCoroutines();
            frontImage.gameObject.SetActive(true);
            backImage.gameObject.SetActive(false);
            flipCoroutine = null;
        }

        State = CardState.Matched;
        GetComponent<CanvasGroup>().blocksRaycasts = false;
        StartCoroutine(PlayDespawn());

    }
    private IEnumerator PlaySpawn()
    {
        yield return Fade(0, 1f, 0.5f);
        yield return new WaitForSeconds(UnityEngine.Random.Range(0.5f,1f));
        if(State != CardState.Matched)
            FlipInvert();
    }
    private IEnumerator PlayDespawn()
    {
        yield return Wiggle(10f, 0.5f);
        yield return Fade(1, 0f, 0.5f);
    }

    private IEnumerator Fade(float from, float to, float time)
    {
        var target = GetComponent<CanvasGroup>();
        float t = 0f;
        while (t < time)
        {
            t += Time.deltaTime;
            float val = Mathf.Lerp(from, to, t / time);
            target.alpha = val;
            yield return null;
        }
        transform.localEulerAngles = new Vector3(0, to, 0);
    }

    IEnumerator Wiggle(float angle, float duration)
    {
        float timer = 0f;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            float z = Mathf.Sin(timer * 40f) * angle * (1 - timer / duration);
            transform.rotation = Quaternion.Euler(0, 0, z);
            yield return null;
        }

        transform.rotation = Quaternion.identity;
    }

    public void MarkProcessing()
    {
        if (State == CardState.FaceUp) State = CardState.Processing;
    }
}
