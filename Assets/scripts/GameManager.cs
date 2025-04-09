using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameObject Fail;
    public GameObject Victory;

    public Card firstCard;
    public Card secondCard;

    float time = 0f;
    
    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        time = TimerBar.Instance.elapsedTime;

        if (time >= 30)
        {
            time = 30f;
            GameOver();
        }
    }
    public void GameOver()
    {
        if (time >= 30f)
        {
            Fail.SetActive(true);
        }
        else
        {
            Victory.SetActive(true);
        }
        Time.timeScale = 0;
    }

    public void Matched()
    {
        if (firstCard.idx == secondCard.idx)
        {
            firstCard.DestroyCard();
            secondCard.DestroyCard();
        }
        else
        {
            CardMissMatch();
           
        }

        firstCard = null;
        secondCard = null;
    }

  

    public void CardMissMatch()
    {
        firstCard.InvokeMissMatched();
        secondCard.InvokeMissMatched();
        TimerBar.Instance.elapsedTime += 1.5f;
    }


}
