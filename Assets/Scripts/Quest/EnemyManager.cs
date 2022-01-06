using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �G���Ǘ��������(�X�e�[�^�X/�N���b�N���ꂽ���̌��o)

public class EnemyManager : MonoBehaviour
{
    //�֐��o�^
    Action tapAction; // �N���b�N���ꂽ���Ɏ��s�������֐�(�O������ݒ肵����)

    public new string name;
    public int hp;
    public int attack;

    // �U������
    public void Attack(PlayerManager player)
    {
        player.Damage(attack);
    }

    // �_���[�W���󂯂�
    public void Damage(int damage)
    {
        hp -= damage;

        if(hp <= 0)
        {
            hp = 0;
        }
    }


    // tapAction�Ɋ֐���o�^����֐������
    public void AddEventListenerOnTap(Action action)
    {
        tapAction += action;
    }

    public void OnTap()
    {
        Debug.Log("�N���b�N���ꂽ");
        tapAction();
    }
}
