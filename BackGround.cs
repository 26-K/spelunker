using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGround : MonoBehaviour
{

    [SerializeField]
    public float speedRatio = 4;
    [SerializeField]
    int spriteCount = 2; //背景オブジェクトの横の数

    float width;
    Vector3 beforeCameraPos = new Vector3(0, 0, 0);

    float basePosy;

    //初期化
    private void Start()
    {
        // スプライトの幅を取得
        width = GetComponent<SpriteRenderer>().bounds.size.x;
        basePosy = this.transform.position.y;
    }

    void LateUpdate()
    {
        var diffPos = beforeCameraPos - Camera.main.transform.position;
        diffPos.z = 0;
        transform.position += diffPos * speedRatio;
        transform.position = new Vector3(this.transform.position.x, Camera.main.transform.position.y + basePosy, transform.position.z);
        var camPos = Camera.main.transform.position;
        if (camPos.x >= transform.position.x + width) //カメラからある程度右に離れた時
        {
            transform.position += Vector3.right * width * spriteCount - Vector3.right;
        }
        else if (camPos.x <= transform.position.x - width) //カメラからある程度左に離れた時
        {
            this.transform.position += Vector3.left * width * spriteCount - Vector3.left;
        }
        beforeCameraPos = Camera.main.transform.position;
    }

    //カメラ外に出たときの処理
    void OnBecameInvisible()
    {
        // var camPos = Camera.main.transform.position;

        // //右端に出たか左端に出たかを調べる
        // if (transform.position.x <= camPos.x) //左端から画面外に出た場合
        // {
        //     // 幅ｘ個数分だけ右へ移動
        //     transform.position += Vector3.right * width * spriteCount + Vector3.left;
        // }
        // else //右端から画面外に出た場合
        // {
        //     transform.position += Vector3.left * width * spriteCount + Vector3.right;
        // }

    }
}
