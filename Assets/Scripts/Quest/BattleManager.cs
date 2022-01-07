using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

// PlayerとEnemyの対戦の管理

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

    // 初期設定
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

        // PlayerがEnemyに攻撃
        int damage = player.Attack(enemy);
        enemyUI.UpdateUI(enemy);
        DialogTextManager.instance.SetScenarios(new string[] { "プレイヤーの攻撃！\nモンスターに"+damage+"ダメージを与えた！" });

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

        playerDamagePanel.DOShakePosition(0.3f, 0.5f, 20, 0, false, true); // (何秒かけて,強さ,回数,?,?,?)
        SoundManager.instance.PlaySE(1);

        // EnemyがPlayerに攻撃
        int damage = enemy.Attack(player);
        playerUI.UpdateUI(player);
        DialogTextManager.instance.SetScenarios(new string[] { "モンスターの攻撃！\nプレイヤーは"+damage+"ダメージを受けた！" });

        yield return new WaitForSeconds(1.5f);
        if (player.hp <= 0)
        {
            // プレイヤーが死んだ場合
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
        DialogTextManager.instance.SetScenarios(new string[] { "モンスターを撃破した！" });
        enemyUI.gameObject.SetActive(false);
        Destroy(enemy.gameObject);
        SoundManager.instance.PlayBGM("Quest");
        questManager.EndBattle();
    }

    void EffectGenerate()
    {
        // エフェクトを表示する
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Instantiate(enemy.hitEffect, mousePos, transform.rotation, transform.parent);
    }
}
