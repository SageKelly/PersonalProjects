using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace One_of_Us
{
    /// <summary>
    /// Contains a list of steps a card must take to completion
    /// </summary>
    public class Process
    {
        public bool b_in_process;
        public Step CurrentStep, FirstStep, LastStep;
        private Process nextProcess;
        public Process PreviousProcess;
        public Process NextProcess
        {
            get
            {
                return nextProcess;
            }
            private set
            {
                nextProcess = value;
                if (PAEvent != null)
                    PAEvent(nextProcess);
            }
        }
        bool b_complete, b_cancelled;

        public delegate void ProcessAdded(Process p);
        /// <summary>
        /// Handles a process being completed
        /// </summary>
        public event CompletionHandler ProcessCompletionEvent;
        /// <summary>
        /// Handles a process being cancelled
        /// </summary>
        public event CancellationHandler ProcessCancellationEvent;
        /// <summary>
        /// Handles when a process has been added to this process
        /// </summary>
        public event ProcessAdded PAEvent;

        /// <summary>
        /// Creates a process
        /// </summary>
        /// <param name="firstStep">The first step of the process</param>
        public Process(Step firstStep)
        {
            FirstStep = firstStep;
            CurrentStep = firstStep;
            SetupProcedure();
        }

        private bool Complete
        {
            get
            {
                return b_complete;
            }
            set
            {
                b_complete = value;
                OnComplete();
            }
        }

        private bool Cancelled
        {
            get
            {
                return b_cancelled;
            }
            set
            {
                b_cancelled = value;
                if (b_cancelled)
                    OnCancellation();
            }
        }

        public void UpdateProcedure(object sender)
        {
            CurrentStep = CurrentStep.NextStep;
        }

        public void UpdateProcedure()
        {
            CurrentStep.CompletionEvent -= UpdateProcedure;
            if (CurrentStep == LastStep)
                Complete = true;
            else
            {
                CurrentStep = CurrentStep.NextStep;
                CurrentStep.NextStep.CompletionEvent += new CompletionHandler(UpdateProcedure);
            }
            #region Old Code
            //FIX THIS
            /* i_step_counter++;
             if (i_step_counter != Steps.Count)
                 CurrentStep = Steps[i_step_counter];
             else
                 Complete = true;*/
            #endregion
        }

        private void ResetProcess(object sender)
        {
            CurrentStep = FirstStep;
            Complete = false;
            Cancelled = false;
        }

        public void AttachProcess(Process process)
        {
            NextProcess = new Process(process.FirstStep);
            nextProcess.PreviousProcess = this;
        }

        private void SetupProcedure()
        {
            ProcessCompletionEvent += new CompletionHandler(ResetProcess);
            FirstStep.CompletionEvent += new CompletionHandler(UpdateProcedure);
            CurrentStep.SAEvent+=new Step.StepAdded(UpdateLastStep);
        }

        private void OnComplete()
        {
            if (ProcessCompletionEvent != null && Complete)
                ProcessCompletionEvent(this);
        }

        private void OnCancellation()
        {
            if (ProcessCancellationEvent != null)
                ProcessCancellationEvent(this);
        }
        private void UpdateLastStep(Step s)
        {
            LastStep = s;
        }
    }
}
