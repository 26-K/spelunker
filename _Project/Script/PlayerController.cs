using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    const string LeftAnimName = "Left";
    const string RightAnimName = "Right";
    const string LeftIdleAnimName = "LeftIdle";
    const string RightIdleAnimName = "RightIdle";
    public Text PlayerSpeed;
    public SpriteRenderer PlayerTexture;
    public Animator playerAnimator;
    public float baseMoveSpeed = 1000.0f;
    public float maxSpeed = 10.0f;
    public bool jumpFlag = false;
    public bool isGround = false;
    public float jumpPower = 50.0f;
    public float fallenOutDistance = 1;
    int jumpWait = 0; //ジャンプが連続で発動しないように
    bool isSuichokuRakka = false; //ジャンプしないで落下した場合
    bool isMiss = false;
    Rigidbody2D rgd;
    bool isLeft = false; //左を向いているか
    string nowAnimState = RightIdleAnimName;
    //　落下してからの距離
    private bool isFallStart;
    private float fallStartPos;
    float DeadTimer = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
        rgd = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame

    void Update()
    {
        PlayerMove();
        SpeedLimit();

        PlayerAction();
        DeadCheck();

        AnimeUpdate();
    }

    private void AnimeUpdate()
    {
        if (rgd.velocity.x < 0)
        {
            isLeft = true;
            changeAnimeState(LeftAnimName);
        }
        else if (rgd.velocity.x > 0)
        {
            isLeft = false;
            changeAnimeState(RightAnimName);
        }
        else
        {
            if (isLeft)
            {
                changeAnimeState(LeftIdleAnimName);
            }
            else
            {
                changeAnimeState(RightIdleAnimName);
            }
        }
    }

    private void changeAnimeState(string name)
    {
        if (nowAnimState != name)
        {
            nowAnimState = name;
            playerAnimator.SetTrigger(name);
        }
    }

    private void DeadCheck()
    {
        if (rgd.velocity.y < 0 && !isGround && !isFallStart)
        {
            isFallStart = true;
            fallStartPos = transform.position.y;
            Debug.Log("落下開始");
        }
        if (fallStartPos - transform.position.y >= fallenOutDistance)
        {
            Debug.Log("落下死");
            rgd.simulated = false;
            isMiss = true;
        }
        if (isGround)
        {
            fallStartPos = transform.position.y;
        }
    }

    void FixedUpdate()
    {
        PlayerSpeed.text = $"{rgd.velocity.x}";
        if (isMiss)
        {
            float alpha_Sin = Mathf.Sin(Time.time * 50) / 2 + 0.5f;
            if (alpha_Sin < 0.5f) { alpha_Sin = 0; }
            else { alpha_Sin = 1; }
            PlayerTexture.color = new Color(1.0f, 1.0f, 1.0f, alpha_Sin);
            DeadTimer += Time.deltaTime;
            if (DeadTimer >= 2.0f) //死亡演出終わり
            {
                isMiss = false;
                DeadTimer = 0;
                this.transform.position = GameManager.Instance.RespawnPosition;
                fallStartPos = this.transform.position.y;
                rgd.simulated = true;
            }
        }
        else
        {
            PlayerTexture.color = new Color(255, 255, 255, 255);
        }
    }

    void PlayerMove()
    {
        float x = GetArrowKey();
        if (isSuichokuRakka)
        {
            x = 0;
        }
        float calcPlayerSpd_x = CalcPlayerSpeedX(x);
        if (!jumpFlag)
        {
            calcPlayerSpd_x = rgd.velocity.x;
        }
        if (calcPlayerSpd_x > maxSpeed)
        {
            Vector2 vel = new Vector2(maxSpeed, rgd.velocity.y);
            rgd.velocity = vel;
        }
        else if (calcPlayerSpd_x < -maxSpeed)
        {
            Vector2 vel = new Vector2(-maxSpeed, rgd.velocity.y);
            rgd.velocity = vel;
        }
        else
        {
            Vector2 velo = new Vector2(calcPlayerSpd_x, rgd.velocity.y);
            rgd.velocity = velo;
        }
    }

    float GetArrowKey()
    {
        float calcval = 0;
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            calcval += -1;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            calcval += 1;
        }
        return calcval;
    }

    private void PlayerAction()
    {
        bool debug = false;
        //スペースキーを押した時にジャンプする。上昇中はジャンプできない
        if (Input.GetKeyDown(KeyCode.Space) && jumpFlag == true && rgd.velocity.y <= 0.1f)
        {
            jumpFlag = false;
            Vector2 vec = new Vector2(0, jumpPower);
            rgd.AddForce(vec);
        }
        if (Input.GetKeyDown(KeyCode.Space) && debug)
        {
            jumpFlag = false;
            Vector2 vec = new Vector2(0, jumpPower);
            rgd.AddForce(vec);
        }
    }

    float CalcPlayerSpeedX(float spd_x)
    {
        if (spd_x == 0) { return 0; }
        return rgd.velocity.x + Vector2.right.x * spd_x * baseMoveSpeed;
    }

    void SpeedLimit()
    {
        if (rgd.velocity.x > maxSpeed)
        {
            Vector2 vel = new Vector2(maxSpeed, rgd.velocity.y);
            rgd.velocity = vel;
        }
        if (rgd.velocity.x < -maxSpeed)
        {
            Vector2 vel = new Vector2(-maxSpeed, rgd.velocity.y);
            rgd.velocity = vel;
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.transform.tag == "Ground")
        {
            isFallStart = false;
            jumpFlag = true;
            isGround = true;
            isSuichokuRakka = false;
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        //地面から離れた時ジャンプフラグが残っていると慣性を0にして落下させる
        if (other.transform.tag == "Ground")
        {
            isGround = false;
            if (jumpFlag == true && rgd.velocity.y <= 0)
            {
                Vector2 vel = new Vector2(0, rgd.velocity.y - 4);
                rgd.velocity = vel;
                isSuichokuRakka = true;
            }
        }
    }
}
