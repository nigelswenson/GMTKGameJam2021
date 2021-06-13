using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{

	[SerializeField] Text nameText;
	[SerializeField] Text dialogueText;
	[SerializeField] Image portrait;
	[SerializeField] Dialogue[] dialogues;
	public int dialogueIndex = 0;

	[SerializeField] Animator animator;
	[SerializeField] float textSpeed = .02f;

	private Queue<string> sentences;

	// Use this for initialization
	void Start()
	{
		sentences = new Queue<string>();
		animator.SetBool("IsOpen", true);
		StartDialogue(dialogues[dialogueIndex]);
	}

	public void StartDialogue(Dialogue dialogue)
	{
		portrait.sprite = dialogue.portrait;
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
		dialogueIndex++;
		//animator.SetBool("IsOpen", false);
		if(dialogueIndex <= dialogues.Length)
		{
			StartDialogue(dialogues[dialogueIndex]);
		}
		else
        {
			FindObjectOfType<SceneLoader>().LoadNextScene();
        }
	}

}