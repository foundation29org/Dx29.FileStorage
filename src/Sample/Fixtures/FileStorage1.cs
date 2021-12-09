using System;
using System.Net.Http;
using System.Threading.Tasks;

using Dx29;

namespace Sample
{
    class FileStorage1
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
            http.POST($"FileStorage/file/usr-0001/mec-0001/{resourceId}/hello.txt", "Hello world!");

            // Download file
            Console.WriteLine("Downloading...");
            var res = http.GET($"FileStorage/file/usr-0001/mec-0001/{resourceId}/hello.txt");
            Console.WriteLine(res);

            // Delete file
            Console.WriteLine("Delete file...");
            http.DELETE($"FileStorage/file/usr-0001/mec-0001/{resourceId}/hello.txt");


            // Upload object
            Console.WriteLine("Uploading object...");
            http.POST<MyClass>($"FileStorage/file/usr-0001/mec-0001/{resourceId}/sample.json", new MyClass { Id = "abcd", Date = DateTime.UtcNow });

            // Download object
            Console.WriteLine("Downloading object...");
            var obj = http.GET<MyClass>($"FileStorage/file/usr-0001/mec-0001/{resourceId}/sample.json");
            Console.WriteLine(obj.Serialize());

            // Delete object
            Console.WriteLine("Delete object...");
            http.DELETE($"FileStorage/file/usr-0001/mec-0001/{resourceId}/sample.json");
        }

        class MyClass
        {
            public string Id { get; set; }
            public DateTimeOffset Date { get; set; }
        }
    }
}
