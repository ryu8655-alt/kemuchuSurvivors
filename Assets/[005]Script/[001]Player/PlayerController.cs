using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using System;
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
/// 
/// 
/// 
/// 
/// 
/// </summary>

public class PlayerController : MonoBehaviour
{

    //�v���C���[�̈ړ�����
    public Vector2 _forward;//���݂͎�肦��Start�ŉE�����Ɍ��ߑł�SceneManager�Ő���������̂Ō�ŏ���������
    //���⑫���E�����ł��������͕ς��Ȃ��A�㉺�̈ړ��ł͌����̕ύX�͍s��Ȃ�

    //�v���C���[�̐i�s����
    private Vector2 _moveInput;

    //�e��R���|�[�l���g�擾�p�ϐ�
    private Rigidbody2D _rigidbody2d;
    //private Animator _animator;   //���Start���Ŏ擾�̏���������
    //GameSceneManager _gameManager;

    //Init����Player�����Ɋi�[����I�u�W�F�N�g�֌W
    GameSceneManager _gameSceneManager;
    Slider _sliderXP;


    public CharacterStatus _characterStatus;

    //�U���N�[���_�E���ϐ�
    private float _attackCooldownTimer;//�N�[���_�E���v���p�ϐ�
    private float _attackCooldownMax = 0.5f;//�N�[���_�E���̏���l

    //���x���A�b�v�ɕK�v�ƂȂ�o���l���X�g
    List<int> _levelRequirements;
    //�G�̐������u
    EnemySpawnerController _enemySpawner;
    //���݂̃��x����\�������̃e�L�X�g�I�u�W�F�N�g
    TextMeshProUGUI _levelText;
    //���l��L�̃e�L�X�g�ϐ��ɂ��ẮA�݌v�I��Player���Ǘ�������̂ł͖����̂�
    //��ʓ�UI�ł���LV�e�L�X�g�ɊǗ�������C����
    //Player���g���ێ�����ׂ����e��GetLV()�ŕK�v�ƂȂ�e���Ƀ��x���f�[�^�݂̂�n������



    //���ݑ������̕���
    public List<BaseWeaponSpawner> _weaponSpawners;


    //�ǉ��@UI�ɒʒm���s���C�x���g
    public event Action<float, float> OnHPChanged;
    public event Action<float, float> OnXPChanged;
    public event Action<int> OnLevelChanged;


    public void Init(GameSceneManager  gameSceneManager,EnemySpawnerController enemySpawnerController,
           CharacterStatus characterStatus,TextMeshProUGUI textLV , Slider sliderXP)
    {
        //GameSceneManager���ɂ�InGameScene��J�n��������Player���X�|�[�����邽��
        //GameSceneManager�����Ăяo����悤��public�L�q


        //�ȉ��̕����Ɋe�����o�ϐ��̏��������������s
        this._levelRequirements = new List<int>();
        this._weaponSpawners = new List<BaseWeaponSpawner>();

        this._gameSceneManager = gameSceneManager;
        this._enemySpawner = enemySpawnerController;
        this._characterStatus = characterStatus;
        this._levelText = textLV;
        this._sliderXP = sliderXP;
        
        this._rigidbody2d = GetComponent<Rigidbody2D>();
        //�R�[�f�B���O�i�K�ł�Animation�̑f�ށA�{���ɃA�^�b�`������e�̑f�ނ��ł��Ă��Ȃ��̂�
        //�R�����g�A�E�g
        //this._animator = GetComponent<Animator>();

        _forward = Vector2.right;


        //�o���l��臒l���X�g���쐬���鏈��
        CreateXpTable();

        //�ŏ��̃��x���A�b�v(���x��2)�ɕK�v�Ȍo���l��ݒu
        _characterStatus.MaxXP = _levelRequirements[1];


        //����f�[�^�̃Z�b�g
        foreach(var item in _characterStatus._defaultWeaponIds)
        {
            //����̃X�|�[���������s��
            AddWeaponSpawner(item);
        }


        //�ȉ���UI�֘A�̏��������s��
        //�ȉ���UI�֌W�̂��̂͐؂藣�����s��
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
        
        //�v���C���[�̈ړ�����
        MovePlayer();

        
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
        _rigidbody2d.MovePosition(_rigidbody2d.position + _moveInput * _characterStatus.MoveSpeed * Time.fixedDeltaTime);

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
        pos.x = Mathf.Clamp(pos.x, _gameSceneManager._worldStart.x, _gameSceneManager._worldEnd.x);
        pos.y = Mathf.Clamp(pos.y, _gameSceneManager._worldStart.y, _gameSceneManager._worldEnd.y);
        _rigidbody2d.position = pos;
    }


