using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using System.Xml;

namespace QuizMaker
{
    class Program
    {
        //Still a problem with the Listener method call.
        public struct TermDefAnswer
        {
            public int Question, TermOrDef, answer;
            public TermDefAnswer(int q, int tod)
            {
                answer = 0;
                Question = q;
                TermOrDef = tod;
            }
            public TermDefAnswer(int q, int tod, int a)
                : this(q, tod)
            {
                answer = a;
            }
        }
        static Random rand;
        static XmlDocument DocData;
        static string DefaultDir = "";
        static string Directory = "C:/Users/Kasha/Documents/World Politics/Quiz Meh!.xml";
        static ConsoleKeyInfo CKI;

        static List<QuizText> Questions;
        static List<List<string>> QuestionText;
        static List<string> Messages;
        static List<TermDefAnswer> TDAs;
        enum QuizStates
        {
            MainMenu,
            Load,
            Quiz,
            Review,
            FileLoc,
            Exit,
            Dummy
        }
        static QuizStates States;
        static void Main(string[] args)
        {
            Console.Title = "QUIZ MACHINE";
            ConsUI.SetupTyper(10);
            Questions = new List<QuizText>();
            Messages = new List<string>();
            TDAs = new List<TermDefAnswer>();
            QuestionText = new List<List<string>>();
            CKI = new ConsoleKeyInfo();
            DocData = new XmlDocument();
            DocData.Load(Directory);

            //Console.Write(DocData.FirstChild.InnerXml);


            //ConsoleUI.Print("....QUIZZZZZ MACHINEEEEE....", false, true);
            HandleStates();



            #region Temp Printing Code (*DEBUG*)
            /*int counter = 1;
            List<string> Info = new List<string>();
            foreach (QuizText QT in Questions)
            {
                Info.Add("Term: " + QT.Term);
                if (QT.Definitions.Length != 0)
                {
                    Info.Add("Definitions:");
                    int size = QT.Definitions.Length;
                    for (int i = 0; i < size; i++)
                        Info.Add("Definition " + (i + 1) + ": " + QT.Definitions[i].ToString());
                }
                else
                    Info.Add("Definition: " + QT.Definition);
                if (QT.BuzzWords.Length != 0)
                {
                    Info.Add("Buzzwords:");
                    int size = QT.BuzzWords.Length;
                    for (int i = 0; i < size; i++)
                        Info.Add("Buzzword " + (i + 1) + ": " + QT.BuzzWords[i].ToString());
                }
                int types_amount = QT.Types.Length;
                Info.Add("Quiz Types: ");
                for (int i = 0; i < types_amount; i++)
                {
                    Info.Add("Type " + (i + 1) + ": " + QT.Types[i].ToString());
                }
                Info.Add("");
                ConsoleUI.MultipleChoiceCK("Quiz Text " + counter + ": ", Info, true);
                Info.Clear();
                counter++;
            }*/
            #endregion
        }


