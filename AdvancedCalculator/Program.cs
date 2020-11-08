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
        static bool IsNothing(char text)
        {
            if ((" =".IndexOf(text) != -1))
                return true;
            else
                return false;
        }
        static string ParseExpression(string text)
        {
            Stack<char> opStack = new Stack<char>();
            string output = "";
            for (int i = 0; i < text.Length; i++)
            {
                if (IsDigit(text[i]))
                {
                    while (!IsNothing(text[i]) && !IsOp(text[i]))
                    {
                        if (text.Length - i >= 3 && text[i + 1] == ',' && IsDigit(text[i + 2]))
                        {
                            output += text[i] + text[i + 1] + text[i + 2];
                            i += 2;
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
                if (IsOp(text[i]))
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
    }
}