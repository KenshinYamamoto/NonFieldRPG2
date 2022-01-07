using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

// �G���Ǘ��������(�X�e�[�^�X/�N���b�N���ꂽ���̌��o)

public class EnemyManager : MonoBehaviour
{
    //�֐��o�^
    Action tapAction; // �N���b�N���ꂽ���Ɏ��s�������֐�(�O������ݒ肵����)

    public new string name;
    public int hp;
    public int attack;
    public GameObject hitEffect;

    // �U������
    public int Attack(PlayerManager player)
    {
        int damage = player.Damage(attack);
        return damage;
    }

    // �_���[�W���󂯂�
    public int Damage(int damage)
    {
        //Instantiate(hitEffect, this.transform, false);
        transform.DOShakePosition(0.3f, 0.5f, 20, 0, false, true);
        hp -= damage;

        if(hp <= 0)
        {
            hp = 0;
        }
        return damage;
    }


    // tapAction�Ɋ֐���o�^����֐������
    public void AddEventListenerOnTap(Action action)
    {
        tapAction += action;
    }

    public void OnTap()
    {
        tapAction();
    }
}
