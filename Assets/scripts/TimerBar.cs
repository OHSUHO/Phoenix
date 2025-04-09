using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class TimerBar : MonoBehaviour
{
    public static TimerBar Instance;

    [Header("UI Referencees")]
    [SerializeField] private Image timerFillImage;
    [SerializeField] private RectTransform phoenix;
    [SerializeField] private RectTransform shakeTarget; // TimerBar 전체의 RectTransform

    [Header("Timer Settings")]
    [SerializeField] private float totalTime = 30f;

    [Header("Color Thresholds")]
    [SerializeField] private float warningTimeThreshold = 15f;
    [SerializeField] private float dangerTimeThreshold = 5f;
    [SerializeField] private Color normalColor = Color.white;
    [SerializeField] private Color warningColor = Color.yellow;
    [SerializeField] private Color dangerColor = Color.red;

    [Header("Audio Settings")]
    [SerializeField] private AudioClip warningSound; // 경고음
    [SerializeField] private AudioClip gameOverSound; // 게임오버음
    [SerializeField] private float bgmSpeedUpTime = 10f; // BGM 속도 빨라지는 시점
    [SerializeField] private float shakeStartTime = 25f;

    public float elapsedTime = 0f;
    private float barWidth;
    private Vector3 originalPosition;
    private int lastWarningSecond = -1;

    private AudioSource bgmSource;
    private AudioSource sfxSource;

    private bool isRunning = false; // 타이머 작동 확인용으로 게임 오버 후에도 Update가 계속 호출되는 것을 방지
    private bool hasSpeedUp = false;
    private bool playedEndSound = false;
    private float shakeTimer = 0f;

    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        Time.timeScale = 1;

    }
    private void Start()
    {
        barWidth = ((RectTransform)timerFillImage.transform).rect.width; // 게이지 전체 너비 계산
        isRunning = true;
        originalPosition = shakeTarget.anchoredPosition;

        bgmSource = AudioManager.Instance.GetAudioSource();
        sfxSource = gameObject.AddComponent<AudioSource>();

        StartTimer();
    }

    private void Update()
    {
        if (!isRunning) return;

        elapsedTime += Time.deltaTime;
        float t = Mathf.Clamp01(elapsedTime / totalTime);

        UpdateTimerUI(t);
        UpdateBGM(t);
        UpdateWarningSound();
        UpdateShake();

        CheckGameOver();
    }

    private void UpdateTimerUI(float t)
    {
        timerFillImage.fillAmount = t;

        // 색상 변경
        if (elapsedTime >= totalTime - dangerTimeThreshold)
            timerFillImage.color = dangerColor;
        else if (elapsedTime >= totalTime - warningTimeThreshold)
            timerFillImage.color = warningColor;
        else
            timerFillImage.color = normalColor;

        // 불사조 이동
        Vector2 pos = phoenix.anchoredPosition;
        pos.x = barWidth * t;
        phoenix.anchoredPosition = pos;
    }

    private void UpdateBGM(float t)
    {
        if (!hasSpeedUp && (totalTime - elapsedTime) <= bgmSpeedUpTime)
        {
            if (bgmSource != null)
                bgmSource.pitch = 1.3f;

            hasSpeedUp = true;
        }
    }
    private void UpdateWarningSound()
    {
        if (elapsedTime < totalTime - dangerTimeThreshold || elapsedTime >= totalTime) return;

        int remaining = Mathf.FloorToInt(totalTime - elapsedTime);
        if (remaining != lastWarningSecond)
        {
            lastWarningSecond = remaining;
            if (warningSound)
                sfxSource.PlayOneShot(warningSound);
        }
    }
    private void UpdateShake()
    {
        if (elapsedTime < shakeStartTime) return;

        shakeTimer += Time.deltaTime;
        if (shakeTimer >= 1f)
        {
            StartCoroutine(ShakeUI());
            shakeTimer = 0f;
        }
    }

    // 타이머가 흔들리는 연출
    private IEnumerator ShakeUI()
    {
        float duration = 0.2f;
        float magnitude = 10f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float offsetX = Random.Range(-magnitude, magnitude);
            float offsetY = Random.Range(-magnitude, magnitude);

            shakeTarget.anchoredPosition = originalPosition + new Vector3(offsetX, offsetY, 0f);

            elapsed += Time.deltaTime;
            yield return null;
        }
        shakeTarget.anchoredPosition = originalPosition;
    }


    private void CheckGameOver()
    {
        if (elapsedTime < totalTime) return;

        if (!playedEndSound)
        {
            playedEndSound = true;

            if (bgmSource != null)
                bgmSource.Stop();

            if (gameOverSound)
                sfxSource.PlayOneShot(gameOverSound);
        }

        isRunning = false;
        //GameManager.Instance.GameOver();
    }

    public void ResetTimer()
    {
        elapsedTime = 0f;
        isRunning = true;
        playedEndSound = false;
        lastWarningSecond = -1;
        hasSpeedUp = false;
        shakeTimer = 0f;

        timerFillImage.color = normalColor;

        if (bgmSource != null)
            bgmSource.pitch = 1f;
    }

    public void StartTimer()
    {
        ResetTimer();
    }

}
