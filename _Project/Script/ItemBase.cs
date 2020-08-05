using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBase : MonoBehaviour
{
    bool isVisible = true;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!isVisible) { return; }
        if (other.transform.tag == "Player")
        {
            this.gameObject.SetActive(false);
            PickUpEvent();
            isVisible = false;
            GameManager.Instance.RespawnPosition = this.gameObject.transform.position;
        }
    }

    public virtual void PickUpEvent()
    {

    }
}
