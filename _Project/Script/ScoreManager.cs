using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : SingletonMonoBehaviour<ScoreManager>
{
    public Text scoreText;
    int displayScore = 0;
    int realScore = 0;
    [Range(0, 1)] public float easeRatio;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (displayScore != realScore)
        {
            RefreshDisplayScore();
            scoreText.text = $"{displayScore}";
        }
    }

    private void RefreshDisplayScore()
    {
        var diff = realScore - displayScore;
        var deltaFloat = diff * easeRatio;
        int delta;
        if (Mathf.Abs(deltaFloat) < 1)
        {
            delta = (int)Mathf.Sign(diff);
        }
        else
        {
            delta = Mathf.FloorToInt(deltaFloat);
        }
        displayScore += delta;
    }


    public void AddScore(int value)
    {
        realScore += value;
    }
}
