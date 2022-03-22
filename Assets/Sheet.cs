using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleSheetsToUnity;

public class Sheet : MonoBehaviour
{
    [SerializeField] string sheetId;
    [SerializeField] string sheetName;

    [ContextMenu("Download")]
    public void DownloadData()
    {
        SpreadsheetManager.Read(new GSTU_Search(sheetId, sheetName), Callback);
    }

    private void Callback(GstuSpreadSheet sheet)
    {
        //Dictionary<int, List<GSTU_Cell>> _sheet = new Dictionary<int, List<GSTU_Cell>>();
        List<GSTU_Cell> rowData = sheet.rows[1];
        Debug.Log(rowData.Count);
    }
}
