using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    [SerializeField] private RectTransform instructionTextTransform;
    [SerializeField] private TextMeshProUGUI instructionText;

    private void Start()
    {
        instructionTextTransform.localPosition = new Vector3(0, 201.32F, 0);
    }

    public void SwitchToGameScene()
    {
        instructionTextTransform.localPosition = new Vector3(0, -201.32F, 0);
        instructionText.text = "Starting game...";
        SceneManager.LoadScene("GameScene");
    }
}
