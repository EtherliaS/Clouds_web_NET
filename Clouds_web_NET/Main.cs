using System.Diagnostics;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;

namespace WebServerNET
{
    public class Exercise
    {
        public int[] rowx { get; set; }
        public int[] rowy { get; set; }
        public Exercise()
        {
            rowx = new int[10];
            rowy = new int[10];
        }
    }

    class Server
    {
        static readonly HttpListener server = new();
        static bool assistant;
        static bool ScanAround(int x, int y, int w, int h, int[,] map)
        {
            // scan limits
            if (x + w > 10) return false;
            if (y + h > 10) return false;

            // scan above
            if (y != 0)
            {
                for (int i = x; i < (x + w); i++)
                    if (map[i, y - 1] != 0) return false;
                
            }

            // scan under
            if (y + h < 10)
            {
                for (int i = x; i < (x + w); i++)
                    if (map[i, h + y] != 0) return false;
                
            }

            // scan left
            if (x != 0)
            {
                for (int i = y; i < (y + h); i++)
                    if (map[x - 1, i] != 0) return false;
                
            }

            // scan right
            if (x + w < 10)
            {
                for (int i = y; i < (y + h); i++)
                    if (map[x + w, i] != 0) return false;
                
            }

            // scan corners
            if (x != 0 && y != 0) if (map[x - 1, y - 1] != 0) return false;
            if (x + w != 10 && y + h != 10) if (map[x + w, y + h] != 0) return false;
            if (x + w != 10 && y != 0) if (map[x + w, y - 1] != 0) return false;
            if (x != 0 && y + h != 10) if (map[x - 1, y + h] != 0) return false;

            return true;
        }

        static void Writetable(int[,] mos)
        {
            Console.WriteLine("|==========|");
            Console.Write("|");

            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    if (mos[j, i] == 1) Console.Write("1");
                    else Console.Write("0");
                }

                Console.Write("|\n|");
            }

