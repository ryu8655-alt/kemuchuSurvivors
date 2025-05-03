using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextDamageController : MonoBehaviour
{
    //ダメージテキストの表示時間
    private float _dispTextTime = 1.0f;
    private GameObject _target;

    // Start is called before the first frame update
    void Start()
    {
        SpawnAnimation();
    }

    // Update is called once per frame
    void Update()
    {
       
        UpdateTextPosition();

    }

    /// <summary>
    /// ダメージテキストのスポーン時のアニメーション設定
    /// </summary>
    private void SpawnAnimation()
    {
        //テキストが膨らんでから小さくなっていき消える演出を行う
        //テキストが膨らむ演出から始まり、完了後に小さくなる演出を行う
        transform.DOScale(new Vector2(1, 1), _dispTextTime / 2).SetRelative()
            .OnComplete(() =>
            {
                //テキストが小さくなって消える演出
                transform.DOScale(new Vector2(0, 0), _dispTextTime / 2).SetRelative()
                    .OnComplete(() => Destroy(gameObject));
            });
    }


    /// <summary>
    ///テキストの位置更新を行う処理
    ///対象となるオブジェクトの位置を取得し、テキストの位置を更新する
    /// </summary>
    private void UpdateTextPosition()
    {
         if (!_target) return;
         //テキストの位置を更新する
         Vector3 targetPos = RectTransformUtility.WorldToScreenPoint(Camera.main, _target.transform.position);
        transform.position = targetPos;
    }


    public void Init(GameObject target , float damage)
    {
        this._target = target;
        //コンポーネント取得
        TextMeshProUGUI text = GetComponent<TextMeshProUGUI>();

        text.text = ""+ (int)damage;

    }
}
