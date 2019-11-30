using System;
using System.IO;

namespace MovieDatabase
{
    class Program
    {
        static void Main(string[] args)
        {
            Program p = new Program();

            Console.Write("Selecione uma opção: \n 1 - Títulos\n " +
                "2 - Pessoas\n 3 - Sair\n => ");

            p.StartMenu(p);
        }

        private void StartMenu(Program p)
        {
            string response = Console.ReadLine();

            switch (response)
            {
                case "1":
                    p.AskTitle(p);
                    break;
                case "2":

                    break;
                case "3":
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("Digite uma das opções disponíveis");
                    break;
            }
        }

        private void AskTitle(Program p)
        {
            string option;
            Query q = new Query();

            Console.Write("1 - Pesquisar\n2 - Voltar\n => ");
            option = Console.ReadLine();

            if (option == "1")
            {
                // Call method with all the titles with the certain word
                string s;
                //Console.Write("Digite o que quer pesquisar.\n => ");
                s = Console.ReadLine();
                q.SearchTitle(s);
            }
            else if (option == "2")
                // Call method of start menu
                p.StartMenu(p);
            else
                Console.WriteLine("Digite uma das opções disponíveis");
        }
    }
}
