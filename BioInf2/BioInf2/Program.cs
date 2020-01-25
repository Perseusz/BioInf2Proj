using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace BioInf2
{
    class Program
    {
        private static List<Node> graph;
        private static List<string> spectrum;
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
            graph = new List<Node>();
            spectrum = new List<string>();

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
                Console.Write("Ilość podsekwencji: ");
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


            // --- sprawdzenie czy ze spektrum da się złożyć sekwencję DNA podaną wcześniej
            MakeGrapf(spectrum, k);
            correctGraph = ValidateGraph(graph, lengthOfSpectrum);
            if(!correctGraph)
            {
                Console.WriteLine("W grafie nie da się wyznaczyć ścieżki Eulera!");
                Console.WriteLine("Z tego spektrum nie stworzy się DNA!");
            }
            else
            {
                string seq = EulerPathDNA(graph, lengthOfSpectrum);
                // --> TO DO!!!!!!!!!!!!!

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

        // tworzenie grafu
        public static void MakeGrapf(List<string> spectrum, int k)
        {
            for (int i = 0; i < spectrum.Count(); i++)
            {
                Node n = new Node(spectrum[i]);
                int next = i + 1;
                if (next >= spectrum.Count())
                {
                    next = spectrum.Count() - 1;
                }

                int before = i - 1;
                if (before >= 0)
                {
                    if (graph[graph.Count - 1].GetCoverage(n.GetValue()) == 0)
                    {
                        if (n.GetCoverage(spectrum[next]) == 0)
                        {
                            //error
                        }
                        graph.Add(n);
                        n.SetId(graph.Count);
                    }
                    else
                    {
                        graph.Add(n);
                        n.SetId(graph.Count);
                    }
                }
                else
                {
                    if (n.GetCoverage(spectrum[next]) == 0)
                    {
                        //error
                    }
                    else
                    {
                        graph.Add(n);
                        n.SetId(graph.Count);
                    }
                }
            }
            
            foreach (Node node in graph)
            {
                foreach (Node node1 in graph)
                {
                    if (node1 != node)
                    {
                        node.AddNeighbour(node1);
                    }

                }
            }

        }
        
        // sprawdzenie czy w grafie można stworzyć ścieżkę Eulera, bez tego nie stworzymy DNA ze spektrum
        public static bool ValidateGraph(List<Node> graph, int lengthOfSpectrum)
        {
            bool isGoodGraph = true;
            int tempConnectionValue = 0;
            int nodeEdgesValue = 0;
            int incorrect = 0;
            int maxIncorrect = 2;
            for(int i = 0; i < lengthOfSpectrum; i++)
            {
                for(int j = 0; j < lengthOfSpectrum - 1; j++)
                {
                    if (graph[i] != graph[j])
                    {
                        tempConnectionValue = graph[i].GetNeighbourConnection(graph[j]);
                        if(tempConnectionValue > 0)
                        {
                            nodeEdgesValue++;
                        }
                    }
                }
                if(nodeEdgesValue%2 != 0)
                {
                    incorrect++;
                }
                if(incorrect > maxIncorrect)
                {
                    isGoodGraph = false;
                    break;
                }
                nodeEdgesValue = 0;
            }

            return isGoodGraph;
        }

        // tworzenie ścieżki Eulera -> DNA
        public static string EulerPathDNA(List<Node> graph, int lengthOfSpectrum) // --> TO DO!!!!!!!!!!!!!
        {
            string madeSeq = "";
            int counter = 0;
            bool isDone = false;
            while(!isDone)
            {




            }

            return madeSeq;
        }
    }   
}
