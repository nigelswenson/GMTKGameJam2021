using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
// Characters now instantiate at the start of the script, need to edit script to manage the three decks separately.
public enum BattleState { START, PLAYERTURN, TEXT, ENEMYTURN, ESCAPE, VICTORY, DEFEAT, RUN }

public class BattleManager : MonoBehaviour
{
    //config variables
    [Header("Player Character")]
    [SerializeField] GameObject characterTemplate;
    public List<PlayerCharacter> party = new List<PlayerCharacter>();
    [SerializeField] GameObject characterArea;

    [Header("Enemy")] 
    [SerializeField] Enemy enemy;
    [SerializeField] GameObject enemyArea;
    [SerializeField] Image enemyBleedImage;
    [SerializeField] Text enemyBleedCount;
    [SerializeField] Image enemyArmorImage;
    [SerializeField] Text enemyArmorCount;
    [SerializeField] Slider enemyHpSlider;
    [SerializeField] Image enemyHealImage;

    [Header("Card")]
    [SerializeField] int handSize;
    [SerializeField] GameObject cardTemplate;
    private List<Card> deck = new List<Card>();
    private List<GameObject> activeCards = new List<GameObject>();
    private List<Card> discardPile = new List<Card>();

    [SerializeField] GameObject playerAreas;
    public GameObject deckArea;
    public GameObject dropZone;
    public GameObject discardZone;
    public bool doubleStrike = false;

    [Header("Sound")]
    public SFX_Playing sfx;

    [Header("UI")]
    public Image battleInfo;
    public Text battleText;
    public Button battleTextContinueButton;
    public Image endTurnImage;


    //state variables
    public BattleState state;
    private bool enemyDefeated = false;

    Card playedCard;
    PlayerCharacter playedCardOwner;


    void Start()
    {
        state = BattleState.START;
        sfx.PlayMusic();
        StartCoroutine(SetupBattle());
    }

    private IEnumerator SetupBattle()
    {
        //instantiate player characters and generate decks
        foreach (PlayerCharacter partyMember in party)
        {
            //clears discardpile and deck in case there is leftover data before setting up this instance of the characters
            partyMember.discardPile.Clear();
            partyMember.deck.Clear();
            partyMember.actions = 1;
            partyMember.currentHp = partyMember.maxHp;

            yield return StartCoroutine(InstantiateCharacters(partyMember));
            InstantiateCards(partyMember);

        }

        InstantiateEnemies();
        DrawNewHand();
        enemy.SetBehavior();

        enemy.EnemySetup();

        DisableHealImage();

        state = BattleState.PLAYERTURN;
    }

    private void InstantiateEnemies()
    {
        Instantiate(enemy, new Vector3(0, 0, 0), Quaternion.identity, enemyArea.transform);

    }

    private IEnumerator InstantiateCharacters(PlayerCharacter partyMember)
    {
        GameObject newCharacter = Instantiate(characterTemplate, new Vector3(0, 0, 0), Quaternion.identity);
        newCharacter.GetComponent<CharacterDisplay>().character = partyMember;
        newCharacter.GetComponent<CharacterDisplay>().DisableTargetIndicator();
        newCharacter.transform.Find("DropZone").gameObject.GetComponent<DropZone>().character = partyMember;
        newCharacter.transform.SetParent(playerAreas.transform, false);


        partyMember.playerArea = newCharacter.GetComponent<CharacterDisplay>().cardArea;

        newCharacter.GetComponent<CharacterDisplay>().SetHp();
        yield return new WaitForSeconds(0);
    }

    private void InstantiateCards(PlayerCharacter partyMember)
    {
        //instantiates card templates and applys card data to them, marking each card as "owned" by a particular character so we can access their specific discard pile
        foreach (Card card in partyMember.deckData)
        {
            GameObject newCard = Instantiate(cardTemplate, new Vector3(0, 0, 0), Quaternion.identity);
            card.wasPlayed = false;
            newCard.GetComponent<CardDisplay>().card = card;
            newCard.GetComponent<CardDisplay>().SetColor(partyMember.cardColor);
            newCard.GetComponent<CardDisplay>().owner = partyMember;
            newCard.transform.SetParent(deckArea.transform, false);
            partyMember.deck.Add(newCard);
        }
    }

    public void MuteAudio()
    {
        if(AudioListener.volume != 0)
        {
            AudioListener.volume = 0;
        }
        else
        {
            AudioListener.volume = 1;
        }
        
    }

    public void Execute(GameObject cardToExecute)
    {
        playedCard = cardToExecute.GetComponent<CardDisplay>().card;
        playedCard.wasPlayed = true;
        playedCardOwner = cardToExecute.GetComponent<CardDisplay>().owner;
        //StartCoroutine(TargetAndInvoke(cardToExecute));
        if (playedCardOwner.actions >= 1)
        {
            foreach (string method in playedCard.methodList)
            {
                Invoke(method, 0);
            }
            if (doubleStrike)
            {
                foreach (string method in playedCard.methodList)
                {

                    Invoke(method, 0);
                }
                doubleStrike = false;
            }
            playedCardOwner.actions -= 1;
            EndTurnGlow();
            DiscardCard(cardToExecute);
        }
        else
        {
            cardToExecute.transform.SetParent(cardToExecute.GetComponent<CardDisplay>().owner.playerArea.transform, false);
        }
    }

