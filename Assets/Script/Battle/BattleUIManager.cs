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
    [SerializeField] private TMP_Text HealthText;
    [SerializeField] private GameObject playerGO;
    [SerializeField] private PlayerBattleManager _playerBattle;

    [Header("Enemy Stats")]
    [SerializeField] private TMP_Text enemyCurrentHealthBar;
    [SerializeField] private Image enemyCurrentHealthBarImage;
    [SerializeField] private TMP_Text enemyCurrentMana;
    [SerializeField] private Image enemyCurrentManaImage;
    [SerializeField] private Image enemyEnergy;
    private EnemyBattle enemyBattle;

    [Header("action System")]
    [SerializeField] private EnumTurns currentTurn;
    [SerializeField] private GameObject BattlePanel;
    private PlayerManager playerOpenWorld;
    private bool isBattle = false;
    private AudioManager _audioManager;
    private int defeatedEnemy = 2;
    private ButtonMenuManager _buttonMenuManager;

    public EnumTurns GetCurrentTurn()
    {
        return currentTurn;
    }

    public void StartBattle()
    {
        playerOpenWorld.SetPlayerObject(false);
        playerGO.transform.localPosition = new Vector3(-0.4f, 0.2f, 0);
        BattlePanel.SetActive(true);
        if(enemyBattle != null)
        {
            enemyBattle.SetEnemySprites(false);
        }
    }
    public void FinishBattle()
    {
        BattlePanel.SetActive(false);
        playerOpenWorld.SetPlayerObject(true);
        _audioManager.StopAllAudio();
        _audioManager.PlayAudio("openWorld");
        if(_playerBattle.GetDefeatedEnemy() == defeatedEnemy)
        {
            _buttonMenuManager.ShowGameOver(false);
        }
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
        if (_playerBattle != null)
        {
            SetCurrentHealthPlayer();
            SetCurrentManaPlayer();
            SetEnergyPlayer();
            SetHealthItemPlayer();
        }
        if (enemyBattle != null)
        {
            SetCurrentHealthEnemy();
            SetCurrentManaEnemy();
        }
    }

    private void Start()
    {
        SetCurrentTurn(EnumTurns.player);
        _audioManager = FindFirstObjectByType<AudioManager>();
        playerOpenWorld = FindFirstObjectByType<PlayerManager>();
        _buttonMenuManager = FindFirstObjectByType<ButtonMenuManager>();
    }

    public void SetHealthItemPlayer()
    {
        HealthText.text = "Health X"+_playerBattle.GetPlayerStats().healthItem;
    }

    public void SetEnemyBattleManager(EnemyBattle attackedEnemy)
    {
        enemyBattle = attackedEnemy;
    }

    public void SetCurrentHealthPlayer()
    {
        float modifiedHealth = ((float)_playerBattle.GetPlayerStats().hp / (float)_playerBattle.GetPlayerStats().maxHp) * 70 - 70;
        playerCurrentHealthBarImage.rectTransform.offsetMax = new Vector2(modifiedHealth, 0);
        playerCurrentHealthBar.text = _playerBattle.GetPlayerStats().hp.ToString() + " / " + _playerBattle.GetPlayerStats().maxHp.ToString();
    }

    public void SetCurrentManaPlayer()
    {
        float modifiedMana = ((float)_playerBattle.GetPlayerStats().mana / (float)_playerBattle.GetPlayerStats().maxMana) * 70 - 70;
        playerCurrentManaImage.rectTransform.offsetMax = new Vector2(modifiedMana, 0);
        playerCurrentMana.text = _playerBattle.GetPlayerStats().mana.ToString() + " / " + _playerBattle.GetPlayerStats().maxMana.ToString();
    }

    public void SetCurrentHealthEnemy()
    {
        float modifiedHealth = ((float)enemyBattle.GetEnemyStats().hp / (float)enemyBattle.GetEnemyStats().maxHp) * 83 - 83;
        enemyCurrentHealthBarImage.rectTransform.offsetMin = new Vector2(-1 * modifiedHealth, 0);
        enemyCurrentHealthBar.text = enemyBattle.GetEnemyStats().hp.ToString() + " / " + enemyBattle.GetEnemyStats().maxHp.ToString();
    }

    public void SetCurrentManaEnemy()
    {
        float modifiedMana = ((float)enemyBattle.GetEnemyStats().mana / (float)enemyBattle.GetEnemyStats().maxMana) * 83 - 83;
        enemyCurrentManaImage.rectTransform.offsetMin = new Vector2(-1 * modifiedMana, 0);
        enemyCurrentMana.text = enemyBattle.GetEnemyStats().mana.ToString() + " / " + enemyBattle.GetEnemyStats().maxMana.ToString();
    }

    public void SetEnergyPlayer()
    {
        float modifiedEnergy = ((float)_playerBattle.GetPlayerStats().energy / (float)_playerBattle.GetPlayerStats().maxEnergy);
        playerEnergy.fillAmount = modifiedEnergy;
    }
}
