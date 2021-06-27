using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class ItemSerch : MonoBehaviour
{
    [SerializeField, Header("�A�C�e�����擾���錴�_(��������ɋ߂��A�C�e���A�����A�C�e�������܂�)")] private GameObject originPoint;

    public List<GameObject> ItemList { get; private set; } = new List<GameObject>();

    [Header("�A�C�e�����X�g����ɍX�V")]
    public bool isAlwaysUpdate = true;
    // �A�C�e�����X�g���X�V���ꂽ��
    private bool isItemListUpdate;

    private void Start()
    {
        // �A�C�e���폜�֐������s�J�n
        StartCoroutine(LateFixedUpdate());
    }

    /// <summary>
    /// �I�u�W�F�N�g�ƐڐG�������Ă΂��
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Item"))
        {
            isItemListUpdate = true;
            var item = other.GetComponent<Item>();
            // �A�C�e�����E�������A�C�e�����X�g���珜�O����C�x���g��o�^
            item.onPickUp.AddListener(() => ItemList.Remove(other.gameObject));
            item.onPickUp.AddListener(() => isItemListUpdate = true);
            ItemList.Add(other.gameObject);
        }
    }

    /// <summary>
    /// �I�u�W�F�N�g�����ꂽ���Ă΂��
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
    /// FixedUpdate�̎��s�^�C�~���O�ň�ԍŌ�Ɏ��s�����֐�
    /// </summary>
    /// <returns></returns>
    private IEnumerator LateFixedUpdate()
    {
        var waitForFixed = new WaitForFixedUpdate();
        while (true)
        {
            // ��ɍX�V�t���O�������Ă��邩�A�A�C�e�����X�g���X�V���ꂽ�������v�f�̓���ւ������s
            if (isAlwaysUpdate || isItemListUpdate)
            {
                PickUpNearItemFirst();
                isItemListUpdate = false;
            }
            yield return waitForFixed;
        }
    }

    /// <summary>
    /// ��ԋߏ�̃A�C�e����z��̐擪�Ɏ����Ă���
    /// </summary>
    /// <returns></returns>
    private void PickUpNearItemFirst()
    {
        if (ItemList.Count <= 1) return;

        var originPos = originPoint.transform.position;
        // �����ŏ��l��ݒ�
        var minDirection = Vector3.Distance(ItemList[0].transform.position, originPos);
        // ��ڂ̃A�C�e������擾�|�C���g�Ƃ̋������v�Z
        for (int itemNum = 1; itemNum < ItemList.Count; itemNum++)
        {
            var direction = Vector3.Distance(ItemList[itemNum].transform.position, originPos);
            // ���߂��I�u�W�F�N�g��0�Ԗڂ̗v�f�ɑ��
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
    /// ��ԋ߂��A�C�e����Ԃ�
    /// </summary>
    /// <returns></returns>
    public GameObject GetNearItem()
    {
        if (ItemList.Count <= 0) return null;

        return ItemList[0];
    }

#if UNITY_EDITOR // UnityEditor�̂�
    /// <summary>
    /// �E���Ώۂ̃A�C�e���ɃM�Y����\��
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