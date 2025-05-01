using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class TitleSceneManagerController: MonoBehaviour
{
    //���j���[UIs(�{�^���ƃ��j���[�w�i�摜)�̃X���C�h�Ɏg�p����ϐ�
    [Header("���j���[UI�̃X���C�h�Ɏg�p����ϐ�")]
    [SerializeField,Header("MenuUis")] RectTransform _menuUIsTransform; // RectTransform���i�[����ϐ�
    [SerializeField,Header("�X���C�h�J�n�n�_")] Vector3 _startPos; // �X���C�h�J�n�n�_
    [SerializeField,Header("�X���C�h��n�_")] Vector3 _targetPos; // �X���C�h�I���n�_
    [SerializeField,Header("�X���C�h�ɂ����鎞��")] float _slideTime = 1.5f; // �X���C�h�ɂ����鎞��
    private bool _menuShow = false; // ���j���[UI���\������Ă��邩�ǂ����̃t���O


    // Start is called before the first frame update
    void Start()
    {
        if(_menuUIsTransform != null)
        {
            //�X���C�h�J�n�n�_��ݒ�
            _menuUIsTransform.anchoredPosition = _startPos; // �X���C�h�J�n�n�_�ݒ�
        }

    }

    // Update is called once per frame
    void Update()
    {
        if(!_menuShow&& Input.GetKeyDown(KeyCode.Return))
        {
            SlideMenuUis();
        }

        if (_menuShow && Input.GetKeyDown(KeyCode.Space))
        {
            BackMenuUis();
        }
    }

    /// <summary>
    /// �J�n�n�_�̃|�W�V��������I�_�̃|�W�V�����܂ŃX���C�h�ړ����Ă���
    /// �X���C�h�̑ΏۂɂȂ�̂̓��j���[UI��RectTransform
    /// </summary>
    private void SlideMenuUis()
    {
        if(_menuUIsTransform != null)
        {
            _menuUIsTransform.DOAnchorPos(_targetPos, _slideTime).SetEase(Ease.OutBounce);
            _menuShow = true; // ���j���[UI���\������Ă���t���O�𗧂Ă�
        }

    }

    private void BackMenuUis()
    {
        if (_menuUIsTransform != null)
        {
            _menuUIsTransform.DOAnchorPos(_startPos, 0.5f).SetEase(Ease.InSine);
            _menuShow = false; // ���j���[UI���\������Ă��Ȃ��t���O�𗧂Ă�
        }
    }
}
