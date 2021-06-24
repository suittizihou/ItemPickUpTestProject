using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;

public class ItemPickUp : MonoBehaviour
{
    [SerializeField] private ItemSerch itemSerch;

    /// <summary>
    /// ItemPickUpにバインドされているボタンが押されたら呼ばれる
    /// </summary>
    public void OnItemPickUp()
    {
        var item = itemSerch.GetNearItem();
        if (item == null) return;
        item.GetComponent<Item>().PickUp();
    }
}
