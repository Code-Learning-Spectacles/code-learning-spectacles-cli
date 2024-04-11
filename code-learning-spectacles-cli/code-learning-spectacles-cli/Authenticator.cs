using code_learning_spectacles_cli.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace code_learning_spectacles_cli
{
    internal class Authenticator
    {
        public Authenticator() { 
        }

        public static bool authenticationSuccessful = false;
        public static string loginName = "";
        public async static void AuthenticateAsync()
        {
            //string url = "https://github.com/login/device/code?client_id=" + Environment.GetEnvironmentVariable("CLIENT_ID") + "&scope=read:user";
            string url = "https://github.com/login/device/code?client_id=" + "6ab621f34a0c32c827fe" + "&scope=read:user";

            HttpRequestMessage msg = new HttpRequestMessage(HttpMethod.Post, url);
            msg.Headers.Add("Accept", "application/json");
            HttpResponseMessage response = await Endpoints.client.SendAsync(msg);

            string response2 = await response.Content.ReadAsStringAsync();

            DeviceVerification deviceVerification = JsonSerializer.Deserialize<DeviceVerification>(response2);
            Console.WriteLine("Please log in:");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"Go to {deviceVerification.verification_uri} and use code {deviceVerification.user_code}");
            Console.ResetColor();
            if (deviceVerification != null)
            {
                bool success = false;
                Console.Write("Waiting");
                while (!success)
                {
                    success = await Helpers.GetAccessTokenAsync(Endpoints.client, deviceVerification);
                    Console.Write(".");
                }
                Endpoints.client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Environment.GetEnvironmentVariable("ACCESS_TOKEN"));
                Console.WriteLine("authenticated");
                Helpers.Profile = await GetProfileAsync();
                authenticationSuccessful = true;
            }
        }

        public async static void LoginHelper()
        {
            Helpers.Profile = await GetProfileAsync();
        }

        public async static Task<AuthObject?> GetProfileAsync()
        {
            try
            {
                string url = "https://api.github.com/user";
                HttpRequestMessage msg = new HttpRequestMessage(HttpMethod.Get, url);
                msg.Headers.Add("User-Agent", "CodeLearningSpectaclesAPI");
                msg.Headers.Add("Authorization", "Bearer " + Environment.GetEnvironmentVariable("ACCESS_TOKEN"));
                HttpResponseMessage response = await Endpoints.client.SendAsync(msg);
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    var auth = JsonSerializer.Deserialize<AuthObject>(content);
                    CheckProfile(auth.login);
                    loginName = auth.login;
                    return auth;
                }
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine(ex.Message);
            }
            return null;
        }

        private async static void CheckProfile(string name)
        {
            try
            {
                Endpoints.client.DefaultRequestHeaders.Accept.Clear();
                Endpoints.client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var profiles = await Endpoints.client.GetFromJsonAsync<List<Profile>>("Profiles");
                if (profiles != null && profiles.Count > 0)
                {
                    var profile = profiles.Find(x => x.Name.Equals(name));
                    if (profile == null)
                    {
                        Console.WriteLine("Creating new profile");
                        // Create profile for user
                        using StringContent jsonContent = new(JsonSerializer.Serialize(new { Name = name }), Encoding.UTF8, "application/json");
                        using HttpResponseMessage createProfileResponse = await Endpoints.client.PostAsync("Profiles", jsonContent);
                        Profile? newProfile = await createProfileResponse.Content.ReadFromJsonAsync<Profile>();
                        Helpers.ProfileID = "" + newProfile.Profileid;
                    }
                    else
                    {
                        //Store id for future use
                        Helpers.ProfileID = "" + profile.Profileid;
                    }
                    loginName = name;
                }
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"{ex.Message} Error Checking Profile");
            }
        }
    }
}