        public static void HandleStates()
        {
            while (CKI.Key != ConsoleKey.Escape)
            {
                switch (States)
                {
                    case QuizStates.MainMenu:

                        //For right now, I'll load in one file

                        ConsUI.Print("Entering Quiz Machine...", true, false);
                        Messages.Add("  1. Execute Quiz");
                        Messages.Add("  2. Locate a File");
                        Messages.Add("  3. Exit Quiz Machine");
                        CKI = ConsUI.MultipleChoiceCK("Choose your state:", Messages, false);
                        Messages.Clear();
                        switch (CKI.Key)
                        {
                            case ConsoleKey.D1:
                            case ConsoleKey.NumPad1:
                                Questions.Clear();
                                QuestionText.Clear();
                                TDAs.Clear();
                                States = QuizStates.Load;
                                break;
                            case ConsoleKey.D2:
                            case ConsoleKey.NumPad2:
                                States = QuizStates.FileLoc;
                                break;
                            case ConsoleKey.D3:
                            case ConsoleKey.NumPad3:
                                States = QuizStates.Exit;
                                break;
                        }
                        break;
                    case QuizStates.Load:
                        ParseQuizXML(DocData);
                        ConsUI.Print("Quiz parsing finished.", false, true);
                        MakeQuestions();
                        int size = Questions.Count;
                        int size2 = size * 2;
                        bool full_lap = false, looped = false, terms_done = false, defs_done = false;
                        #region Randomly Grabs questions and adds them to a list of text to be printed
                        for (int question = 0; question < size2; question++)
                        {
                            int TermorDef;

                            if (terms_done)
                                TermorDef = 1;
                            else if (defs_done)
                                TermorDef = 0;
                            else
                                TermorDef = rand.Next(2);

                            int rand_index = 0, starting_index = 0;

                            rand_index = rand.Next(size);
                            starting_index = rand_index;
                            if (TermorDef == 0 && !terms_done)//Term Question will be used
                            {
                                while (Questions[rand_index].TermUsed)
                                {
                                    rand_index++;
                                    if (rand_index >= size)
                                    {
                                        rand_index = 0;
                                        looped = true;
                                    }
                                    if (rand_index == starting_index && looped)
                                    {
                                        terms_done = true;
                                        full_lap = true;
                                        break;
                                    }
                                }
                            }
                            else if (TermorDef == 1 && !defs_done)//Def Question will be used
                            {
                                while (Questions[rand_index].DefUsed)
                                {
                                    rand_index++;
                                    if (rand_index >= size)
                                    {
                                        rand_index = 0;
                                        looped = true;
                                    }
                                    if (rand_index == starting_index && looped)
                                    {
                                        defs_done = true;
                                        full_lap = true;
                                        break;
                                    }
                                }
                            }
                            else break;
                            if (!full_lap)
                            {
                                QuestionText.Add(new List<string>());
                                int index = QuestionText.Count;
                                if (TermorDef == 0)
                                {
                                    Questions[rand_index].TermUsed = true;
                                    QuestionText[index - 1].AddRange(Questions[rand_index].TermAnswers);
                                    for (int i = 0; i < QuestionText[index - 1].Count; i++)
                                    {
                                        QuestionText[index - 1][i] = (i + 1) + ": " + QuestionText[index - 1][i];
                                    }
                                }
                                else
                                {
                                    Questions[rand_index].DefUsed = true;
                                    QuestionText[index - 1].AddRange(Questions[rand_index].DefAnswers);
                                    for (int i = 0; i < QuestionText[index - 1].Count; i++)
                                    {
                                        QuestionText[index - 1][i] = (i + 1) + ": " + QuestionText[index - 1][i];
                                    }
                                }
                                TDAs.Add(new TermDefAnswer(rand_index, TermorDef));

                                looped = false;
                            }
                            else
                            {
                                looped = false;
                                full_lap = false;
                                --question;
                                continue;
                            }
                        }
                        #endregion

                        States = QuizStates.Quiz;
                        break;
                    case QuizStates.Quiz:
                        //ConsoleUI.Print("quiz.Count=0. continue", true, true);
                        //Asks each question and takes in input
                        for (int i = 0; i < QuestionText.Count; i++)
                        {
                            string question = "";
                            if (TDAs[i].TermOrDef == 0)
                            {
                                question = "Question " + (i + 1) + "/" + QuestionText.Count + ": " +
                                    Questions[TDAs[i].Question].DefQuestion + "\n\n";
                            }
                            else
                            {
                                question = "Question " + (i + 1) + "/" + QuestionText.Count + ": " +
                                    Questions[TDAs[i].Question].TermQuestion + "\n\n";
                            }
                            CKI = ConsUI.MultipleChoiceCK(question, QuestionText[i], true, true);
                            if (ConsUI.IsANumber(CKI.KeyChar.ToString()))
                                TDAs[i] = new TermDefAnswer(TDAs[i].Question, TDAs[i].TermOrDef,
                                    int.Parse(CKI.KeyChar.ToString()));
                            else
                                TDAs[i] = new TermDefAnswer(TDAs[i].Question, TDAs[i].TermOrDef, -1);
                            /*Later, allow for review, showing the correct answer with the answer chosen.*/
                        }
                        bool decision = ConsUI.YesOrNo("Quiz complete. Would you like to review? (Press \"Y\" or \"1\" for true, "+
                            "\"N\" or \"0\" for no)", true);
                        if (decision)
                            States = QuizStates.Review;
                        else
                            States = QuizStates.MainMenu;
                        break;
                    case QuizStates.FileLoc:
                        //Nothing Yet.
                        ConsUI.Print("Entered void state. continue.", true, true);
                        States = QuizStates.MainMenu;
                        break;
                    case QuizStates.Review:
                        QuestionText.Clear();
                        ConsUI.Print("And now, for the results:", true, true);
                        size = TDAs.Count;
                        for (int i = 0; i < size; i++)
                        {
                            int question = TDAs[i].Question;
                            int answer = TDAs[i].answer;
                            int real_answer;
                            QuestionText.Add(new List<string>());
                            if (TDAs[i].TermOrDef == 0)
                            {
                                real_answer = Questions[question].CORRECT_TERM;
                                QuestionText[i].Add(Questions[question].TermQuestion + "\n");
                                QuestionText[i].Add("Your Answer: " + Questions[question].DefAnswers[answer - 1] + "\n");
                                QuestionText[i].Add("The REAL Answer: " + Questions[question].DefAnswers[real_answer]);
                            }
                            else
                            {
                                real_answer = Questions[question].CORRECT_DEF;
                                QuestionText[i].Add(Questions[question].DefQuestion + "\n");
                                QuestionText[i].Add("Your Answer: " + Questions[question].TermAnswers[answer - 1] + "\n");
                                QuestionText[i].Add("The REAL Answer: " + Questions[question].TermAnswers[real_answer]);
                            }
                            ConsUI.MultipleChoiceCK("Question " + (i + 1) + ":\n", QuestionText[i], true);
                        }
                        States = QuizStates.MainMenu;
                        break;
                    case QuizStates.Exit:
                        ConsUI.Print("You are exiting the Quiz Machine.", true, true);
                        States = QuizStates.MainMenu;
                        return;
                    default:
                        States = QuizStates.MainMenu;
                        break;
                }
            }
        }

