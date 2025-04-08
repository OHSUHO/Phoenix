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
    [SerializeField] private float totalTime = 30f;

    

    [SerializeField] private Color normalColor = Color.white;
    [SerializeField] private Color warningColor = Color.yellow;
    [SerializeField] private Color dangerColor = Color.red;

    public float elapsedTime = 0f;
    private float barWidth;
    private bool isRunning = false; // Ÿ�̸� �۵� Ȯ�ο����� ���� ���� �Ŀ��� Update�� ��� ȣ��Ǵ� ���� ����

    [SerializeField] private RectTransform shakeTarget; // TimerBar ��ü�� RectTransform

    private float shakeTimer = 0f;
    private Vector3 originalPosition;

    void Start()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        barWidth = ((RectTransform)timerFillImage.transform).rect.width; // ������ ��ü �ʺ� ���
        isRunning = true;
        originalPosition = shakeTarget.anchoredPosition;
    }

    void Update()
    {
        if (!isRunning) return;

        elapsedTime += Time.deltaTime;
        
        float t = Mathf.Clamp01(elapsedTime / totalTime); // ��ü �ð� �� �󸶳� �������� 0~1 ������ ������ ���

        // ������ ä���
        timerFillImage.fillAmount = t; // Image ������Ʈ�� fillAmount�� ����

        // �� ����
        UpdateBarColor();

        // �һ���
        Vector2 pos = phoenix.anchoredPosition; //���� �һ����� ��Ŀ ���� ��ġ�� pos�� ����
        pos.x = barWidth * t; // �������� ��ü �ʺ� t(�ð� ���� ����)�� �����ݴϴ�. t�� 0.0~1.0�� ���� �����ϴ�.
        phoenix.anchoredPosition = pos; // ������ ��ǥ ����

        // ��鸲 ����
        if (elapsedTime >= 25f)
        {
            shakeTimer += Time.deltaTime;
            if (shakeTimer >= 1f)
            {
                StartCoroutine(ShakeUI());
                shakeTimer = 0f;
            }
        }

        // Ÿ�� ����
        CheckGameOver();
    }

    // Ÿ�̸Ӱ� ��鸮�� ����
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
            //���� �Ŵ������� ���ӿ��� �Լ� ����
            //GameManager.Instance.GameOver();
        }
    }

}
