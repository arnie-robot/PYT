using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PYT
{
    class Coordinate
    {
        // Dictionary of coordinate / value pairs
        protected Dictionary<string, double> coordinates = new Dictionary<string, double>();

        protected List<string> keys;

        /**
         * Constructs the coordinate
         * 
         * @param List<string> keys     the keys that this coordinate will have (e.g. x,y,z)
         * 
         * @return Coordinate
         */
        public Coordinate(List<string> keys)
        {
            foreach (string k in keys)
            {
                this.coordinates.Add(k.ToLower(), 0);
            }
            this.keys = keys;
        }

        /**
         * Allows the updating of a coordinate value
         * 
         * @param string key        the key to update, must exist in the coordinate from construction
         * @param double value      the value to set the coordinate to
         * 
         * @return void
         */
        public void setCoordinate(string key, double value)
        {
            key = key.ToLower();
            if (this.coordinates.ContainsKey(key))
            {
                this.coordinates[key] = value;
            }
            else
            {
                throw new KeyNotFoundException("Key " + key + " does not exist in Coordinate");
            }
        }

        /**
         * Allows the retrieval of a coordinate value
         * 
         * @param string key        the key to retrieve, must exist in the coordinate from construction
         * 
         * @return double
         */
        public double getCoordinate(string key)
        {
            key = key.ToLower();
            if (this.coordinates.ContainsKey(key))
            {
                return this.coordinates[key];
            }
            else
            {
                throw new KeyNotFoundException("Key " + key + " does not exist in Coordinate");
            }
        }

        /**
         * Returns a list containing all the coordinate names in the Coordinate
         * 
         * @return List<string>
         */
        public List<string> getCoordinateNames()
        {
            return this.keys;
        }

        /**
         * Convert this coordinate to bytes
         * 
         * @return byte[]
         */
        public byte[] toBytes()
        {
            System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();
            string str = "";
            List<string> list = new List<string>();
            foreach (KeyValuePair<string, double> coord in this.coordinates)
            {
                list.Add(coord.Value.ToString());
            }
            str = string.Join(",", list.ToArray());
            return encoding.GetBytes(str);
        }

        /**
         * Returns a serialisation for comparison
         */
        public string getCompareString()
        {
            string str = "";
            foreach (KeyValuePair<string, double> coord in this.coordinates)
            {
                str += coord.Key + "," + coord.Value.ToString() + ";";
            }
            return str;
        }

        /**
         * Converts the string from UDP to a Coordinate object
         * 
         * @param List<string> coordinates  list of coordinates to process
         * @param string lastPosition       the string representation
         * 
         * @return Coordinate
         */
        static public Coordinate fromString(List<string> coordinates, string input)
        {
            string[] i = input.Split(',');
            Coordinate c = new Coordinate(coordinates);
            int j = 0;
            foreach (string coord in coordinates)
            {
                c.setCoordinate(coord, double.Parse(i[j].Trim()));
                j++;
            }
            return c;
        }
    }
}
