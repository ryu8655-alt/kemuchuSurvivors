using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TreeEditor;
using UnityEngine;

public class ShieldController : BaseWeapon
{

    //プレイヤーからの距離
    const float DISTANCE = 1.0f;
    //現在の角度
    public float _angle;

    // Start is called before the first frame update
    void Start()
    {
        //出現時のアニメーション
        transform.localScale = new Vector3(0, 0, 0);
        transform.DOScale(new Vector3(1,1,1),1.5f).SetEase(Ease.OutBounce);


    }

    // Update is called once per frame
    void Update()
    {
        _angle  -=_weaponStatus.Speed * Time.deltaTime;

        //Cos関数にラジアン角を指定し、X座標を取得、DISTANCEを乗算してワールド座標に変換する
        float x = Mathf.Cos(_angle * Mathf.Deg2Rad) * DISTANCE;
        //Sin関数にラジアン角を指定し、Y座標を取得、DISTANCEを乗算してワールド座標に変換する
        float y = Mathf.Sin(_angle * Mathf.Deg2Rad) * DISTANCE;

        //ポジション更新
        transform.position = transform.root.position + new Vector3(x, y, 0);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //敵以外は無視する
        if(!collision.gameObject.TryGetComponent<EnemyController>(out var enemy)) return;

        //反対方向へ敵を跳ね返す・押し出す
        Vector3 foward = enemy.transform.position - transform.position;
        enemy.GetComponent<Rigidbody2D>().AddForce(foward.normalized * 5);

        //敵へのダメージ処理
        AttackEnemy(collision);
    }
}
