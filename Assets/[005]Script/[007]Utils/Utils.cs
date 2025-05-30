using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// ゲーム内で使用する共通関数をまとめたクラス
/// </summary>

public class Utils : MonoBehaviour
{

    /// <summary>
    /// 経過時間を0:00の形式で文字に変換を行う
    /// </summary>
    /// <param name="timer"></param>
    /// <returns></returns>
    public static string GetTextTimer(float timer)
    {
        int seconds = (int)timer % 60;
        int minutes = (int)timer / 60;
        return minutes.ToString() + ":" + seconds.ToString("00");
    }

   public static bool IsColliderTile(Tilemap tilemapCollider , Vector2 position)
    {
        //セル位置に変換
        Vector3Int cenllPosition = tilemapCollider.WorldToCell(position);

        //当たり判定がある
        if(tilemapCollider.GetTile(cenllPosition))
        {
            return true;
        }

        return false;
    }

}
