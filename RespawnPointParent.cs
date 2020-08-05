using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnPointParent : MonoBehaviour
{
    public Vector3 addPos = new Vector3(0, 0.3f, 0);
    void OnTriggerEnter2D(Collider2D other)
    {
        GameManager.Instance.RespawnPosition = this.transform.parent.position + addPos;
    }
}
