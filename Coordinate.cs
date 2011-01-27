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
    }
}
