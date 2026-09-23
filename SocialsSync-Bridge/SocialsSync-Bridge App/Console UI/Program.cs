
using Microsoft.Extensions.Configuration;

using System.Text.Json;

namespace SocialsSync_Bridge_App
{
    internal class Program
    {
        static async Task Main(string[] args)
        {

            var configuration = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
                .Build();


            //var facebookAppId = configuration["Facebook:AppId"];

            //Console.WriteLine($"Facebook App ID: {facebookAppId}");


            //const string redirectUri = "http://localhost:5000/facebook-callback/";

            //using var listener = new HttpListener();
            //listener.Prefixes.Add(redirectUri);
            //listener.Start();


            //Console.WriteLine("Facebook callback listener started.");
            //Console.WriteLine($"Listening at: {redirectUri}");


            //var loginUrl =
            //          $"https://www.facebook.com/dialog/oauth" +
            //          $"?client_id={facebookAppId}" +
            //          $"&redirect_uri={Uri.EscapeDataString(redirectUri)}" +
            //          $"&response_type=code" +
            //          $"&scope=public_profile";

            //Console.WriteLine("Opening Facebook Login...");

            //System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
            //{
            //    FileName = loginUrl,
            //    UseShellExecute = true
            //});
            //Console.WriteLine("Waiting for Facebook...");





            //var context = listener.GetContext();

            //Console.WriteLine("Facebook callback received!");

            //var code = context.Request.QueryString["code"];

            //Console.WriteLine($"Authorization code received: {code}");

            //using var httpClient = new HttpClient();

            //var tokenUrl =
            //    "https://graph.facebook.com/oauth/access_token" +
            //    $"?client_id={facebookAppId}" +
            //    $"&client_secret={Uri.EscapeDataString(configuration["Facebook:AppSecret"]!)}" +
            //    $"&redirect_uri={Uri.EscapeDataString(redirectUri)}" +
            //    $"&code={Uri.EscapeDataString(code!)}";

            //var response = await httpClient.GetAsync(tokenUrl);

            //var responseBody = await response.Content.ReadAsStringAsync();

            //Console.WriteLine($"Status: {(int)response.StatusCode}");
            //Console.WriteLine(responseBody);

            //var tokenData = JsonSerializer.Deserialize<JsonElement>(responseBody);

            //var accessToken = tokenData.GetProperty("access_token").GetString();

            //Console.WriteLine("Access token extracted!");

            //var meUrl =
            //         "https://graph.facebook.com/me" +
            //         "?fields=id,name" +
            //         $"&access_token={Uri.EscapeDataString(accessToken!)}";

            //var meResponse = await httpClient.GetAsync(meUrl);
            //var meBody = await meResponse.Content.ReadAsStringAsync();

            //Console.WriteLine($"Graph API Status: {(int)meResponse.StatusCode}");
            //Console.WriteLine(meBody);



            var instagramUserId = configuration["Instagram:UserId"];
            var instagramAccessToken = configuration["Instagram:AccessToken"];

            using var httpClient = new HttpClient();

            var url =
                $"https://graph.instagram.com/{instagramUserId}" +
                $"?fields=id,username" +
                $"&access_token={Uri.EscapeDataString(instagramAccessToken!)}";

            var response = await httpClient.GetAsync(url);
            var body = await response.Content.ReadAsStringAsync();

            Console.WriteLine($"Instagram API Status: {(int)response.StatusCode}");
            Console.WriteLine(body);



            //var imageUrl = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcTx8Yo-z077PYmQttFI7Q44JYUnMS10rxI0wEJmYKTEkQ&s=10";

            Console.Write("Image URL: ");
            var imageUrl = Console.ReadLine();

            Console.Write("Caption: ");
            var caption = Console.ReadLine();


            var containerUrl =
                $"https://graph.instagram.com/{instagramUserId}/media" +
                $"?image_url={Uri.EscapeDataString(imageUrl)}" +
                $"&caption={Uri.EscapeDataString(caption ?? "")}" +
                //$"&caption={Uri.EscapeDataString("SocialSync-Bridge Instagram POC 🚀")}" +
                $"&access_token={Uri.EscapeDataString(instagramAccessToken!)}";

            var containerResponse = await httpClient.PostAsync(containerUrl, null);
            var containerBody = await containerResponse.Content.ReadAsStringAsync();

            Console.WriteLine($"Container Status: {(int)containerResponse.StatusCode}");
            Console.WriteLine(containerBody);

            var containerData = JsonSerializer.Deserialize<JsonElement>(containerBody);
            var containerId = containerData.GetProperty("id").GetString();


            await Task.Delay(5000);

            var statusUrl =
                $"https://graph.instagram.com/{containerId}" +
                $"?fields=status_code,status" +
                $"&access_token={Uri.EscapeDataString(instagramAccessToken!)}";

            var statusResponse = await httpClient.GetAsync(statusUrl);
            var statusBody = await statusResponse.Content.ReadAsStringAsync();

            Console.WriteLine($"Container Processing Status: {(int)statusResponse.StatusCode}");
            Console.WriteLine(statusBody);



            var publishUrl =
                $"https://graph.instagram.com/{instagramUserId}/media_publish" +
                $"?creation_id={Uri.EscapeDataString(containerId!)}" +
                $"&access_token={Uri.EscapeDataString(instagramAccessToken!)}";

            var publishResponse = await httpClient.PostAsync(publishUrl, null);
            var publishBody = await publishResponse.Content.ReadAsStringAsync();

            Console.WriteLine($"Publish Status: {(int)publishResponse.StatusCode}");
            Console.WriteLine(publishBody);

            //Console.ReadLine();



            //string responseText = "Facebook login callback received. You can close this window.";

            //byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responseText);

            //context.Response.ContentLength64 = buffer.Length;
            //context.Response.ContentType = "text/html";
            //context.Response.OutputStream.Write(buffer, 0, buffer.Length);
            //context.Response.OutputStream.Close();

            //var requiredSettings = new[]
            //{
            //  "X:ClientId",
            //  "X:ClientSecret",
            //  "X:RedirectUri"
            //};

            //foreach (var setting in requiredSettings)
            //{
            //    var value = configuration[setting];

            //    Console.WriteLine(
            //        $"{setting}: {(string.IsNullOrWhiteSpace(value) ? "MISSING" : "Loaded")}");
            //}


            //Console.WriteLine("Hello, World!");
        }
    }
}