        public static void ParseQuizXML(XmlDocument XMLD)
        {
            XmlNode HeadNode = XMLD.FirstChild;
            //search for termdef nodes
            List<XmlNode> TermDefNodes = FindXMLNodes("termdef", HeadNode);
            if (TermDefNodes.Count != 0)
            {
                foreach (XmlNode TermDef in TermDefNodes)
                {
                    #region Term Setup
                    //Search for the Termtype
                    string termtype = FindXMLAttribute("termtype", TermDef);
                    if (termtype == null)
                    {
                        ConsUI.Print("ERROR: TERMTYPES COULD NOT COMPLETE TASK!", false, true);
                        return;
                    }
                    //Search for the quiztypes and delimit them
                    string[] quiztype_s_ = FindXMLAttribute("quiztype", TermDef).Split(',');
                    if (quiztype_s_.Length == 0)
                    {
                        ConsUI.Print("ERROR: QUIZTYPES COULD NOT COMPLETE TASK!", false, true);
                        return;
                    }

                    int size = quiztype_s_.Length;//Need for checking quiztype Attribute

                    bool FTBExists = false, WATExists = false, VocabExists = false, MultiExists = false;
                    for (int i = 0; i < size; i++)
                    {
                        if (quiztype_s_[i].ToUpper() == "WAT")
                        {
                            WATExists = true;
                            continue;
                        }
                        if (quiztype_s_[i].ToUpper() == "VOCAB")
                        {
                            VocabExists = true;
                            continue;
                        }
                        if (quiztype_s_[i].ToUpper() == "MULTI")
                        {
                            MultiExists = true;
                            continue;
                        }
                        if (quiztype_s_[i].ToUpper() == "FTB")
                        {
                            FTBExists = true;
                            continue;
                        }
                    }

                    //Search for the terms
                    List<XmlNode> Terms = FindXMLNodes("term", TermDef);
                    #endregion
                    if (Terms.Count != 0)
                    {
                        //Seach within the terms
                        foreach (XmlNode Term in Terms)
                        {
                            #region Singular and Past
                            //Search for if it's singular
                            bool singular = false;
                            switch (FindXMLAttribute("singular", Term).ToUpper())
                            {
                                case "TRUE":
                                    singular = true;
                                    break;
                                case "FALSE":
                                    singular = false;
                                    break;
                                case "":
                                    singular = true;
                                    break;
                            }
                            //Search for if it's past
                            bool past = false;
                            switch (FindXMLAttribute("past", Term).ToUpper())
                            {
                                case "TRUE":
                                    past = true;
                                    break;
                                case "FALSE":
                                    past = false;
                                    break;
                                case "":
                                    if (termtype == "EVENT")
                                    {
                                        ConsUI.Print("ERROR: TERM PAST COULD NOT COMPLETE TASK!", false, true);
                                        return;
                                    }
                                    else
                                        past = false;
                                    break;
                            }
                            #endregion
                            #region Definition Search
                            //Search for the Term's value
                            string definition = FindXMLValue(Term);
                            //Search for the choices and delimit them
                            List<XmlNode> choices = FindXMLNodes("choice", Term);

                            List<string> str_choices = new List<string>();//Just in case a single definition doesn't exist
                            if (choices != null)
                            {
                                foreach (XmlNode Choice in choices)
                                {
                                    str_choices.Add(FindXMLValue(Choice));
                                }
                            }
                            if (definition == null)
                            {
                                //Check for if the attributes for the current termdef is "WAT" and there are no choices
                                foreach (XmlNode choice in choices)
                                {
                                    string text = FindXMLValue(choice);
                                    if (text == null && WATExists)
                                    {
                                        ConsUI.Print("ERROR: THE TERMTYPE IS WAT, BUT THERE ARE MISSING CHOICES!", false, true);
                                        return;
                                    }
                                    //TODO: CHECK FOR THERE ARE REALLY NO CHOICES
                                    else if (!WATExists)
                                    {
                                        //Along with that...
                                        ConsUI.Print("ERROR: NO VALUE EXISTS FOR THIS TERM!", false, true);
                                        return;
                                    }
                                    else
                                        str_choices.Add(text);
                                }
                                if (choices.Count != 0 && (VocabExists || MultiExists))//This may be removed later if a contingency opposed to this appears
                                {
                                    ConsUI.Print("ERROR: VOCAB OR MULTI WANTS TO BE USED, BUT CHOICES EXIST.", false, true);
                                    return;
                                }
                            }
                            /*else if (definition != null && choices.Count != 0)
                            {
                                ConsoleUI.Print("ERROR: TOO MANY VALUES FOR THIS TERM! USE EITHER BASIC TERM VALUE OR CHOICES!", false, true);
                                return;
                            }*/

                            //Search for the buzzwords and delimit them
                            List<XmlNode> Buzzwords = FindXMLNodes("buzzwords", Term);
                            string[] buzzwords = new string[0];
                            if (Buzzwords != null)
                            {
                                buzzwords = new string[Buzzwords.Count];
                                buzzwords = FindXMLValue(Buzzwords[0]).Split(',');
                                int buzz_size = buzzwords.Length;
                                for (int i = 0; i < buzz_size; i++)
                                    buzzwords[i] = buzzwords[i].Trim();
                            }
                            //Check for if the attributes for the current termdef is "FTB" and there are no buzzwords
                            else if (buzzwords.Length == 0 && FTBExists)
                            {
                                ConsUI.Print("ERROR: THIS TERM IS FTB, BUT THERE ARE NO BUZZWORDS!", false, true);
                                return;
                            }
                            #endregion
                            #region Attribute Searching
                            //Search for the name
                            string name = FindXMLAttribute("name", Term);
                            if (name == null)
                            {
                                ConsUI.Print("ERROR: TERM NAME COULD NOT COMPLETE TASK!", false, true);
                                return;
                            }

                            #endregion
                            //If you made it this far without a hitch...
                            //Create the quiztext
                            #region Final Packaging
                            if (WATExists)
                            {
                                if (buzzwords.Length == 0)
                                {
                                    Questions.Add(new QuizText(name, str_choices, termtype, singular, past, quiztype_s_));
                                    continue;
                                }
                                else
                                {
                                    Questions.Add(new QuizText(name, str_choices, termtype, singular, past, quiztype_s_, buzzwords));
                                    continue;
                                }
                            }
                            if (VocabExists || MultiExists)
                            {
                                if (buzzwords.Length == 0)
                                {
                                    Questions.Add(new QuizText(name, definition, termtype, singular, past, quiztype_s_));
                                    continue;
                                }
                                else if (buzzwords.Length > 0 || FTBExists)
                                {
                                    Questions.Add(new QuizText(name, definition, termtype, singular, past, quiztype_s_, buzzwords));
                                    continue;
                                }
                            }
                            #endregion
                            //Reset all variables
                            #region Cleanup
                            //strings
                            termtype = definition = name = "";
                            //string[]s
                            quiztype_s_ = buzzwords = new string[0];
                            //List<string>s
                            str_choices.Clear();
                            //List<XmlNode>s
                            Terms = choices = Buzzwords = new List<XmlNode>();
                            //bools
                            FTBExists = WATExists = VocabExists = MultiExists = singular = past = false;
                            //ints
                            size = 0;
                            #endregion
                        }
                    }
                }
            }
        }

