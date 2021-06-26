using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Item : MonoBehaviour
{
    [Header("アイテムが拾われた時のイベント")]
    public UnityEvent onPickUp;
    /// <summary>
    /// 拾われた時呼ばれる
    /// </summary>
    public void PickUp()
    {
        print("拾われた！");
        onPickUp.Invoke();
        Destroy(gameObject);
    }
}
