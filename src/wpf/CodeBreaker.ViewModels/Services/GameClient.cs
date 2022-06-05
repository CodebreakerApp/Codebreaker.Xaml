﻿using CodeBreaker.Shared.APIModels;

using System.Net.Http.Json;

namespace CodeBreaker.ViewModels.Services;

public class GameClient
{
    private readonly HttpClient _httpClient;

    public GameClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<CreateGameResponse> StartGameAsync(string name)
    {
        CreateGameRequest request = new(name);
        var responseMessage = await _httpClient.PostAsJsonAsync("v1/start", request);
        responseMessage.EnsureSuccessStatusCode();
        var response = await responseMessage.Content.ReadFromJsonAsync<CreateGameResponse>();
        return response;
    }

    public async Task<string[]> SetMove(string gameId, int moveNumber, params string[] colorNames)
    {
        MoveRequest moveRequest = new(gameId, moveNumber, colorNames);

        var responseMessage = await _httpClient.PostAsJsonAsync("move", moveRequest);
        responseMessage.EnsureSuccessStatusCode();
        var response = await responseMessage.Content.ReadFromJsonAsync<MoveResponse>();
        return response.KeyPegs?.ToArray() ?? new string[0];
    }
}
