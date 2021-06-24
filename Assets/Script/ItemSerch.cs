using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSerch : MonoBehaviour
{
    [SerializeField, Header("アイテムを取得する原点(これを元に近いアイテム、遠いアイテムが決まる)")] private GameObject originPoint;

    public List<GameObject> ItemList { get; private set; } = new List<GameObject>();

    private void FixedUpdate()
    {
        if (ItemList.Count >= 1) ItemList.Clear();
    }

    /// <summary>
    /// 一番近いアイテムを返す
    /// </summary>
    /// <returns></returns>
    public GameObject GetNearItem()
    {
        if (ItemList.Count <= 0) return null;
        if (ItemList.Count == 1) return ItemList[0];

        var nearItem = ItemList[0];
        // 初期最小値を設定
        var minDirection = float.MaxValue;
        // 二つ目のアイテムから取得ポイントとの距離を計算
        for(int itemNum = 0; itemNum < ItemList.Count; itemNum++)
        {
            if (ItemList[itemNum] == null) continue;
            var direction = Vector3.Distance(ItemList[itemNum].transform.position, originPoint.transform.position);
            // より近いオブジェクトを代入
            if (minDirection > direction)
            {
                minDirection = direction;
                nearItem = ItemList[itemNum];
            }
        }
        return nearItem;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Item"))
        {
            ItemList.Add(other.gameObject);
        }
    }

#if UNITY_EDITOR
    /// <summary>
    /// 拾う対象のアイテムにギズモを表示
    /// </summary>
    public void SetPickUpTargetItemMarker()
    {
        var item = GetNearItem();
        if (item == null) return;
        Gizmos.DrawSphere(item.transform.position, 0.1f);
    }
    private void OnDrawGizmos()
    {
        SetPickUpTargetItemMarker();
    }    
#endif
}
