using System.Diagnostics;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace WebServerNET
{
    class Server
    {
        static readonly HttpListener server = new();

        static private void OpenUrl(string url)
        {
            try
            {
                Process.Start(url);
            }
            catch
            {
                //: https://github.com/dotnet/corefx/issues/10361
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    url = url.Replace("&", "^&");
                    Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    Process.Start("xdg-open", url);
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                {
                    Process.Start("open", url);
                }
                else
                {
                    throw;
                }
            }
        }
        private static double Generate()
        {
            double result = 0;
            return result;
        }
        private static bool Check(float table, float rowx, float rowy)
        {
            return true;
        }

        static async Task Listen()
        {
            CheckFiles();
            string? html = File.ReadAllText("../../../API/index.html", Encoding.UTF8);
            string? css = File.ReadAllText("../../../API/styles.css", Encoding.UTF8);
            string? js = File.ReadAllText("../../../API/script.js", Encoding.UTF8);
            byte[] ico = File.ReadAllBytes("../../../API/favicon.ico");
            while (true)
            {
                var context = await server.GetContextAsync();
                var response = context.Response;
                Console.WriteLine(context.Request.RawUrl);
                response.StatusCode = 200;
                switch (context.Request.RawUrl)
                {
                    case "/":
                        {
                            context.Response.ContentType = "text/html";
                            byte[] buffer = Encoding.UTF8.GetBytes(html);
                            response.ContentLength64 = buffer.Length;
                            using Stream output = response.OutputStream;
                            await output.WriteAsync(buffer);
                            await output.FlushAsync();
                            break;
                        }
                    case "/styles.css":
                        {
                            byte[] buffer = Encoding.UTF8.GetBytes(css);
                            response.ContentLength64 = buffer.Length;
                            using Stream output = response.OutputStream;
                            await output.WriteAsync(buffer);
                            await output.FlushAsync();
                            break;
                        }
                    case "/script.js":
                        {
                            byte[] buffer = Encoding.UTF8.GetBytes(js);
                            response.ContentLength64 = buffer.Length;
                            using Stream output = response.OutputStream;
                            await output.WriteAsync(buffer);
                            await output.FlushAsync();
                            break;
                        }
                    case "/favicon.ico":
                        {
                            response.ContentType = "image/vnd.microsoft.icon";
                            var dataStream = new MemoryStream(ico);
                            using Stream output = response.OutputStream;
                            await output.WriteAsync(dataStream.ToArray());
                            await output.FlushAsync();
                            break;
                        }
                    case "/api":
                        {
                            byte[] buffer = Encoding.UTF8.GetBytes("API Server is online!");
                            response.ContentLength64 = buffer.Length;
                            using Stream output = response.OutputStream;
                            await output.WriteAsync(buffer);
                            await output.FlushAsync();
                            break;
                        }
                    case "/api/d00":
                        {
                            byte[] buffer = Encoding.UTF8.GetBytes("200");
                            response.ContentLength64 = buffer.Length;
                            using Stream output = response.OutputStream;
                            await output.WriteAsync(buffer);
                            await output.FlushAsync();
                            break;
                        }
                    default:
                        {
                            //example
                            // /api/0000000000000000000000001110000000111000000011100000001110000000111000000000000000000000000000000000
                            //if (context.Request.RawUrl.StartsWith("api/check/"))
                            //{
                            //    response.StatusCode = 200;
                            //    string moss = context.Request.RawUrl[5..];

                            //}
                            //else
                            //{
                            //    response.StatusCode = 404;
                            //    byte[] buffer = Encoding.UTF8.GetBytes("Page does not exist -_-");
                            //    response.ContentLength64 = buffer.Length;
                            //    using Stream output = response.OutputStream;
                            //    await output.WriteAsync(buffer);
                            //    await output.FlushAsync();
                            //}
                            //cut
                            byte[] buffer = Encoding.UTF8.GetBytes("Page does not exist -_-");
                            response.ContentLength64 = buffer.Length;
                            using Stream output = response.OutputStream;
                            await output.WriteAsync(buffer);
                            await output.FlushAsync();
                            //cut

                            break;
                        }
                }




                context.Response.ContentType = "text/html";

                //Console.WriteLine("Registered new request");
            }

        }
        private static void CheckFiles()
        {
            FileInfo index = new("../../../API/index.html");
            if (!index.Exists)
            {
                Console.WriteLine("Не найден файл index.html");
                Environment.Exit(0);
            }
            FileInfo css = new("../../../API/styles.css");
            if (!css.Exists)
            {
                Console.WriteLine("Не найден файл styles.css");
                Environment.Exit(0);
            }
            FileInfo js = new("../../../API/script.js");
            if (!js.Exists)
            {
                Console.WriteLine("Не найден файл script.js");
                Environment.Exit(0);
            }
            FileInfo cpp = new("../../../API/including.cpp");
            Console.WriteLine("Got all files, running...");
        }


        static async Task Main()
        {
            Console.WriteLine("Http Clouds server v1.0.0\nStarting...");

            string adress = @"http://127.0.0.1:1488/";
            server.Prefixes.Add(adress);
            server.Start();
            Console.WriteLine(value: "Server working on " + adress);
            Listen();
            //OpenUrl(adress);
            //System.Diagnostics.Process.Start(adress);

            Console.WriteLine("Listening has been started!");

            bool work = true;
            string? input;
            while (work)
            {
                input = Console.ReadLine().ToLower();
                switch (input)
                {
                    case "exit":
                        {
                            work = false;
                            break;
                        }
                    case "e": goto case "exit";
                    case "q": goto case "exit";
                    case "s": goto case "exit";
                    case "quit": goto case "exit";
                    case "close": goto case "exit";
                    case "stop": goto case "exit";
                    case "clear":
                        {
                            Console.Clear();
                            break;
                        }
                    case "c": goto case "clear";
                    case "restart":
                        {
                            Console.WriteLine("Stopping...");
                            server.Stop();
                            Console.WriteLine("Restarting...");
                            server.Start();
                            if (server.IsListening) Console.WriteLine("Listening started!");
                            else
                            {
                                Console.WriteLine("Listening err, going to shutdown...");
                                work = false;
                            }
                            break;
                        }
                    case "info":
                        {
                            Console.WriteLine("Not realised(");
                            break;
                        }
                    default:
                        {
                            Console.WriteLine("Cant understand :(");
                            break;
                        }
                }
            }
            server.Close();
            Console.WriteLine("Server stopped");
        }
    }
}