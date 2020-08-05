using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    const string playerTagName = "Player";

    public float moveSpeed = 2.0f;
    public bool isRidePlayer = false;

    Rigidbody2D rgd;
    // Start is called before the first frame update
    void Start()
    {
        rgd = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isRidePlayer == true)
        {
            //プレイヤーが触れている場合
            if (Input.GetKey(KeyCode.DownArrow))
            {
                rgd.velocity = new Vector3(0, -moveSpeed, 0);
            }
            else if (Input.GetKey(KeyCode.UpArrow))
            {
                rgd.velocity = new Vector3(0, moveSpeed, 0);
            }
            else
            {
                rgd.velocity = new Vector3(0, 0, 0);
            }
        }
        else
        {
            rgd.velocity = new Vector3(0, 0, 0);
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.transform.tag == playerTagName)
        {
            isRidePlayer = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.transform.tag == playerTagName)
        {
            isRidePlayer = false;
        }
    }
}
