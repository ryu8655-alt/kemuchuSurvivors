using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Unity.VisualScripting;
/// <summary>
///  タイトル画面でのUI(Menuボタンらと背景画像の演出関係を担うスクリプト
/// </summary>
public class MenuUiController : MonoBehaviour
{
    //メニューUIをスライドさせるための変数
    [SerializeField] Vector3 _startPos; // スライド開始地点デフォルトで
    [SerializeField] Vector3 _endPos; // スライド終了地点
    [SerializeField] float _slideTime = 1.5f; // スライドにかける時間


    private RectTransform _rectTransform; // RectTransformを格納する変数
    // Start is called before the first frame update
    void Start()
    {
        // RectTransformを取得
        _rectTransform = GetComponent<RectTransform>();
        //スライド開始地点設定
        _rectTransform.anchoredPosition = _startPos;


    }

    // Update is called once per frame
    void Update()
    {
        //エンターキーを押したらメニューUIをスライドさせる
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
    /// 開始地点のポジションから終点のポジションまでスライド移動してくる
    /// メンバー変数を扱うためインスペクター上で設定可能にする
    /// </summary>
    private void SlideMenuUis()
    {
        _rectTransform.DOAnchorPos(_endPos, _slideTime).SetEase(Ease.OutBounce);
    }

    /// <summary>
    /// メニューUIをスライドさせて元の位置に戻す(テスト用)
    /// </summary>
    private void BackMenuUis()
    {
        _rectTransform.DOAnchorPos(_startPos, 0.5f).SetEase(Ease.InSine);
    }
}

