using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattleUIManager : MonoBehaviour
{
    [Header("Player Stats")]
    [SerializeField] private TMP_Text playerCurrentHealthBar;
    [SerializeField] private Image playerCurrentHealthBarImage;
    [SerializeField] private TMP_Text playerCurrentMana;
    [SerializeField] private Image playerCurrentManaImage;
    [SerializeField] private Image playerEnergy;
    [SerializeField] private PlayerBattleManager _playerBattle;

    [Header("Enemy Stats")]
    [SerializeField] private TMP_Text enemyCurrentHealthBar;
    [SerializeField] private Image enemyCurrentHealthBarImage;
    [SerializeField] private TMP_Text enemyCurrentMana;
    [SerializeField] private Image enemyCurrentManaImage;
    [SerializeField] private Image enemyEnergy;
    [SerializeField] private EnemyBattle enemyBattle;

    [Header("action System")]
    [SerializeField] private EnumTurns currentTurn;
    private bool isBattle = false;

    public EnumTurns GetCurrentTurn()
    {
        return currentTurn;
    }
    
    public void SetCurrentTurn(EnumTurns turn)
    {
        currentTurn = turn;
    }

    public void SetIsBattle(bool isBattling)
    {
        isBattle = isBattling;
    }

    private void Update()
    {
        SetCurrentHealthPlayer();
        SetCurrentManaPlayer();
    }

    private void Start()
    {
        SetCurrentTurn(EnumTurns.player);
        _playerBattle = FindFirstObjectByType<PlayerBattleManager>();
    }

    public void SetEnemyBattleManager(EnemyBattle attackedEnemy)
    {
        enemyBattle = attackedEnemy;
    }

    public void SetCurrentHealthPlayer()
    {
        float modifiedHealth = ((float)_playerBattle.GetPlayerStats().hp / (float)_playerBattle.GetPlayerStats().maxHp) * 70 - 70;
        playerCurrentHealthBarImage.rectTransform.offsetMax = new Vector2 (modifiedHealth,0);
        playerCurrentHealthBar.text = _playerBattle.GetPlayerStats().hp.ToString() + " / " +_playerBattle.GetPlayerStats().maxHp.ToString();
    }

    public void SetCurrentManaPlayer()
    {
        float modifiedMana = ((float)_playerBattle.GetPlayerStats().mana / (float)_playerBattle.GetPlayerStats().maxMana) * 70 - 70;
        playerCurrentManaImage.rectTransform.offsetMax = new Vector2(modifiedMana, 0);
        playerCurrentMana.text = _playerBattle.GetPlayerStats().mana.ToString() + " / " + _playerBattle.GetPlayerStats().maxMana.ToString();
    }
}
