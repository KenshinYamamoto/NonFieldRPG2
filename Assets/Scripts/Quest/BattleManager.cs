using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

// Player��Enemy�̑ΐ�̊Ǘ�

public class BattleManager : MonoBehaviour
{
    public Transform playerDamagePanel;
    public QuestManager questManager;
    public PlayerUIManager playerUI;
    public EnemyUIManager enemyUI;
    public PlayerManager player;
    EnemyManager enemy;

    public bool canAttack = false;

    private void Start()
    {
        enemyUI.gameObject.SetActive(false);
        playerUI.SetupUI(player);
    }

    // �����ݒ�
    public void Setup(EnemyManager enemyManager)
    {
        canAttack = true;
        SoundManager.instance.PlayBGM("Battle");
        enemyUI.gameObject.SetActive(true);
        enemy = enemyManager;
        enemyUI.SetupUI(enemy);
        playerUI.SetupUI(player);
        enemy.AddEventListenerOnTap(PlayerAttack);
    }

    void PlayerAttack()
    {
        if (!canAttack)
        {
            return;
        }
        canAttack = false;
        
        StopAllCoroutines();
        SoundManager.instance.PlaySE(1);

        // Player��Enemy�ɍU��
        int damage = player.Attack(enemy);
        enemyUI.UpdateUI(enemy);
        DialogTextManager.instance.SetScenarios(new string[] { "�v���C���[�̍U���I\n�����X�^�[��"+damage+"�_���[�W��^�����I" });

        EffectGenerate();

        if (enemy.hp <= 0)
        {
            StartCoroutine(EndBattle());
        }
        else
        {
            StartCoroutine(EnemyTurn());
        }
    }

    IEnumerator EnemyTurn()
    {
        yield return new WaitForSeconds(2f);

        playerDamagePanel.DOShakePosition(0.3f, 0.5f, 20, 0, false, true); // (���b������,����,��,?,?,?)
        SoundManager.instance.PlaySE(1);

        // Enemy��Player�ɍU��
        int damage = enemy.Attack(player);
        playerUI.UpdateUI(player);
        DialogTextManager.instance.SetScenarios(new string[] { "�����X�^�[�̍U���I\n�v���C���[��"+damage+"�_���[�W���󂯂��I" });

        yield return new WaitForSeconds(1.5f);
        if (player.hp <= 0)
        {
            // �v���C���[�����񂾏ꍇ
            StartCoroutine(questManager.PlayerDeath());
        }
        else
        {
            canAttack = true;
        }
    }

    IEnumerator EndBattle()
    {
        yield return new WaitForSeconds(2f);
        DialogTextManager.instance.SetScenarios(new string[] { "�����X�^�[�����j�����I" });
        enemyUI.gameObject.SetActive(false);
        Destroy(enemy.gameObject);
        SoundManager.instance.PlayBGM("Quest");
        questManager.EndBattle();
    }

    void EffectGenerate()
    {
        // �G�t�F�N�g��\������
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Instantiate(enemy.hitEffect, mousePos, transform.rotation, transform.parent);
    }
}
