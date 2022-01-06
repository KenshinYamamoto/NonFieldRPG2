using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIManager : MonoBehaviour
{
    public Text hpText;
    public Text attackText;

    public void SetupUI(PlayerManager player)
    {
        hpText.text = string.Format("HP : {0}", player.hp);
        attackText.text = string.Format("AT : {0}", player.attack);
    }

    public void UpdateUI(PlayerManager player)
    {
        hpText.text = string.Format("HP : {0}", player.hp);
    }
}
