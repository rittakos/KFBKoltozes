using KFBKoltozes;

string path = @"C:\Users\akosr\Downloads\Szűrő-doksi-státuszhoz-2021_22_3-1.xlsx";
Reader reader = new Reader(path);
List<Student> students = reader.read();
Graph graph = Graph.CreateFromValues(students);

Evaluator evaluator = new Evaluator(graph);
//foreach(Student student in graph.inRoom(1113))
//{
//    Console.WriteLine(student.ToString());
//}
//Console.WriteLine(evaluator.getStudent("Z8WK8D").ToString());

evaluator.run();
