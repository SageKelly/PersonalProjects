using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SerializationTest
{
    /// <summary>
    /// An object to be serialized later
    /// </summary>
    [Serializable]
    public class SerialObject
    {
        /// <summary>
        /// It's an arbitrary item
        /// </summary>
        struct Item
        {
            string _name;
            int _number;
            public Item(string name, int number)
            {
                _name = name;
                _number = number;
            }
        }
        List<Item> Items;

        /// <summary>
        /// Instantiates the object
        /// </summary>
        public SerialObject()
        {
            Items = new List<Item>();
        }

        /// <summary>
        /// Adds an item to the object
        /// </summary>
        /// <param name="name">The name of the object</param>
        /// <param name="number">The number of the object (I know it doesn't make sense, but this is a test; just do it.</param>
        public void AddItem(string name, int number)
        {
            Items.Add(new Item(name, number));
        }

        [OnDeserialized]
        private void Setup()
        {

        }
    }
}