    private void EndTurnGlow()
    {
        var noActions = true;
        foreach (PlayerCharacter character in party)
        {
            if (character.actions != 0)
            {
                noActions = false;
            }
        }
        if (noActions)
        {
            endTurnImage.color = new Color32(68, 180, 250, 255);
        }
    }

    private void ResetEndTurnButton()
    {
        endTurnImage.color = new Color32(255, 255, 255, 255);
    }

    //Card Methods
    private void CardDraw()
    {
        foreach (PlayerCharacter partyMember in party)
        {
            if ((partyMember.characterName == playedCard.target) | (playedCard.target == "all"))
            {
                Draw(partyMember, playedCard.cardsToDraw, partyMember.deck);
            }
        }
    }
    private void Heal()
    {
        foreach (PlayerCharacter partyMember in party)
        {
            if ((partyMember.characterName == playedCard.target) | (playedCard.target == "all"))
            {
                partyMember.Heal(playedCard.healingDone);
                sfx.PlayHeal();
            }

        }
    }
    private void Damage()
    {
        enemy.TakeDamage(playedCard.damageDealt);
        sfx.PlayDamage();
    }
    private void Armor()
    {
        foreach (PlayerCharacter partyMember in party)
        {
            if ((partyMember.characterName == playedCard.target) || (playedCard.target == "all"))
            {
                partyMember.Armor(playedCard.armorAdded);
                sfx.PlayArmor();
            }
        }
    }
    private void Bleed()
    {
        enemy.Bleed(playedCard.bleedAdded);
        sfx.PlayBleed();
    }
    private void ActionAdd()
    {
        foreach (PlayerCharacter partyMember in party)
        {
            if ((partyMember.characterName == playedCard.target) | (playedCard.target == "all"))
            {
                partyMember.ActionAdd(playedCard.actionAdded);
            }
        }
    }
    private void BleedToDamage()
    {
        enemy.TakeDamage(enemy.bleed);
        sfx.PlayDamage();
    }
    private void BleedToArmor()
    {
        sfx.PlayArmor();
        foreach (PlayerCharacter partyMember in party)
        {
            if ((partyMember.characterName == playedCard.target) | (playedCard.target == "all"))
            {
                partyMember.Armor(enemy.bleed);
            }
        }
    }
    private void BleedToBleed()
    {
        enemy.Bleed(enemy.bleed);
        sfx.PlayBleed();
    }
    private void ChangeTarget()
    {  // Needs to be fixed
        //if (enemy.target != "all")
        //{
        //    enemy.target = playedCard.target;
        //}
    }
    private void CheckArmored()
    {
        if (playedCardOwner.armor <= 0)
        {
            playedCard.damageDealt = 0;
        }
        else
        {
            playedCard.damageDealt = 5;
        }
    }
    private void HurtAllies()
    {
        foreach (PlayerCharacter partyMember in party)
        {
            if (partyMember != playedCardOwner)
            {
                partyMember.TakeDamage(5);
            }
        }
    }
    private void SetDuplicate()
    {
        doubleStrike = true;
    }


    //Enemy Turn Coroutine
    private IEnumerator EnemyTurn()
    {
        yield return new WaitForSeconds(.5f);
        enemy.DoBehavior();
        DisableHealImage();
        var characterDisplays = FindObjectsOfType<CharacterDisplay>();

        var alive = false;
        foreach (PlayerCharacter character in party)
        {
            if (character.currentHp > 0)
            {
                alive = true;
            }
        }
        if (!alive)
        {
            GameOver();
        }
        enemy.SetBehavior();

        yield return new WaitForSeconds(.5f);
    }

    //used when the enemy takes an action
    public void SetBlink(Color32 color)
    {
        StartCoroutine(Blink(color));
    }
    public IEnumerator Blink(Color32 color)
    {
        Image image = FindObjectOfType<Enemy>().transform.Find("Image").GetComponent<Image>();
        for (var i = 0; i <= 2; i++)
        {
            image.color = color;
            yield return new WaitForSeconds(.2f);
            image.color = new Color32(255, 255, 255, 255);
            yield return new WaitForSeconds(.2f);
        }
    }


    //Turn Process Functions
    public void EndTurn()
    {
        if(state == BattleState.PLAYERTURN)
        {
            StartCoroutine(EnemyTurn());
            foreach (PlayerCharacter partyMember in party)
            {
                partyMember.EndTurn();
            }
            var alive = false;
            foreach (PlayerCharacter character in party)
            {
                if (character.currentHp > 0)
                {
                    alive = true;
                }
            }
            if (!alive)
            {
                GameOver();
            }
            enemy.EndTurn();
            //Discards current hand before drawing a new one
            DiscardHand();
            doubleStrike = false;
            //reset active cards to recieve a new hand
            activeCards.Clear();
            //Draws a fresh hand of cards
            DrawNewHand();
            ResetEndTurnButton();
        }
    }

