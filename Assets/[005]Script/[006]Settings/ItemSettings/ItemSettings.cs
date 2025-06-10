using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//右クリックメニューにて表示する,filename
[CreateAssetMenu(fileName = "ItemSettings", menuName = "ScriptableObjects/ItemSettings")]
public class ItemSettings : ScriptableObject
{

    //アイテムデータ
    [SerializeField,Header("アイテムデータ")]
    public List<ItemData> _datas;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

////アイテムデータ
[System.Serializable]
public class ItemData
{
    public string Title;//リスト項目のタイトル =　インスペクター上で表示する値名のような物

    //アイテムID
    public int Id;
    //アイテム名
    public string Name;
    //アイテム説明
    [TextArea] public string Description;
    //アイテムアイコン
    public Sprite Icon;
    //ボーナス
    public List<BonusStatus> BonusStatusesType;
}

public enum BonusType
{
    //定数的な数値上昇
    Bonus,
    //割合的な数値上昇
    Boost,
}



[Serializable]
public class BonusStatus
{
    // 追加タイプ
    public BonusType Type;
    // 追加するプロパティ
    public CharacterStatusType Key;
    // 追加する値
    public float Value;
}
