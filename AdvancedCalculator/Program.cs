using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace AdvancedCalculator
{
    class Program
    {
        static void Main()
        {
        }
        static string ReadFile() //Чтение из файла
        {
            string path = Environment.CurrentDirectory + "\\text.txt";
            string text = File.ReadAllText(path);
            return text;
        }

    }
}