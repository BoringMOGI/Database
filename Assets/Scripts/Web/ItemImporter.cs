using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemImporter : MonoBehaviour
{
    [SerializeField] string fileName;
    [SerializeField] Item[] items;

    private void Start()
    {
        ImportItemData();
    }

    [ContextMenu("Import Item Data")]
    private void ImportItemData()
    {
        TextData textData = Importer.ReadData(fileName);    // fileName을 가지는 데이터를 읽는다.
        items = new Item[textData.Count];                   // 해당 데이터의 개수만큼 아이템 배열 생성.

        // 각 배열에 Item객체 생성.
        for (int i = 0; i < items.Length; i++)
            items[i] = new Item(textData, i);
    }

}

