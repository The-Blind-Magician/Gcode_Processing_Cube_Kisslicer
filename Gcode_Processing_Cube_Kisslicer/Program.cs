using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using System.Threading;

namespace Gcode_Processing_Cube_Kisslicer
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            OpenFileDialog open = new OpenFileDialog();
            open.Filter = "Gcode Files |*.gcode; *.bfb";
            try
            {
                string modLines = @"C:\Users\chris\Documents\GitHub\Gcode_Processing_Cube_Kisslicer\Gcode_Processing_Cube_Kisslicer\ModLines.txt";
                string[] insertLines = File.ReadAllLines(modLines);

                if (args[0] == null) open.ShowDialog();
                string modFilePath = (args[0] != null) ? args[0] : open.FileName;
                string writePath = modFilePath.Substring(0, modFilePath.LastIndexOf('.'));
                var list = File.ReadAllLines(modFilePath).ToList();

                int prefixLength = @"C:\Users\chris\Desktop\D&D\".Length-1;

                string newFile = modFilePath.Substring(prefixLength);

                int M108Num = 0;
                int lineNum = 0;

                for (int i = 0; i < 20; i++)
                {
                    var str = list.ElementAt(i);
                    if (str.Contains("M108"))
                    {
                        M108Num++;
                        lineNum = i + 1;
                    }
                    if (M108Num == 3)
                    {
                        foreach (string line in insertLines)
                        {
                            list.Insert(lineNum++, line);
                        }
                        break;
                    }
                }
                File.WriteAllLines(modFilePath.Replace(" ", "_"), list);
                Thread.Sleep(1000);
                System.Diagnostics.Process.Start(@"C:\Users\chris\Desktop\D&D\.cubepro-encoder.exe", args[0].Replace(" ", "_"));
                Thread.Sleep(2000);
                string f = $"{args[0].Replace(" ", "_").Substring(0, args[0].IndexOf('.')) }.cubepro";

                File.Move(f, f.Replace(".cubepro", ".cube"));
                if(modFilePath.Contains(" "))
                    File.Delete(modFilePath);
            }
            catch (SystemException e)
            {
                Console.WriteLine($"An Error Occured \n {e}");
                Console.Read();
            }
        }
    }
}
