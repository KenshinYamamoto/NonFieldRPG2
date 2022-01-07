using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

// クエスト全体を管理

public class QuestManager : MonoBehaviour
{
    public StageUIManager stageUI;
    public GameObject[] enemyPrefab;
    public BattleManager battleManager;
    public SceneTransitionManager sceneTransitionManager;
    public GameObject questBackground;

    int currentStage = 0; // 現在のステージ進行度

    void Start()
    {
        stageUI.UpdateUI(currentStage);
        DialogTextManager.instance.SetScenarios(new string[] { "森についた。" });
    }

    IEnumerator Searching()
    {
        DialogTextManager.instance.SetScenarios(new string[] { "探索中..." });
        // (new Vector3(Xの大きさ,Yの大きさ,Zの大きさ),何秒かけて)
        // この書き方は、2秒かけて画像を大きくした後すぐに元の大きさに戻すやり方。
        // 背景を大きく
        questBackground.transform.DOScale(new Vector3(1.5f, 1.5f, 1.5f), 2f).OnComplete(() => questBackground.transform.localScale = new Vector3(1f, 1f, 1f));

        // フェードアウト(2秒かけて0にする(透明度を),その後に0秒かけて1にする(透明度を))
        SpriteRenderer questBackgroundSpriteRenderer = questBackground.GetComponent<SpriteRenderer>();
        questBackgroundSpriteRenderer.DOFade(0, 2f).OnComplete(() => questBackgroundSpriteRenderer.DOFade(1, 0));

        // 2秒間処理を待機する
        yield return new WaitForSeconds(2f);

        currentStage++;
        // 進行度をUIに反映
        stageUI.UpdateUI(currentStage);

        int dice = Random.Range(0, 100);

        if (ParamsSO.Entity.stage <= currentStage)
        {
            QuestClear();
        }
        else if(dice < ParamsSO.Entity.rate)
        {
            EncountEnemy();
        }
        else
        {
            stageUI.ShowButton(true);
        }
    }

    // Nextボタンが押されたら
    public void OnNextButton()
    {
        SoundManager.instance.PlaySE(0);
        stageUI.ShowButton(false);
        StartCoroutine(Searching());
    }

    public void OnToTownButton()
    {
        SoundManager.instance.PlaySE(0);
    }

    void EncountEnemy()
    {
        DialogTextManager.instance.SetScenarios(new string[] { "モンスターに遭遇した" });
        int dice = Random.Range(0, enemyPrefab.Length);
        GameObject enemyObj = Instantiate(enemyPrefab[dice]);
        EnemyManager enemy = enemyObj.GetComponent<EnemyManager>();
        stageUI.ShowButton(false);
        battleManager.Setup(enemy);
    }

    public void EndBattle()
    {
        stageUI.ShowButton(true);
    }

    void QuestClear()
    {
        DialogTextManager.instance.SetScenarios(new string[] { "宝箱を手に入れた。\n街に戻ろうか" });
        SoundManager.instance.StopBGM();
        SoundManager.instance.PlaySE(2);

        // 街に戻るボタンのみ表示する
        stageUI.ShowClearText();
    }

    public IEnumerator PlayerDeath()
    {
        DialogTextManager.instance.SetScenarios(new string[] { "戦闘不能になってしまった..." });
        yield return new WaitForSeconds(2f);
        DialogTextManager.instance.SetScenarios(new string[] { "街に戻ります" });
        yield return new WaitForSeconds(1f);
        sceneTransitionManager.LoadTo("Town");
    }
}
