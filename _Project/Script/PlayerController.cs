using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public Text PlayerSpeed;
    public float baseMoveSpeed = 1000.0f;
    public float maxSpeed = 10.0f;
    public bool jumpFlag = false;
    public bool isGround = false;
    public float jumpPower = 50.0f;
    Rigidbody2D rgd;
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
    }
    void FixedUpdate()
    {
        PlayerSpeed.text = $"{rgd.velocity.x}";
    }

    void PlayerMove()
    {
        float x = GetArrowKey();

        float calcPlayerSpd_x = CalcPlayerSpeedX(x);
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
        if (Input.GetKeyDown(KeyCode.Space) && jumpFlag == true && rgd.velocity.y <= 0)
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
            jumpFlag = true;
            isGround = true;
        }
    }
}
