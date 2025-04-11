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
    #region ���� ����
    public static TimerBar Instance;

    [Header("UI Referencees")]
    [SerializeField] private Image timerFillImage;
    [SerializeField] private RectTransform phoenix;
    [SerializeField] private RectTransform shakeTarget; // TimerBar ��ü�� RectTransform

    [Header("Color Thresholds")]
    private float totalTime;

    [Header("Color Thresholds")]
    [SerializeField] private float warningTimeThreshold = 15f;
    [SerializeField] private float dangerTimeThreshold = 5f;
    [SerializeField] private Color normalColor = Color.white;
    [SerializeField] private Color warningColor = Color.yellow;
    [SerializeField] private Color dangerColor = Color.red;

    [Header("Audio Settings")]
    [SerializeField] private AudioClip warningSound; // �����
    [SerializeField] private AudioClip gameOverSound; // ���ӿ�����
    [SerializeField] private float bgmSpeedUpTime = 10f; // BGM �ӵ� �������� ����
    [SerializeField] private float shakeStartThreshold = 5f; // ������ �� �� ������ ��鸱��

    public float elapsedTime = 0f;
   
    private float barWidth;
    private Vector3 originalPosition;
    private int lastWarningSecond = -1;

    private AudioSource bgmSource;
    private AudioSource sfxSource;

    private bool isRunning = false; // Ÿ�̸� �۵� Ȯ�ο����� ���� ���� �Ŀ��� Update�� ��� ȣ��Ǵ� ���� ����
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
            StartTimer(DifficultSetting.Instance.Getting());
        }
        Time.timeScale = 1;

    }
    private void Start()
    {
        barWidth = ((RectTransform)timerFillImage.transform).rect.width; // ������ ��ü �ʺ� ���
        isRunning = true;
        originalPosition = shakeTarget.anchoredPosition;

        bgmSource = AudioManager.Instance.GetAudioSource();
        sfxSource = gameObject.AddComponent<AudioSource>();

        
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
    
    public void SettingTotalTime(float time)
    {
        totalTime = time;
    }
    public float GettingTotalTime()
    {
        return totalTime;
    }


    #region Ÿ�̸� ������Ʈ �Լ�
    private void UpdateTimerUI(float t)
    {
        timerFillImage.fillAmount = t;

        // ���� ����
        if (elapsedTime >= totalTime - dangerTimeThreshold)
            timerFillImage.color = dangerColor;
        else if (elapsedTime >= totalTime - warningTimeThreshold)
            timerFillImage.color = warningColor;
        else
            timerFillImage.color = normalColor;

        // �һ��� �̵�
        Vector2 pos = phoenix.anchoredPosition;
        pos.x = barWidth * t;
        phoenix.anchoredPosition = pos;
    }
    #endregion

    #region ������� ������Ʈ �Լ�
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

    #region ����� �Լ�
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

    #region ��鸲 ���� �˻� �Լ�
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

    #region ��鸲 �Լ�
    private IEnumerator ShakeUI()
    {
        // �ߺ� ���� ����
        isShaking = true;

        // ��鸮�� �ð� �� ����
        float duration = 0.2f;
        float magnitude = 10f;
        float elapsed = 0f;

        // duration ���� ��鸮�� �ð� ����
        while (elapsed < duration)
        {
            // ������ ���� �κ�
            float offsetX = Random.Range(-magnitude, magnitude);
            float offsetY = Random.Range(-magnitude, magnitude);
            shakeTarget.anchoredPosition = originalPosition + new Vector3(offsetX, offsetY, 0f);
            
            // ���� �����ӱ��� ���
            elapsed += Time.deltaTime;
            yield return null; 
        }
        //���� �� ���� ��ġ�� ���� �� ���� �ʱ�ȭ
        shakeTarget.anchoredPosition = originalPosition;
        isShaking = false;
    }
    #endregion

    #region ���� ���� üũ �Լ�
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

    #region Ÿ�̸� ���� �Լ�
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

    #region Ÿ�̸� ���� �Լ�
    public void StartTimer(DifficultSetting.Difficulty difficulty)
    {
        
        switch (difficulty)
        {
            case DifficultSetting.Difficulty.Easy:
                SettingTotalTime(60f);
                Debug.Log("�������");
                Debug.Log(totalTime);
                break;
            case DifficultSetting.Difficulty.Normal:
                Debug.Log("�븻���");
                SettingTotalTime(45f);
                Debug.Log(totalTime);
                
                break;
            case DifficultSetting.Difficulty.Hard:
                Debug.Log("�ϵ���");
                SettingTotalTime(30f);
                Debug.Log(totalTime);
                
                break;
        }

        ResetTimer();
    }
    #endregion



    #region �ð� �߰� �Լ�
    public void AddTime(float extraTime)
    {
        totalTime += extraTime;
        GameManager.Instance.totaltime += extraTime;

        // �ð� �������� BGM �ӵ� ������ �ٲ� �� �����Ƿ� �ٽ� üũ
        if ((totalTime - elapsedTime) > bgmSpeedUpTime)
        {
            if (bgmSource != null)
                bgmSource.pitch = 1f;

            hasSpeedUp = false;
        }
        UpdateTimerUI(elapsedTime / totalTime);
    }
    #endregion

    #region �ð� ���� �Լ�
    public void ReduceTime(float amount)
    {
        totalTime -= amount;

        // totalTime�� elapsedTime���� ������ ��� ���� ���� ����
        if (totalTime <= elapsedTime)
        {
            elapsedTime = totalTime;
            UpdateTimerUI(1f); // ���� ������Ʈ
            CheckGameOver();   // �ٷ� ���� ���� Ȯ��
        }
        else
        {
            UpdateTimerUI(elapsedTime / totalTime);
        }
    }
    #endregion

    #region Ÿ�̸� ���� �簳 �Լ�
    public void PauseTimer()
    {
        isPaused = true;
    }

    public void TimeFreezing(float time)
    {
        PauseTimer();
        Invoke("ResumeTimer", time);
        

    }


    public void ResumeTimer()
    {
        isPaused = false;
    }
    #endregion
}
