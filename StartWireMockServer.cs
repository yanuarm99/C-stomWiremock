using Newtonsoft.Json.Linq;
using WireMock.Server;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Settings;
using System;
using System.IO;

namespace CustomWireMock
{
    internal class StartWireMockServer
    {
        private static int port = 30000;

        public static void Run()
        {
            var serverSettings = new WireMockServerSettings
            {
                Port = port
            };

            var wireMockServer = WireMockServer.Start(serverSettings);

            string projectDirectory = Environment.GetEnvironmentVariable("CUSTOM_WIREMOCK") ?? "default_project_directory";

            string[] jsonFiles = Directory.GetFiles(projectDirectory, "*.json");

            foreach (string jsonFile in jsonFiles)
            {
                string fileName = Path.GetFileName(jsonFile);

                string jsonContent = File.ReadAllText(jsonFile);
                JObject jsonObject = JObject.Parse(jsonContent);

                string method = jsonObject.SelectToken("request.method").ToString();
                string url = jsonObject.SelectToken("request.url").ToString();

                switch (method.ToUpper())
                {
                    case "POST":
                        wireMockServer
                            .Given(Request.Create().WithPath(url).UsingPost())
                            .RespondWith(
                                Response.Create()
                                    .WithBodyFromFile(jsonFile)
                                    .WithStatusCode(200)
                            );
                        break;
                    case "GET":
                        wireMockServer
                            .Given(Request.Create().WithPath(url).UsingGet())
                            .RespondWith(
                                Response.Create()
                                    .WithBodyFromFile(jsonFile)
                                    .WithStatusCode(200)
                            );
                        break;
                    case "PUT":
                        wireMockServer
                            .Given(Request.Create().WithPath(url).UsingPut())
                            .RespondWith(
                                Response.Create()
                                    .WithBodyFromFile(jsonFile)
                                    .WithStatusCode(200)
                            );
                        break;
                    case "DELETE":
                        wireMockServer
                            .Given(Request.Create().WithPath(url).UsingDelete())
                            .RespondWith(
                                Response.Create()
                                    .WithBodyFromFile(jsonFile)
                                    .WithStatusCode(200)
                            );
                        break;
                    case "OPTIONS":
                        wireMockServer
                            .Given(Request.Create().WithPath(url).UsingOptions())
                            .RespondWith(
                                Response.Create()
                                    .WithBodyFromFile(jsonFile)
                                    .WithStatusCode(200)
                            );
                        break;
                    default:
                        Console.WriteLine("Invalid HTTP method in JSON file: " + method);
                        break;
                }
            }

            Console.WriteLine("WireMock server running on port {0}.", port);
            Console.ReadLine();

            wireMockServer.Stop();
        }
    }
}