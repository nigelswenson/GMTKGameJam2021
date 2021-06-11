using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
	public Dialogue[] dialogues;
	public Dialogue dialogue;
	public int dialogueIndex = 0;

    public void TriggerDialogue()
	{
			FindObjectOfType<DialogueManager>().StartDialogue(dialogues[dialogueIndex]);
	}
}