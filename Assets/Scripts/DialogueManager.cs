using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{

	[SerializeField] Text nameText;
	[SerializeField] Text dialogueText;

	[SerializeField] Animator animator;
	[SerializeField] float textSpeed = .02f;

	private Queue<string> sentences;

	// Use this for initialization
	void Start()
	{
		sentences = new Queue<string>();
	}

	public void StartDialogue(Dialogue dialogue)
	{
		animator.SetBool("IsOpen", true);

		nameText.text = dialogue.name;

		sentences.Clear();

		foreach (string sentence in dialogue.sentences)
		{
			sentences.Enqueue(sentence);
		}

		DisplayNextSentence();
	}

	public void DisplayNextSentence()
	{
		if (sentences.Count == 0)
		{
			EndDialogue();
			return;
		}

		string sentence = sentences.Dequeue();
		StopAllCoroutines();
		StartCoroutine(TypeSentence(sentence));
	}

	IEnumerator TypeSentence(string sentence)
	{
		dialogueText.text = "";
		foreach (char letter in sentence.ToCharArray())
		{
			dialogueText.text += letter;
			yield return new WaitForSeconds(textSpeed);
		}
	}

	void EndDialogue()
	{
		//animator.SetBool("IsOpen", false);
		var trigger = FindObjectOfType<DialogueTrigger>();
		trigger.dialogueIndex++;
		if (trigger.dialogueIndex <= trigger.dialogues.Length)
        {
			trigger.TriggerDialogue();
        }
	}

}