        public static List<XmlNode> FindXMLNodes(string name, XmlNode HeadNode)
        {
            List<XmlNode> result = new List<XmlNode>();

            if (HeadNode.HasChildNodes)
            {
                int size = HeadNode.ChildNodes.Count;
                for (int i = 0; i < size; i++)
                {
                    if (HeadNode.ChildNodes[i].Name == name)
                        result.Add(HeadNode.ChildNodes[i]);
                }
            }
            if (result.Count > 0)
                return result;
            else
                return null;
        }

        private static string FindXMLAttribute(string name, XmlNode HeadNode)
        {
            string result = "";
            int size = HeadNode.Attributes.Count;
            int counter = 0;
            for (int i = 0; i < size; i++)
            {
                if (HeadNode.Attributes[i].Name == name)
                {
                    result = HeadNode.Attributes[name].Value;
                    counter++;
                }
            }
            if (counter > 1)
            {
                ConsUI.Print("ERROR: TOO MANY OF ATTRIBUTE " + name.ToUpper() + "!!!", false, true);
                return null;
            }
            else
                return result;

        }

        private static string FindXMLValue(XmlNode HeadNode)
        {
            string result = "";

            if (HeadNode.InnerText.Length != 0)
            {
                int index = HeadNode.InnerXml.IndexOf("<");
                if (index <= 0)
                    result = HeadNode.InnerXml.Trim();
                else
                    result = HeadNode.InnerXml.Substring(0, index).Trim();
                return result;
            }
            else
                return null;
        }

