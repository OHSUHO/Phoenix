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



    }

    public void Setting(int num)
    {
        idx = num;
        frontImage.sprite = Resources.Load<Sprite>($"Phoenix{num}");
    }


    public void InvokeMissMatched()
    {
        Invoke("MissMatched", 1f);
    }
    public void MissMatched()
    {

        anim.SetBool("isSelected", false);
        InvokeSetUnActiveFront();
        InvokeSetActiveBack();

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
        SelectedCard();

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


}