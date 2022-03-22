using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleSheetsToUnity;
using System.Linq;

public class Sheet : MonoBehaviour
{
    [SerializeField] string sheetId;
    [SerializeField] string sheetName;

    [Header("아이템")]
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
            // key에 해당하는 행 데이터.
            IEnumerable<string> rowKeys = from cell in sheet.rows[1]
                                          select cell.value;
            // value에 해당하는 행 데이터.
            IEnumerable<string> rowValues = from cell in sheet.rows[i + 2]
                                            select cell.value;

            // 검색 데이터를 배열로 변경.
            string[] keys = rowKeys.ToArray();
            string[] datas = rowValues.ToArray();

            // 딕셔너리 생성.
            Dictionary<string, string> dic = new Dictionary<string, string>();
            for (int j = 0; j < keys.Length; j++)
                dic.Add(keys[j].Trim(), datas[j].Trim());

            // 결과에 대입.
            result[i] = dic;
        }

        // 최종 결과물.
        TextData textData = new TextData(result);
        items = new Item[textData.Count];
        for(int i = 0; i < textData.Count; i++)
            items[i] = new Item(textData, i);
    }
}
