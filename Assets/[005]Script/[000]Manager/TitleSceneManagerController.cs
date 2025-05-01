using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class TitleSceneManagerController: MonoBehaviour
{
    //メニューUIs(ボタンとメニュー背景画像)のスライドに使用する変数
    [Header("メニューUIのスライドに使用する変数")]
    [SerializeField,Header("MenuUis")] RectTransform _menuUIsTransform; // RectTransformを格納する変数
    [SerializeField,Header("スライド開始地点")] Vector3 _startPos; // スライド開始地点
    [SerializeField,Header("スライド先地点")] Vector3 _targetPos; // スライド終了地点
    [SerializeField,Header("スライドにかける時間")] float _slideTime = 1.5f; // スライドにかける時間
    private bool _menuShow = false; // メニューUIが表示されているかどうかのフラグ


    // Start is called before the first frame update
    void Start()
    {
        if(_menuUIsTransform != null)
        {
            //スライド開始地点を設定
            _menuUIsTransform.anchoredPosition = _startPos; // スライド開始地点設定
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
    /// 開始地点のポジションから終点のポジションまでスライド移動してくる
    /// スライドの対象になるのはメニューUIのRectTransform
    /// </summary>
    private void SlideMenuUis()
    {
        if(_menuUIsTransform != null)
        {
            _menuUIsTransform.DOAnchorPos(_targetPos, _slideTime).SetEase(Ease.OutBounce);
            _menuShow = true; // メニューUIが表示されているフラグを立てる
        }

    }

    private void BackMenuUis()
    {
        if (_menuUIsTransform != null)
        {
            _menuUIsTransform.DOAnchorPos(_startPos, 0.5f).SetEase(Ease.InSine);
            _menuShow = false; // メニューUIが表示されていないフラグを立てる
        }
    }
}
