using System;
using UnityEngine;

/// <summary>
/// プレイヤー・敵キャラクターのステータスを管理するクラス
/// </summary>
/// 

//プレイヤーと敵キャラクターのステータス項目
public enum CharacterStatusType
{
    Attack,
    Defense,
    MoveSpeed,
    HP,
    MaxHP,
    XP,
    MaxXP,
    PickUpRange,
    AliveTime,
}


public class BaseCharacterStatus
{
    //Inspectorから設定するステータス
    //Inspectorで表示されるタイトル
    public string Title;

    //データID
    public int Id;
    //設定レベル
    public int Level;
    //名前
    public string Name;
    //説明文
    [TextArea] public string Description;
    //攻撃力
    public float Attack;
    //防御力
    public float Defense;
    //HP
    public float HP;
    //最大HP
    public float MaxHP;
    //経験値
    public float XP;
    //最大経験値
    public float MaxXP;
    //移動速度
    public float MoveSpeed;
    //取得範囲
    public float PickUpRange;
    //フィールドにいる時間(数秒経過すると自動消滅する敵用)
    public float AliveTime;

    //CaharacterStatusTypeとの紐づけを行う
    //紐づけにはインデクサを使用する
    public float this[CharacterStatusType type]
    {
        //get
        //{
        //    if(type == CharacterStatusType.Attack) return Attack;
        //    else if (type == CharacterStatusType.Defense) return Defense;
        //    else if (type == CharacterStatusType.MoveSpeed) return MoveSpeed;
        //    else if (type == CharacterStatusType.HP) return HP;
        //    else if (type == CharacterStatusType.MaxHP) return MaxHP;
        //    else if (type == CharacterStatusType.XP) return XP;
        //    else if (type == CharacterStatusType.MaxXP) return MaxXP;
        //    else if (type == CharacterStatusType.PickUpRange) return PickUpRange;
        //    else if (type == CharacterStatusType.AliveTime) return AliveTime;
        //    else return 0;
        //}

        get => type switch
        {
            CharacterStatusType.Attack => Attack,
            CharacterStatusType.Defense => Defense,
            CharacterStatusType.MoveSpeed => MoveSpeed,
            CharacterStatusType.HP => HP,
            CharacterStatusType.MaxHP => MaxHP,
            CharacterStatusType.XP => XP,
            CharacterStatusType.MaxXP => MaxXP,
            CharacterStatusType.PickUpRange => PickUpRange,
            CharacterStatusType.AliveTime => AliveTime,
            _ => 0,
        };

        //set 
        //{
        //    if(type == CharacterStatusType.Attack) Attack = value;
        //    else if (type == CharacterStatusType.Defense) Defense = value;
        //    else if (type == CharacterStatusType.MoveSpeed) MoveSpeed = value;
        //    else if (type == CharacterStatusType.HP) HP = value;
        //    else if (type == CharacterStatusType.MaxHP) MaxHP = value;
        //    else if (type == CharacterStatusType.XP) XP = value;
        //    else if (type == CharacterStatusType.MaxXP) MaxXP = value;
        //    else if (type == CharacterStatusType.PickUpRange) PickUpRange = value;
        //    else if (type == CharacterStatusType.AliveTime) AliveTime = value;
        //}

        set
        {
            switch(type)
            {
                case CharacterStatusType.Attack:
                    Attack = value;
                    break;
                case CharacterStatusType.Defense:
                    Defense = value;
                    break;
                case CharacterStatusType.MoveSpeed:
                    MoveSpeed = value;
                    break;
                case CharacterStatusType.HP:
                    HP = value;
                    break;
                case CharacterStatusType.MaxHP:
                    MaxHP = value;
                    break;
                case CharacterStatusType.XP:
                    XP = value;
                    break;
                case CharacterStatusType.MaxXP:
                    MaxXP = value;
                    break;
                case CharacterStatusType.PickUpRange:
                    PickUpRange = value;
                    break;
                case CharacterStatusType.AliveTime:
                    AliveTime = value;
                    break;
            }
        }
    }

    //コピーしたデータを返す
    public BaseCharacterStatus GetCopy()
    {
        return (BaseCharacterStatus)MemberwiseClone();
    }

}

