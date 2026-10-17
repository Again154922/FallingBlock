using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    [SerializeField] private RectTransform instructionTextTransform;
    [SerializeField] private TextMeshProUGUI instructionText;

    public void SwitchToGameScene()
    {
        Debug.Log(instructionTextTransform.localPosition);
        instructionTextTransform.localPosition = -instructionTextTransform.localPosition;
        instructionText.text = "Starting game...";
        SceneManager.LoadScene("GameScene");
    }
}
