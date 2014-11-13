using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace One_of_Us
{
    public delegate void CompletionHandler(object sender);
    public delegate void CancellationHandler(object sender);
    public delegate void SkipHandler(object sender);

    #region VARIABLES
    #region PUBLIC
    #region Primitives
    #endregion
    #region Objects
    #endregion
    #region Delegates/Events
    #endregion
    #endregion
    #region INTERNAL
    #region Primitives
    #endregion
    #region Objects
    #endregion
    #region Delegates/Events
    #endregion
    #endregion
    #region PRIVATE
    #region Primitives
    #endregion
    #region Objects
    #endregion
    #region Delegates/Events
    #endregion
    #endregion
    #endregion

    public class Deck : GameComponent
    {
        //SERIOUS OVERHAUL REQUIRED: MUST BE ABLE HANDLE THE CURRENT Card CLASS
        #region VARIABLES
        #region PUBLIC
        #region Primitives

        #endregion
        #region Objects
        /// <summary>
        /// The name speaks for itself. It's a list of 
        /// all the cards that have been played.
        /// </summary>
        public List<Card> CardsInPlay;
        /// <summary>
        /// The list of all possible cards in a deck
        /// </summary>
        public List<Card> TemplateCards;
        /// <summary>
        /// The shuffled version of the deck.
        /// </summary>
        public List<Card> ShuffledDeck
        {
            get;
            private set;
        }
        /// <summary>
        /// The discard deck
        /// </summary>
        public List<Card> Discard;
        /// <summary>
        /// A list of all the possible processes
        /// within this deck
        /// </summary>
        public List<Process> Processes;
        /// <summary>
        /// A list of all the possible steps
        /// within this deck.
        /// </summary>
        public List<Step> Steps;

        public Dictionary<Card, float> CardFrequencies;
        #endregion
        #region Delegates/Events
        public delegate void MouseClickHandler(MouseState MS);
        public delegate void KeyPressHandler(KeyboardState KS);


        public event MouseClickHandler MCEvent;
        public event KeyPressHandler KPEvent;
        #endregion
        #endregion

        #region INTERNAL
        #region Primitives
        #endregion
        #region Objects
        #endregion
        #region Delegates/Events
        #endregion
        #endregion

        #region PRIVATE
        #region Primitives
        private int deckSize, deckDelta, dealSize;
        private bool isSetup;
        #endregion
        #region Objects
        private MouseState CurMState;
        private MouseState PrevMState = Mouse.GetState();
        private KeyboardState CurKState;
        private KeyboardState PrevKState = Keyboard.GetState();

        private Game mainGame;
        #endregion
        #region Delegates/Events
        #endregion
        #endregion
        #endregion

        public Deck(Game game)
            : base(game)
        {
            TemplateCards = new List<Card>();
            deckDelta = 0;
            deckSize = 100;
            isSetup = false;
            mainGame = game;
        }

        public Deck(Game game, int deckSize, int dealSize)
            : this(game)
        {
            this.deckSize = deckSize;

        }

        #region Methods

        #region Class-based Methods

        /// <summary>
        /// Populates and shuffles the game deck
        /// </summary>
        public void SetupDeck()
        {
            //Use frequencies to fill a temporary deck
            List<Card> TempDeck = new List<Card>();
            foreach (Card c in TemplateCards)
            {
                int card_amount = (int)(deckSize * c.DeckFrequency);
                for (int i = 0; i < card_amount; i++)
                {
                    TempDeck.Add(new Card(c.Name, c.CP, c.DeckFrequency));
                }
            }

            //Shuffle deck
            for (int i = 0; i < 2; i++)
            {
                Shuffle(ref TempDeck);
            }

            ShuffledDeck = TempDeck;
            isSetup = true;
            //Clean-up
            TempDeck.Clear();
        }

        /// <summary>
        /// Shuffles the deck
        /// </summary>
        /// <param name="deck">The deck the shuffle</param>
        /// <returns>a shuffled version of the deck</returns>
        public void Shuffle(ref List<Card> deck)
        {
            Random r = new Random();
            int half = 0, LorR = 0, Alt = 1, Cards = 0, counter = -1;
            //Break the deck in roughly half
            half = deck.Count / 2 - r.Next(0, 7);

            List<Card> Deck1 = deck.GetRange(0, half);
            List<Card> Deck2 = deck.GetRange(half, deck.Count - half);

            List<Card> ResultDeck = new List<Card>();
            List<List<Card>> Decks = new List<List<Card>>();
            Decks.Add(Deck1);
            Decks.Add(Deck2);
            //Reverse the decks so that the bottoms are on top for easier access
            Deck1.Reverse();
            Deck2.Reverse();
            //Throws the deck back together into the shuffled deck
            while (Decks[LorR].Count != 0 || Decks[Alt].Count != 0)
            {
                counter++;
                /*
                 * first decide if it's the left deck (0)
                 * or right deck (1).Then continuously 
                 * shift between both decks, taking a few 
                 * cards from each until both decks are empty
                 */
                if (counter == 0)
                {
                    LorR = r.Next(0, 2);
                }
                else
                {
                    LorR = counter % 2;
                }
                Alt = Math.Abs((LorR - 1) % 2);
                Cards = r.Next(2, 4);
                //As long as the amount you want to take is there...
                if (Decks[LorR].Count >= Cards)
                {
                    //...take it,
                    ResultDeck.InsertRange(0, Decks[LorR].Take(Cards));
                    Decks[LorR].RemoveRange(0, Cards);
                }
                else//...
                {
                    //...take what's left
                    ResultDeck.InsertRange(0, Decks[LorR].Take(Decks[LorR].Count));
                    Decks[LorR].RemoveRange(0, Decks[LorR].Count);
                }
            }
            deck = ResultDeck;
        }


        private void ResetDeck()
        {
            ShuffledDeck.Clear();
            SetupDeck();
        }

        /// <summary>
        /// Deals a certain amount of cards
        /// </summary>
        /// <param name="amount">The amount of cards</param>
        /// <returns>The dealt cards</returns>
        public List<Card> Deal(int amount)
        {
            List<Card> Hand = new List<Card>();
            if (ShuffledDeck.Count >= amount)
            {
                Hand = (List<Card>)ShuffledDeck.Take(amount);
                ShuffledDeck.RemoveRange(0, amount);
            }
            else
            {
                Hand = (List<Card>)ShuffledDeck.Take(ShuffledDeck.Count);
                ShuffledDeck.Clear();
            }
            return Hand;
        }


        /// <summary>
        /// Adds a card to the list of template cards
        /// </summary>
        /// <param name="card"></param>
        public void AddCard(Card card)
        {
            TemplateCards.Add(card);
        }

        private void DiscardCard(object sender)
        {
            if (sender == typeof(Card))
                Discard.Insert(0, (Card)sender);
        }

        public void ShuffleFromDiscard()
        {
            ShuffledDeck = Discard;
            Shuffle(ref ShuffledDeck);
        }

        /*
        /// <summary>
        /// Creates Deck from a .txt file. It MUST be outline as such: 
        /// for each line, the card name is first, followed by a space,
        /// then the percentage of frequency of that card in decimal format
        /// </summary>
        /// <param name="directory">
        /// The full path of the .txt file
        /// </param>
        public void FetchDeck(string directory)
        {

        }

        /// <summary>
        /// Creates Deck from a Dictionary, containing the distinct list of cards 
        /// possible to appear in the deck.
        /// </summary>
        /// <param name="Cards">
        /// The Dictionary containing the list of cards. 
        /// The name of the card as a key, and the percentage frequency
        /// of the card as the definition, in decimal format
        /// </param>
        public void FetchDeck(Dictionary<string, float> Cards, int DeckSize)
        {

        }
        */

        /*
        private bool CheckFrequencies()
        {
            float f_perc_sum = 0.0f;
            float f_tolerance = 0.100000000f;
            for (int i = 0; i < i_deck_size; i++)
            {
                f_perc_sum += Cards[i].f_deck_frequency;
            }
            if (f_perc_sum + f_tolerance >= 1)
                return true;
            return false;
        }
        */
        #endregion

        #region Event-based Methods
        private void UpdateMouse(GameTime gameTime)
        {
            CurMState = Mouse.GetState();
            if (CurMState.LeftButton == ButtonState.Released &&
               PrevMState.LeftButton == ButtonState.Pressed)
            {
                if (MCEvent != null)
                    MCEvent(CurMState);
            }
            PrevMState = CurMState;
        }

        /// <summary>
        /// Raises KPEvents
        /// </summary>
        /// <param name="gameTime"></param>
        private void UpdateKeyBoard(GameTime gameTime)
        {
            CurKState = Keyboard.GetState();
            if (CurKState.GetPressedKeys().Length > PrevKState.GetPressedKeys().Length)
            {
                if (KPEvent != null)
                    KPEvent(CurKState);
            }
            PrevKState = CurKState;
        }

        public void PlayCard()
        {
            Card card = ShuffledDeck[0];
            card.CardCompletionEvent += new CompletionHandler(DiscardCard);
            CardsInPlay.Add(card);
            ShuffledDeck.RemoveAt(0);
        }

        /// <summary>
        /// Checks each card in play for mouse-click events
        /// </summary>
        /// <param name="MS">The necessary MouseState</param>
        private void CheckCardClick(MouseState MS)
        {
            foreach (Card C in CardsInPlay)
            {
                C.CurProcess.CurrentStep.MCH(MS);
            }
        }

        /// <summary>
        /// Checks each card in play for a key-press event
        /// </summary>
        /// <param name="KS">The necessary keyboard state</param>
        private void CheckCardKeyPress(KeyboardState KS)
        {
            foreach (Card C in CardsInPlay)
            {
                C.CurProcess.CurrentStep.KEH(KS);
            }
        }
        #endregion


        #region GameComponent Methods

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            UpdateMouse(gameTime);
            UpdateKeyBoard(gameTime);
        }
        #endregion
        #endregion
    }
}
