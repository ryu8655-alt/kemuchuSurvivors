using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro.EditorUtilities;
using UnityEditor;

public class EnemyController : MonoBehaviour
{

    public CharacterStatus _characterStatus;

    [SerializeField] GameSceneManager _gameSceneManager;
    Rigidbody2D _rigidbody2d;

    //攻撃クールダウン変数
    float _attackCooldownTimer;//クールダウン計測用変数
    float _attackCooldownMax = 0.5f;//クールダウンの上限値

    //移動方向
    Vector2 _forward;

    //状態
    enum State
    {
        Alive,
        Dead,
    }

    private State _state;

    // Start is called before the first frame update
    void Start()
    {
        init(this._gameSceneManager, CharacterSettings.Instance.Get(100));
    }

    // Update is called once per frame
    void Update()
    {
        UpdateTimer();
        MoveEnemy();
    }


    public void init(GameSceneManager sceneManager, CharacterStatus characterStatus)
    {
        this._gameSceneManager = sceneManager;
        this._characterStatus = characterStatus;

        _rigidbody2d = GetComponent<Rigidbody2D>();

        //アニメーション演出
        //ランダムを活用して緩急をつける
        float random = 0.8f;
        float speed = 1 / _characterStatus.MoveSpeed * random;

        //サイズ
        float addx = 0.8f;
        float x = addx * random;
        transform.DOScale(x, speed).SetRelative().SetLoops(-1, LoopType.Yoyo);

        //回転演出
        float addz = 10f;
        float z = Random.Range(-addz, addz) * random;
        //初期値
        Vector3 rotation = transform.rotation.eulerAngles;
        rotation.z = z;
        //目標値
        transform.eulerAngles = rotation;
        transform.DORotate(new Vector3(0, 0, -z), speed).SetLoops(-1, LoopType.Yoyo);

        //進行方向
        PlayerController player = _gameSceneManager._playerController;
        Vector2 dir = player.transform.position - transform.position;
        _forward = dir;

        _state = State.Alive;
    }


    //敵の移動
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
    }

    //各種内部タイマーの更新
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
            //経験値の生成処理を後で追加する
        }

        _state = State.Dead;
    }

    //衝突次判定
    private void OnTriggerEnter2D(Collider2D collision)
    {

    }

    //衝突判定
    private void OnTriggerStay2D(Collider2D collision)
    {

    }

    private void OnTriggerExit2D(Collider2D collision)
    {

    }


}
