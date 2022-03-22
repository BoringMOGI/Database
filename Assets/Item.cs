using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CLASS
{
    Warrior,            // 전사.
    Fighter,            // 무도가.
    Hunter,             // 헌터.
    Wizard,             // 마법사.
    Assassin,           // 암살자.
    Specialist          // 스페셜리스트.
}
public enum PART
{
    Helmet,             // 머리.
    Shoulder,           // 어깨.
    Top,                // 상의.
    Pants,              // 하의.
    Gloves,             // 장갑.
    Weapon,             // 무기.
}

public enum GRADE
{
    Normal,             // 일반.
    Rare,               // 희귀.
    Hero,               // 영웅.
    Legend,             // 전설.
    Relic,              // 유물.
    Ancient             // 고대.
}

[System.Serializable]
public class Item
{
    public int id;          // 아이디.
    public string name;     // 이름.
    public GRADE grade;     // 등급.
    public PART part;       // 부위.
    public CLASS job;       // 직업.
    public int level;       // 아이템 레벨.
    public int tier;        // 티어.

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