     public void Damage(float attack)
    {
        //��A�N�e�B�u��Ԃ̎��̓_���[�W���󂯂Ȃ�
        if (!enabled) return;

        float damage = Mathf.Max(0, attack - _characterStatus.Defense);
        _characterStatus.HP -= damage;

        Debug.Log($"Player Damaged: {damage}");
        //�_���[�W�\��
        _gameSceneManager.DispDamage(gameObject, damage);

        //HP�ω��̃C�x���g�𔭍s����UI�ɒʒm�𑗂�
        OnHPChanged?.Invoke(_characterStatus.HP, _characterStatus.MaxHP);

        //TODO�@�Q�[���I�[�o�[������ǉ�����
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


    //�Փ˂������̏���
    private void OnCollisionEnter2D(Collision2D collision)
    {
        AttackEnemy(collision);
    }

    //�Փ˒��̏���
    private void OnCollisionStay2D(Collision2D collision)
    {
        AttackEnemy(collision);
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

    /// <summary>
    /// Init����Player�̃��x���A�b�v�ɕK�v�ƂȂ�
    /// �o���l��臒l���X�g���쐬����
    /// �܂��K�v�ƂȂ�o���l��S���x���Œ�ʂƂȂ�Ȃ��悤��
    /// 
    /// </summary>
    private void CreateXpTable()
    {
        _levelRequirements.Add(0);
        for(int i = 1; i < 1000; i++)
        {
            //�P�O��臒l�̎Q�Ƃ��s��
            int prevXp = _levelRequirements[i-1];
            //���x����41�𒴂����ꍇ�ɂ͒萔�I��16XP�����Z���Ă���
            int addxp = 16;

            //����̃��x���A�b�v�ɕK�v�Ȍo���l��5XP
            if (i == 1)
            {
                addxp = 5;
            }
            //���x����20�ȉ��̏ꍇ�ɂ�10XP�����Z���Ă���
            else if( 20 >= i)
            {

                addxp = 10;
            }
            //���x����40�ȉ��̏ꍇ�ɂ�13XP�����Z���Ă���
            else if (40 >= i)
            {
                addxp = 13;
            }

            //�e���x���̕K�v�o���l���v�Z�����X�g�Ɋi�[���s��
            int xp = prevXp + addxp;
            _levelRequirements.Add(xp);

        }
    }

    private void SetTextLv()
    {
        _levelText.text = "LV" + _characterStatus.Level;
    }


    //�����ǉ�����
    private void AddWeaponSpawner(int id)
    {
        //TODO�@�@�����ς݂Ȃ烌�x���A�b�v����
        BaseWeaponSpawner weaponSpawner = _weaponSpawners.Find(item => item._weaponStatus.Id == id);


            if (weaponSpawner)
        {
            return;
        }

        //�V�K�ɒǉ����鏈��
        weaponSpawner = WeaponSpawnerSettings.Instance.CreateWeaponSpawner(id,_enemySpawner, transform);

        if(null == weaponSpawner)
        {
            Debug.LogError("����f�[�^������܂���");
            return;
        }
        //�����ς݂̃��X�g�֒ǉ�����
        _weaponSpawners.Add(weaponSpawner);

    }

    //�ȉ��e��UI�I�u�W�F�N�g�ɒ񋟂�����Public�ϐ�
    public float currentHP => _characterStatus.HP;
    public float maxHP => _characterStatus.MaxHP;

    public float currentXP => _characterStatus.XP;
    public float maxXP => _characterStatus.MaxXP;

    public int currentLevel => _characterStatus.Level;


    public void GetXP(float xp)
    {
        _characterStatus.XP += xp;

        //���x������̏ꍇ�͏����𔲂���
        if (_levelRequirements.Count - 1 < _characterStatus.Level) return;

        //���x���A�b�v����
        if (_levelRequirements[_characterStatus.Level] <= _characterStatus.XP)
        {
            _characterStatus.Level++;

            //���̌o���l��ݒ肷��
            if(_characterStatus.Level < _levelRequirements.Count)
            {
                _characterStatus.XP = 0;
                _characterStatus.MaxXP = _levelRequirements[_characterStatus.Level];
            }

            //TODO���x���A�b�v�p�l���̕\������
            SetTextLv();
        }

        //�w�o�\���X�V
        SetSliderXP();

    }

}
