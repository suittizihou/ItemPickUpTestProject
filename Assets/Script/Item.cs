using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    /// <summary>
    /// 拾われた時呼ばれる
    /// </summary>
    public void PickUp()
    {
        print("拾われた！");
        Destroy(gameObject);
    }
}
