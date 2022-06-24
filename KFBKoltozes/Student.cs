using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KFBKoltozes
{
    internal class Student : IEquatable<Student>
    {
        private string  neptun;
        private string  name;
        private int     fromRoom;
        private int     toRoom;
        private int     currentRoom;
        private bool    movedOut;
        private bool    movesOut;
        private bool    stays;

        public const int noRoom = -1;

        public Student(string neptun, String name)
        {
            this.name = name;
            this.neptun = neptun;
            this.movedOut = false;
            this.stays = false;
            this.movesOut = false;
            currentRoom = noRoom;
        }

        public string Name      { get { return name; } set { name = value; } }
        public string Neptun    { get { return neptun; } set { neptun = value; } }
        public int FromRoom { get { return fromRoom; } set { fromRoom = value; currentRoom = fromRoom; } }
        public int ToRoom { get { return toRoom; } 
                            set { 
                                toRoom = value; 
                                if (toRoom == fromRoom) { stays = true; } 
                                if(toRoom == noRoom) { movesOut = true; }
                            } }
        public int CurrentRoom { get { return currentRoom; } set { currentRoom = value; } }
        public bool MovedOut { get { return movedOut; }}

        public bool MovesOut { get { return movesOut; }}
        public bool Stays { get { return stays; }}

        public void move()
        {
            currentRoom = toRoom;
            movedOut = true;
        }

        public bool Equals(Student other)
        {
            return this.neptun == other.neptun;
        }

        public string ToString()
        {
            string result = "";

            result += name;

            return result;
        }
    }
}
