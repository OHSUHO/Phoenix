using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    [SerializeField] private TimerBar timerBar;

    void Start()
    {
        timerBar.ResetTimer();
    }
}
