using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SongProofWP8
{
    public class KVTuple<T1, T2> : INotifyPropertyChanged
    {
        private T1 kvp_key;
        private T2 kvp_value;

        public T1 Key
        {
            get
            {
                return kvp_key;
            }
            set
            {
                if (!kvp_key.Equals(value))
                {
                    kvp_key = value;
                    OnPropertyChanged("Key");
                }
            }
        }
        public T2 Value
        {
            get
            {
                return kvp_value;
            }
            set
            {
                if (!kvp_value.Equals(value))
                {
                    kvp_value = value;
                    OnPropertyChanged("Value");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public KVTuple() { }

        public KVTuple(T1 key, T2 value)
        {
            kvp_key = key;
            kvp_value = value;
        }

        private void OnPropertyChanged(string property_name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property_name));
            }
        }
    }
}
