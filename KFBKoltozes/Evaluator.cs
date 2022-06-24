using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KFBKoltozes
{
    abstract class Command
    {
        public abstract void execute();
    }

    class HelpCommand : Command
    {
        public override void execute()
        {
            Console.WriteLine("Commands:");
            Console.WriteLine("help");
            Console.WriteLine("exit");
            Console.WriteLine();
        }
    }

    class ExitCommand : Command
    {
        Evaluator e;
        public ExitCommand (Evaluator e)
        {
            this.e = e;
        }
        public override void execute()
        {
            e.stop();
        }
    }

    class FindCommand : Command
    {
        Graph g;
        string key;
        public FindCommand(Graph g, string key)
        {
            this.g = g;
            this.key = key;
        }
        public override void execute()
        {
            Student result = g.getStudent(key.ToUpper());
            if (result == null)
                Console.WriteLine("Nincs találat!");
            else
            {
                Console.WriteLine(result.ToString());
                if(g.canMoveStudent().Contains(result))
                    Console.WriteLine("Akadálymentesen költözhet a {0} szobából a {1} szobába",result.FromRoom, result.ToRoom);
                else
                    Console.WriteLine("Akadálymentesen NEM költözhet!");
            }
        }
    }

    class FindConflictsCommand : Command
    {
        Graph g;
        string key;
        public FindConflictsCommand(Graph g, string key)
        {
            this.g = g;
            this.key = key;
        }
        public override void execute()
        {
            Student student = g.inRoom(Convert.ToInt32(key))[0];//g.getStudent(key.ToUpper());
            List<Student> result = g.needsToMove(student);
            foreach (Student s in result)
            {
                Console.WriteLine("\t{0}", s.ToString());
            }
            g.unVisitAllNode();
        }
    }

    class CanMoveCommand : Command
    {
        Graph g;
        string type;
        public CanMoveCommand(Graph g, string type)
        {
            this.g = g;
            this.type=type;
        }
        public override void execute()
        { 
            if(type == "room")
            {
                List<int> result = g.canMoveRoom();
                if (result.Count == 0)
                    Console.WriteLine("Nincs találat!");
                else
                {
                    Console.WriteLine("Akadálymentesen költözhetnek az alábbi szobákból: ");
                    foreach (int room in result)
                        Console.WriteLine("\t {0} szobából", room);
                }
            }
            else if (type == "student")
            {
                List<Student> result = g.canMoveStudent();
                if (result.Count == 0)
                    Console.WriteLine("Nincs találat!");
                else
                {
                    Console.WriteLine("Akadálymentesen költözhetnek: ");
                    foreach (Student student in result)
                        Console.WriteLine("\t {0} a {1} szobából, a {2} szobába", student.Name, student.FromRoom, student.ToRoom);
                }
            }
        }
    }

    internal class Evaluator
    {
        private Graph graph;
        private bool running;
        public Evaluator(Graph graph)
        {
            this.graph = graph;
            this.running = false;
        }
        
        public void stop() { running = false; }

        public Student getStudent(string neptun)
        {
            return graph.getStudent(neptun.ToUpper());
        }

        Command getCommand(string[] cmd)
        {
            if (cmd[0] == "help")
                return new HelpCommand();
            if (cmd[0] == "exit")
                return new ExitCommand(this);
            if (cmd[0] == "find")
                return new FindCommand(graph, cmd[1]);
            if (cmd[0] == "canMove")
            {
                string type = "";
                if (cmd.Length == 1)
                    type = "room";
                else
                    type = cmd[1];
                return new CanMoveCommand(graph, type);
            }
            if (cmd[0] == "conflict")
            {
                return new FindConflictsCommand(graph, cmd[1]);
            }
            return null;
        }

        public void run()
        {
            running = true;

            while (running)
            {
                string[] input = Console.ReadLine().Split(" ");
                Command cmd = getCommand(input);
                if(cmd != null)
                    cmd.execute();
            }
        }
    }
}
