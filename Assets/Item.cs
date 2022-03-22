using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CLASS
{
    Warrior,            // ����.
    Fighter,            // ������.
    Hunter,             // ����.
    Wizard,             // ������.
    Assassin,           // �ϻ���.
    Specialist          // ����ȸ���Ʈ.
}
public enum PART
{
    Helmet,             // �Ӹ�.
    Shoulder,           // ���.
    Top,                // ����.
    Pants,              // ����.
    Gloves,             // �尩.
    Weapon,             // ����.
}

public enum GRADE
{
    Normal,             // �Ϲ�.
    Rare,               // ���.
    Hero,               // ����.
    Legend,             // ����.
    Relic,              // ����.
    Ancient             // ���.
}

[System.Serializable]
public class Item
{
    public int id;          // ���̵�.
    public string name;     // �̸�.
    public GRADE grade;     // ���.
    public PART part;       // ����.
    public CLASS job;       // ����.
    public int level;       // ������ ����.
    public int tier;        // Ƽ��.

    public Item(TextData textData, int index)
    {
        id = int.Parse(textData.GetValue(index, "ID"));
        name = textData.GetValue(index, "Name");
        grade = (GRADE)System.Enum.Parse(typeof(GRADE), textData.GetValue(index, "Grade"));
        part = (PART)System.Enum.Parse(typeof(PART), textData.GetValue(index, "Part"));
        job = (CLASS)System.Enum.Parse(typeof(CLASS), textData.GetValue(index, "Job"));
        level = int.Parse(textData.GetValue(index, "Level"));
        tier = int.Parse(textData.GetValue(index, "Tier"));
    }
}
