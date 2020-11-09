using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Text;

namespace AdvancedCalculator
{
    class Program
    {
        static void Main()
        {
            string[] text = ReadFile();
            WriteFile(text);
        }
        static void StreamWrite(string path, string text)
        {
            using (StreamWriter writer = new StreamWriter(path, true))
            {
                writer.WriteLine(text);
            }
        }
        static void WriteFile(string[] text)
        {
            string path = Environment.CurrentDirectory + "\\Output.txt";
            File.WriteAllText(path, "");
            StreamWrite(path, "ОПЗ:");
            string steps = RedactText(ReadFile()[1]); //ОПЗ
            string numX = ReadFile()[0];
            int firstRange = 0;
            int secondRange = 0;
            for (int i = 0; i < numX.Length; i++)
            {
                if (IsDigit(numX[i]))
                {
                    string temp = "";
                    while (IsDigit(numX[i]))
                    {
                        temp += numX[i];
                        i++;
                        if (i == numX.Length)
                            break;
                    }
                    if (firstRange == 0)
                        firstRange = int.Parse(temp);
                    else
                        secondRange = int.Parse(temp);
                    i--;
                }
            }
            int firstRange2 = firstRange;
            while (firstRange2 <= secondRange)
            {
                string tempSteps = steps.Replace("x", firstRange2.ToString());
                StreamWrite(path, $"x = {firstRange2}            {ParseExpression(tempSteps)}");
                firstRange2++;
            }
            StreamWrite(path, " ");
            StreamWrite(path, "Значения функции:");
            while (firstRange <= secondRange)
            {
                string tempSteps = steps.Replace("x", firstRange.ToString());
                StreamWrite(path, $"x = {firstRange}" + new string(' ', 20) + $"y ={Calculate(ParseExpression(tempSteps))}");                
                firstRange++;
            }
        }
        static string[] ReadFile() //Чтение из файла
        {
            string path = Environment.CurrentDirectory + "\\text.txt";
            string[] text = File.ReadAllLines(path);
            return text;
        }
        static string RedactText(string text)
        {
            text = text.Replace(" ", "");
            return text;
        }
        static bool IsDigit(char text)                              //Проверяем на число
        {
            if (int.TryParse(text.ToString(), out _))
                return true;
            else
                return false;
        }
        static bool IsOp(char text)                                 //Проверяем на операнд
        {
            if ("+-/*^()".Contains(text.ToString()))
                return true;
            else
                return false;
        }
        static short SetPrior(char prior)                          //Устанавливаем приоретет операций
        {
            switch (prior)
            {
                case '(': return 0;
                case ')': return 1;
                case '+': return 2;
                case '-': return 3;
                case '*': return 4;
                case '/': return 4;
                case '^': return 5;
                default: return 6;
            }
        }
        static string ParseExpression(string text)
        {
            Stack<char> opStack = new Stack<char>();
            string output = "";
            for (int i = 0; i < text.Length; i++)
            {
                if (IsDigit(text[i]))
                {
                    while (!IsOp(text[i]))
                    {
                        if (text.Length - i >= 3 && text[i + 1] == ',')
                        { 
                            output += text[i].ToString() + text[i + 1].ToString();
                            i ++;
                        }
                        else
                        {
                            output += text[i];
                        }
                        i++;
                        if (i == text.Length)
                            break;
                    }
                    output += " ";
                    i--;
                }
                else if (IsOp(text[i]))
                {
                    if (text[i] == '(')
                        opStack.Push(text[i]);
                    else if (text[i] == ')')
                    {
                        char s = opStack.Pop();
                        while (s != '(')           //Изменил
                        {
                            output += s + " ";
                            s = opStack.Pop();
                        }
                    }
                    else
                    {
                        if (opStack.Count > 0 && SetPrior(text[i]) <= SetPrior(opStack.Peek()))
                            output += opStack.Pop().ToString() + " ";
                        opStack.Push(text[i]);
                    }
                }
            }
            while (opStack.Count > 0)
            {
                output += opStack.Pop() + " ";
            }
            return output;
        }
        static double Calculate(string text)
        {
            Stack<double> numbers = new Stack<double>();
            double result = 0;
            for (int i = 0; i < text.Length; i++)
            {
                if (IsDigit(text[i]))
                {
                    string tempNum = "";
                    while (!IsOp(text[i]) && text[i] != ' ')
                    {
                        if (text.Length - i >= 3 && text[i + 1] == ',')
                        {
                            tempNum += text[i].ToString() + text[i + 1].ToString();
                            i++;
                        }
                        else
                        {
                            tempNum += text[i];
                        }
                        i++;
                        if (i == text.Length)
                            break;
                    }
                    numbers.Push(double.Parse(tempNum));
                    i--;
                }
                else if (IsOp(text[i]))
                {
                    double firstNum = numbers.Pop();
                    double secondNum = numbers.Pop();
                    switch (text[i])
                    {
                        case '+':
                            result = secondNum + firstNum;
                            break;
                        case '-':
                            result = secondNum - firstNum;
                            break;
                        case '*':
                            result = secondNum * firstNum;
                            break;
                        case '/':
                            result = secondNum / firstNum;
                            break;
                        case '^':
                            result = Math.Pow(firstNum, secondNum);
                            break;
                    }
                    numbers.Push(result);
                }
            }
            return numbers.Peek();
        }
    }
}