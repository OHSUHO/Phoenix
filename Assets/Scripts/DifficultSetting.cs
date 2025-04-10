using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultSetting : MonoBehaviour
{
    public static DifficultSetting Instance;
    Difficulty diffic = Difficulty.Normal;
    private void Awake()
    {
        if (DifficultSetting.Instance == null)
        {
            DifficultSetting.Instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            DontDestroyOnLoad(this);
        }
    }

    #region ≥≠¿Ãµµ enum
    public enum Difficulty
    {
        Easy,
        Normal,
        Hard
    }
    #endregion

    public void Setting(Difficulty diff)
    {
        diffic = diff;
    }

    public Difficulty Getting()
    {
        return diffic;
    }

  

}
