using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dialogue
{
    List<string> dialogueList;
}
public class NPCManager : MonoBehaviour
{
    [SerializeField] private List<string> listDialogue;
    [SerializeField] private string nama;
    [SerializeField] private GameObject notification;
    [SerializeField] private Sprite iconSprite;
    
    private DialogueUIManager _dialogueUIManager;

    public string GetDialogue(int index)
    {
        return listDialogue[index];
    }

    public int GetDialogueLength()
    {
        return listDialogue.Count;
    }

    public void SetActiveNotif(bool isActive)
    {
        notification.SetActive(isActive);
    }

    public string GetName()
    {
        return nama;
    }

    public Sprite GetSpriteNPC()
    {
        return iconSprite;
    }

    private void Start()
    {
        _dialogueUIManager = FindFirstObjectByType<DialogueUIManager>();
    }
}
