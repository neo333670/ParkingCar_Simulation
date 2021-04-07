using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManerger : MonoBehaviour
{
    public CarEntity m_targetCar;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI gearText;
    int score = 100;

    public void UpdateScore(int deductpoint) {
        score -= deductpoint;
        scoreText.text = "Score: " + score;
    }

    public void updateGearText(string gear_name) {
        gearText.text = gear_name;
    }
}