        #region Extra Code
        /*
        #region Question Printing
            int counter = 0;//I'll get rid of this later;
            List<List<string>> messages = new List<List<string>>();//Same with this
            foreach (List<QuizText> List in ListList)
            {
                foreach (QuizText question in List)
                {
                    //Print the original Term type and if it's singular or not
                    messages.Add(new List<string>());
                    messages[counter].Add("TermType: " + question.TermType);
                    messages[counter].Add(question.IsSingular ? "Singular" : "Plural");
                    //Print the original Term
                    messages[counter].Add("Term: " + question.Term);
                    //Print the original Definition
                    if (question.Definitions.Length <= 0)
                        messages[counter].Add("Definition: " + question.Definition);
                    else
                    {
                        for (int i = 0; i < question.Definitions.Length; i++)
                        {
                            messages[counter].Add("Definition " + (i + 1) + ": " + question.Definitions[i]);
                        }
                    }
                    //Print the questions
                    messages[counter].Add("Term Question: " + question.TermQuestion);
                    if (question.Definitions.Length <= 0)
                        messages[counter].Add("Definition Question: " + question.DefQuestion);
                    //Print the answers
                    for (int i = 0; i < question.DefAnswers.Length; i++)
                    {
                        messages[counter].Add("\t--DefAns " + (i + 1) + ": " + question.DefAnswers[i]);
                    }

                    for (int i = 0; i < question.TermAnswers.Length; i++)
                    {
                        messages[counter].Add("\t--TermAns " + (i + 1) + ": " + question.TermAnswers[i]);
                    }
                    counter++;
                }
            }
            int size = messages.Count;
            for (int i = 0; i < size; i++)
            {
                ConsoleUI.MultipleChoiceCK("Question " + (i + 1) + ":", messages[i], true);
            }
            #endregion*/
        #endregion

