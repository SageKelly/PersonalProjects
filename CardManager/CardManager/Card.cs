using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace One_of_Us
{

    public class Card : DrawableGameComponent
    {
        //TODO: Allow to make a description for the Card
        //TODO: Allow to toggling of drawing description
        #region VARIABLES
        #region PUBLIC

        #region Primitives
        public string Name
        {
            get;
            private set;
        }

        /// <summary>
        /// Denotes whether or not the card has been played
        /// </summary>
        public bool HasRun;

        /// <summary>
        /// Denotes whether or not the card has been fully played
        /// </summary>
        public bool Complete
        {
            get
            {
                return b_complete;
            }
            private set
            {
                b_complete = value;
                if (b_complete)
                {
                    HasRun = true;
                    OnComplete();
                }
            }
        }

        /// <summary>
        /// Denotes whether or not the card has been cancelled
        /// </summary>
        public bool Cancelled
        {
            get
            {
                return b_cancel;
            }
            private set
            {
                b_cancel = value;
                if (b_cancel)
                    OnCancel();
            }
        }


        public string ChangerName;

        /// <summary>
        /// How percentage rate the card appears in the deck
        /// </summary>
        public float DeckFrequency
        {
            get;
            private set;
        }

        #endregion
        #region Objects
        /// <summary>
        /// The method registered to the card
        /// </summary>
        public CardPlayer CP;
        /*
         * This is used for remembering what particular process
         * is currently running at the time.
         */
        public Process CurProcess, FirstProcess, LastProcess;
        #endregion
        #region Delegates/Events
        /*
         * This will be used to play a method that either "sets 
         * up" the card or whatever is happening in the main game.
         * It does not have to be used.
         */
        public delegate void CardPlayer(object sender, object receiver);
        public event CompletionHandler CardCompletionEvent;
        public event CancellationHandler CardCancellationEvent;
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
        bool b_complete, b_cancel;
        #endregion
        #region Objects
        /*
         * These will be used for the general look of the card,
         * and for anything that the card may need to be the
         * mose accessible.
         */
        private Rectangle tempPiece, hitBounds, srcBounds;
        private Vector2 position, velocity, acceleration;
        private Texture2D image;
        private Game mainGame;
        #endregion
        #region Delegates/Events
        #endregion
        #endregion
        #endregion

        /// <summary>
        /// Creates a card with a given event method
        /// </summary>
        /// <param name="game">The game in which this will be used</param>
        /// <param name="name">The name of the card</param>
        /// <param name="method">The method to be run once the card is played</param>
        /// <param name="deck_frequency">The frequency this card appears in a
        /// deck, in demical percentage format (e.g. 10% = .1)</param>
        public Card(Game game, string name, CardPlayer method, float deck_frequency)
            : base(game)
        {
            mainGame = game;
            Name = name;
            CP += method;
            DeckFrequency = deck_frequency;
            HasRun = false;
        }

        /// <summary>
        /// Creates a multi-step card
        /// </summary>
        /// <param name="game">The game in which this will be used</param>
        /// <param name="name">The name of the card</param>
        /// <param name="firstProcess">The first process for this card</param>
        /// <param name="deck_frequency">The frequency this card appears in a
        /// deck, in demical percentage format (e.g. 10% = .1)</param>
        public Card(Game game, string name, Process firstProcess, float deck_frequency)
            : base(game)
        {
            mainGame = game;
            Name = name;
            FirstProcess = firstProcess;
            CurProcess = firstProcess;
            CurProcess.PAEvent += new Process.ProcessAdded(UpdateLastProcess);
            DeckFrequency = deck_frequency;
            HasRun = false;
        }

        #region Methods
        #region Card-based Methods
        /// <summary>
        /// Allows playing the effect of a player's card onto another player
        /// </summary>
        /// <param name="Sender">the object playing the card</param>
        /// <param name="Receiver">the object receiving the effect of the played card</param>
        public void Play(object Sender, object Receiver)
        {
            CP(Sender, Receiver);
        }

        #endregion
        #region Event-based Methods

        private void UpdateCard(object sender)
        {
            if (sender == typeof(Card))
            {
                CurProcess.ProcessCompletionEvent -= UpdateCard;
                if (CurProcess == LastProcess)
                    Complete = true;
                else
                {
                    CurProcess = CurProcess.NextProcess;
                    CurProcess.ProcessCompletionEvent += new CompletionHandler(UpdateCard);
                }
            }
        }

        private void OnComplete()
        {
            if (CardCompletionEvent != null)
                CardCompletionEvent(this);
            Complete = false;
        }

        private void OnCancel()
        {
            if (CardCancellationEvent != null)
                CardCancellationEvent(this);
            Cancelled = false;
        }

        private void UpdateLastProcess(Process p)
        {
            LastProcess = p;
        }
        #endregion
        #endregion
    }
}
