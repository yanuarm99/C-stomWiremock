using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;

namespace CustomWireMock
{

    public class JsonResponseTemplate
	{
		private string method;
		private string url;
		private string fixedDelayMilliseconds;
		private string jsonBody;

		public JsonResponseTemplate(string method, string url, string fixedDelayMilliseconds, string jsonBody)
		{
			this.method = method;
			this.url = url;
			this.fixedDelayMilliseconds = fixedDelayMilliseconds;
			this.jsonBody = jsonBody;
		}

		public string BeautifyJson(string json)
		{
			JToken parsedJson = JToken.Parse(json);
			return parsedJson.ToString(Formatting.Indented);
		}

		public string GenerateJson()
		{
			string json = @"{
	  ""scenarioName"": ""C#-Simulator"",
	  ""request"": {
		""method"": """ + method + @""",
		""url"": """ + url + @"""
	  },
	  ""response"": {
		""status"": 200,
		""fixedDelayMilliseconds"": """ + fixedDelayMilliseconds + @""",
		""headers"": {
		  ""X-Simulator"": ""C#-Simulator"",
		  ""Content-Type"": ""application/json;charset=UTF-8"",
		  ""Access-Control-Allow-Origin"": ""*"",
		  ""Access-Control-Allow-Methods"": """ + method + @""",
		  ""Access-Control-Allow-Credential"": ""true"",
		  ""Access-Control-Allow-Headers"": ""accept, content-type""
		},
		""jsonBody"": " + jsonBody + @"
	  }
	}";

			return BeautifyJson(json);
		}
	}

	public class FileManagement
	{
		private static readonly string ProjectDirectory = Environment.GetEnvironmentVariable("CUSTOM_WIREMOCK");

		public static void AddJsonResponse()
		{
			Console.Write("Enter method (GET/POST/PUT/DELETE): ");
			string method = Console.ReadLine().ToUpper();

			Console.Write("Enter URL: ");
			string url = Console.ReadLine();

			Console.Write("Enter fixed delay (in milliseconds): ");
			string fixedDelayMilliseconds = Console.ReadLine();

			string txtFilePath = Path.Combine(ProjectDirectory, "dummy.txt");

			try
			{
				if (!Directory.Exists(ProjectDirectory))
				{
					Directory.CreateDirectory(ProjectDirectory);
				}
				CreateDummyFile(txtFilePath);

				string jsonBody = ConvertToJsonString(txtFilePath);

				JsonResponseTemplate template = new JsonResponseTemplate(method, url, fixedDelayMilliseconds, jsonBody);
				string finalJson = template.GenerateJson();

				string fileName = GetValidFileName(url);
				string jsonFilePath = Path.Combine(ProjectDirectory, fileName + ".json");

				File.WriteAllText(jsonFilePath, finalJson);

				File.Delete(txtFilePath);

				Console.WriteLine("Response JSON added successfully.");
				Console.ReadLine();
				Console.WriteLine("Press Enter to go back to the main menu.");
				Console.ReadLine();
				Program.Main(new String[0]);
			}
			catch (NullReferenceException e)
			{
				Console.WriteLine("NullReferenceException occurred: " + e.Message);
				Console.ReadLine();
				Console.WriteLine("Press Enter to go back to the main menu.");
				Console.ReadLine();
			}
		}

		public static string ConvertToJsonString(string filePath)
		{
			try
			{
				string jsonString = File.ReadAllText(filePath);
				// jsonString = @jsonString.Replace("\"", "\"\"");
				return jsonString;
			}
			catch (FileNotFoundException)
			{
				throw new FileNotFoundException("File not found.");
			}
			catch (IOException)
			{
				throw new IOException("An error occurred while reading the file.");
			}
			catch (Exception)
			{
				throw new Exception("An error occurred while converting JSON to string.");
			}
		}

		// public static void CreateDummyFile(string path)
		// {
		// 	string txtFilePath = path;

		// 	try
		// 	{
		// 		using (StreamWriter writer = new StreamWriter(txtFilePath))
		// 		{
		// 			Console.WriteLine("Silakan isi file dummy.txt dengan jsonBody yang kamu inginkan.");
		// 			Process process = Process.Start("notepad.exe", txtFilePath);

		// 			// Tambahkan perulangan untuk menunggu hingga file disimpan dan Notepad ditutup
		// 			while (!process.HasExited)
		// 			{
		// 				Thread.Sleep(100);
		// 			}

		// 			string content;
		// 			using (StreamReader reader = new StreamReader(txtFilePath))
		// 			{
		// 				content = reader.ReadToEnd();
		// 			}
		// 			writer.Write(content);
		// 		}

		// 		Console.WriteLine("File dummy.txt berhasil dibuat.");
		// 		Console.ReadLine();
		// 	}
		// 	catch (Exception e)
		// 	{
		// 		Console.WriteLine("Terjadi kesalahan: " + e.Message);
		// 		Console.ReadLine();
		// 		Console.WriteLine("Tekan Enter untuk kembali ke menu awal.");
		// 		Console.ReadLine();
		// 	}
		// }

		public static void CreateDummyFile(string path)
		{
			string txtFilePath = path;

			try
			{
				if (!File.Exists(txtFilePath))
				{
					File.Create(txtFilePath).Close(); // Buat file dummy.txt jika belum ada
				}
				Console.WriteLine("Silakan masukkan konten JSON ke dalam file dummy.txt.");

				// Baca konten JSON dari masukan pengguna hingga ditemui baris kosong
				using (StreamWriter writer = new StreamWriter(txtFilePath))
				{
					string line;
					while (!string.IsNullOrEmpty(line = Console.ReadLine()))
					{
						writer.WriteLine(line);
					}
				}

				Console.WriteLine("File dummy.txt berhasil dibuat.");
				Console.ReadLine();
			}
			catch (Exception e)
			{
				Console.WriteLine("Terjadi kesalahan: " + e.Message);
				Console.ReadLine();
				Console.WriteLine("Tekan Enter untuk kembali ke menu utama.");
				Console.ReadLine();
			}
		}

		private static string GetValidFileName(string url)
		{
			string sanitizedUrl = Regex.Replace(url, "/|\\\\", "-");
			return sanitizedUrl.Substring(1);
		}
	}
}