        private static void MakeQuestions()
        {
            List<QuizText> Actions = new List<QuizText>();
            List<QuizText> Events = new List<QuizText>();
            List<QuizText> Ideas = new List<QuizText>();
            List<QuizText> Objects = new List<QuizText>();
            List<QuizText> People = new List<QuizText>();
            //Organizes all of the questions into their appropriate sections of their lists


            foreach (QuizText question in Questions)
            {
                switch (question.TermType)
                {
                    case QuizText.TermTypes.Action:
                        //if singular, store at the end of the list
                        if (question.IsSingular)
                        {
                            question.TermQuestion = "What is " + question.Term + "?";
                            if (question.Definitions.Length <= 0)
                                question.DefQuestion = "What is " + question.Definition + "?";
                        }
                        else//else...
                        {
                            //store at the beginning with a marker dictating the last plural element of the list
                            question.TermQuestion = "What are " + question.Term + "?";
                            if (question.Definitions.Length <= 0)
                                question.DefQuestion = "What are " + question.Definition + "?";
                        }
                        Actions.Add(question);
                        break;
                    case QuizText.TermTypes.Event:
                        if (question.IsSingular)
                        {
                            question.TermQuestion = "What " + (question.IsPast ? "was " : "is ") + question.Term + "?";
                            if (question.Definitions.Length <= 0)
                                question.DefQuestion = "What " + (question.IsPast ? "was " : "is ") + question.Definition + "?";
                        }
                        else
                        {
                            question.TermQuestion = "What " + (question.IsPast ? "were " : "are ") + question.Term + "?";
                            if (question.Definitions.Length <= 0)
                                question.DefQuestion = "What " + (question.IsPast ? "were " : "are ") + question.Definition + "?";
                        }
                        Events.Add(question);
                        break;
                    case QuizText.TermTypes.Idea:
                        if (question.IsSingular)
                        {
                            question.TermQuestion = "What is " + question.Term + "?";
                            if (question.Definitions.Length <= 0)
                                question.DefQuestion = "What is " + question.Definition + "?";
                        }
                        else
                        {
                            question.TermQuestion = "What are " + question.Term + "?";
                            if (question.Definitions.Length <= 0)
                                question.DefQuestion = "What are " + question.Definition + "?";
                        }
                        Ideas.Add(question);
                        break;
                    case QuizText.TermTypes.Object:
                        if (question.IsSingular)
                        {
                            question.TermQuestion = "What is a " + question.Term + "?";
                            if (question.Definitions.Length <= 0)
                                question.DefQuestion = "What is a " + question.Definition + "?";
                        }
                        else
                        {
                            question.TermQuestion = "What are " + question.Term + "?";
                            if (question.Definitions.Length <= 0)
                                question.DefQuestion = "What are " + question.Definition + "?";
                        }
                        Objects.Add(question);
                        break;
                    case QuizText.TermTypes.Person:
                        if (question.IsSingular)
                        {
                            question.TermQuestion = "Who is " + question.Term + "?";
                            if (question.Definitions.Length <= 0)
                                question.DefQuestion = "Who is " + question.Definition + "?";
                        }
                        else
                        {
                            question.TermQuestion = "Who are " + question.Term + "?";
                            if (question.Definitions.Length <= 0)
                                question.DefQuestion = "Who are " + question.Definition + "?";
                        }
                        People.Add(question);
                        break;
                }
            }

            Questions.Clear();

            #region Answer Creation and Randomization

            List<List<QuizText>> ListList = new List<List<QuizText>>();
            ListList.Add(Actions);
            ListList.Add(Events);
            ListList.Add(Ideas);
            ListList.Add(Objects);
            ListList.Add(People);

            foreach (List<QuizText> List in ListList)
            {
                MakeAnswers(List, true);
                MakeAnswers(List, false);
                if (List.Count != 0)
                    Questions.AddRange(List);
            }
            #endregion

        }

