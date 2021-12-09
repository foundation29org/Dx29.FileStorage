using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

using Dx29;

namespace Sample
{
    class FileStorage2
    {
        static public async Task RunAsync()
        {
            await Task.Delay(500);
            Run();
        }

        private static void Run()
        {
            var http = new HttpClient() { BaseAddress = new Uri("http://localhost:5700/api/v1/") };

            // About version
            Console.WriteLine(http.GET("About/version"));

            var resourceId = DateTime.UtcNow.ToString("yyyyMMdd-HHmmss.ff");

            // Upload file
            Console.WriteLine("Uploading...");
            http.POST($"FileStorage2/usr-0001/mec-0001?path={resourceId}/sample.txt", "Hello world!");

            // Copy file
            Console.WriteLine("Copying...");
            http.PUT($"FileStorage2/usr-0001?source=mec-0001/{resourceId}/sample.txt&target=mec-0001/{resourceId}/sample2.txt&copy=true", "");

            // Move file
            Console.WriteLine("Moving...");
            http.PUT($"FileStorage2/usr-0001?source=mec-0001/{resourceId}/sample2.txt&target=mec-0001/{resourceId}/sample3.txt", "");

            // Download file
            Console.WriteLine("Downloading...");
            var res = http.GET($"FileStorage2/usr-0001/mec-0001?path={resourceId}/sample.txt");
            Console.WriteLine(res);

            // Get share
            Console.WriteLine("Sharing...");
            var share = http.GET($"FileStorage2/usr-0001/mec-0001/share?path={resourceId}/sample.txt");
            Console.WriteLine(share);

            // Download share
            Console.WriteLine("Downloading share...");
            var cli = new WebClient();
            res = cli.DownloadString(share);
            Console.WriteLine(res);

            // Delete files
            Console.WriteLine("Delete files...");
            http.DELETE($"FileStorage2/usr-0001/mec-0001?path={resourceId}/sample.txt");
            http.DELETE($"FileStorage2/usr-0001/mec-0001?path={resourceId}/sample3.txt");

            // Upload object
            Console.WriteLine("Uploading object...");
            http.POST($"FileStorage2/usr-0001/mec-0001?path={resourceId}/sample.json", new MyClass { Id = "abcd", Date = DateTime.UtcNow });

            // Download object
            Console.WriteLine("Downloading object...");
            var obj = http.GET($"FileStorage2/usr-0001/mec-0001?path={resourceId}/sample.json");
            Console.WriteLine(obj.Serialize());

            // Delete object
            Console.WriteLine("Delete object...");
            http.DELETE($"FileStorage2/usr-0001/mec-0001?path={resourceId}/sample.json");
        }

        class MyClass
        {
            public string Id { get; set; }
            public DateTimeOffset Date { get; set; }
        }
    }
}
