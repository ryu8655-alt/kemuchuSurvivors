using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using System;
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
/// 
/// 
/// 
/// 
/// 
/// </summary>

public class PlayerController : MonoBehaviour
{

    //プレイヤーの移動方向
    public Vector2 _forward;//現在は取りえずStartで右方向に決め打ちSceneManagerで生成させるので後で書き換える
    //→補足左右方向でしか向きは変えない、上下の移動では向きの変更は行わない

    //プレイヤーの進行方向
    private Vector2 _moveInput;

    //各種コンポーネント取得用変数
    private Rigidbody2D _rigidbody2d;
    //private Animator _animator;   //後でStart内で取得の処理を入れる
    //GameSceneManager _gameManager;

    //Init内でPlayer内部に格納するオブジェクト関係
    GameSceneManager _gameSceneManager;
    Slider _sliderXP;


    public CharacterStatus _characterStatus;

    //攻撃クールダウン変数
    private float _attackCooldownTimer;//クールダウン計測用変数
    private float _attackCooldownMax = 0.5f;//クールダウンの上限値

    //レベルアップに必要となる経験値リスト
    List<int> _levelRequirements;
    //敵の生成装置
    EnemySpawnerController _enemySpawner;
    //現在のレベルを表示する先のテキストオブジェクト
    TextMeshProUGUI _levelText;
    //備考上記のテキスト変数については、設計的にPlayerが管理するものでは無いので
    //画面内UIであるLVテキストに管理役割を任せる
    //Player自身が保持するべき内容はGetLV()で必要となる各所にレベルデータのみを渡す処理



    //現在装備中の武器
    public List<BaseWeaponSpawner> _weaponSpawners;


    //追加　UIに通知を行うイベント
    public event Action<float, float> OnHPChanged;
    public event Action<float, float> OnXPChanged;
    public event Action<int> OnLevelChanged;


    public void Init(GameSceneManager  gameSceneManager,EnemySpawnerController enemySpawnerController,
           CharacterStatus characterStatus,TextMeshProUGUI textLV , Slider sliderXP)
    {
        //GameSceneManager内にてInGameScene画開始した時にPlayerをスポーンするため
        //GameSceneManager内より呼び出せるようにpublic記述


        //以下の部分に各メンバ変数の初期化処理を実行
        this._levelRequirements = new List<int>();
        this._weaponSpawners = new List<BaseWeaponSpawner>();

        this._gameSceneManager = gameSceneManager;
        this._enemySpawner = enemySpawnerController;
        this._characterStatus = characterStatus;
        this._levelText = textLV;
        this._sliderXP = sliderXP;
        
        this._rigidbody2d = GetComponent<Rigidbody2D>();
        //コーディング段階ではAnimationの素材、本環境にアタッチする内容の素材ができていないので
        //コメントアウト
        //this._animator = GetComponent<Animator>();

        _forward = Vector2.right;


        //経験値の閾値リストを作成する処理
        CreateXpTable();

        //最初のレベルアップ(レベル2)に必要な経験値を設置
        _characterStatus.MaxXP = _levelRequirements[1];


        //武器データのセット
        foreach(var item in _characterStatus._defaultWeaponIds)
        {
            //武器のスポーン処理を行う
            AddWeaponSpawner(item);
        }


        //以下にUI関連の初期化を行う
        //以下のUI関係のものは切り離しを行う
        SetTextLv();
        SetSliderXP();




    }


    // Update is called once per frame
    void Update()
    {
        GetInput();
        UpdateTimer();

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
       //移動範囲の制限処理
        ClampToMoveBounds(_rigidbody2d.position);

        //プレイヤーの移動処理
        _rigidbody2d.MovePosition(_rigidbody2d.position + _moveInput * _characterStatus.MoveSpeed * Time.fixedDeltaTime);

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
        pos.x = Mathf.Clamp(pos.x, _gameSceneManager._worldStart.x, _gameSceneManager._worldEnd.x);
        pos.y = Mathf.Clamp(pos.y, _gameSceneManager._worldStart.y, _gameSceneManager._worldEnd.y);
        _rigidbody2d.position = pos;
    }


