using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
// Characters now instatiate at the start of the script, need to edit script to manage the three decks separately.
public enum BattleState { START, PLAYERTURN, ENEMYTURN, ESCAPE, VICTORY, DEFEAT, RUN }

public class BattleManager : MonoBehaviour
{
    //config variables
    [SerializeField] GameObject characterTemplate;
    [SerializeField] List<PlayerCharacter> party = new List<PlayerCharacter>();
    [SerializeField] GameObject characterArea;

    [SerializeField] Enemy enemy;
    [SerializeField] GameObject enemyArea;

    [SerializeField] int handSize;
    [SerializeField] GameObject cardTemplate;
    private List<Card> deck = new List<Card>();
    private List<GameObject> activeCards = new List<GameObject>();
    private List<Card> discardPile = new List<Card>();

    public GameObject playerArea;
    public GameObject deckArea;
    public GameObject dropZone;
    public GameObject discardZone;
    

    Card playedCard;
    PlayerCharacter playedCardOwner;

    //state variables
    BattleState state;


    void Start()
    {
        state = BattleState.START;
        SetupBattle();
    }

    public void Test(string someText)
    {
        Debug.Log("ping");
    }

    void SetupBattle()
    {
        //instatiate player characters and generate decks
        foreach (PlayerCharacter partyMember in party)
        {
            //clears discardpile and deck in case there is leftover data before setting up this instance of the characters
            partyMember.discardPile.Clear();
            partyMember.deck.Clear();

            InstatiateCharacters(partyMember);
            InstatiateCards(partyMember);
        }

        InstatiateEnemies();
        DrawNewHand();
    }

    private void InstatiateEnemies()
    {
        Instantiate(enemy, new Vector3(0, 0, 0), Quaternion.identity, enemyArea.transform);
    }

    private void InstatiateCharacters(PlayerCharacter partyMember)
    {
        GameObject newCharacter = Instantiate(characterTemplate, new Vector3(0, 0, 0), Quaternion.identity);
        newCharacter.GetComponent<CharacterDisplay>().character = partyMember;
        newCharacter.transform.SetParent(characterArea.transform, false);
    }

    private void InstatiateCards(PlayerCharacter partyMember)
    {
        //instatiates card templates and applys card data to them, marking each card as "owned" by a particular character so we can access their specific discard pile
        foreach (Card card in partyMember.deckData)
        {
            GameObject newCard = Instantiate(cardTemplate, new Vector3(0, 0, 0), Quaternion.identity);
            newCard.GetComponent<CardDisplay>().card = card;
            newCard.GetComponent<CardDisplay>().owner = partyMember;
            newCard.transform.SetParent(deckArea.transform, false);
            partyMember.deck.Add(newCard);
        }
    }

    public void Execute(GameObject cardToExecute)
    {
        playedCard = cardToExecute.GetComponent<CardDisplay>().card;
        playedCardOwner = cardToExecute.GetComponent<CardDisplay>().owner;
        if (playedCardOwner.actions >= 1)
        {
            foreach (string method in playedCard.methodList)
            {
                
                Invoke(method, 0);
            }
            playedCardOwner.actions -= 1;
            DiscardCard(cardToExecute);
        }
    }

    //Card Methods
    private void CardDraw()
    {
        Draw(playedCardOwner, playedCard.cardsToDraw, playedCardOwner.deck);
    }
    private void Heal()
    {
        foreach (PlayerCharacter partyMember in party)
        {
            if ((partyMember.characterName == playedCard.target)|(playedCard.target == "all"))
            {
                partyMember.Heal(playedCard.healingDone);
            }

        }
    }    
    private void Damage()
    {
        enemy.TakeDamage(playedCard.damageDealt);
    }
    private void Armor()
    {
        foreach (PlayerCharacter partyMember in party)
        {
            if ((partyMember.characterName == playedCard.target)|(playedCard.target == "all"))
            {
                partyMember.Armor(playedCard.armorAdded);
            }

        }
    }
    private void Bleed()
    {
        enemy.Bleed(playedCard.bleedAdded);
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
    private void GetBleed()
    {
        playedCard.damageDealt = enemy.bleed;
        playedCard.armorAdded = enemy.bleed;
        playedCard.bleedAdded = enemy.bleed;
    }
    private void ChangeTarget()
    {
        if (enemy.target != "all")
        {
            enemy.target = playedCard.target;
        }
    }
    private void CheckArmored()
    {
        if (playedCardOwner.armor <= 0)
        {
            playedCard.damageDealt = 0;
        }
    }
    private void SelectTarget()
    {
        Debug.Log ("starting the select target");
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        Debug.Log ("got past raycast stuff");
        if(Physics.Raycast (ray, out hit))
        {
            if(hit.transform.name == "Shield Area")
            {
                Debug.Log ("Clicked on Shield");
                playedCard.target = "shield";
            }
            else if(hit.transform.name == "Mark Area")
            {
                Debug.Log ("Clicked on Mark");
                playedCard.target = "mark";
            }
            else if(hit.transform.name == "Sword Area")
            {
                Debug.Log ("Clicked on Sword");
                playedCard.target = "sword";
            }
            else
            {
                Debug.Log("ya missed");
            }
        }
    }
    
    public void EndTurn()
    {
        foreach (PlayerCharacter partyMember in party)
        {
            partyMember.EndTurn();
            Debug.Log(partyMember.actions.ToString());
        }
        enemy.EndTurn();
        Debug.Log(enemy.currentHp.ToString());
        //Discards current hand before drawing a new one
        DiscardHand();
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
            activeDeck[randNum].transform.SetParent(playerArea.transform, false);
            activeCards.Add(activeDeck[randNum]);
            activeDeck.RemoveAt(randNum);
        }
    }

    //shuffle currently broken because drawing currently attaches the scriptable object to the card template prefab, need to do that in setup instead of draw, that way we have a list of GameObjects that draw pulls from instead of cards, because discard is a list of Gameobjects.  
    public void shuffle(List<GameObject> emptyDeck, List<GameObject> fullDiscard)
    {
        emptyDeck.AddRange(fullDiscard);
        fullDiscard.Clear();
    }
}