            Console.WriteLine("==========|");
        }

        static void Randomize(int[] mx, int[] my, bool assist)
        {
            Random rnd = new();
            var clouds = rnd.Next(5, 8);
            var maxiterations = 1000;

            var moss = new int[10, 10];

            for (int i = 0; i < 0; i++)
                for (int j = 0; j < 0; j++)
                    moss[i, j] = 0;
                
 // filling 0

            int pointx;
            int pointy;
            int width;
            int height;

            for (int i = 0; i < clouds; i++) //change clouds count
            {
                for (int j = 0; j < maxiterations; j++) //random 100 iterations, very high chance for dropping good position
                {
                    pointx = rnd.Next(0, 10);
                    pointy = rnd.Next(0, 10);
                    width = rnd.Next(2, 4);
                    height = rnd.Next(2, 4);

                    if (ScanAround(pointx, pointy, width, height, moss))
                    {
                        // Console.WriteLine("========\nGenerated cloud: X-" + (pointx).ToString() + " Y-" + (pointy).ToString() + "\nW:" + (width).ToString() + " H-" + (height).ToString());
                        // Console.WriteLine("Cloud set");
                        for (int k = pointx; k < pointx + width; k++)
                        {
                            for (int l = pointy; l < pointy + height; l++)
                                moss[k, l] = 1;
                            
                        }

                        break;
                    }
                }
            }

            // count filled points, and insert into massives
            int counter;

            for (int i = 0; i < 10; i++)
            {
                counter = 0;

                for (int j = 0; j < 10; j++)
                    if (moss[i, j] == 1) counter++;
                

                mx[i] = counter;
            }

            for (int i = 0; i < 10; i++)
            {
                counter = 0;

                for (int j = 0; j < 10; j++)
                    if (moss[j, i] == 1) counter++;
                

                my[i] = counter;
            }

            if (assist)
            {
                Writetable(moss);
            }
        }

        static void OpenUrl(string url)
        {
            try
            {
                Process.Start(url);
            }
            catch
            {
                // : https://github.com/dotnet/corefx/issues/10361
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

        static async Task Listen()
        {
            var html = File.ReadAllText("index.html", Encoding.UTF8); //change this strings to "../../../string_itself" if u need to run this app in vs compiler (visual studio issues)
            var css = File.ReadAllText("styles.css", Encoding.UTF8);
            var js = File.ReadAllText("script.js", Encoding.UTF8);
            var ico = File.ReadAllBytes("favicon.ico");
            Exercise ex = new();

            while (server.IsListening)
            {
                var context = await server.GetContextAsync();
                var response = context.Response;
                Debug.WriteLine("Request: " + context.Request.RawUrl);
                response.StatusCode = 200;

                switch (context.Request.RawUrl)
                {
                    case "/":
                        {
                            context.Response.ContentType = "text/html";
                            var buffer = Encoding.UTF8.GetBytes(html);
                            response.ContentLength64 = buffer.Length;
                            using var output = response.OutputStream;
                            await output.WriteAsync(buffer);
                            await output.FlushAsync();
                            break;
                        }
                    case "/styles.css":
                        {
                            var buffer = Encoding.UTF8.GetBytes(css);
                            response.ContentLength64 = buffer.Length;
                            using var output = response.OutputStream;
                            await output.WriteAsync(buffer);
                            await output.FlushAsync();
                            break;
                        }
                    case "/script.js":
                        {
                            var buffer = Encoding.UTF8.GetBytes(js);
                            response.ContentLength64 = buffer.Length;
                            using var output = response.OutputStream;
                            await output.WriteAsync(buffer);
                            await output.FlushAsync();
                            break;
                        }
                    case "/favicon.ico":
                        {
                            response.ContentType = "image/vnd.microsoft.icon";
                            var dataStream = new MemoryStream(ico);
                            using var output = response.OutputStream;
                            await output.WriteAsync(dataStream.ToArray());
                            await output.FlushAsync();
                            break;
                        }
                    case "/api":
                        {
                            var buffer = Encoding.UTF8.GetBytes("API Server is online!");
                            response.ContentLength64 = buffer.Length;
                            using var output = response.OutputStream;
                            await output.WriteAsync(buffer);
                            await output.FlushAsync();
                            break;
                        }
                    case "/api/update":
                        {
                            response.ContentType = "text/json";
                            Randomize(ex.rowx, ex.rowy, assistant);
                            var ww = JsonSerializer.Serialize<Exercise>(ex);
                            var buffer = Encoding.UTF8.GetBytes(ww);
                            // Console.WriteLine(JsonSerializer.Serialize(ex));
                            response.ContentLength64 = buffer.Length;
                            using var output = response.OutputStream;
                            await output.WriteAsync(buffer);
                            await output.FlushAsync();
                            break;
                        }
                    default: //done
                        {
                            response.StatusCode = 404;
                            var buffer = Encoding.UTF8.GetBytes("Page does not exist -_-");
                            response.ContentLength64 = buffer.Length;
                            using var output = response.OutputStream;
                            await output.WriteAsync(buffer);
                            await output.FlushAsync();
                            break;
                        }
                }
            }
        }

        static void CheckFiles()
        {
            FileInfo index = new("index.html");

            if (!index.Exists)
            {
                Console.WriteLine("Не найден файл index.html");
                Console.ReadKey();
                Environment.Exit(-1);
            }

            FileInfo css = new("styles.css");

            if (!css.Exists)
            {
                Console.WriteLine("Не найден файл styles.css");
                Console.ReadKey();
                Environment.Exit(-1);
            }

            FileInfo js = new("script.js");

            if (!js.Exists)
            {
                Console.WriteLine("Не найден файл script.js");
                Console.ReadKey();
                Environment.Exit(-1);
            }

            Console.WriteLine("Got all files, running...");
        }

        static async Task Main()
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.WriteLine("Http Clouds server v1.0.1\nStarting...");

            var adress = @"http://127.0.0.1:1488/";
            server.Prefixes.Add(adress);
            CheckFiles();
            server.Start();
            Console.WriteLine(value: "Server working on " + adress);
            Listen();
            OpenUrl(adress);
            // System.Diagnostics.Process.Start(adress);

            Console.WriteLine("Listening has been started!");

            var work = true;
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
                    case "c": goto case "exit";
                    case "restart":
                        {
                            Console.WriteLine("Stopping...");
                            server.Stop();
                            Console.WriteLine("Restarting...");
                            server.Start();
                            Listen();

                            if (server.IsListening) Console.WriteLine("Listening started!");
                            else
                            {
                                Console.WriteLine("Listening err, going to shutdown...");
                                work = false;
                            }

                            break;
                        }
                    case "open":
                        {
                            OpenUrl(adress);
                            break;
                        }
                    case "help":
                        {
                            Console.WriteLine("Help:\n stop / s / exit / e / quit / q / close / c -- exit app\n clear -- clear the console\n open -- open web page\n restart -- listening restart\n assist - enable/disable assistant");
                            break;
                        }
                    case "assist":
                        {
                            if (!assistant)
                            {
                                assistant = true;
                                Console.WriteLine("Assistant enabled!");
                            }
                            else
                            {
                                assistant = false;
                                Console.WriteLine("Assistant disabled!");
                            }

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