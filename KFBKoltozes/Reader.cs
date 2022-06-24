using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.IO;
using System.Linq;
using Microsoft.Office.Interop.Excel;

namespace KFBKoltozes
{
    internal class Reader
    {
        private SourceType sourceType;
        public enum SourceType {XLSX}
        public string FilePath { get; set; }  

        public Reader(string filePath)
        {
            FilePath = filePath;
            sourceType = SourceType.XLSX;
        }

        private Student readXLSRecord()
        {
            string neptun = "";
            string name = "";
            string toRoom;
            string fromRoom;



            return new Student(neptun, name);
        }

        private List<Student> readExcel()
        {
            List<Student> students = new List<Student>();

            Microsoft.Office.Interop.Excel.Application _excelApp = new Microsoft.Office.Interop.Excel.Application();
            _excelApp.Visible = true;

            Workbook workbook = _excelApp.Workbooks.Open(FilePath,
                Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                Type.Missing, Type.Missing);
     
            Worksheet worksheet = (Worksheet)workbook.Worksheets[1];

            Microsoft.Office.Interop.Excel.Range excelRange = worksheet.UsedRange;

            object[,] valueArray = (object[,])excelRange.get_Value(
                        XlRangeValueDataType.xlRangeValueDefault);

            for (int row = 2; row <= worksheet.UsedRange.Rows.Count; ++row)
            {
                string neptun = null;
                try
                {
                     neptun = valueArray[row, 3].ToString().ToUpper();
                } catch (System.NullReferenceException e)
                { 
                    break; 
                }

                string name = valueArray[row, 2].ToString();

                if (neptun == null)
                    continue;

                Student student = new Student(neptun, name);

                string fromRoom = valueArray[row, 1].ToString();
                student.FromRoom = Convert.ToInt32(fromRoom);
         
                try
                {
                    string toRoom = valueArray[row, 9].ToString();
                    if(toRoom == null || toRoom == "")
                        student.ToRoom = Student.noRoom;
                    else
                        student.ToRoom = Convert.ToInt32(toRoom);
                }
                catch (System.NullReferenceException e)
                {
                    student.ToRoom = Student.noRoom;
                }

                try
                {
                    string isOut = valueArray[row, 7].ToString();
                    if (isOut == "1" )
                    {
                        student.move();
                    }
                }
                catch (System.NullReferenceException e)
                {
                    
                }


                students.Add(student);
            }

            workbook.Close(false, Type.Missing, Type.Missing);
            //Marshal.ReleaseComObject(workbook);

            _excelApp.Quit();
            //Marshal.FinalReleaseComObject(_excelApp);

            return students;
        }

        public List<Student> read()
        {
            List<Student> students = new List<Student>();

            switch (sourceType)
            {
                case SourceType.XLSX:
                    students = readExcel();
                    break;
            }

            return students;
        }
    }
}
