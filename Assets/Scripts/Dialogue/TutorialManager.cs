using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{

	[SerializeField] Text nameText;
	[SerializeField] Text dialogueText;
	[SerializeField] Button continueButton;
	[SerializeField] GameObject tutorialHolder;
	[SerializeField] Dialogue[] dialogues;
	public int dialogueIndex = 0;

	[SerializeField] float textSpeed = .02f;

	private Queue<string> sentences;

	// Use this for initialization
	void Start()
	{
		sentences = new Queue<string>();
		StartDialogue(dialogues[dialogueIndex]);
	}

	public void StartDialogue(Dialogue dialogue)
	{
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
		if (dialogueIndex + 1 < dialogues.Length)
		{
			dialogueIndex++;
			StartDialogue(dialogues[dialogueIndex]);
		}
		else
		{
			Destroy(tutorialHolder);
		}

	}

}