     public void Damage(float attack)
    {
        //非アクティブ状態の時はダメージを受けない
        if (!enabled) return;

        float damage = Mathf.Max(0, attack - _characterStatus.Defense);
        _characterStatus.HP -= damage;

        Debug.Log($"Player Damaged: {damage}");
        //ダメージ表示
        _gameSceneManager.DispDamage(gameObject, damage);

        //HP変化のイベントを発行してUIに通知を送る
        OnHPChanged?.Invoke(_characterStatus.HP, _characterStatus.MaxHP);

        //TODO　ゲームオーバー処理を追加する
        if ( 0 > _characterStatus.HP)
        {
         
        }

        if(0 > _characterStatus.HP) _characterStatus.HP = 0;
    }


    private void SetSliderXP()
    {
        _sliderXP.maxValue = _characterStatus.MaxXP;
        _sliderXP.value = _characterStatus.XP;
    }


    //衝突した時の処理
    private void OnCollisionEnter2D(Collision2D collision)
    {
        AttackEnemy(collision);
    }

    //衝突中の処理
    private void OnCollisionStay2D(Collision2D collision)
    {
        AttackEnemy(collision);
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

    /// <summary>
    /// Init内でPlayerのレベルアップに必要となる
    /// 経験値の閾値リストを作成処理
    /// また必要となる経験値画全レベルで定量とならないように
    /// 
    /// </summary>
    private void CreateXpTable()
    {
        _levelRequirements.Add(0);
        for(int i = 1; i < 1000; i++)
        {
            //１つ前の閾値の参照を行う
            int prevXp = _levelRequirements[i-1];
            //レベルが41を超えた場合には定数的に16XPずつ加算していく
            int addxp = 16;

            //初回のレベルアップに必要な経験値は5XP
            if (i == 1)
            {
                addxp = 5;
            }
            //レベルが20以下の場合には10XPずつ加算していく
            else if( 20 >= i)
            {

                addxp = 10;
            }
            //レベルが40以下の場合には13XPずつ加算していく
            else if (40 >= i)
            {
                addxp = 13;
            }

            //各レベルの必要経験値を計算しリストに格納を行う
            int xp = prevXp + addxp;
            _levelRequirements.Add(xp);

        }
    }

    private void SetTextLv()
    {
        _levelText.text = "LV" + _characterStatus.Level;
    }


    //武器を追加する
    private void AddWeaponSpawner(int id)
    {
        //TODO　　装備済みならレベルアップする
        BaseWeaponSpawner weaponSpawner = _weaponSpawners.Find(item => item._weaponStatus.Id == id);


            if (weaponSpawner)
        {
            return;
        }

        //新規に追加する処理
        weaponSpawner = WeaponSpawnerSettings.Instance.CreateWeaponSpawner(id,_enemySpawner, transform);

        if(null == weaponSpawner)
        {
            Debug.LogError("武器データがありません");
            return;
        }
        //装備済みのリストへ追加する
        _weaponSpawners.Add(weaponSpawner);

    }

    //以下各種UIオブジェクトに提供をするPublic変数
    public float currentHP => _characterStatus.HP;
    public float maxHP => _characterStatus.MaxHP;

    public float currentXP => _characterStatus.XP;
    public float maxXP => _characterStatus.MaxXP;

    public int currentLevel => _characterStatus.Level;


    public void GetXP(float xp)
    {
        _characterStatus.XP += xp;

        //レベル上限の場合は処理を抜ける
        if (_levelRequirements.Count - 1 < _characterStatus.Level) return;

        //レベルアップ処理
        if (_levelRequirements[_characterStatus.Level] <= _characterStatus.XP)
        {
            _characterStatus.Level++;

            //次の経験値を設定する
            if(_characterStatus.Level < _levelRequirements.Count)
            {
                _characterStatus.XP = 0;
                _characterStatus.MaxXP = _levelRequirements[_characterStatus.Level];
            }

            //TODOレベルアップパネルの表示処理
            SetTextLv();
        }

        //ＸＰ表示更新
        SetSliderXP();

    }

}
