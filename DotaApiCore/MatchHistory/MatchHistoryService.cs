﻿using System.Net.Http;
using System.Threading.Tasks;
using DotaApiCore.MatchHistory.Models;
using DotaApiCore.Requests;
using DotaApiCore.SharedLib;
using Newtonsoft.Json;

namespace DotaApiCore.MatchHistory
{
    internal class MatchHistoryService : IMatchHistoryService
    {
        private readonly string _apiKey;
        private readonly IHttpHandler _client;

        public MatchHistoryService(IHttpHandler handler, string apiKey)
        {
            _client = handler;
            _apiKey = apiKey;
        }

        public MatchHistoryRequestResult GetMatchHistory(long? accountId = null,
            int? heroId = null, int? gameMode = null, int? skill = null,
            int? minPlayers = null, long? startingMatchId = null,
            int? matchesRequested = 100)
        {
            var matchHistoryRequest = new MatchHistoryRequest(_apiKey, accountId, heroId, gameMode,
                skill, minPlayers, startingMatchId, matchesRequested);

            var responseBody = SendAndValidateRequest(matchHistoryRequest);

            var results = JsonConvert.DeserializeObject<MatchHistoryRequestResult>(responseBody.Result);

            return results;
        }

        private async Task<string> SendAndValidateRequest(MatchHistoryRequest matchHistoryRequest)
        {
            var result = SendRequest(matchHistoryRequest.RequestUrl);
            result.EnsureSuccessStatusCode();

            var responseBody = await result.Content.ReadAsStringAsync();

            return responseBody;
        }

        public HttpResponseMessage SendRequest(string requestUrl)
        {
            var result = _client.GetAsync(requestUrl).Result;

            return result;
        }
    }
}
