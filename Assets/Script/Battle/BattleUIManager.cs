using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattleUIManager : MonoBehaviour
{
    [Header("Player Stats")]
    [SerializeField] private TMP_Text playerCurrentHealthBar;
    [SerializeField] private TMP_Text playerMaxHealthBar;
    [SerializeField] private Image playerCurrentHealthBarImage;
    [SerializeField] private TMP_Text playerCurrentMana;
    [SerializeField] private Image playerCurrentManaImage;
    [SerializeField] private TMP_Text playerMaxMana;
    [SerializeField] private Image playerEnergy;

    [Header("Enemy Stats")]
    [SerializeField] private TMP_Text enemyCurrentHealthBar;
    [SerializeField] private Image enemyCurrentHealthBarImage;
    [SerializeField] private TMP_Text enemyMaxHealthBar;
    [SerializeField] private TMP_Text enemyCurrentMana;
    [SerializeField] private Image enemyCurrentManaImage;
    [SerializeField] private TMP_Text enemyMaxMana;
    [SerializeField] private Image enemyEnergy;

    [Header("action System")]
    [SerializeField] private EnumActions currentTurn;
    

}
