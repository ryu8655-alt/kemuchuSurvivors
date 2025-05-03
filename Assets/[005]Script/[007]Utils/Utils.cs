using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// �Q�[�����Ŏg�p���鋤�ʊ֐����܂Ƃ߂��N���X
/// </summary>

public class Utils : MonoBehaviour
{
    public static string GetTextTimer(float timer)
    {
        int seconds = (int)timer % 60;
        int minutes = (int)timer / 60;
        return minutes.ToString() + ":" + seconds.ToString("00");
    }

   public static bool IsColliderTile(Tilemap tilemapCollider , Vector2 position)
    {
        //�Z���ʒu�ɕϊ�
        Vector3Int cenllPosition = tilemapCollider.WorldToCell(position);

        //�����蔻�肪����
        if(tilemapCollider.GetTile(cenllPosition))
        {
            return true;
        }

        return false;
    }

}
