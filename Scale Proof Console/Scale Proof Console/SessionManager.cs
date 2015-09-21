using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scale_Proof_Console
{
    [Serializable]
    /// <summary>
    /// Handles Session querying and deletion
    /// </summary>
    class SessionManager
    {
        /// <summary>
        /// The creation time of the most recently-created session
        /// </summary>
        private DateTime most_recent_session;

        private Dictionary<DateTime, Session> Sessions;

        /// <summary>
        /// Holds the list of sessions based off the current query
        /// </summary>
        private List<Session> resultingSessions;

        /// <summary>
        /// Holds the currently-selected year in query
        /// </summary>
        private int year;

        /// <summary>
        /// Holds the currently-selected month in query
        /// </summary>
        private int month;

        /// <summary>
        /// Sets up a new session manager
        /// </summary>
        public SessionManager()
        {
            Sessions = new Dictionary<DateTime, Session>();
            resultingSessions = new List<Session>();
            most_recent_session = DateTime.Now;
            year = 2015;
            month = 1;
        }

        /// <summary>
        /// Adds a Session to the internal data
        /// </summary>
        /// <param name="difficulty">The difficulty of the session. Use Resources for determination</param>
        /// <param name="scale_in_use">The scale being used in the session. Refer to Resources for determination</param>
        /// <param name="note_length">The amount of notes being attempted in this session</param>
        public void AddNewSession(Resources.Difficulties difficulty,Scale scale_in_use, int note_length)
        {
            Session NewSesh = new Session(difficulty, scale_in_use, note_length);
            Sessions.Add(NewSesh.ID, NewSesh);
        }

        public List<Session> SearchByYear(int year)
        {
            List<KeyValuePair<DateTime, Session>> keyValues;

            keyValues = Sessions.Where(x => x.Key.Year == year).ToList();

            List<Session> result = new List<Session>();

            foreach (KeyValuePair<DateTime, Session> kvp in keyValues)
            {
                result.Add(kvp.Value);
            }


            return result;
        }

        public List<Session> SearchByMonth(int month)
        {
            List<KeyValuePair<DateTime, Session>> keyValues;

            keyValues = Sessions.Where(x => x.Key.Month == month).ToList();

            List<Session> result = new List<Session>();

            foreach (KeyValuePair<DateTime, Session> kvp in keyValues)
            {
                result.Add(kvp.Value);
            }

            return result;
        }

        public List<Session> SearchByweek(int week)
        {
            //TODO: Fix this method
            List<KeyValuePair<DateTime, Session>> keyValues;

            keyValues = Sessions.Where(x => x.Key.Month == month).ToList();

            List<Session> result = new List<Session>();

            foreach (KeyValuePair<DateTime, Session> kvp in keyValues)
            {
                result.Add(kvp.Value);
            }

            return result;
        }
    }
}
