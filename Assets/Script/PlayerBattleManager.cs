using UnityEngine;

public class PlayerBattleManager : MonoBehaviour
{
    [SerializeField] private StatsSO playerStat;

    private void Start()
    {
        playerStat = ScriptableObject.Instantiate(playerStat);
    }
}
