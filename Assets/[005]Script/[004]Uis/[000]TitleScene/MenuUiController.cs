using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Unity.VisualScripting;
/// <summary>
///  �^�C�g����ʂł�UI(Menu�{�^����Ɣw�i�摜�̉��o�֌W��S���X�N���v�g
/// </summary>
public class MenuUiController : MonoBehaviour
{
    //���j���[UI���X���C�h�����邽�߂̕ϐ�
    [SerializeField] Vector3 _startPos; // �X���C�h�J�n�n�_�f�t�H���g��
    [SerializeField] Vector3 _endPos; // �X���C�h�I���n�_
    [SerializeField] float _slideTime = 1.5f; // �X���C�h�ɂ����鎞��


    private RectTransform _rectTransform; // RectTransform���i�[����ϐ�
    // Start is called before the first frame update
    void Start()
    {
        // RectTransform���擾
        _rectTransform = GetComponent<RectTransform>();
        //�X���C�h�J�n�n�_�ݒ�
        _rectTransform.anchoredPosition = _startPos;


    }

    // Update is called once per frame
    void Update()
    {
        //�G���^�[�L�[���������烁�j���[UI���X���C�h������
        if (Input.GetKeyDown(KeyCode.Return))
        {
            SlideMenuUis();
        }
        if(Input.GetKeyDown(KeyCode.Space))
        {
            BackMenuUis();
        }

    }

    /// <summary>
    /// �J�n�n�_�̃|�W�V��������I�_�̃|�W�V�����܂ŃX���C�h�ړ����Ă���
    /// �����o�[�ϐ����������߃C���X�y�N�^�[��Őݒ�\�ɂ���
    /// </summary>
    private void SlideMenuUis()
    {
        _rectTransform.DOAnchorPos(_endPos, _slideTime).SetEase(Ease.OutBounce);
    }

    /// <summary>
    /// ���j���[UI���X���C�h�����Č��̈ʒu�ɖ߂�(�e�X�g�p)
    /// </summary>
    private void BackMenuUis()
    {
        _rectTransform.DOAnchorPos(_startPos, 0.5f).SetEase(Ease.InSine);
    }
}

