using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QuizMaker
{
    class QuizText
    {
        public bool TermUsed, DefUsed, HasRead;//For checking if the question has been used or its answer has been read during quiz creation
        public string Term
        {
            get;
            private set;
        }//the term for term-definition pairs
        public string Definition
        {
            get;
            private set;
        }//The answer for term-definition pairs
        public string[] BuzzWords
        {
            get;
            private set;
        }//Buzzwords used for answer comparison and FTB questions
        public string[] Definitions
        {
            get;
            private set;
        }//used to store the actual answers in case of FTB or pac (pick-and-choose)
        public bool IsSingular
        {
            get;
            private set;
        }
        public bool IsPast
        {
            get;
            private set;
        }
        public string TermQuestion;//Used after identifying what type of term this is
        public string DefQuestion;//Used after identifying what type of term this is
        public string[] TermAnswers;//used for the final question
        public string[] DefAnswers;//used for the final question
        public int CORRECT_TERM = 4, CORRECT_DEF = 4;
        public const int CHOICE_SIZE = 5;
        public QuizTypes[] Types;
        public bool IsFTB, IsMulti, IsVocab, IsWAT;
        public enum TermTypes
        {
            Action,
            Event,
            Idea,
            Object,
            Person,
        }
        public enum QuizTypes
        {
            FTB,
            Multi,
            Vocab,
            WAT
        }
        public TermTypes TermType;

        //Precondition: All delimiting is done before reaching these constructors
        public QuizText(string term, string definition, string termType, bool is_singular, bool is_past, string[] quizTypes)
        {
            IsFTB = IsMulti = IsVocab = IsWAT = false;
            TermUsed = DefUsed = HasRead = false;
            BuzzWords = new string[0];
            Definitions = new string[0];
            DefAnswers = new string[CHOICE_SIZE];
            TermAnswers = new string[CHOICE_SIZE];
            Types = new QuizTypes[0];
            Term = term;
            Definition = definition;
            TranslateTermType(termType);
            TranslateQuizTypes(quizTypes);
            IsSingular = is_singular;
            IsPast = is_past;
        }

        public QuizText(string term, string definition, string termType, bool is_singular, bool is_past, string[] quizTypes, string[] buzzwords)
            : this(term, definition, termType, is_singular, is_past, quizTypes)
        {
            BuzzWords = buzzwords;
        }

        public QuizText(string term, List<string> definitions, string termType, bool is_singular, bool is_past, string[] quizTypes)
        {
            IsFTB = IsMulti = IsVocab = IsWAT = false;
            TermUsed = DefUsed = HasRead = false;
            BuzzWords = new string[0];
            Definitions = new string[0];
            DefAnswers = new string[CHOICE_SIZE];
            TermAnswers = new string[CHOICE_SIZE];
            Types = new QuizTypes[0];
            Term = term;
            Definitions = definitions.ToArray<string>();
            TranslateTermType(termType);
            TranslateQuizTypes(quizTypes);
            IsSingular = is_singular;
            IsPast = is_past;
        }
        public QuizText(string term, List<string> definitions, string termType, bool is_singular, bool is_past, string[] quizTypes, string[] buzzwords)
            : this(term, definitions, termType, is_singular, is_past, quizTypes)
        {
            BuzzWords = buzzwords;
        }

        private void TranslateTermType(string type)
        {
            switch (type.ToUpper())
            {
                case "ACTION":
                    TermType = TermTypes.Action;
                    break;
                case "EVENT":
                    TermType = TermTypes.Event;
                    break;
                case "IDEA":
                    TermType = TermTypes.Idea;
                    break;
                case "OBJECT":
                    TermType = TermTypes.Object;
                    break;
                case "PERSON":
                    TermType = TermTypes.Person;
                    break;
            }
        }

        private void TranslateQuizTypes(string[] quiztypes)
        {
            int size = quiztypes.Length;
            Types = new QuizTypes[size];
            for (int i = 0; i < size; i++)
            {
                switch (quiztypes[i].ToUpper())
                {
                    case "MULTI":
                        Types[i] = QuizTypes.Multi;
                        IsMulti = true;
                        break;
                    case "FTB":
                        Types[i] = QuizTypes.FTB;
                        IsFTB = true;
                        break;
                    case "VOCAB":
                        Types[i] = QuizTypes.Vocab;
                        IsVocab = true;
                        break;
                    case "WAT":
                        Types[i] = QuizTypes.WAT;
                        IsWAT = true;
                        break;
                }
            }
        }

        /// <summary>
        /// Checks to see if the answer input is true
        /// </summary>
        /// <param name="Term">Determines if the term answer or the definition answer should be checked</param>
        /// <param name="Answer">The array-register-based input</param>
        /// <returns>Returns true if the answer is correct. It returns false
        /// if either it is incorrect or the answer input doesn't exist</returns>
        public bool CheckAnswer(bool Term, int Answer)
        {
            if (Term)
            {
                if (Answer == CORRECT_TERM)
                {
                    return true;
                }
                else if (Answer >= TermAnswers.Length || Answer != CORRECT_DEF)
                    return false;
            }
            else
            {
                if (Answer == CORRECT_DEF)
                {
                    return true;
                }
                else if (Answer >= DefAnswers.Length || Answer != CORRECT_DEF)
                    return false;
            }
            return false;
        }

    }
}
