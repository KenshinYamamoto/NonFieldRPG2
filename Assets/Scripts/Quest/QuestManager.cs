using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// クエスト全体を管理

public class QuestManager : MonoBehaviour
{
    public StageUIManager stageUI;
    public GameObject enemyPrefab;
    public BattleManager battleManager;
    public SceneTransitionManager sceneTransitionManager;

    //敵に遭遇するテーブル:-1なら遭遇しない、0なら遭遇
    int[] encountTable = { -1, -1, 0, -1, 0, -1 };

    int currentStage = 0; // 現在のステージ進行度

    void Start()
    {
        stageUI.UpdateUI(currentStage);
    }

    // Nextボタンが押されたら
    public void OnNextButton()
    {
        currentStage++;
        // 進行度をUIに反映
        stageUI.UpdateUI(currentStage);

        if(encountTable.Length <= currentStage)
        {
            Debug.Log("クエストクリア");
            QuestClear();
        }
        else if(encountTable[currentStage] == 0)
        {
            Debug.Log("敵に遭遇");
            EncountEnemy();
        }
    }

    void EncountEnemy()
    {
        GameObject enemyObj = Instantiate(enemyPrefab);
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
        // クエストクリアと表示する

        // 街に戻るボタンのみ表示する
        stageUI.ShowClearText();


        //sceneTransitionManager.LoadTo("Town");
    }
}