    private void DrawNewHand()
    {
        foreach (PlayerCharacter partyMember in party)
        {
            Draw(partyMember, handSize / party.Count, partyMember.deck);
        }
    }

    private void DiscardCard(GameObject card)
    {
        card.GetComponent<CardDisplay>().owner.discardPile.Add(card);
        card.transform.SetParent(discardZone.transform, false);
    }
    private void DiscardHand()
    {
        foreach (GameObject card in activeCards)
        {
            card.GetComponent<CardDisplay>().owner.discardPile.Add(card);
            //cards are moved off screen because we still want to access them but hide them from the player
            card.transform.SetParent(discardZone.transform, false);
        }
    }

    public void Draw(PlayerCharacter partyMember, int numCards, List<GameObject> activeDeck)
    {
        if (partyMember.currentHp > 0)
        {
            for (var i = 0; i < numCards; i++)
            {
                //check if deck is empty, if so shuffle
                if (activeDeck.Count <= 0)
                {
                    shuffle(activeDeck, partyMember.discardPile);
                }

                int randNum = Random.Range(0, activeDeck.Count);
                GameObject drawnCard = activeDeck[randNum];
                drawnCard.GetComponent<CardDisplay>().card.wasPlayed = false;
                drawnCard.transform.SetParent(drawnCard.GetComponent<CardDisplay>().owner.playerArea.transform, false);
                activeCards.Add(activeDeck[randNum]);
                activeDeck.RemoveAt(randNum);
            }
        }
    }
 
    public void shuffle(List<GameObject> emptyDeck, List<GameObject> fullDiscard)
    {
        emptyDeck.AddRange(fullDiscard);
        foreach (GameObject card in fullDiscard)
        {
            card.transform.SetParent(deckArea.transform, false);
        }
        fullDiscard.Clear();
    }

    //Enemy UI behavior
    public void SetEnemyBleed(int bleed)
    {
        if (bleed <= 0)
        {
            enemyBleedImage.enabled = false;
            enemyBleedCount.enabled = false;
        }
        else
        {
            enemyBleedCount.text = enemy.bleed.ToString();
            enemyBleedImage.enabled = true;
            enemyBleedCount.enabled = true;
        }
    }

    public void SetEnemyArmor(int armor)
    {
        if (armor <= 0)
        {
            enemyArmorImage.enabled = false;
            enemyArmorCount.enabled = false;
        }
        else
        {
            enemyArmorCount.text = enemy.armor.ToString();
            enemyArmorImage.enabled = true;
            enemyArmorCount.enabled = true;
        }
    }

    public void SetEnemyHp()
    {
        if (enemyHpSlider.maxValue != enemy.maxHp)
        {
            enemyHpSlider.maxValue = enemy.maxHp;
        }
        enemyHpSlider.value = enemy.currentHp;
    }

    public void EnableHealImage()
    {
        enemyHealImage.enabled = true;
    }

    public void DisableHealImage()
    {
        enemyHealImage.enabled = false;
    }

    public void EnableTargetIndicator(PlayerCharacter target = null, string damage = null)
    {
        var characterDisplays = FindObjectsOfType<CharacterDisplay>();
        if (target == null)
        {
            foreach (CharacterDisplay display in characterDisplays)
            {
                display.EnableTargetIndicator(damage);
            }
        }
        else
        {
            foreach (CharacterDisplay display in characterDisplays)
            {
                if (display.character == enemy.target)
                {
                    display.EnableTargetIndicator(damage);
                }
            }
        }
    }

    public void DisableTargetIndicator()
    {
        var characterDisplays = FindObjectsOfType<CharacterDisplay>();
        foreach (CharacterDisplay display in characterDisplays)
        {
            display.DisableTargetIndicator();
        }
    }

    public void EnemyDeath()
    {
        StartCoroutine(Dying());
    }
    public IEnumerator Dying()
    {
        ShowBattleText(enemy.enemyName + " has been slain!");
        enemyDefeated = true;
        yield return StartCoroutine(Blink(enemy.attackColor));
    }

    public void GameOver()
    {
        FindObjectOfType<SceneLoader>().LoadSpecificScene("GameOver");
    }

    public void ShowBattleText(string text)
    {
        state = BattleState.TEXT;

        battleInfo.gameObject.SetActive(true);
        battleText.gameObject.SetActive(true);
        battleTextContinueButton.gameObject.SetActive(true);

        battleText.text = text;
    }

    public void HideBattleText()
    {
        battleInfo.gameObject.SetActive(false);
        battleText.gameObject.SetActive(false);
        battleTextContinueButton.gameObject.SetActive(false);

        state = BattleState.PLAYERTURN;

        if (enemyDefeated == true)
        {
            FindObjectOfType<SceneLoader>().LoadNextScene();
        }
    }


}