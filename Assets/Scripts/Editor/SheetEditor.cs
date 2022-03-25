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

        // �ٿ�ε� �Ϸ� �ݹ� ���.
        sheet.OnCompleteDownload += OnCompleteDownload;
    }
    private void OnDisable()
    {
        // �ٿ�ε� �Ϸ� �ݹ� ����.
        sheet.OnCompleteDownload -= OnCompleteDownload;
    }

    public override void OnInspectorGUI()
    {
        //base.OnInspectorGUI();

        // ���� ���.
        sheetID.isExpanded = EditorGUILayout.Foldout(sheetID.isExpanded, "�ּ� ������");
        if(sheetID.isExpanded)
        {
            sheetID.stringValue = EditorGUILayout.PasswordField(sheetID.stringValue);
            sheetName.stringValue = EditorGUILayout.TextField(sheetName.stringValue);

            if (GUILayout.Button("Downlaod"))
            {
                if (EditorUtility.DisplayDialog("������ �ٿ�ε�", "�����͸� ��Ʈ���� �ٿ�ε��Ͻðڽ��ϱ�?", "��", "�ƴϿ�"))
                    sheet.DownloadData();
            }
        }

        // �������.
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
        EditorGUILayout.LabelField("�̸�", labelStyle, GUILayout.MinWidth(60), GUILayout.MaxWidth(150));
        EditorGUILayout.LabelField("���", labelStyle, GUILayout.Width(50));
        EditorGUILayout.LabelField("����", labelStyle, GUILayout.Width(50));
        EditorGUILayout.LabelField("����", labelStyle, GUILayout.Width(60));
        EditorGUILayout.LabelField("����", labelStyle, GUILayout.Width(50));
        EditorGUILayout.LabelField("Ƽ��", labelStyle, GUILayout.Width(40));

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
        EditorUtility.DisplayDialog("Complete", "������ �ٿ�ε带 �����߽��ϴ�.", "Ȯ��");
        EditorUtility.SetDirty(sheet);
    }
}
