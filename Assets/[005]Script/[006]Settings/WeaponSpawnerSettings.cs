using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEngine;

// 右クリックメニューに表示する、filenameはデフォルトのファイル名
[CreateAssetMenu(fileName = "WeaponSpawnerSettings", menuName = "ScriptableObjects/WeaponSpawnerSettings")]


public class WeaponSpawnerSettings : ScriptableObject
{
    //データ
    public List<BaseWeaponStatus> baseWeaponStatuseList;

    static WeaponSpawnerSettings instance;
    public static WeaponSpawnerSettings Instance
    {
        get
        {
            if (!instance)
            {
                instance = Resources.Load<WeaponSpawnerSettings>(nameof (WeaponSpawnerSettings));
            }

            return instance;
        }
    }

    //リストのIDからデータを検索する
    public BaseWeaponStatus Get(int id , int lv)
    {
        //指定されたレベルのデータが中江ラバ1番高いレベルのデータを返す
        BaseWeaponStatus ret = null;

        //指定レベルと一致するものを取得する
        foreach(var item in baseWeaponStatuseList)
        {

            if (id != item.Id) continue;

            //指定レベルと一致した場合
            if(lv == item.Level)
            {
                return (BaseWeaponStatus)item.GetCopy();
            }

            //仮のデータがセットされていないか、それを超えるレベルがあったら入れ替える
            if(null == ret)
            {
                ret = item;
            }

            //探しているレベルよりも下で、かつ暫定データよりもおおきい場合
            else if (item.Level < lv && ret.Level < item.Level)
            {
                ret = item;
            }
        }

        return (BaseWeaponStatus)ret.GetCopy();
    }


    //作成
    public BaseWeaponSpawner CreateWeaponSpawner(int id,EnemySpawnerController enemySpawnerController,Transform parent = null)
    {
        //データを取得
        BaseWeaponStatus weaponStatus = Get(id, 1);
        //オブジェクトの作成
        GameObject obj = Instantiate(weaponStatus.Prefab, parent);

        //データセット
        BaseWeaponSpawner weaponSpawner = obj.GetComponent<BaseWeaponSpawner>();
        weaponSpawner.Init(enemySpawnerController,weaponStatus);

        return weaponSpawner;
    }


}
