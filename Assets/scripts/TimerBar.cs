using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class TimerBar : MonoBehaviour
{
    public static TimerBar Instance;

    [SerializeField] private Image timerFillImage;
    [SerializeField] private RectTransform phoenix;
    [SerializeField] public float totalTime = 30f;



    [SerializeField] private Color normalColor = Color.white;
    [SerializeField] private Color warningColor = Color.yellow;
    [SerializeField] private Color dangerColor = Color.red;

    public float elapsedTime = 0f;
    private float barWidth;
    private bool isRunning = false; // 타이머 작동 확인용으로 게임 오버 후에도 Update가 계속 호출되는 것을 방지

    [SerializeField] private RectTransform shakeTarget; // TimerBar 전체의 RectTransform

    private float shakeTimer = 0f;
    private Vector3 originalPosition;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    void Start()
    {
        barWidth = ((RectTransform)timerFillImage.transform).rect.width; // 게이지 전체 너비 계산
        isRunning = true;
        originalPosition = shakeTarget.anchoredPosition;
    }

    void Update()
    {
        if (!isRunning) return;

        elapsedTime += Time.deltaTime;
        float t = Mathf.Clamp01(elapsedTime / totalTime); // 전체 시간 중 얼마나 지났는지 0~1 사이의 비율로 계산

        // 게이지 채우기
        timerFillImage.fillAmount = t; // Image 컴포넌트의 fillAmount를 조정

        // 색 변경
        UpdateBarColor();

        // 불사조
        Vector2 pos = phoenix.anchoredPosition; //현재 불사조의 앵커 기준 위치를 pos에 저장
        pos.x = barWidth * t; // 게이지바 전체 너비에 t(시간 진행 비율)을 곱해줍니다. t는 0.0~1.0의 값을 가집니다.
        phoenix.anchoredPosition = pos; // 수정한 좌표 적용

        // 흔들림 시작
        if (elapsedTime >= 25f)
        {
            shakeTimer += Time.deltaTime;
            if (shakeTimer >= 1f)
            {
                StartCoroutine(ShakeUI());
                shakeTimer = 0f;
            }
        }

        // 타임 오버
        CheckGameOver();
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

    public void ResetTimer()
    {
        elapsedTime = 0f;
        isRunning = true;
        timerFillImage.color = Color.white;
    }

    private void UpdateBarColor()
    {
        if (elapsedTime >= totalTime - 5f)
            timerFillImage.color = dangerColor;
        else if (elapsedTime >= totalTime / 2f)
            timerFillImage.color = warningColor;
        else
            timerFillImage.color = normalColor;
    }

    private void CheckGameOver()
    {
        if (elapsedTime >= totalTime)
        {
            isRunning = false;
            //GameManager.Instance.GameOver();
        }
    }

}