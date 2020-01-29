using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace BioInf2
{
    class Program
    {
        private static List<string> spectrum;
        private static List<TrueNode> trueGraph;
        static void Main(string[] args)
        {
            Regex regex1 = new Regex(@"[ACGT]");
            int k = 0; //długość podsekwencji, na które podzielimy sekwencje
            bool correctK = false; // czy poprawne k
            int lengthOfSpectrum = 0; // wielkość spektrum
            int howManyPossibleErrors = 0; // ile możliwych błędów
            bool correctLengthOfSpectrum = false; // czy poprawne spektrum
            bool correctSubSeq = false; // flaga do sprawdzania poprawności wpisywanych podsekwencji dla spektrum
            bool correctErrorLength = false; // czy poprawna ilość możliwych błędów (0-3)
            bool correctGraph = false; // czy graf jest poprawny pod tworzenie ścieżki Eulera
            spectrum = new List<string>();
            trueGraph = new List<TrueNode>();

            Console.WriteLine("Sekwencjonowanie przez hybrydyzację.");
            
            // --- użytkownik podaje wielkość słów jakie będą w spektrum
            Console.WriteLine("Podaj wielkość słów w spektrum, dla którego będzie sprawdzana sekwencja DNA."); 
            while (!correctK)
            {
                Console.Write("K -> Wielkość słów w spektrum: ");
                string tempK = Console.ReadLine();
                Console.WriteLine("");
                bool isNumeric = int.TryParse(tempK, out int temp);
                if (isNumeric && temp > 1)
                {
                    correctK = true;
                    k = temp;
                }
                else
                {
                    Console.WriteLine("Wprowadzone dane są złe. Prosze wprowadzć liczbę naturalną, która jest większa niż 1");
                }
            }
            // --- użytkownik podaje ilość podsekwencji w spektrum
            Console.WriteLine("Podaj ilość podsekwencji, które będą tworzyć spektrum");
            while (!correctLengthOfSpectrum)
            {
                Console.Write("Ilość podsekwencji: ");
                string tempSpectrumLength = Console.ReadLine();
                Console.WriteLine("");
                bool isNumeric = int.TryParse(tempSpectrumLength, out int temp);
                if (isNumeric && temp > 0)
                {
                    correctLengthOfSpectrum = true;
                    lengthOfSpectrum = temp;
                }
                else
                {
                    Console.WriteLine("Wprowadzone dane są złe. Prosze wprowadzć liczbę naturalną, która jest większa niż 0");
                }
            }

            // --- użytkownik podaje podsekwencje wchodzące w skład spektrum
            Console.WriteLine("Podaj podsekwencje wchodzące w skład spektrum, każde w nowej linii.");
            for (int i = 0; i < lengthOfSpectrum; i++)
            {
                while (!correctSubSeq)
                {
                    Console.WriteLine("Pozostałe podsekwencje do wpisania: {0}", lengthOfSpectrum - i);
                    Console.WriteLine("Możliwe nukelotydy do budowy podsekwencji: 'A', 'C', 'G', 'T'.");
                    Console.WriteLine("Poprawna długość każdej podsekwencji to: {0}", k);
                    Console.Write("Podsekwencja: ");
                    string tempSubSeq = Console.ReadLine();
                    Console.WriteLine("");
                    if (!regex1.IsMatch(tempSubSeq))
                    {
                        Console.WriteLine("Użyto niepoprawnego nukleotydu!");
                    }
                    else
                    {
                        if (tempSubSeq.Length != k)
                        {
                            Console.WriteLine("Stworzono podsekwencję nieporawnej długośći!");
                        }
                        else
                        {
                            if (spectrum.Contains(tempSubSeq))
                            {
                                Console.WriteLine("Taka podsekwencja znajduje się już w spektrum, wpisz inną!");
                            }
                            else
                            {
                                spectrum.Add(tempSubSeq);
                                correctSubSeq = true;
                            }
                            
                        }
                    }
                }
                correctSubSeq = false;
            }

            // --- ile jest dopuszczalnych błędów
            Console.WriteLine("Podaj liczbę możliwych błędów pozytywnych/negatywnych.");
            while (!correctErrorLength)
            {
                Console.Write("Ilość możliwych błędów: ");
                string tempSpectrumLength = Console.ReadLine();
                Console.WriteLine("");
                bool isNumeric = int.TryParse(tempSpectrumLength, out int temp);
                if (isNumeric && ((temp == 0) || (temp == 1) || (temp == 2) || (temp == 3)))
                {
                    correctErrorLength = true;
                    howManyPossibleErrors = temp;
                }
                else
                {
                    Console.WriteLine("Wprowadzone dane są złe. Prosze wprowadzć liczbę naturalną 0-3");
                }
            }

            MakeTrueGraph(k);
            string result = EulerianTrueGraphPath(k, howManyPossibleErrors);

            if(result == "0")
            {
                Console.WriteLine("Niestety nie udało się stworzyć DNA.");
                Console.WriteLine("Przynajmniej jeden wierzchołek ma zbyt dużą nierówność ilości połączeń wchodzących i wychodzących.");
            }
            else if(result == "1")
            {
                Console.WriteLine("Niestety nie udało się stworzyć DNA.");
                Console.WriteLine("Liczba wierzchołków, które pozwalają na stworzenie ścieżki Eulera, które mają różną ilość połączeń wchodzących i wychodzących jest różna od 2.");
            }
            else if(result == "2")
            {
                Console.WriteLine("Niestety nie udało się stworzyć DNA.");
                Console.WriteLine("Wierzchołki z różną liczbą wejść i wyjść nie dopełniają się(1 ma mieć więcej wyjść drugi wejść.");
            }
            else if(result == "3")
            {
                Console.WriteLine("Niestety nie udało się stworzyć DNA.");
                Console.WriteLine("Liczba błędów przekroczyła dopuszczalną przez użytkownika wartość.");
            }
            else
            {
                Console.WriteLine("Sukces! Udało się stworzyć DNA.");
                Console.WriteLine("DNA: {0}", result);
            }

            string stop = Console.ReadLine();
        }

        // generowanie losowej sekwencji DNA
        private static string GenerateRandomSequence(int length)
        {
            char[] letters = "ACGT".ToCharArray();
            Random random = new Random();
            string randomString = "";
            for (int i = 0; i < length; i++)
            {
                randomString += letters[random.Next(0, 4)].ToString();
            }

            return randomString;
        }

        // wypisanie spektrum
        public static void PrintSpectrum(List<string> spectrum)
        {
            Console.Write("Wybrane spektrum: \n [");
            for(int i = 0; i < spectrum.Count(); i++)
            {
                Console.Write(" " + spectrum[i] + " ");
            }
            Console.WriteLine("]");
        }
        
        // tworzy graf jak w wykładzie w5 z dawniejszych wykładów
        public static void MakeTrueGraph(int k)
        {
            for(int i = 0; i < spectrum.Count(); i++)
            {
                if (i == 0)
                {
                    TrueNode temp = new TrueNode(spectrum[i].Substring(0, k - 1));
                    TrueNode temp2 = new TrueNode(spectrum[i].Substring(1, k - 1));
                    if(spectrum[i].Substring(0, k - 1) == spectrum[i].Substring(1, k - 1))
                    {
                        temp.AddOutNeighbour(temp2.GetValue());
                        trueGraph.Add(temp);
                    }
                    else
                    {
                        temp.AddOutNeighbour(temp2.GetValue());
                        trueGraph.Add(temp);
                        trueGraph.Add(temp2);
                    }
                }
                else
                {
                    bool isNew = true;
                    int counter = 0;
                    foreach (TrueNode n in trueGraph)
                    {
                        if (spectrum[i].Substring(0, k - 1) == n.GetValue())
                        {
                            trueGraph[counter].AddOutNeighbour(spectrum[i].Substring(1, k - 1));
                            isNew = false;
                            break;
                        }
                        counter++;
                    }
                    if(isNew)
                    {
                        TrueNode temp = new TrueNode(spectrum[i].Substring(0, k - 1));
                        TrueNode temp2 = new TrueNode(spectrum[i].Substring(1, k - 1));
                        if (spectrum[i].Substring(0, k - 1) == spectrum[i].Substring(1, k - 1))
                        {
                            temp.AddOutNeighbour(temp2.GetValue());
                            trueGraph.Add(temp);
                        }
                        else
                        {
                            isNew = true;
                            foreach (TrueNode n in trueGraph)
                            {
                                if (spectrum[i].Substring(1, k - 1) == n.GetValue())
                                {
                                    isNew = false;
                                    break;
                                }
                            }
                            if (isNew)
                            {
                                temp.AddOutNeighbour(temp2.GetValue());
                                trueGraph.Add(temp);
                                trueGraph.Add(temp2);
                            }
                            else
                            {
                                temp.AddOutNeighbour(temp2.GetValue());
                                trueGraph.Add(temp);
                            }
                        }
                    }
                    else
                    {
                        isNew = true;
                        foreach (TrueNode n in trueGraph)
                        {
                            if (spectrum[i].Substring(1, k - 1) == n.GetValue())
                            {
                                isNew = false;
                                break;
                            }
                        }
                        if(isNew)
                        {
                            TrueNode temp2 = new TrueNode(spectrum[i].Substring(1, k - 1));
                            trueGraph.Add(temp2);
                        }
                    }
                }
            }

            foreach(TrueNode n in trueGraph)
            {
                foreach(TrueNode m in trueGraph)
                {
                    if(m.IsInOutNeighbours(n.GetValue()))
                    {
                        n.AddInNeighbour(m.GetValue());
                    }
                }
            }

        }

        // wybieranie startowego wierzchołka przy tworzeniu ścieżki Eulera
        public static string GetStartingTrueNode()
        {
            string result = trueGraph[0].GetValue();
            int howManyNotTheSame = 0;
            int howManyMoreIn = 0;
            int howManyMoreOut = 0;
            bool isGood = true;

            foreach(TrueNode n in trueGraph)
            {
                if(n.CountInNeighbours() != n.CountOutNeighbours())
                {
                    howManyNotTheSame++;
                    if(n.CountInNeighbours() > n.CountOutNeighbours())
                    {
                        if((n.CountInNeighbours() - n.CountOutNeighbours()) > 1)
                        {
                            isGood = false;
                            break;
                        }
                        howManyMoreIn++;
                    }
                    if (n.CountInNeighbours() < n.CountOutNeighbours())
                    {
                        if ((n.CountOutNeighbours() - n.CountInNeighbours()) > 1)
                        {
                            isGood = false;
                            break;
                        }
                        result = n.GetValue();
                        howManyMoreOut++;
                    }
                }
            }

            if(!isGood)
            {
                result = "0";
                return result;
            }
            if(howManyNotTheSame > 0 && howManyNotTheSame != 2)
            {
                result = "1";
                return result;
            }
            if(howManyNotTheSame == 2 && howManyMoreIn != 1 && howManyMoreOut != 1)
            {
                result = "2";
                return result;
            }

            return result;
        }

        // tworzenie ścieżki Eulera dla grafu skierowanego
        public static string EulerianTrueGraphPath(int k, int howManyPossibleErrors)
        {
            string madeSeq = "";
            Stack<TrueNode> stack = new Stack<TrueNode>();
            List<TrueNode> eulerPath = new List<TrueNode>();

            if (trueGraph.Count == 0)
            {
                Console.WriteLine("Graf nie ma wierzchołków");
                return madeSeq;
            }

            string start = GetStartingTrueNode();
            if(start.Length == 1)
            {
                return start;
            }
            int index = GetIndexOfValueFromGraph(start);

            TrueNode currentNode = trueGraph[index];

            while (true)
            {
                if(currentNode.CountOutNeighbours() == 0)
                {
                    eulerPath.Add(currentNode);
                    if (currentNode.CountOutNeighbours() == 0 && stack.Count == 0)
                    {
                        break;
                    }
                    currentNode = stack.Pop();
                }
                else
                {
                    stack.Push(currentNode);
                    string node = currentNode.GetOneFromOutNeighbours(0);
                    currentNode.DeleteOneOutNeighbour(node);
                    int indexOfNode = GetIndexOfValueFromGraph(node);
                    currentNode = trueGraph[indexOfNode];
                }
            }

            eulerPath.Reverse();
            bool isFirst = true;
            foreach (TrueNode node in eulerPath)
            {
                madeSeq += isFirst ? node.GetValue()
                                   : node.GetValue().Substring(node.GetValue().Length - 1);
                isFirst = false;
                node.IncVisits();
            }

            int countOvervisits = 0;
            foreach(TrueNode node in trueGraph)
            {
                if(node.GetVisits() != 1)
                {
                    countOvervisits += node.GetVisits() - 1;
                }
            }
            if(countOvervisits > howManyPossibleErrors)
            {
                return "3";
            }

            return madeSeq;
        }

        // szukanie w liście wierzchołków wierzchołka o danej wartości
        public static int GetIndexOfValueFromGraph(string s)
        {
            int index = 0;
            foreach (TrueNode n in trueGraph)
            {
                if (n.GetValue() == s)
                {
                    break;
                }
                index++;
            }
            return index;
        }


    }   
}
