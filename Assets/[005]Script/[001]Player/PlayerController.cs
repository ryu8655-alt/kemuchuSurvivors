using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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




    // Start is called before the first frame update
    void Start()
    {
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

        _rigidbody2d.MovePosition(_rigidbody2d.position + _moveInput * _moveSpeed * Time.fixedDeltaTime);

        if (_moveInput.x != 0)
        {
            Vector3 scale = transform.localScale;
            scale.x = Mathf.Sign(_moveInput.x) * Mathf.Abs(scale.x);//�ړ������ɉ����Č�����ς���
            transform.localScale = scale;
        }
        //TODO�@�ړ��͈͂̐���������ǉ�����

        //TODO�@�A�j���[�V�����̏�����ǉ�����

        _forward = _moveInput;//�ړ��������X�V



    }
}
