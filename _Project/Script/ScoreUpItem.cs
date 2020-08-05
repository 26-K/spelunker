using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreUpItem : ItemBase
{
    public int IncleaseScoreValue = 1000;
    public override void PickUpEvent()
    {
        ScoreManager.Instance.AddScore(IncleaseScoreValue);
    }
}
