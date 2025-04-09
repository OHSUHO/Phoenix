using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    public int idx = 0;

    public Animator anim;
    public GameObject front;
    public GameObject back;
    public SpriteRenderer frontImage;



    private void Start()
    {
     

    }
    public void SelectedCard()
    {
        anim.SetBool("isSelected", true);
        InvokeSetActiveFront();
        InvokeSetUnActiveBack();
        Invoke("MissMatched", 1.0f);
        
        
    }

    public void Setting(int num)
    {
        idx = num;
        frontImage.sprite = Resources.Load<Sprite>($"Phoenix{num}");
    }

    void MissMatched()
    {
        
        anim.SetBool("isSelected", false);
        InvokeSetUnActiveFront();
        InvokeSetActiveBack();
        TimerBar.Instance.elapsedTime += 1.5f;
    }

    
    void InvokeSetActiveFront()
    {
        Invoke("SetActiveFront", 0.5f);
    }

    void SetActiveFront()
    {
        front.SetActive(true);
    }

    void InvokeSetUnActiveFront()
    {
        Invoke("SetUnActiveFront", 0.5f);
    }

    void SetUnActiveFront()
    {
        front.SetActive(false);
    }

    void InvokeSetUnActiveBack()
    {
        Invoke("SetUnActiveBack", 0.5f);
    }
    void SetUnActiveBack()
    {
        back.SetActive(false);
    }

    void InvokeSetActiveBack()
    {
        Invoke("SetActiveBack", 0.5f);
    }
    void SetActiveBack()
    {
        back.SetActive(true);
    }

    public void OpenCard()
    {
        anim.SetBool("isSelected", true);
        front.SetActive(true);
        back.SetActive(false);

        if (GameManager.Instance.firstCard == null)
        {
            GameManager.Instance.firstCard = this;
        }
        else
        {
            GameManager.Instance.secondCard = this;
            GameManager.Instance.Matched();
        }
    }

    public void DestroyCard()
    {
        Invoke("DestoryCardInvoke", 1.0f);
    }

    void DestoryCardInvoke()
    {
        Destroy(gameObject);
    }

    public void CloseCard()
    {
        Invoke("CloseCardInvoke", 1.0f);
    }

    void CloseCardInvoke()
    {
        anim.SetBool("isSelected", false);
        front.SetActive(false);
        back.SetActive(true);
    }
}
