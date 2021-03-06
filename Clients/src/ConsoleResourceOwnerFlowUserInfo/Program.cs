﻿using Clients;
using IdentityModel.Client;
using System;
using System.Threading.Tasks;

namespace ConsoleResourceOwnerFlowUserInfo
{
    class Program
    {
        public static void Main(string[] args) => MainAsync().GetAwaiter().GetResult();

        static async Task MainAsync()
        {
            Console.Title = "Console ResourceOwner Flow UserInfo";

            var response = await RequestTokenAsync();
            response.Show();

            await GetClaimsAsync(response.AccessToken);
        }

        static async Task<TokenResponse> RequestTokenAsync()
        {
            var client = new TokenClient(
                Constants.TokenEndpoint,
                "roclient",
                "secret");

            return await client.RequestResourceOwnerPasswordAsync("bob", "bob", "openid custom.profile");
        }

        static async Task GetClaimsAsync(string token)
        {
            var client = new UserInfoClient(Constants.UserInfoEndpoint);

            var response = await client.GetAsync(token);

            "\n\nUser claims:".ConsoleGreen();
            foreach (var claim in response.Claims)
            {
                Console.WriteLine("{0}\n {1}", claim.Type, claim.Value);
            }
        }
    }
}