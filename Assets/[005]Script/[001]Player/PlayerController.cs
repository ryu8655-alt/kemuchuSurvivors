using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// プレイヤーの動き
/// 移動
/// ->上下左右の移動
///   ->WASDと矢印キー入力で移動
///   　->移動速度はインスペクター上からいじれるようにするけども、まぁPlayer/Enemy共通のデータを持たせて、そこから引っ張ってくるようにする予定なので暫定のものです
///   　
/// 攻撃
/// ->攻撃の処理についてはWeaponSpaenerを使って、WeaponのPrefabをInstantiateして、Weaponのスクリプトに攻撃の処理を持たせる予定
/// 　->時間ごとにスポーンカウンターを減らして行き、0になったら自動でWeaponをスポーンさせる
/// 　  ->この部分の実装はとりあえず後回し
/// 　  
/// 
/// </summary>

public class PlayerController : MonoBehaviour
{
    //プレイヤーの移動速度
    [SerializeField,Header("(仮)移動速度")]
    private  float _moveSpeed = 5.0f;

    //プレイヤーの移動方向
    private Vector2 _forward;//現在は取りえずStartで右方向に決め打ちSceneManagerで生成させるので後で書き換える
    //→補足左右方向でしか向きは変えない、上下の移動では向きの変更は行わない

    //プレイヤーの進行方向
    private Vector2 _moveInput;

    //各種コンポーネント取得用変数
    private Rigidbody2D _rigidbody2d;
    //private Animator _animator;   //後でStart内で取得の処理を入れる




    // Start is called before the first frame update
    void Start()
    {
        //Rigidbody2Dの取得
        _rigidbody2d = GetComponent<Rigidbody2D>();
        _forward = Vector2.right;//右方向に決め打ち→後でInitに移す
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();

    }

    private void FixedUpdate()
    {
        
        //プレイヤーの移動処理
        MovePlayer();
    }




    //--------------------------------------以下に関数に定義記述-------------------------------




    /// <summary>
    /// プレイヤーのWASDキー・矢印キー入力による移動入力を取得して
    /// _moveInputに正規化ベクトルとして格納を行う
    /// </summary>
    /// 
    private void GetInput()
    {
        //プレイヤーの移動処理に必要なInput取得
        //上下左右の入力取得
        float inputHorizontal = Input.GetAxisRaw("Horizontal");
        float inputVertical = Input.GetAxisRaw("Vertical");
        //斜め方向動きを考慮して正規化処理を行う
        _moveInput = new Vector2(inputHorizontal, inputVertical).normalized;


    }

    //RIgidbody2Dを使用したPlayerの移動処理
    //最初FixedUpdate内で処理の呼び出しを行っていたがテストで動かしたら2Dっぽくない動きになったのでUpdate内で変更
    private void MovePlayer()
    {
         //入力がないときは処理を抜ける
        if(_moveInput== Vector2.zero) return;

        _rigidbody2d.MovePosition(_rigidbody2d.position + _moveInput * _moveSpeed * Time.fixedDeltaTime);

        if (_moveInput.x != 0)
        {
            Vector3 scale = transform.localScale;
            scale.x = Mathf.Sign(_moveInput.x) * Mathf.Abs(scale.x);//移動方向に応じて向きを変える
            transform.localScale = scale;
        }
        //TODO　移動範囲の制限処理を追加する

        //TODO　アニメーションの処理を追加する

        _forward = _moveInput;//移動方向を更新



    }
}
