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

    //�U���N�[���_�E���ϐ�
    float _attackCooldownTimer;//�N�[���_�E���v���p�ϐ�
    float _attackCooldownMax = 0.5f;//�N�[���_�E���̏���l

    //�ړ�����
    Vector2 _forward;

    //���
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

        //�A�j���[�V�������o
        //�����_�������p���Ċɋ}������
        float random = 0.8f;
        float speed = 1 / _characterStatus.MoveSpeed * random;

        //�T�C�Y
        float addx = 0.8f;
        float x = addx * random;
        transform.DOScale(x, speed).SetRelative().SetLoops(-1, LoopType.Yoyo);

        //��]���o
        float addz = 10f;
        float z = Random.Range(-addz, addz) * random;
        //�����l
        Vector3 rotation = transform.rotation.eulerAngles;
        rotation.z = z;
        //�ڕW�l
        transform.eulerAngles = rotation;
        transform.DORotate(new Vector3(0, 0, -z), speed).SetLoops(-1, LoopType.Yoyo);

        //�i�s����
        PlayerController player = _gameSceneManager._playerController;
        Vector2 dir = player.transform.position - transform.position;
        _forward = dir;

        _state = State.Alive;
    }


    //�G�̈ړ�
    private void MoveEnemy()
    {
        if (State.Alive != _state) return;

        if (MoveType.TargetPlayer == _characterStatus._moveType)
        {
            PlayerController player = _gameSceneManager._playerController;
            Vector2 dir = player.transform.position - transform.position;
            _forward = dir;

        }

        //�ړ�
        _rigidbody2d.position += _forward.normalized * _characterStatus.MoveSpeed * Time.deltaTime;
    }

    //�e������^�C�}�[�̍X�V
    private void UpdateTimer()
    {
        if (0 < _attackCooldownTimer)
        {
            _attackCooldownTimer -= Time.deltaTime;
        }

        //�������Ԃ�ݒ肳��Ă���ꍇ�̓^�C�}�[�̏������s��
        if (0 < _characterStatus.AliveTime)
        {
            _characterStatus.AliveTime -= Time.deltaTime;
            if (0 > _characterStatus.AliveTime) SetDead(false);
        }
    }


    private void SetDead(bool createXP = true)
    {
        if (State.Alive != _state) return;

        //���������̒�~
        _rigidbody2d.simulated = false;
        //�A�j���[�V�����̒�~
        transform.DOKill();

        //���ŃA�j���[�V�����̍Đ�
        transform.DOScaleY(0, 0.5f).OnComplete(() =>
        {
            Destroy(gameObject);
        });

        //�o���l�̐���
        if (createXP)
        {
            //�o���l�̐�����������Œǉ�����
        }

        _state = State.Dead;
    }

    //�Փˎ�����
    private void OnTriggerEnter2D(Collider2D collision)
    {

    }

    //�Փ˔���
    private void OnTriggerStay2D(Collider2D collision)
    {

    }

    private void OnTriggerExit2D(Collider2D collision)
    {

    }


}
