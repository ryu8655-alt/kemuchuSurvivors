using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 武器専用の基本ステータスクラス
/// </summary>
/// 

//武器ステータス項目
public enum WeaponStatusType
{
    Attack,
    PenetrationCount,
    Speed,
    AliveTime,
    SpawnCount,
    SpawnIntervalMax,
    SpawnIntervalMin,
}

[System.Serializable]
public class BaseWeaponStatus
{
    //Inspectorから設定するステータス
    [Header("基本情報")]
    //Inspectorで表示されるタイトル
    public string Title;

    //武器データID
    public int Id;
    //設定レベル
    public int Level;
    //名前
    public string Name;
    //説明文
    [TextArea] public string Description;

    [Header("戦闘ステータス")]
    //攻撃力
    public float Attack;
    //貫通数
    public float PenetrationCount;
    //弾の移動速度
    public float Speed;
    //フィールド上にいる時間
    public float AliveTime;

    [Header("外観")]
    //武器スポナーのプレハブ
    public GameObject PrefabSpawner;
    //武器アイコン
    public Sprite Icon;

    [Header("スポーン設定")]
    //一度にスポーンされる数
    public int SpawnCount;
    //スポーン間隔の最大値
    public float SpawnIntervalMax;
    //スポーン間隔の最小値
    public float SpawnIntervalMin;

    [Header("レベルアップによる成長定義")]
    //レベルアップによる成長定義
    public List<WeaponGrowthStep> GrowSteps;

    //WeaponStatusTypetとの紐づけを行う
    //紐づけにはインデクサを使用する
    public float this[WeaponStatusType type]
    {
        get => type switch
        {
            WeaponStatusType.Attack => Attack,
            WeaponStatusType.PenetrationCount => PenetrationCount,
            WeaponStatusType.Speed => Speed,
            WeaponStatusType.AliveTime => AliveTime,
            WeaponStatusType.SpawnCount => SpawnCount,
            WeaponStatusType.SpawnIntervalMax => SpawnIntervalMax,
            WeaponStatusType.SpawnIntervalMin => SpawnIntervalMin,
            _ => 0,
        };

        set
        {
            switch (type)
            {
                case WeaponStatusType.Attack:
                    Attack = value;
                    break;
                case WeaponStatusType.PenetrationCount:
                    PenetrationCount = value;
                    break;
                case WeaponStatusType.Speed:
                    Speed = value;
                    break;
                case WeaponStatusType.AliveTime:
                    AliveTime = value;
                    break;
                case WeaponStatusType.SpawnCount:
                    SpawnCount = (int)value;
                    break;
                case WeaponStatusType.SpawnIntervalMax:
                    SpawnIntervalMax = value;
                    break;
                case WeaponStatusType.SpawnIntervalMin:
                    SpawnIntervalMin = value;
                    break;
            }
        }
    }

    public BaseWeaponStatus GetCopy()
    {
        return (BaseWeaponStatus)MemberwiseClone();
    }


    public void ApplyGrowthStep(int level)
    {
        foreach (var step in GrowSteps)
        {
            if (step.Level != level) continue;

            foreach (var bonus in step.Bonuses)
            {
                float original = this[bonus.Type];

                if (bonus.ApplyType == BonusApplyType.Flat)
                {
                    this[bonus.Type] = original + bonus.Value;
                }
                else if (bonus.ApplyType == BonusApplyType.Rate)
                {
                    this[bonus.Type] = original * (1 + bonus.Value);
                }
            }
        }
    }

}




