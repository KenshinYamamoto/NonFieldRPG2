using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// StageUI���Ǘ�(�X�e�[�W����UI/�i�s�{�^��/�X�ɖ߂�{�^��)�̊Ǘ�

public class StageUIManager : MonoBehaviour
{
    public Text stageText;
    public GameObject nextButton;
    public GameObject toTownButton;
    public GameObject stageClearText;

    private void Start()
    {
        stageClearText.SetActive(false);
    }

    public void UpdateUI(int currentStage)
    {
        stageText.text = string.Format("�X�e�[�W : {0}", currentStage+1);
    }

    public void ShowButton(bool isTrue)
    {
        nextButton.SetActive(isTrue);
        toTownButton.SetActive(isTrue);
    }

    public void ShowClearText()
    {
        stageClearText.SetActive(true);
        nextButton.SetActive(false);
        toTownButton.SetActive(true);
    }
}
