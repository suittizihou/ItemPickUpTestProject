using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class ItemSerch : MonoBehaviour
{
    [SerializeField, Header("アイテムを取得する原点(これを元に近いアイテム、遠いアイテムが決まる)")] private GameObject originPoint;

    public List<GameObject> ItemList { get; private set; } = new List<GameObject>();

    [Header("アイテムリストを常に更新")]
    public bool isAlwaysUpdate = true;
    // アイテムリストが更新されたか
    private bool isItemListUpdate;

    private void Start()
    {
        // アイテム削除関数を実行開始
        StartCoroutine(LateFixedUpdate());
    }

    /// <summary>
    /// オブジェクトと接触した時呼ばれる
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Item"))
        {
            isItemListUpdate = true;
            var item = other.GetComponent<Item>();
            // アイテムを拾った時アイテムリストから除外するイベントを登録
            item.onPickUp.AddListener(() => ItemList.Remove(other.gameObject));
            item.onPickUp.AddListener(() => isItemListUpdate = true);
            ItemList.Add(other.gameObject);
        }
    }

    /// <summary>
    /// オブジェクトが離れた時呼ばれる
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Item"))
        {
            isItemListUpdate = true;
            ItemList.Remove(other.gameObject);
        }
    }

    /// <summary>
    /// FixedUpdateの実行タイミングで一番最後に実行される関数
    /// </summary>
    /// <returns></returns>
    private IEnumerator LateFixedUpdate()
    {
        var waitForFixed = new WaitForFixedUpdate();
        while (true)
        {
            // 常に更新フラグが立っているか、アイテムリストが更新された時だけ要素の入れ替えを実行
            if (isAlwaysUpdate || isItemListUpdate)
            {
                PickUpNearItemFirst();
                isItemListUpdate = false;
            }
            yield return waitForFixed;
        }
    }

    /// <summary>
    /// 一番近場のアイテムを配列の先頭に持ってくる
    /// </summary>
    /// <returns></returns>
    private void PickUpNearItemFirst()
    {
        if (ItemList.Count <= 1) return;

        var originPos = originPoint.transform.position;
        // 初期最小値を設定
        var minDirection = Vector3.Distance(ItemList[0].transform.position, originPos);
        // 二つ目のアイテムから取得ポイントとの距離を計算
        for (int itemNum = 1; itemNum < ItemList.Count; itemNum++)
        {
            var direction = Vector3.Distance(ItemList[itemNum].transform.position, originPos);
            // より近いオブジェクトを0番目の要素に代入
            if (minDirection > direction)
            {
                minDirection = direction;
                var temp = ItemList[0];
                ItemList[0] = ItemList[itemNum];
                ItemList[itemNum] = temp;
            }
        }
    }

    /// <summary>
    /// 一番近いアイテムを返す
    /// </summary>
    /// <returns></returns>
    public GameObject GetNearItem()
    {
        if (ItemList.Count <= 0) return null;

        return ItemList[0];
    }

#if UNITY_EDITOR // UnityEditorのみ
    /// <summary>
    /// 拾う対象のアイテムにギズモを表示
    /// </summary>
    private void SetPickUpTargetItemMarker()
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