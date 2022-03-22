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
        // 배열의 범위를 벗어나면 (out of range)
        if (datas.Length <= index || index < 0)
            return string.Empty;

        // key값이 없으면.
        if (!datas[index].ContainsKey(key))
            return string.Empty;

        return datas[index][key];
    }
}


public class Importer : MonoBehaviour
{
    public static TextData ReadData(string path)
    {        
        TextAsset asset = Resources.Load(path) as TextAsset;    // Resourcse 폴더에서 Text데이터를 읽는다.
        string[] line = asset.text.Split('\n');                 // '\n' 문자를 기준으로 자른다.

        Dictionary<string, string>[] result = new Dictionary<string, string>[line.Length - 1];
        const char separator = ',';

        for(int i = 1; i<line.Length; i++)
        {
            // line 데이터를 separator를 기준으로 자른다.
            string[] keys = line[0].Split(separator);       // 키.
            string[] datas = line[i].Split(separator);      // 값.
            
            // 키,값을 한 상으로 Dictionary에 추가.
            // 키,값은 Trim으로 양쪽 공백을 제거한다.
            Dictionary<string, string> dic = new Dictionary<string, string>();
            for(int j = 0; j<keys.Length; j++)
                dic.Add(keys[j].Trim(), datas[j].Trim());

            // 해당 Dictionary를 result에 추가.
            result[i - 1] = dic;
        }

        return new TextData(result);
    }
}
