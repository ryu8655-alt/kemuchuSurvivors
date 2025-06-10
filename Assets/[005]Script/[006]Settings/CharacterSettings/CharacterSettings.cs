using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ゲームに登場する全キャラクターのステータス情報を管理するscriptableObject
/// IDを指定することで、キャラクターのステータスを取得する
/// </summary>
[CreateAssetMenu(fileName = "CharacterSettings", menuName = "ScriptableProject/CharacterSettings")]
public class CharacterSettings : ScriptableObject
{
    [Tooltip("キャラクターのステータスデータリスト")]
    public List<CharacterStatus> _characterStatusList;

    private static CharacterSettings _instance;

    public static CharacterSettings Instance
    {
        get
        {
            if (!_instance)
            {
                _instance = Resources.Load<CharacterSettings>(nameof(CharacterSettings));
                if(_instance == null)
                {
                    Debug.LogError("[CharacterSettings] Resourcesフォルダ内にCharacterSettings.assetが見つかりません");
                }
            }
            return _instance;
        }
    }

    /// <summary>
    /// 指定した ID に一致するキャラクターステータスをコピーして返す
    /// </summary>
    /// <param name="id">キャラクターID</param>
    /// <returns>該当する CharacterStatus のコピー。なければ null。</returns>aram>
    /// <returns></returns>
    public CharacterStatus Get(int id)
    {
        var foundStatus = _characterStatusList.Find(item => item.Id == id);
        if(foundStatus == null)
        {
            Debug.LogError($"[CharacterSettings] ID {id} のキャラクターが見つかりません");
            return null;
        }

        return (CharacterStatus)foundStatus.GetCopy();


        // return (CharacterStatus)_characterStatusList.Find(item => item.Id == id).GetCopy();
    }


    public EnemyController CretaeEnemy(int id , GameSceneManager gameSceneManager, Vector3 position)
    {
        //指定したIDのキャラクターステータスを取得する
        CharacterStatus characterStatus = Instance.Get(id);
        //対象のキャラクタープレハブを取得する
        GameObject obj = Instantiate(characterStatus._characterPrefab, position, Quaternion.identity);

        //データセット
        EnemyController　enemyController = obj.GetComponent<EnemyController>();
        enemyController.Init(gameSceneManager, characterStatus);

        return enemyController;
    }

    public PlayerController CreatePlayer(int id ,GameSceneManager gameSceneManager,EnemySpawnerController enemySpawner,
        TextMeshProUGUI textLV , Slider sliderXP)
    {
        //ID指定されたキャラクターステータスの取得を行う
        CharacterStatus status =  Instance.Get(id);
        //オブジェクト生成
        GameObject obj = Instantiate(status._characterPrefab, Vector3.zero, Quaternion.identity);


        //データセット
        PlayerController ctrl = obj.GetComponent<PlayerController>();
        ctrl.Init(gameSceneManager,enemySpawner,status,textLV,sliderXP);

        return ctrl;
    }       




}









/// <summary>
/// 敵の移動タイプを定義する列挙型
/// </summary>
public enum MoveType
{
    //プレイヤー用
    Player,
    //プレイヤーに向かって進んでくる
    TargetPlayer,
    //一方向に進む
    TargetDirection,
    //動かないで一定距離近づいたPlayerに弾を飛ばす(後ほど実装予定)
    ShootOnSight

}

/// <summary>
/// プレイヤー・敵共通で使うキャラクターステータスデータ構造。
/// ScriptableObject 内で使用される。
/// </summary>
[Serializable]
public class CharacterStatus : BaseCharacterStatus
{
    [Header("キャラクタープレハブ")]
    public GameObject _characterPrefab;

    [Header("プレイヤー専門パラメーター")]
    //初期武器のID = Playerにて使用するパラメーター
    public List<int> _defaultWeaponIds;
    //装備可能な武器のID = Playerにて使用するパラメーター
    public List<int> _usableWeaponIds;
    //装備可能数 = Playerにて使用するパラメーター
    public int _usableWeaponMax;

    [Header("敵専門パラメーター")]
    //移動のタイプ　= Enemyにて使用する
    public MoveType _moveType;

    //アイテムを追加する処理を後で追加する
}

