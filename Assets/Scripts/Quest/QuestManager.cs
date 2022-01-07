using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

// �N�G�X�g�S�̂��Ǘ�

public class QuestManager : MonoBehaviour
{
    public StageUIManager stageUI;
    public GameObject[] enemyPrefab;
    public BattleManager battleManager;
    public SceneTransitionManager sceneTransitionManager;
    public GameObject questBackground;

    int currentStage = 0; // ���݂̃X�e�[�W�i�s�x

    void Start()
    {
        stageUI.UpdateUI(currentStage);
        DialogTextManager.instance.SetScenarios(new string[] { "�X�ɂ����B" });
    }

    IEnumerator Searching()
    {
        DialogTextManager.instance.SetScenarios(new string[] { "�T����..." });
        // (new Vector3(X�̑傫��,Y�̑傫��,Z�̑傫��),���b������)
        // ���̏������́A2�b�����ĉ摜��傫�������シ���Ɍ��̑傫���ɖ߂������B
        // �w�i��傫��
        questBackground.transform.DOScale(new Vector3(1.5f, 1.5f, 1.5f), 2f).OnComplete(() => questBackground.transform.localScale = new Vector3(1f, 1f, 1f));

        // �t�F�[�h�A�E�g(2�b������0�ɂ���(�����x��),���̌��0�b������1�ɂ���(�����x��))
        SpriteRenderer questBackgroundSpriteRenderer = questBackground.GetComponent<SpriteRenderer>();
        questBackgroundSpriteRenderer.DOFade(0, 2f).OnComplete(() => questBackgroundSpriteRenderer.DOFade(1, 0));

        // 2�b�ԏ�����ҋ@����
        yield return new WaitForSeconds(2f);

        currentStage++;
        // �i�s�x��UI�ɔ��f
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

    // Next�{�^���������ꂽ��
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
        DialogTextManager.instance.SetScenarios(new string[] { "�����X�^�[�ɑ�������" });
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
        DialogTextManager.instance.SetScenarios(new string[] { "�󔠂���ɓ��ꂽ�B\n�X�ɖ߂낤��" });
        SoundManager.instance.StopBGM();
        SoundManager.instance.PlaySE(2);

        // �X�ɖ߂�{�^���̂ݕ\������
        stageUI.ShowClearText();
    }

    public IEnumerator PlayerDeath()
    {
        DialogTextManager.instance.SetScenarios(new string[] { "�퓬�s�\�ɂȂ��Ă��܂���..." });
        yield return new WaitForSeconds(2f);
        DialogTextManager.instance.SetScenarios(new string[] { "�X�ɖ߂�܂�" });
        yield return new WaitForSeconds(1f);
        sceneTransitionManager.LoadTo("Town");
    }
}
