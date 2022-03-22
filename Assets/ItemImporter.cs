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
        TextData textData = Importer.ReadData(fileName);    // fileName�� ������ �����͸� �д´�.
        items = new Item[textData.Count];                   // �ش� �������� ������ŭ ������ �迭 ����.

        // �� �迭�� Item��ü ����.
        for (int i = 0; i < items.Length; i++)
            items[i] = new Item(textData, i);
    }

}

