using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextData
{
    Dictionary<string, string>[] datas;
    public int Count => datas.Length;

    public TextData(Dictionary<string, string>[] datas)
    {
        this.datas = datas;
    }

    public string GetValue(int index, string key)
    {
        // �迭�� ������ ����� (out of range)
        if (datas.Length <= index || index < 0)
            return string.Empty;

        // key���� ������.
        if (!datas[index].ContainsKey(key))
            return string.Empty;

        return datas[index][key];
    }
}


public class Importer : MonoBehaviour
{
    public static TextData ReadData(string path)
    {        
        TextAsset asset = Resources.Load(path) as TextAsset;    // Resourcse �������� Text�����͸� �д´�.
        string[] line = asset.text.Split('\n');                 // '\n' ���ڸ� �������� �ڸ���.

        Dictionary<string, string>[] result = new Dictionary<string, string>[line.Length - 1];
        const char separator = ',';

        for(int i = 1; i<line.Length; i++)
        {
            // line �����͸� separator�� �������� �ڸ���.
            string[] keys = line[0].Split(separator);       // Ű.
            string[] datas = line[i].Split(separator);      // ��.
            
            // Ű,���� �� ������ Dictionary�� �߰�.
            // Ű,���� Trim���� ���� ������ �����Ѵ�.
            Dictionary<string, string> dic = new Dictionary<string, string>();
            for(int j = 0; j<keys.Length; j++)
                dic.Add(keys[j].Trim(), datas[j].Trim());

            // �ش� Dictionary�� result�� �߰�.
            result[i - 1] = dic;
        }

        return new TextData(result);
    }
}
