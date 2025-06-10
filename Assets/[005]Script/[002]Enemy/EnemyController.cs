using DG.Tweening;
using UnityEditor.Experimental.GraphView;
using UnityEngine;


/// <summary>
/// 敵キャラクターの制御を行うクラス
/// 移動、ライフタイマー、死亡時の演出などのロジックを担当
/// </summary>

public class EnemyController : MonoBehaviour
{

    public CharacterStatus _characterStatus;


    [Header("スプライトの向き設定")]
    [Tooltip("このスプライトのデフォルト向きを設定します。\n" +
         "ON（チェックあり）：右向きがデフォルトのスプライト\n" +
         "OFF（チェックなし）：左向きがデフォルトのスプライト\n" +
         "進行方向に応じて左右反転処理が正しく行われるようになります。")]
    [SerializeField]
    private bool _isFacingRightDefault = false;

    [SerializeField,Header("GameSceneManager")]
    private GameSceneManager _gameSceneManager;
    
    private Rigidbody2D _rigidbody2d;

    //攻撃クールダウン変数
    private  float _attackCooldownTimer;//クールダウン計測用変数
    private float _attackCooldownMax = 0.5f;//クールダウンの上限値

    //移動方向
    private Vector2 _forward;

    //状態
    private enum State
    {
        Alive,
        Dead,
    }

    private State _state;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        UpdateTimer();
        MoveEnemy();
    }

/// <summary>
/// 初期化処理(SceneManagerとキャラクター情報の設定)を行う
/// </summary>
/// <param name="sceneManager"></param>
/// <param name="characterStatus"></param>
    public void Init(GameSceneManager sceneManager, CharacterStatus characterStatus)
    {
        this._gameSceneManager = sceneManager;
        this._characterStatus = characterStatus;

        _rigidbody2d = GetComponent<Rigidbody2D>();

        //AnimateSpawn();
        SetInitalizeDirection();

        _state = State.Alive;
    }

    /// <summary>
    /// 敵のアニメーション演出の設定を行う
    /// </summary>
    
    private void AnimateSpawn()
    {
        //アニメーション演出(ふわふわとした動きになる)
        float random = 0.8f;
        float speed = 1 / _characterStatus.MoveSpeed * random;

        //サイズ演出
        float addx = 0.8f;
        float x = addx * random;
        transform.DOScale(x, speed).SetRelative().SetLoops(-1, LoopType.Yoyo);

        //回転演出
        float addz = 10f;
        float z = Random.Range(-addz, addz) * random;
        Vector3 rotation = transform.rotation.eulerAngles;
        rotation.z = z;
        transform.eulerAngles = rotation;
        transform.DORotate(new Vector3(0, 0, -z), speed).SetLoops(-1, LoopType.Yoyo);

    }

    /// <summary>
    /// 初期進行方向をプレイヤーの方向に設定する
    /// </summary>
    private void SetInitalizeDirection()
    {
        PlayerController player = _gameSceneManager._playerController;
        Vector2 dir = player.transform.position - transform.position;
        _forward = dir;
    }


    /// <summary>
    ///敵の行動を設定されたMoveTypeに従って行う
    /// </summary>
    private void MoveEnemy()
    {
        if (State.Alive != _state) return;

        if (MoveType.TargetPlayer == _characterStatus._moveType)
        {
            PlayerController player = _gameSceneManager._playerController;
            Vector2 dir = player.transform.position - transform.position;
            _forward = dir;

        }

        //移動
        _rigidbody2d.position += _forward.normalized * _characterStatus.MoveSpeed * Time.deltaTime;


        //移動方向を取得して常に移動方向を向くようにする
        if(_forward.x  != 0)
        {
            Vector3 scale = transform.localScale;
            float direction = Mathf.Sign(_forward.x) *(_isFacingRightDefault ? 1:-1);
            scale.x = direction * Mathf.Abs(scale.x);//移動方向に応じてスプライトの向きを変更
            transform.localScale = scale;
        }


    }

    /// <summary>
    /// 内部クールダウンおよび生存時間の更新処理
    /// </summary>
    private void UpdateTimer()
    {
        if (0 < _attackCooldownTimer)
        {
            _attackCooldownTimer -= Time.deltaTime;
        }

        //生存期間を設定されている場合はタイマーの消化を行う
        if (0 < _characterStatus.AliveTime)
        {
            _characterStatus.AliveTime -= Time.deltaTime;
            if (0 > _characterStatus.AliveTime) SetDead(false);
        }
    }

    /// <summary>
    /// 敵の死亡処理
    /// </summary>
    private void SetDead(bool createXP = true)
    {
        if (State.Alive != _state) return;

        //物理挙動の停止
        _rigidbody2d.simulated = false;
        //アニメーションの停止
        transform.DOKill();

        //消滅アニメーションの再生
        transform.DOScaleY(0, 0.5f).OnComplete(() =>
        {
            Destroy(gameObject);
        });

        //経験値の生成
        if (createXP)
        {
            //経験値生成
            _gameSceneManager.CreateXP(this);
        }

        _state = State.Dead;
    }

    //衝突次判定
   private void OnCollisionEnter2D(Collision2D collision)
    {
        AttackPlayer(collision.collider);
    }

    //衝突判定
    private void OnCollisionStay2D(Collision2D collision)
    {
        AttackPlayer(collision.collider);
    }

 

    /// <summary>
    /// プレイヤーとの衝突判定を行い、攻撃処理を行う
    /// プレイヤーへ攻撃を行うには、PlayerのDamageメソッドを呼び出す
    /// </summary>
    /// <param name="collision"></param>
    private void AttackPlayer(Collider2D collision)
    {
        //判定対象の確認
        if (!collision.gameObject.TryGetComponent<PlayerController>(out var player)) return;
        //タイマー未消化なら処理を抜ける
        if (0 < _attackCooldownTimer) return;
        //死亡していたら処理を抜ける
        if (State.Dead == _state) return;

        player.Damage(_characterStatus.Attack);
        //クールダウンタイマーの初期化
        _attackCooldownTimer = _attackCooldownMax;
    }

    /// <summary>
    /// 攻撃を受けた時の処理
    /// 
    /// </summary>
    /// <param name="attack"></param>
    public float Damage(float attack)
    {
        //自身が死亡している時は処理を抜ける
        if (State.Dead == _state) return 0;

        //ライフの減少処理
        float damage = Mathf.Max(0 , attack - _characterStatus.Defense);
        _characterStatus.HP -= damage;
        Debug.Log($"Enemy Damaged: {damage}");

        //ダメージ数値をテキストで表示する
        _gameSceneManager.DispDamage(gameObject, damage);

        //TODO　消滅
        if ( 0 > _characterStatus.HP)
        {
            SetDead();
        }

        //計算後のダメージを返す
        return damage;
    }


}
