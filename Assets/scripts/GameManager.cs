using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public Text BestScore;
    public Text NowScore;

    public GameObject Fail;
    public GameObject Victory;
    public GameObject BestScorePanel;

    public RectTransform BestScoreTitle;
    public RectTransform NowScoreTitle;
    public RectTransform BestScoreR;
    public RectTransform NowScoreR;

    public Card firstCard;
    public Card secondCard;


    public int TotalCard;

    float time = 0f;
    float TotalTime;
    int score = 0;
    public void Awake()
    {
        Time.timeScale = 1;
        if (Instance == null)
        {
            Instance = this;
        }
        
    }
    // Start is called before the first frame update
    void Start()
    {
        TotalTime = TimerBar.Instance.totalTime;
    }

    // Update is called once per frame
    void Update()
    {
        time = TimerBar.Instance.elapsedTime;
        if (TotalTime-time <= 0)
        {
            time = 0f;
            GameOver();
        }
    }
    public void GameOver()
    {
        if (PlayerPrefs.HasKey("BestScore"))
        {
            float best = PlayerPrefs.GetInt("BestScore");
            if (best < score)
            {
                PlayerPrefs.SetInt("BestScore", score);
                BestScore.text = score.ToString();
            }
            else
            {
                BestScore.text = best.ToString();
            }
        }
        else
        {
            PlayerPrefs.SetInt("BestScore", score);
            BestScore.text = score.ToString();
        }

        if (time <= 0f)//�ð��ʰ�
        {
            //���ھ� Ÿ��Ʋ, ���ھ��ǳ� ��ġ �̵�
            //Ÿ��Ʋ Best x-120 y110
            BestScoreTitle.anchoredPosition = new Vector2(-120, 110);
            //Now x-120 y-30
            NowScoreTitle.anchoredPosition = new Vector2(-120, -30);

            //���ھ� Best x120 y110
            BestScoreR.anchoredPosition = new Vector2(120, 110);
            //���ھ� now x120 y-30
            NowScoreR.anchoredPosition = new Vector2(120, -30);
            //����ȭ�� ���
            Fail.SetActive(true);
        }
        else
        {
            //���ھ� Ÿ��Ʋ, ���ھ��ǳ� ��ġ �̵�
            //Ÿ��Ʋ Best x220 y200
            BestScoreTitle.anchoredPosition = new Vector2(220, 200);
            //Now x-220 y200
            NowScoreTitle.anchoredPosition = new Vector2(-220, 200);
            //���ھ� Best x220 y110
            BestScoreR.anchoredPosition = new Vector2(220, 110);
            //���ھ� Now x-220 y110
            NowScoreR.anchoredPosition = new Vector2(-220, 110);
            //����ȭ�� ���
            Victory.SetActive(true);
        }
        BestScorePanel.SetActive(true);
        Time.timeScale = 0;
    }

    public void Matched()
    {
        if (firstCard.idx == secondCard.idx)
        {
            firstCard.DestroyCard();
            secondCard.DestroyCard();
            score += 5;
            NowScore.text = score.ToString();
            TotalCard -= 2;
            if (TotalCard == 0)
            {
                GameOver();
            }
        }
        else
        {
            firstCard.CloseCard();
            secondCard.CloseCard();
        }

        firstCard = null;
        secondCard = null;
    }
}
