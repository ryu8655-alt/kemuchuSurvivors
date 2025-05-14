using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 成長ボーナスの加算方法
/// </summary>
public enum BonusApplyType
{
    Flat,//固定値加算
    Rate//割合変化
}

[Serializable]
public class WeaponGrowthBonus
{
    [Header("成長ボーナス対象ステータス")]
    //成長ボーナスを適用するステータス
    public WeaponStatusType Type;

    [Header("成長ボーナスの加算方法")]
    //成長ボーナスの加算方法
    public BonusApplyType ApplyType;

    [Header("成長ボーナスの値")]
    //成長ボーナスの値
    public float Value;
}

[Serializable]
public class WeaponGrowthStep
{
    [Header("適応されるレベル")]
    public int Level;

    [Header("このレベルで加算されるボーナス一覧")]
    //このレベルで加算されるボーナス一覧
    public List<WeaponGrowthBonus> Bonuses;
}