        private static void MakeAnswers(List<QuizText> Questions, bool IsDefinition)
        {
            int rand_index = 0;
            bool list_looped = false, full_lap = false;
            rand = new Random();
            int arr_size;
            int list_size = Questions.Count;
            int CorrectIndex = 0;
            if (list_size > 0)
            {
                if (IsDefinition)
                {
                    arr_size = Questions[0].DefAnswers.Length;
                }
                else if (!IsDefinition)
                {
                    arr_size = Questions[0].TermAnswers.Length;
                }
                else
                {
                    arr_size = 0;
                }
            }
            else
                return;
            for (int i = 0; i < list_size; i++)
            {
                /*So it doesn't try to add the current question's definition to the list
                Besides, I'll be doing that manually after the loop.*/
                Questions[i].HasRead = true;

                bool Singular = Questions[i].IsSingular;
                bool Past = Questions[i].IsPast;

                for (int j = 0; j < arr_size; j++)
                {
                    rand_index = rand.Next(list_size);
                    int starting_index = rand_index;

                    if (IsDefinition)
                        CorrectIndex = Questions[0].CORRECT_DEF;
                    else if (!IsDefinition)
                        CorrectIndex = Questions[0].CORRECT_TERM;
                    else
                        CorrectIndex = 0;

                    if (j == CorrectIndex)
                        continue;
                    while (Questions[rand_index].HasRead || Questions[rand_index].IsWAT/*Temporary*/ ||
                        (Singular != Questions[rand_index].IsSingular && Past != Questions[rand_index].IsPast))
                    {
                        rand_index++;
                        if (rand_index >= list_size)
                        {
                            rand_index = 0;
                            list_looped = true;
                        }
                        if (rand_index == starting_index && list_looped)
                        {
                            full_lap = true;
                            break;
                        }
                    }
                    if (!full_lap)
                    {
                        if (IsDefinition)
                            Questions[i].DefAnswers[j] = Questions[rand_index].Definition;
                        else
                            Questions[i].TermAnswers[j] = Questions[rand_index].Term;

                        Questions[rand_index].HasRead = true;
                        list_looped = false;
                    }
                    else
                    {
                        list_looped = false;
                        full_lap = false;
                        break;
                    }
                }
                /*THIS fills the reserved index in the array with the correct answer.
                Don't worry. It'll be moved later.*/
                if (IsDefinition)
                    Questions[i].DefAnswers[CorrectIndex] = Questions[i].Definition;
                else
                    Questions[i].TermAnswers[CorrectIndex] = Questions[i].Term;

                for (int j = 0; j < Questions.Count; j++)
                {
                    Questions[j].HasRead = false;
                }
            }
            full_lap = false;//Cleanup

            #region Random Replacement
            string tempstr = "";
            int result_num = -1;

            //Run through each of the lists
            for (int i = 0; i < list_size; i++)
            {
                if (IsDefinition)
                {
                    //Randomly pick an answer register
                    rand_index = rand.Next(0, arr_size);
                    //Store the correct answer from the array in a temporary string
                    tempstr = Questions[i].DefAnswers[CorrectIndex];//Correct Answer(Not moved)

                    //Replace the correct answer with the randomly chosen array register's answer.
                    Questions[i].DefAnswers[CorrectIndex] = Questions[i].DefAnswers[rand_index];//Random answer (moved)
                    //Replace the randomly chosen array register's answer with the correct answer.
                    Questions[i].DefAnswers[rand_index] = tempstr;//Correct answer(moved)
                    result_num = rand_index;//Store the correct answer's register number
                    Questions[i].CORRECT_DEF = rand_index;//Temporary: does the same as above line of code.

                    tempstr = "";//clear the temp_arr
                    result_num = -1;
                }
                else
                {
                    //Randomly pick an answer register
                    rand_index = rand.Next(0, arr_size);
                    //Store the correct answer from the array in a temporary string
                    tempstr = Questions[i].TermAnswers[CorrectIndex];//Correct Answer(Not moved)

                    //Replace the correct answer with the randomly chosen array register's answer.
                    Questions[i].TermAnswers[CorrectIndex] = Questions[i].TermAnswers[rand_index];//Random answer (moved)
                    //Replace the randomly chosen array register's answer with the correct answer.
                    Questions[i].TermAnswers[rand_index] = tempstr;//Correct answer(moved)
                    result_num = rand_index;//Store the correct answer's register number
                    Questions[i].CORRECT_TERM = rand_index;//Temporary: does the same as above line of code.

                    tempstr = "";//clear the temp_arr
                    result_num = -1;
                }
            }
            #endregion
        }
    }
}