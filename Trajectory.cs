using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PYT
{
    class Trajectory
    {
        const int REQUIRED_SAMPLES = 10;

        // start and end coordinate positions
        protected Coordinate start;
        protected Coordinate end;

        // the sample count
        protected int samples;

        // the list of coordinates we will work with
        protected List<string> coordinates;

        /**
         * Constructs the trajectory
         * 
         * @param Coordinate start      the start coordinate position
         * @param Coordinate end        the end coordinate position
         * @param int samples           the number of samples to interpolate across
         * 
         * @return Trajectory
         */
        public Trajectory(Coordinate start, Coordinate end, int samples, List<string> coordinates = null)
        {
            this.start = start;
            this.end = end;
            this.samples = samples;
            if (coordinates != null)
            {
                this.coordinates = coordinates;
            }
            else
            {
                this.computeMatchingCoordinates();
            }
        }

        /**
         * Computes and returns the trajectory (list of coordinates)
         * 
         * @return List<Coordinate>
         */
        public List<Coordinate> Compute()
        {
            this.Validate();

            List<Coordinate> trajectory = new List<Coordinate>(this.samples);

            // compute the values necessary for the trajectory equations
            Dictionary<string, EquationParameters> parameters = new Dictionary<string, EquationParameters>();
            foreach (string coord in this.coordinates)
            {
                parameters.Add(coord, new EquationParameters(this.start.getCoordinate(coord), this.end.getCoordinate(coord)));
            }

            // iterate over the samples
            for (double i = -1; i <= 1; i += (2.0/this.samples))
            {
                Coordinate c = new Coordinate(new List<string>(new string[] { "x", "y", "z" }));
                // iterate over the coordinates
                foreach (string coord in this.coordinates)
                {
                    c.setCoordinate(coord, parameters[coord].Compute(i));
                }
                trajectory.Add(c);
            }

            return trajectory;
        }

        /**
         * Validates the internal data
         * 
         * @return bool
         */
        protected bool Validate()
        {
            // validate the sample count
            if (this.samples < REQUIRED_SAMPLES)
            {
                throw new InvalidTrajectoryException("Sample count of " + this.samples + " is less than the required minimum, " + REQUIRED_SAMPLES);
            }

            // validate there are some coordinates to match over
            if (this.coordinates.Count < 1)
            {
                throw new InvalidTrajectoryException("There are no common coordinates or coordinates specified to interpolate");
            }

            // validate that each of the expected coordinates are in start and end
            foreach (string coord in this.coordinates)
            {
                try
                {
                    this.start.getCoordinate(coord);
                    this.end.getCoordinate(coord);
                }
                catch (KeyNotFoundException e)
                {
                    throw new InvalidTrajectoryException("Coordinate " + coord + " is not in both start and end coordinate");
                }
            }

            return true;
        }

        /**
         * Generates a list of matching coordinates for the start and end pair
         * 
         * @return void
         */
        protected void computeMatchingCoordinates()
        {
            List<string> coords = this.start.getCoordinateNames();
            List<string> matching = new List<string>();
            foreach (string coord in coords)
            {
                try
                {
                    this.end.getCoordinate(coord);
                    // if this hasn't thrown, add it to the list
                    matching.Add(coord);
                }
                catch (KeyNotFoundException e)
                {
                    // disregard
                }
            }
            // save the list of coordinates matched
            this.coordinates = matching;
        }
    }

    /**
     * Our own InvalidTrajectoryException
     */
    class InvalidTrajectoryException : Exception
    {
        public InvalidTrajectoryException()
        {
        }

        public InvalidTrajectoryException(string message)
            : base(message)
        {
        }

    }
}
