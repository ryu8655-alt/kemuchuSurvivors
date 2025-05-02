using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

//右クリックメニューからScriptableProjectを作成可能
[CreateAssetMenu(fileName = "CharacterSettings", menuName = "ScriptableProject/CharacterSettings")]
public class CharacterSettings : ScriptableObject
{
    //キャラクターデータ
    public List<CharacterStatus> _characterStatusList;

    static CharacterSettings _instance;

    public static CharacterSettings Instance
    {
        get
        {
            if (!_instance)
            {
                _instance = Resources.Load<CharacterSettings>(nameof(CharacterSettings));
            }
            return _instance;
        }
    }

    //リストのIDを参照しデータを検索する
    public CharacterStatus Get(int id)
    {
        return (CharacterStatus)_characterStatusList.Find(item => item.Id == id).GetCopy();
    }
}









//敵の動きの種類
public enum MoveType
{
    //プレイヤーに向かって進んでくる
    TargetPlayer,
    //一方向に進む
    TargetDirection,
    //動かないで一定距離近づいたPlayerに弾を飛ばす(後ほど実装予定)
}

[Serializable]
public class CharacterStatus : BaseCharacterStatus
{
    //キャラクターのプレハブ
    public GameObject _characterPrefab;
    //初期武器のID = Playerにて使用するパラメーター
    public List<int> _defaultWeaponIds;
    //装備可能な武器のID = Playerにて使用するパラメーター
    public List<int> _usableWeaponIds;
    //装備可能数 = Playerにて使用するパラメーター
    public int _usableWeaponMax;
    //移動のタイプ　= Enemyにて使用する
    public MoveType _moveType;

    //アイテムを追加する処理を後で追加する
}
