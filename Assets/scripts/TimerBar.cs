using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Security;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TimerBar : MonoBehaviour
{
    #region 변수 선언
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
    [SerializeField] private float shakeStartThreshold = 5f; // 끝나기 몇 초 전부터 흔들릴지

    public float elapsedTime = 0f;
    private float barWidth;
    private Vector3 originalPosition;
    private int lastWarningSecond = -1;

    private AudioSource bgmSource;
    private AudioSource sfxSource;

    private bool isRunning = false; // 타이머 작동 확인용으로 게임 오버 후에도 Update가 계속 호출되는 것을 방지
    private bool isPaused = false;
    private bool isShaking = false;
    private bool hasSpeedUp = false;
    private bool playedEndSound = false;
    private float shakeTimer = 0f;
    #endregion


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

        StartTimer(Difficulty.Normal);
    }

    private void Update()
    {
        if (!isRunning || isPaused ) return;

        elapsedTime += Time.deltaTime;
        float t = Mathf.Clamp01(elapsedTime / totalTime);

        UpdateTimerUI(t);
        UpdateBGM(t);
        UpdateWarningSound();
        UpdateShake();

        CheckGameOver();
    }
    public void TimeSetting(float time)
    {
        totalTime = time;
    }

    #region 타이머 업데이트 함수
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
    #endregion

    #region 배경음악 업데이트 함수
    private void UpdateBGM(float t)
    {
        float remainingTime = totalTime - elapsedTime;

        if (remainingTime <= bgmSpeedUpTime && !hasSpeedUp)
        {
            if (bgmSource != null)
                bgmSource.pitch = 1.3f;

            hasSpeedUp = true;
        }
        else if (remainingTime > bgmSpeedUpTime && hasSpeedUp)
        {
            if (bgmSource != null)
                bgmSource.pitch = 1f;

            hasSpeedUp = false;
        }
    }
    #endregion

    #region 경고음 함수
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
    #endregion

    #region 흔들림 시작 검사 함수
    private void UpdateShake()
    {
        float remainingTime = totalTime - elapsedTime;

        if (remainingTime > shakeStartThreshold || isShaking) return;

        shakeTimer += Time.deltaTime;
        if (shakeTimer >= 1f)
        {
            StartCoroutine(ShakeUI());
            shakeTimer = 0f;
        }
    }
    #endregion

    #region 흔들림 함수
    private IEnumerator ShakeUI()
    {
        // 중복 실행 방지
        isShaking = true;

        // 흔들리는 시간 및 강도
        float duration = 0.2f;
        float magnitude = 10f;
        float elapsed = 0f;

        // duration 동안 흔들리는 시간 유지
        while (elapsed < duration)
        {
            // 실제로 흔드는 부분
            float offsetX = Random.Range(-magnitude, magnitude);
            float offsetY = Random.Range(-magnitude, magnitude);
            shakeTarget.anchoredPosition = originalPosition + new Vector3(offsetX, offsetY, 0f);
            
            // 다음 프레임까지 대기
            elapsed += Time.deltaTime;
            yield return null; 
        }
        //끝난 후 원래 위치로 복귀 및 상태 초기화
        shakeTarget.anchoredPosition = originalPosition;
        isShaking = false;
    }
    #endregion

    #region 게임 오버 체크 함수
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
    #endregion

    #region 타이머 리셋 함수
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
    #endregion

    #region 타이머 시작 함수
    public void StartTimer(Difficulty difficulty)
    {
        switch (difficulty)
        {
            case Difficulty.Easy:
                totalTime = 45f;
                break;
            case Difficulty.Normal:
                totalTime = 30f;
                break;
            case Difficulty.Hard:
                totalTime = 20f;
                break;
        }

        ResetTimer();
    }
    #endregion

    #region 난이도 enum
    public enum Difficulty
    {
        Easy,
        Normal,
        Hard
    }
    #endregion

    #region 시간 추가 함수
    public void AddTime(float extraTime)
    {
        totalTime += extraTime;

        // 시간 연장으로 BGM 속도 조건이 바뀔 수 있으므로 다시 체크
        if ((totalTime - elapsedTime) > bgmSpeedUpTime)
        {
            if (bgmSource != null)
                bgmSource.pitch = 1f;

            hasSpeedUp = false;
        }
        UpdateTimerUI(elapsedTime / totalTime);
    }
    #endregion

    #region 시간 감소 함수
    public void ReduceTime(float amount)
    {
        totalTime -= amount;

        // totalTime이 elapsedTime보다 작으면 즉시 게임 오버 유도
        if (totalTime <= elapsedTime)
        {
            elapsedTime = totalTime;
            UpdateTimerUI(1f); // 강제 업데이트
            CheckGameOver();   // 바로 게임 오버 확인
        }
        else
        {
            UpdateTimerUI(elapsedTime / totalTime);
        }
    }
    #endregion

    #region 타이머 정지 재개 함수
    public void PauseTimer()
    {
        isPaused = true;
    }

    public void TimeFreezing(float time)
    {
        isRunning = false;
        Invoke("TimeUnFreeze", time);

    }

    void TimeUnFreeze()
    {
        isRunning = true;
    }
    public void ResumeTimer()
    {
        isPaused = false;
    }
    #endregion
}
