using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleSheetsToUnity;
using System.Linq;

public class Sheet : MonoBehaviour
{
    [SerializeField] string sheetId;
    [SerializeField] string sheetName;

    [Header("������")]
    [SerializeField] Item[] items;

    [ContextMenu("Download")]
    public void DownloadData()
    {
        SpreadsheetManager.Read(new GSTU_Search(sheetId, sheetName), Callback);
    }

    private void Callback(GstuSpreadSheet sheet)
    {
        Dictionary<string, string>[] result = new Dictionary<string, string>[sheet.rows.Count];

        for (int i = 0; i <= sheet.rows.Count; i++)
        {
            // key�� �ش��ϴ� �� ������.
            IEnumerable<string> rowKeys = from cell in sheet.rows[1]
                                          select cell.value;
            // value�� �ش��ϴ� �� ������.
            IEnumerable<string> rowValues = from cell in sheet.rows[i + 2]
                                            select cell.value;

            // �˻� �����͸� �迭�� ����.
            string[] keys = rowKeys.ToArray();
            string[] datas = rowValues.ToArray();

            // ��ųʸ� ����.
            Dictionary<string, string> dic = new Dictionary<string, string>();
            for (int j = 0; j < keys.Length; j++)
                dic.Add(keys[j].Trim(), datas[j].Trim());

            // ����� ����.
            result[i] = dic;
        }

        // ���� �����.
        TextData textData = new TextData(result);
        items = new Item[textData.Count];
        for(int i = 0; i < textData.Count; i++)
            items[i] = new Item(textData, i);
    }
}
