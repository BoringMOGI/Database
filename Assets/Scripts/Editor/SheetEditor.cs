using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Sheet))]
public class SheetEditor : Editor
{
    SerializedProperty sheetID;
    SerializedProperty sheetName;
    Sheet sheet;

    void OnEnable()
    {
        sheet = (Sheet)target;

        sheetID = serializedObject.FindProperty("sheetId");
        sheetName = serializedObject.FindProperty("sheetName");

        // 다운로드 완료 콜백 등록.
        sheet.OnCompleteDownload += OnCompleteDownload;
    }
    private void OnDisable()
    {
        // 다운로드 완료 콜백 제거.
        sheet.OnCompleteDownload -= OnCompleteDownload;
    }

    public override void OnInspectorGUI()
    {
        //base.OnInspectorGUI();

        // 접기 기능.
        sheetID.isExpanded = EditorGUILayout.Foldout(sheetID.isExpanded, "주소 데이터");
        if(sheetID.isExpanded)
        {
            sheetID.stringValue = EditorGUILayout.PasswordField(sheetID.stringValue);
            sheetName.stringValue = EditorGUILayout.TextField(sheetName.stringValue);

            if (GUILayout.Button("Downlaod"))
            {
                if (EditorUtility.DisplayDialog("데이터 다운로드", "데이터를 시트에서 다운로드하시겠습니까?", "예", "아니오"))
                    sheet.DownloadData();
            }
        }

        // 띄워쓰기.
        EditorGUILayout.Space();
        

        EditorGUILayout.Space();

        DrawTitle();
        DrawLine();
        DrawValues();
    }
    private void DrawTitle()
    {
        EditorGUILayout.BeginHorizontal();

        GUIStyle labelStyle = new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter };

        EditorGUILayout.LabelField("ID", labelStyle, GUILayout.Width(30));
        EditorGUILayout.LabelField("이름", labelStyle, GUILayout.MinWidth(60), GUILayout.MaxWidth(150));
        EditorGUILayout.LabelField("등급", labelStyle, GUILayout.Width(50));
        EditorGUILayout.LabelField("부위", labelStyle, GUILayout.Width(50));
        EditorGUILayout.LabelField("직업", labelStyle, GUILayout.Width(60));
        EditorGUILayout.LabelField("레벨", labelStyle, GUILayout.Width(50));
        EditorGUILayout.LabelField("티어", labelStyle, GUILayout.Width(40));

        EditorGUILayout.EndHorizontal();
    }
    private void DrawValues()
    {
        GUIStyle labelStyle = new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter };

        foreach (Item item in sheet.items)
        {
            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.LabelField(item.id.ToString(),      labelStyle, GUILayout.Width(30));
            EditorGUILayout.LabelField(item.name,               GUILayout.MinWidth(60), GUILayout.MaxWidth(150));
            EditorGUILayout.LabelField(item.grade.ToString(),   labelStyle, GUILayout.Width(50));
            EditorGUILayout.LabelField(item.part.ToString(),    labelStyle, GUILayout.Width(50));
            EditorGUILayout.LabelField(item.job.ToString(),     labelStyle, GUILayout.Width(60));
            EditorGUILayout.LabelField(item.level.ToString(),   labelStyle, GUILayout.Width(50));
            EditorGUILayout.LabelField(string.Format("Tier{0}", item.tier), labelStyle, GUILayout.Width(40));

            EditorGUILayout.EndHorizontal();
        }
    }    
    private void DrawLine(float height = 1.0f)
    {
        Rect rect = EditorGUILayout.GetControlRect(false, height);
        EditorGUI.DrawRect(rect, new Color(0.5f, 0.5f, 0.5f, 1));
    }

    private void OnCompleteDownload()
    {
        EditorUtility.DisplayDialog("Complete", "데이터 다운로드를 성공했습니다.", "확인");
        EditorUtility.SetDirty(sheet);
    }
}
