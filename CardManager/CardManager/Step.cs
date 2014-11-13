using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace One_of_Us
{
    /// <summary>
    /// A singular task for a particular process
    /// </summary>
    public class Step
    {
        /*
         * Steps are seperate event checkers within procedures, each
         * of them containing their own special method that takes in
         * either a MouseState or KeyboardState. It runs these
         * whenever their designated trigger methods (OnMouseClick, 
         * OnKeyPress) are called. After which, the step is either
         * determined confirmed or cancelled based on the method's
         * construction. BOTH STATES MUST BE HANDLED IN THE
         * METHODS APPENDED TO THE DELEGATES. Once
         * confirmed/cancelled the step will either move to the next
         * step, or alert its procedure to be dropped.
         */
        public event KeyboardEventHandler KeyboardEvent;
        public event MouseClickHandler MouseEvent;
        /*
         * This raises an event to the subscriber, alerting them of this step's completion.
         * If used with the Procedure class, it will notify the holding Procedure to check
         * its current step for completion and update its current step.
         */
        public event CompletionHandler CompletionEvent;
        public event CancellationHandler CancellationEvent;
        public event SkipHandler SkipEvent;//To be defined when necessary
        public event StepAdded SAEvent;

        public delegate void KeyboardEventHandler(KeyboardState KS);
        public delegate void MouseClickHandler(MouseState MS);
        public delegate void StepAdded(Step s);

        public MouseClickHandler MCH;
        public KeyboardEventHandler KEH;

        public Step PreviousStep, nextStep;
        public Step NextStep
        {
            get
            {
                return nextStep;
            }
            set
            {
                nextStep = value;
                if (SAEvent != null)
                    SAEvent(this);
            }
        }

        public bool b_complete;
        public bool b_is_cancelled;
        public bool HasRun;

        public bool Complete
        {
            get
            {
                return b_complete;
            }
            set
            {
                b_complete = value;
                if (b_complete)
                {
                    HasRun = true;
                    OnCompletion();
                }
            }
        }
        public bool Cancelled
        {
            get
            {
                return b_is_cancelled;
            }
            private set
            {
                b_is_cancelled = value;
                if (b_is_cancelled)
                    OnCancellation();
            }

        }

        #region Constructors
        /// <summary>
        /// Creates a basic step within a procedure
        /// </summary>
        public Step()
        {
            ResetStep(this);
            SetupStep();
        }

        /// <summary>
        /// Creates a basic step within a procedure
        /// </summary>
        /// <param name="MEH">A method to call for a mouse event</param>
        public Step(MouseClickHandler MEH)
            : this()
        {
            MouseEvent += MEH;
        }

        /// <summary>
        /// Creates a basic step within a procedure
        /// </summary>
        /// <param name="KEH">A method to call for the a keyboard event</param>
        public Step(KeyboardEventHandler KEH)
            : this()
        {
            KeyboardEvent += KEH;
        }
        #endregion

        #region Methods
        #region Event-Based
        public void OnMouseClick(MouseState M)
        {
            if (MouseEvent != null)
                MouseEvent(M);
        }

        public void OnKeyPress(KeyboardState KS)
        {
            if (KeyboardEvent != null)
                KeyboardEvent(KS);
        }

        private void OnCompletion()
        {
            if (CompletionEvent != null)
                CompletionEvent(this);
        }

        private void OnCancellation()
        {
            if (CancellationEvent != null)
                CancellationEvent(this);
        }
        #endregion
        #region Standard
        /// <summary>
        /// Adds the next step to this object
        /// </summary>
        /// <param name="s">the next step</param>
        public void AttachStep(Step s)
        {
            nextStep = new Step(s.KEH);
            nextStep.MCH = s.MCH;
            nextStep.PreviousStep = this;
        }

        private void ResetStep(object sender)
        {
            if (sender == this)
            {
                b_complete =
                    b_is_cancelled =
                    false;
            }
        }
        private void SetupStep()
        {
            CompletionEvent += new CompletionHandler(ResetStep);
            HasRun = false;
        }
        private void UpdateStep()
        {
        }
        #endregion
        #endregion
    }
}
