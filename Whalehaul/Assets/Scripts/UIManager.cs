using TMPro;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text winText;
    [SerializeField] private Image[] lives;

    //score
    public void SetScore(int score)
    {
        scoreText.SetText(score.ToString());
    }
    //win text
    public void SetWin()
    {
        winText.SetText("Congratulations!! Your spirit can now pass on!!");
    }
    //lives
    public void SetLives(int livesRemaining)
    {
        switch (livesRemaining)
        {
            case 3:
                lives[0].enabled = true;
                lives[1].enabled = true;
                lives[2].enabled = true;
                break;
            case 2:
                lives[0].enabled = false;
                lives[1].enabled = true;
                lives[2].enabled = true;
                break;
            case 1:
                lives[0].enabled = false;
                lives[1].enabled = false;
                lives[2].enabled = true;
                break;
            case 0:
                lives[0].enabled = false;
                lives[1].enabled = false;
                lives[2].enabled = false;
                break;
        }
    }
}
