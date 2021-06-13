using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
// Characters now instantiate at the start of the script, need to edit script to manage the three decks separately.
public enum BattleState { START, PLAYERTURN, TARGETING, ENEMYTURN, ESCAPE, VICTORY, DEFEAT, RUN }

public class BattleManager : MonoBehaviour
{
    //config variables
    [SerializeField] GameObject characterTemplate;
    public List<PlayerCharacter> party = new List<PlayerCharacter>();
    [SerializeField] GameObject characterArea;

    public SFX_Playing sfx;

    [SerializeField] Enemy enemy;
    [SerializeField] GameObject enemyArea;
    [SerializeField] Image enemyBleedImage;
    [SerializeField] Text enemyBleedCount;
    [SerializeField] Image enemyArmorImage;
    [SerializeField] Text enemyArmorCount;
    [SerializeField] Slider enemyHpSlider;
    [SerializeField] Image enemyHealImage;


    [SerializeField] Button targetButton;
    private List<Button> targetButtons = new List<Button>();

    [SerializeField] Button endTurnButton;

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

    Card playedCard;
    PlayerCharacter playedCardOwner;

    //state variables
    BattleState state;
    bool targetSelected = false;


    void Start()
    {
        state = BattleState.START;
        StartCoroutine(SetupBattle());
        enemyHealImage.enabled = false;
    }

    public void Test(string someText)
    {
        Debug.Log("ping");
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
        var characterDisplays = FindObjectsOfType<CharacterDisplay>();
        foreach (CharacterDisplay display in characterDisplays)
        {
            if (display.character == enemy.target)
            {
                display.EnableTargetIndicator();
            }
        }
    }

    private void InstantiateEnemies()
    {
        Instantiate(enemy, new Vector3(0, 0, 0), Quaternion.identity, enemyArea.transform);
        
    }

    private IEnumerator InstantiateCharacters(PlayerCharacter partyMember)
    {
        GameObject newCharacter = Instantiate(characterTemplate, new Vector3(0, 0, 0), Quaternion.identity);
        newCharacter.GetComponent<CharacterDisplay>().character = partyMember;
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
            newCard.GetComponent<CardDisplay>().card = card;
            newCard.GetComponent<CardDisplay>().SetColor(partyMember.cardColor);
            newCard.GetComponent<CardDisplay>().owner = partyMember;
            newCard.transform.SetParent(deckArea.transform, false);
            partyMember.deck.Add(newCard);
        }
    }

    public void Execute(GameObject cardToExecute)
    {
        playedCard = cardToExecute.GetComponent<CardDisplay>().card;
        playedCardOwner = cardToExecute.GetComponent<CardDisplay>().owner;
        StartCoroutine(TargetAndInvoke(cardToExecute));
    }

    private IEnumerator TargetAndInvoke(GameObject cardToExecute)
    {
        if (playedCardOwner.actions >= 1)
        {
            if (playedCard.isTargeted)
            {
                yield return StartCoroutine(SelectTarget());
            }
            foreach (string method in playedCard.methodList)
            {
                Invoke(method, 0);
            }
            if (doubleStrike)
            {
                if (playedCard.isTargeted)
                {
                    yield return StartCoroutine(SelectTarget());
                }
                foreach (string method in playedCard.methodList)
                {
    
                    Invoke(method, 0);
                }
                doubleStrike = false;
            }
            playedCardOwner.actions -= 1;
            DiscardCard(cardToExecute);
            targetSelected = false;
        }
        else
        {
            cardToExecute.transform.SetParent(cardToExecute.GetComponent<CardDisplay>().owner.playerArea.transform, false);
        }
        
    }

    private IEnumerator SelectTarget()
    {
        state = BattleState.TARGETING;
        var characterDisplays = FindObjectsOfType<CharacterDisplay>();
        foreach (CharacterDisplay display in characterDisplays)
        {
            var characterPos = display.gameObject.transform.position;
            Button button = Instantiate(targetButton, characterPos, Quaternion.identity);
            button.transform.SetParent(FindObjectOfType<Canvas>().transform, true);
            button.GetComponent<TargetButton>().targetCharacterName = display.character.characterName;
            targetButtons.Add(button);
        }
        yield return new WaitUntil(() => targetSelected);
        state = BattleState.PLAYERTURN;
    }

    public void SetTarget(string target)
    {
        playedCard.target = target;
        targetSelected = true;
        foreach (Button button in targetButtons)
        {
            Destroy(button.gameObject);
        }
        targetButtons.Clear();
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
            if ((partyMember.characterName == playedCard.target)|(playedCard.target == "all"))
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
            Debug.Log(playedCard.target);
            if ((partyMember.characterName == playedCard.target)||(playedCard.target == "all"))
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
            if ((partyMember.characterName == playedCard.target)|(playedCard.target == "all"))
            {
                partyMember.ActionAdd(playedCard.actionAdded);
            }
        }
    }
    private void BleedToDamage()
    {
        enemy.TakeDamage(enemy.bleed);
    }
    private void BleedToArmor()
    {
        foreach (PlayerCharacter partyMember in party)
        {
            if ((partyMember.characterName == playedCard.target)|(playedCard.target == "all"))
            {
                partyMember.Armor(enemy.bleed);
            }
        }
    }
    private void BleedToBleed()
    {
        enemy.Bleed(enemy.bleed*2);
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
    private void Delay()
    {
        
    }

    //Enemy Turn Coroutine
    private IEnumerator EnemyTurn()
    {
        yield return new WaitForSeconds(.5f);
        enemy.DoBehavior();
        var characterDisplays = FindObjectsOfType<CharacterDisplay>();
        foreach (CharacterDisplay display in characterDisplays)
        {
            display.DisableTargetIndicator();
            enemyHealImage.enabled = false;
        }
        enemy.SetBehavior();
        
        foreach (CharacterDisplay display in characterDisplays)
        {
            if (display.character == enemy.target)
            {
                display.EnableTargetIndicator();
            }
        }
            yield return new WaitForSeconds(.5f);
    }

    //Turn Process Functions
    public void EndTurn()
    {
        StartCoroutine(EnemyTurn());
        foreach (PlayerCharacter partyMember in party)
        {
            partyMember.EndTurn();
        }
        enemy.EndTurn();
        Debug.Log(enemy.currentHp.ToString());
        //Discards current hand before drawing a new one
        DiscardHand();
        doubleStrike = false;
        //reset active cards to recieve a new hand
        activeCards.Clear();
        //Draws a fresh hand of cards
        DrawNewHand();
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
        for (var i = 0; i < numCards; i++)
        {
            //check if deck is empty, if so shuffle
            if (activeDeck.Count <= 0)
            {
                shuffle(activeDeck, partyMember.discardPile);
            }

            int randNum = Random.Range(0, activeDeck.Count);
            GameObject drawnCard = activeDeck[randNum];
            drawnCard.transform.SetParent(drawnCard.GetComponent<CardDisplay>().owner.playerArea.transform, false);
            activeCards.Add(activeDeck[randNum]);
            activeDeck.RemoveAt(randNum);
        }
    }

    //shuffle currently broken because drawing currently attaches the scriptable object to the card template prefab, need to do that in setup instead of draw, that way we have a list of GameObjects that draw pulls from instead of cards, because discard is a list of Gameobjects.  
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
        if(enemyHpSlider.maxValue != enemy.maxHp)
        {
            enemyHpSlider.maxValue = enemy.maxHp;
        }
        enemyHpSlider.value = enemy.currentHp;
    }

    public void EnableHealImage()
    {
        enemyHealImage.enabled = true;
    }
}