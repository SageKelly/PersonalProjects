using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace ClockWatcher
{
    [Serializable]
    public class ProgramData : INotifyPropertyChanged, ISerializable
    {
        /// <summary>
        /// Holds a list of all comments made within the session
        /// </summary>
        public List<string> Sessions;
        /// <summary>
        /// The list of comment entries that exist as option across multiple sessions
        /// </summary>
        public List<string> PersistentCommentEntries;

        private DateTime start_time;

        public event PropertyChangedEventHandler PropertyChanged;

        public DateTime StartTime
        {
            get
            {
                return start_time;
            }
            private set
            {
                if (start_time != value)
                {
                    start_time = value;
                    OnPropertyChanged("StartTime");
                }
            }
        }


        public ProgramData(Session currrentSession)
        {
            Sessions = new List<string>();

            PersistentCommentEntries = new List<string>();
            start_time = DateTime.Now;
        }

        protected ProgramData(SerializationInfo info, StreamingContext context)
        {
            Sessions = (List<string>)info.GetValue("Sessions", typeof(List<string>));
            PersistentCommentEntries = (List<string>)info.GetValue("PersistentCommentEntries", typeof(List<string>));
            start_time = (DateTime)info.GetValue("start_time", typeof(DateTime));
        }

        public Session OpenSession(int index)
        {
            if (File.Exists(Sessions[index]))
            {
                IFormatter formatter = new BinaryFormatter();
                Stream stream = new FileStream(Sessions[index], FileMode.OpenOrCreate, FileAccess.Read, FileShare.None);
                Session sesh = (Session)formatter.Deserialize(stream);
                stream.Close();
                return sesh;
            }
            return null;
        }

        public List<Session> OpenSession(IList items)
        {
            List<Session> result = new List<Session>();
            bool complete = true;
            foreach (string s in items)
            {
                string temp = Session.FileName(s);
                if (File.Exists(temp))
                {
                    IFormatter formatter = new BinaryFormatter();
                    Stream stream = new FileStream(temp, FileMode.OpenOrCreate, FileAccess.Read, FileShare.None);
                    Session sesh = (Session)formatter.Deserialize(stream);
                    stream.Close();
                    result.Add(sesh);
                }
                else
                {
                    complete = false;
                    break;
                }
            }
            if (complete)
                return result;
            return null;
        }

        private void OnPropertyChanged([CallerMemberName] string member_name = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(member_name));
            }
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Sessions", Sessions, typeof(List<string>));
            info.AddValue("PersistentCommentEntries", PersistentCommentEntries, typeof(List<string>));
            info.AddValue("start_time", start_time, typeof(DateTime));
        }

        public void DeleteSession(int index)
        {
            File.Delete(Sessions[index]);
            Sessions.RemoveAt(index);
        }
    }
}
