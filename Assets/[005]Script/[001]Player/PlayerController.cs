using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// �v���C���[�̓���
/// �ړ�
/// ->�㉺���E�̈ړ�
///   ->WASD�Ɩ��L�[���͂ňړ�
///   �@->�ړ����x�̓C���X�y�N�^�[�ォ�炢�����悤�ɂ��邯�ǂ��A�܂�Player/Enemy���ʂ̃f�[�^���������āA����������������Ă���悤�ɂ���\��Ȃ̂Ŏb��̂��̂ł�
///   �@
/// �U��
/// ->�U���̏����ɂ��Ă�WeaponSpaener���g���āAWeapon��Prefab��Instantiate���āAWeapon�̃X�N���v�g�ɍU���̏�������������\��
/// �@->���Ԃ��ƂɃX�|�[���J�E���^�[�����炵�čs���A0�ɂȂ����玩����Weapon���X�|�[��������
/// �@  ->���̕����̎����͂Ƃ肠�������
/// �@  
/// 
/// </summary>

public class PlayerController : MonoBehaviour
{
    //�v���C���[�̈ړ����x
    [SerializeField,Header("(��)�ړ����x")]
    private  float _moveSpeed = 5.0f;

    //�v���C���[�̈ړ�����
    private Vector2 _forward;//���݂͎�肦��Start�ŉE�����Ɍ��ߑł�SceneManager�Ő���������̂Ō�ŏ���������
    //���⑫���E�����ł��������͕ς��Ȃ��A�㉺�̈ړ��ł͌����̕ύX�͍s��Ȃ�

    //�v���C���[�̐i�s����
    private Vector2 _moveInput;

    //�e��R���|�[�l���g�擾�p�ϐ�
    private Rigidbody2D _rigidbody2d;
    //private Animator _animator;   //���Start���Ŏ擾�̏���������
    GameSceneManager _gameManager;

    //���Ƃ�Init�Ɉړ�����ϐ��B
    [SerializeField,Header("GameSceneManager")]
    private GameSceneManager _gameSceneManager;

    [SerializeField,Header("SliderXP")]
    private Slider _sliderXP;

    [SerializeField, Header("SliderHP")]
    private Slider _sliderHP;

    public CharacterStatus _characterStatus;

    //�U���N�[���_�E���ϐ�
    private float _attackCooldownTimer;//�N�[���_�E���v���p�ϐ�
    private float _attackCooldownMax = 0.5f;//�N�[���_�E���̏���l



    // Start is called before the first frame update
    void Start()
    {
        this._gameManager = GameObject.Find("GameSceneManager").GetComponent<GameSceneManager>();


        //Rigidbody2D�̎擾
        _rigidbody2d = GetComponent<Rigidbody2D>();
        _forward = Vector2.right;//�E�����Ɍ��ߑł������Init�Ɉڂ�
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();

    }

    private void FixedUpdate()
    {
        
        //�v���C���[�̈ړ�����
        MovePlayer();

        //HP�X���C�_�[�̈ړ�����
        MoveSliderHP();
    }




    //--------------------------------------�ȉ��Ɋ֐��ɒ�`�L�q-------------------------------




    /// <summary>
    /// �v���C���[��WASD�L�[�E���L�[���͂ɂ��ړ����͂��擾����
    /// _moveInput�ɐ��K���x�N�g���Ƃ��Ċi�[���s��
    /// </summary>
    /// 
    private void GetInput()
    {
        //�v���C���[�̈ړ������ɕK�v��Input�擾
        //�㉺���E�̓��͎擾
        float inputHorizontal = Input.GetAxisRaw("Horizontal");
        float inputVertical = Input.GetAxisRaw("Vertical");
        //�΂ߕ����������l�����Đ��K���������s��
        _moveInput = new Vector2(inputHorizontal, inputVertical).normalized;


    }

    //RIgidbody2D���g�p����Player�̈ړ�����
    //�ŏ�FixedUpdate���ŏ����̌Ăяo�����s���Ă������e�X�g�œ���������2D���ۂ��Ȃ������ɂȂ����̂�Update���ŕύX
    private void MovePlayer()
    {
         //���͂��Ȃ��Ƃ��͏����𔲂���
        if(_moveInput== Vector2.zero) return;
       //�ړ��͈͂̐�������
        ClampToMoveBounds(_rigidbody2d.position);

        //�v���C���[�̈ړ�����
        _rigidbody2d.MovePosition(_rigidbody2d.position + _moveInput * _moveSpeed * Time.fixedDeltaTime);

        if (_moveInput.x != 0)
        {
            Vector3 scale = transform.localScale;
            scale.x = Mathf.Sign(_moveInput.x) * Mathf.Abs(scale.x);//�ړ������ɉ����Č�����ς���
            transform.localScale = scale;
        }
        //TODO�@�A�j���[�V�����̏�����ǉ�����

        _forward = _moveInput;//�ړ��������X�V

    }

    /// <summary>
    /// �v���C���[�̈ړ���������
    /// </summary>
    private void  ClampToMoveBounds(Vector3 position)
    {
        Vector2 pos = _rigidbody2d.position;
        pos.x = Mathf.Clamp(pos.x, _gameManager._worldStart.x, _gameManager._worldEnd.x);
        pos.y = Mathf.Clamp(pos.y, _gameManager._worldStart.y, _gameManager._worldEnd.y);
        _rigidbody2d.position = pos;
    }

    /// <summary>
    /// Canvas���HP�X���C�_�[��v���C���[��Ǐ]���鏈��
    /// 
    private void MoveSliderHP()
    {
        Vector3 pos = RectTransformUtility.WorldToScreenPoint(Camera.main, transform.position);
        pos.y -= 125;
        _sliderHP.transform.position = pos;
    }


     public void Damage(float attack)
    {
        //��A�N�e�B�u��Ԃ̎��̓_���[�W���󂯂Ȃ�
        if (!enabled) return;

        float damage = Mathf.Max(0, attack - _characterStatus.Defense);
        _characterStatus.HP -= damage;

        //TODO�@�Q�[���I�[�o�[������ǉ�����
        if ( 0 > _characterStatus.HP)
        {
         
        }

        if(0 > _characterStatus.HP) _characterStatus.HP = 0;

        steSliderHP();
    }


    /// <summary>
    /// HP�X���C�_�[�̏����������ƍX�V���s��
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

 
    //�Փ˂������̏���
    private void OnCollisionEnter2D(Collision2D collision)
    {
        
    }

    //�Փ˒��̏���
    private void OnCollisionStay2D(Collision2D collision)
    {

    }

    //�Փ˂��I��������̏���
    private void OnCollisionExit2D(Collision2D collision)
    {
    }

    private void AttackEnemy(Collision2D collision)
    {
        //�Ώۂ��G�l�~�[����Ȃ��Ƃ�
        if (!collision.gameObject.TryGetComponent<EnemyController>(out var enemy)) return;
        //�^�C�}�[���������̎��͏����𔲂���
        if (0 < _attackCooldownTimer) return;

        //���EnemyController�Ƀ_���[�W������ǉ�����
        enemy.Damage(_characterStatus.Attack);
        //�U���N�[���_�E���^�C�}�[�̏�����
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
