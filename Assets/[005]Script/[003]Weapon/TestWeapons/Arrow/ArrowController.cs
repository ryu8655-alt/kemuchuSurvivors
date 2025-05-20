using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 矢についてのクラス
/// 動きを制御する
/// --主な挙動--
/// プレイヤーのいる位置よりランダム敵に向かって矢を放つ
/// 放つ間隔、一度に発射する本数、矢のスピードなどのパラメータは
/// ScriptableObject WeaponSpawnerSettingsで設定する
/// /// </summary>



public class ArrowController : BaseWeapon
{

    //矢を発射するターゲット
    public EnemyController _targetEnemy;

    // Start is called before the first frame update
    void Start()
    {
        //進行方向ベクトル
        Vector2 forward = _targetEnemy.transform.position - transform.position;
        //角度に変換する
        float angle = Mathf.Atan2(forward.y, forward.x) * Mathf.Rad2Deg;
        //角度を代入する
        transform.rotation = Quaternion.Euler(0, 0, angle);

    }

    // Update is called once per frame
    void Update()
    {
        //ターゲットが見つからない場合は自身を削除する
        if(!_targetEnemy)
        {
            Destroy(gameObject);
            return;
        }

        //ターゲットに向かって飛んでいく
        Vector2 forward = _targetEnemy.transform.position - transform.position;
        _rigidbody2d.position += forward.normalized * _weaponStatus.Speed * Time.deltaTime;

    }



    //衝突した場合の処理
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //敵以外にあたった場合には処理を抜ける
        if(!collision.gameObject.TryGetComponent<EnemyController>(out var enemy)) return;

        //通常のダメージを取得する
        float attackPower = _weaponStatus.Attack;
        //あたった対象がターゲットだった場合はターゲット対象を空にする
        if(_targetEnemy == enemy)
        {
            _targetEnemy = null;
        }
        else//ターゲット以外にあたった場合は30%のダメージを与える
        {
            attackPower /= 3;
        }
        
        AttackEnemy(collision, attackPower);
    }
}
