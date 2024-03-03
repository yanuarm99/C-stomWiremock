using WireMock.Server;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using System;
using System.IO;
using System.Text.RegularExpressions;
using WireMock.Settings;

namespace CustomWireMock
{
    internal class Program
    {
        internal static void Main(string[] args)
        {
            Console.WriteLine("Selamat datang di Program C#stomWireMock!");
            Console.WriteLine("Silakan pilih opsi:");
            Console.WriteLine("1. Jalankan WireMock Server");
            Console.WriteLine("2. Tambahkan Response JSON");
            Console.WriteLine("3. Keluar");

            bool isRunning = true;

            while (isRunning)
            {
                Console.Write("Pilihan: ");
                string input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        StartWireMockServer.Run();
                        break;
                    case "2":
                        FileManagement.AddJsonResponse();
                        break;
                    case "3":
                        isRunning = false;
                        break;
                    default:
                        Console.WriteLine("Pilihan tidak valid. Silakan coba lagi.");
                        break;
                }

                Console.WriteLine();
            }

            Console.WriteLine("Terima kasih telah menggunakan Program C#stomWireMock!");
        }
    }
}