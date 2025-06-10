using DG.Tweening;
using UnityEditor.Experimental.GraphView;
using UnityEngine;


/// <summary>
/// �G�L�����N�^�[�̐�����s���N���X
/// �ړ��A���C�t�^�C�}�[�A���S���̉��o�Ȃǂ̃��W�b�N��S��
/// </summary>

public class EnemyController : MonoBehaviour
{

    public CharacterStatus _characterStatus;


    [Header("�X�v���C�g�̌����ݒ�")]
    [Tooltip("���̃X�v���C�g�̃f�t�H���g������ݒ肵�܂��B\n" +
         "ON�i�`�F�b�N����j�F�E�������f�t�H���g�̃X�v���C�g\n" +
         "OFF�i�`�F�b�N�Ȃ��j�F���������f�t�H���g�̃X�v���C�g\n" +
         "�i�s�����ɉ����č��E���]�������������s����悤�ɂȂ�܂��B")]
    [SerializeField]
    private bool _isFacingRightDefault = false;

    [SerializeField,Header("GameSceneManager")]
    private GameSceneManager _gameSceneManager;
    
    private Rigidbody2D _rigidbody2d;

    //�U���N�[���_�E���ϐ�
    private  float _attackCooldownTimer;//�N�[���_�E���v���p�ϐ�
    private float _attackCooldownMax = 0.5f;//�N�[���_�E���̏���l

    //�ړ�����
    private Vector2 _forward;

    //���
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
/// ����������(SceneManager�ƃL�����N�^�[���̐ݒ�)���s��
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
    /// �G�̃A�j���[�V�������o�̐ݒ���s��
    /// </summary>
    
    private void AnimateSpawn()
    {
        //�A�j���[�V�������o(�ӂ�ӂ�Ƃ��������ɂȂ�)
        float random = 0.8f;
        float speed = 1 / _characterStatus.MoveSpeed * random;

        //�T�C�Y���o
        float addx = 0.8f;
        float x = addx * random;
        transform.DOScale(x, speed).SetRelative().SetLoops(-1, LoopType.Yoyo);

        //��]���o
        float addz = 10f;
        float z = Random.Range(-addz, addz) * random;
        Vector3 rotation = transform.rotation.eulerAngles;
        rotation.z = z;
        transform.eulerAngles = rotation;
        transform.DORotate(new Vector3(0, 0, -z), speed).SetLoops(-1, LoopType.Yoyo);

    }

    /// <summary>
    /// �����i�s�������v���C���[�̕����ɐݒ肷��
    /// </summary>
    private void SetInitalizeDirection()
    {
        PlayerController player = _gameSceneManager._playerController;
        Vector2 dir = player.transform.position - transform.position;
        _forward = dir;
    }


    /// <summary>
    ///�G�̍s����ݒ肳�ꂽMoveType�ɏ]���čs��
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

        //�ړ�
        _rigidbody2d.position += _forward.normalized * _characterStatus.MoveSpeed * Time.deltaTime;


        //�ړ��������擾���ď�Ɉړ������������悤�ɂ���
        if(_forward.x  != 0)
        {
            Vector3 scale = transform.localScale;
            float direction = Mathf.Sign(_forward.x) *(_isFacingRightDefault ? 1:-1);
            scale.x = direction * Mathf.Abs(scale.x);//�ړ������ɉ����ăX�v���C�g�̌�����ύX
            transform.localScale = scale;
        }


    }

    /// <summary>
    /// �����N�[���_�E������ѐ������Ԃ̍X�V����
    /// </summary>
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

    /// <summary>
    /// �G�̎��S����
    /// </summary>
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
            //�o���l����
            _gameSceneManager.CreateXP(this);
        }

        _state = State.Dead;
    }

    //�Փˎ�����
   private void OnCollisionEnter2D(Collision2D collision)
    {
        AttackPlayer(collision.collider);
    }

    //�Փ˔���
    private void OnCollisionStay2D(Collision2D collision)
    {
        AttackPlayer(collision.collider);
    }

 

    /// <summary>
    /// �v���C���[�Ƃ̏Փ˔�����s���A�U���������s��
    /// �v���C���[�֍U�����s���ɂ́APlayer��Damage���\�b�h���Ăяo��
    /// </summary>
    /// <param name="collision"></param>
    private void AttackPlayer(Collider2D collision)
    {
        //����Ώۂ̊m�F
        if (!collision.gameObject.TryGetComponent<PlayerController>(out var player)) return;
        //�^�C�}�[�������Ȃ珈���𔲂���
        if (0 < _attackCooldownTimer) return;
        //���S���Ă����珈���𔲂���
        if (State.Dead == _state) return;

        player.Damage(_characterStatus.Attack);
        //�N�[���_�E���^�C�}�[�̏�����
        _attackCooldownTimer = _attackCooldownMax;
    }

    /// <summary>
    /// �U�����󂯂����̏���
    /// 
    /// </summary>
    /// <param name="attack"></param>
    public float Damage(float attack)
    {
        //���g�����S���Ă��鎞�͏����𔲂���
        if (State.Dead == _state) return 0;

        //���C�t�̌�������
        float damage = Mathf.Max(0 , attack - _characterStatus.Defense);
        _characterStatus.HP -= damage;
        Debug.Log($"Enemy Damaged: {damage}");

        //�_���[�W���l���e�L�X�g�ŕ\������
        _gameSceneManager.DispDamage(gameObject, damage);

        //TODO�@����
        if ( 0 > _characterStatus.HP)
        {
            SetDead();
        }

        //�v�Z��̃_���[�W��Ԃ�
        return damage;
    }


}
