using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KFBKoltozes
{
    class Node
    {
        private Student value;
        private List<Node> neighboursTo;

        public bool Visited { get; set; }

        public Student getValue() { return value; }
        public void clearNeighbours() { neighboursTo.Clear(); }
        public Node(Student student)
        {
            neighboursTo = new List<Node>();
            this.value = student;
        }
        public void addNeighbour(Node other)
        {
            neighboursTo.Add(other);
        }
        public int getNeighbourCount()
        {
            return neighboursTo.Count();
        }

        public List<Node> getNeighbours()
        {
            return neighboursTo;
        }
    }

    struct Line
    {
        public Line(Node nodeFrom, Node nodeTo)
        {
            this.nodeFrom = nodeFrom;
            this.nodeTo = nodeTo;
        }

        Node nodeFrom;
        Node nodeTo;
    }

    internal class Graph
    {
        private List<Node> nodes;
        private List<Line> lines;

        public Graph()
        {
            nodes = new List<Node>();
            lines = new List<Line>();
        }

        public void rebuildLines()
        {
            lines.Clear();
            foreach (Node student in nodes)
            {
                student.clearNeighbours(); 
                foreach (Node potentialNeighbour in nodes)
                {
                    if(student.getValue().ToRoom == potentialNeighbour.getValue().FromRoom)
                    {
                        if (potentialNeighbour.getValue().MovedOut)
                            continue;
                        student.addNeighbour(potentialNeighbour);
                        lines.Add(new Line(student, potentialNeighbour));
                    }
                }
            }
        }

        public Student getStudent(string neptun)
        {
            foreach(Node s in nodes)
            {
                if (s.getValue().Neptun == neptun)
                    return s.getValue();
            }

            return null;
        }

        public void unVisitAllNode()
        {
            foreach (Node node in nodes)
                node.Visited = false;
        }
        public List<Student> needsToMove(Student becausOf)
        {
            List<Student> needsToMove = new List<Student> ();

            if (canMoveStudent().Contains(becausOf) || becausOf.MovesOut )
                return needsToMove;

            foreach (Node node in nodes)
            {
                //if(node.getValue().FromRoom == becausOf.ToRoom && !node.getValue().MovedOut && !node.Visited
                //    && !node.getValue().MovesOut && !canMoveIn(node.getValue()))
                if(node.getValue().FromRoom == becausOf.ToRoom && !node.Visited && !canMoveIn(node.getValue()))
                {
                    node.Visited = true;
                    List<Student> nextStep = this.needsToMove(node.getValue());
                    needsToMove.Add(node.getValue());
                    if (!(nextStep.Count() == 0 && !needsToMove.Contains(becausOf)))
                    { 
                        needsToMove.AddRange(nextStep);
                    }
                }
            }

             return needsToMove;
        }

        public List<Student> inRoom(int room)
        {
            List<Student> result = new List<Student> ();
            foreach(Node node in nodes)
            {
                if(node.getValue().CurrentRoom == room)
                    result.Add(node.getValue());
            }
            return result;
        }

        public bool canMoveIn(Student student)
        {
            List<Student> students = inRoom(student.ToRoom);
            if (students.Contains(student))
                return true;
            if (students.Count == 4)
                return false;

            foreach (Student s in students)
            {
                if (s.MovesOut && !s.MovedOut)
                    return false;
            }

            return false;
        }

        public List<int> canMoveRoom()
        {
            List<int> result = new List<int>();

            foreach (Student s in canMoveStudent())
            {
                if (!result.Contains(s.FromRoom))
                    result.Add(s.FromRoom);
            }

            return result;
        }

        public List<Student> canMoveStudent()
        {
            List<Student> result = new List<Student>();

            foreach (Node node in nodes)
            {
                Student student = node.getValue();
                if (canMoveIn(student))
                    result.Add(student);
            }

            return result;
        }

        public static Graph CreateFromValues(List<Student> students)
        {
            Graph graph = new Graph();

            foreach(Student value in students)
            {
                graph.nodes.Add(new Node(value));
            }

            graph.rebuildLines();

            return graph;
        }
    }
}
