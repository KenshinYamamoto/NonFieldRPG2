using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �N�G�X�g�S�̂��Ǘ�

public class QuestManager : MonoBehaviour
{
    public StageUIManager stageUI;
    public GameObject enemyPrefab;
    public BattleManager battleManager;
    public SceneTransitionManager sceneTransitionManager;

    //�G�ɑ�������e�[�u��:-1�Ȃ瑘�����Ȃ��A0�Ȃ瑘��
    int[] encountTable = { -1, -1, 0, -1, 0, -1 };

    int currentStage = 0; // ���݂̃X�e�[�W�i�s�x

    void Start()
    {
        stageUI.UpdateUI(currentStage);
    }

    // Next�{�^���������ꂽ��
    public void OnNextButton()
    {
        currentStage++;
        // �i�s�x��UI�ɔ��f
        stageUI.UpdateUI(currentStage);

        if(encountTable.Length <= currentStage)
        {
            Debug.Log("�N�G�X�g�N���A");
            QuestClear();
        }
        else if(encountTable[currentStage] == 0)
        {
            Debug.Log("�G�ɑ���");
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
        // �N�G�X�g�N���A�ƕ\������

        // �X�ɖ߂�{�^���̂ݕ\������
        stageUI.ShowClearText();


        //sceneTransitionManager.LoadTo("Town");
    }
}
