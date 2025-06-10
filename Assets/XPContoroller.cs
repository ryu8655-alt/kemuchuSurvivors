using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XPContoroller : MonoBehaviour
{
    GameSceneManager _gameSceneManager;
    Rigidbody2D _rifgidbody2d;
    SpriteRenderer _spriteRenderer;

    //経験値量
    private float _xp;

    //経験値アイテムがマップ上に存在する時間
    float _aliveTimer = 50.0f;
    //経験値アイテムが消滅にかかる時間
    float fadeTime = 10.0f;

    //変数初期化処理
    public void Init(GameSceneManager sceneManager , float xp)
    {
        this._gameSceneManager = sceneManager;
        this._xp = xp;

        _rifgidbody2d = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }


    // Update is called once per frame
    void Update()
    {

        //ゲームが停止している場合は処理を抜ける
        if (!_gameSceneManager.enabled) return;

        //タイマー消化処理
        _aliveTimer -= Time.deltaTime;

        if( 0 > _aliveTimer) 
        {
            //アルファ値を設定
            Color color = _spriteRenderer.color;
            color.a -= 1.0f / fadeTime * Time.deltaTime;
            _spriteRenderer.color = color;

            //アルファ値が0以下になったら削除
            if(0 >= color.a)
            {
                Destroy(gameObject);
                return;
            }

        }

        //プレイヤーとの距離を参照する
        float distance = Vector2.Distance(transform.position,_gameSceneManager._playerController.transform.position);
        //プレイヤーの取得範囲内ならプレイヤーに吸い込まれる
        if(distance < _gameSceneManager._playerController._characterStatus.PickUpRange)
        {
            //少し早く動く
            float speed = _gameSceneManager._playerController._characterStatus.MoveSpeed * 1.1f;
            Vector2 foward = _gameSceneManager._playerController.transform.position - transform.position;
            _rifgidbody2d.position += foward.normalized * speed * Time.deltaTime;
        }

    }



    //衝突時処理
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //プレイヤー以外衝突した時は処理を抜ける
        if (!collision.gameObject.TryGetComponent<PlayerController>(out var player)) return;

        //経験値を取得する
        //プレイヤーに経験値取得処理を追加する
        player.GetXP(_xp);
        Destroy(gameObject);
    }
}
