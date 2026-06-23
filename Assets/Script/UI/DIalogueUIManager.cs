using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueUIManager : MonoBehaviour
{
    [SerializeField] private  TMP_Text dialogueText;
    [SerializeField] private  TMP_Text dialogueName;
    [SerializeField] private  Image dialogueSprite;
    [SerializeField] private  GameObject DialogueUI;

    public void SetDialogueText(string text)
    {
        dialogueText.text = text;
    }

    public void SetDialogueName(string name)
    {
        dialogueName.text = name;
    }

    public void SetSprite(Sprite sprite)
    {
        dialogueSprite.sprite = sprite;
    }

    public void StatusDialogueUI(bool isActive)
    {
        DialogueUI.SetActive(isActive);
    }
}
