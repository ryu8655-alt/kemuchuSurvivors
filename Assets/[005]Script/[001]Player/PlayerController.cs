using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
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
    GameSceneManager _gameManager;

    //あとでInitに移動する変数達
    [SerializeField,Header("GameSceneManager")]
    private GameSceneManager _gameSceneManager;

    [SerializeField,Header("SliderXP")]
    private Slider _sliderXP;

    [SerializeField, Header("SliderHP")]
    private Slider _sliderHP;

    public CharacterStatus _characterStatus;

    //攻撃クールダウン変数
    private float _attackCooldownTimer;//クールダウン計測用変数
    private float _attackCooldownMax = 0.5f;//クールダウンの上限値



    // Start is called before the first frame update
    void Start()
    {
        this._gameManager = GameObject.Find("GameSceneManager").GetComponent<GameSceneManager>();


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

        //HPスライダーの移動処理
        MoveSliderHP();
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
       //移動範囲の制限処理
        ClampToMoveBounds(_rigidbody2d.position);

        //プレイヤーの移動処理
        _rigidbody2d.MovePosition(_rigidbody2d.position + _moveInput * _moveSpeed * Time.fixedDeltaTime);

        if (_moveInput.x != 0)
        {
            Vector3 scale = transform.localScale;
            scale.x = Mathf.Sign(_moveInput.x) * Mathf.Abs(scale.x);//移動方向に応じて向きを変える
            transform.localScale = scale;
        }
        //TODO　アニメーションの処理を追加する

        _forward = _moveInput;//移動方向を更新

    }

    /// <summary>
    /// プレイヤーの移動制限処理
    /// </summary>
    private void  ClampToMoveBounds(Vector3 position)
    {
        Vector2 pos = _rigidbody2d.position;
        pos.x = Mathf.Clamp(pos.x, _gameManager._worldStart.x, _gameManager._worldEnd.x);
        pos.y = Mathf.Clamp(pos.y, _gameManager._worldStart.y, _gameManager._worldEnd.y);
        _rigidbody2d.position = pos;
    }

    /// <summary>
    /// Canvas上のHPスライダー画プレイヤーを追従する処理
    /// 
    private void MoveSliderHP()
    {
        Vector3 pos = RectTransformUtility.WorldToScreenPoint(Camera.main, transform.position);
        pos.y -= 125;
        _sliderHP.transform.position = pos;
    }


     public void Damage(float attack)
    {
        //非アクティブ状態の時はダメージを受けない
        if (!enabled) return;

        float damage = Mathf.Max(0, attack - _characterStatus.Defense);
        _characterStatus.HP -= damage;

        //TODO　ゲームオーバー処理を追加する
        if ( 0 > _characterStatus.HP)
        {
         
        }

        if(0 > _characterStatus.HP) _characterStatus.HP = 0;

        steSliderHP();
    }


    /// <summary>
    /// HPスライダーの初期化処理と更新を行う
    /// </summary>
    private void steSliderHP()
    {
        _sliderHP.maxValue = _characterStatus.MaxHP;
        _sliderHP.value = _characterStatus.HP;

    }

    private void SliderXP()
    {
        _sliderXP.maxValue = _characterStatus.MaxXP;
        _sliderXP.value = _characterStatus.XP;
    }

 
    //衝突した時の処理
    private void OnCollisionEnter2D(Collision2D collision)
    {
        
    }

    //衝突中の処理
    private void OnCollisionStay2D(Collision2D collision)
    {

    }

    //衝突が終わった時の処理
    private void OnCollisionExit2D(Collision2D collision)
    {
    }

    private void AttackEnemy(Collision2D collision)
    {
        //対象がエネミーじゃないとき
        if (!collision.gameObject.TryGetComponent<EnemyController>(out var enemy)) return;
        //タイマーが未消化の時は処理を抜ける
        if (0 < _attackCooldownTimer) return;

        //後でEnemyControllerにダメージ処理を追加する
        enemy.Damage(_characterStatus.Attack);
        //攻撃クールダウンタイマーの初期化
        _attackCooldownTimer = _attackCooldownMax;
    }

    private void UpdateTimer()
    {
        if(0 < _attackCooldownTimer)
        {
            _attackCooldownTimer -= Time.deltaTime;
        }
    }
}
