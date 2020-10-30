using UnityEngine;

[CreateAssetMenu(fileName = "HighScoreSO", menuName = "Game/HighScore")]
[System.Serializable]
public class HighScoreSO : ScriptableObject
{
    public int score;
}
