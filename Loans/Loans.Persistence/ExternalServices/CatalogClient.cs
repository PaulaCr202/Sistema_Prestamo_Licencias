using System;
using System.Collections.Generic;
using System.Text;
using Loans.Application.Contracts.External;
using Loans.Application.Exceptions;
using Loans.Domain.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;
using System.Net;
using System.Text.Json;

namespace Loans.Persistence.ExternalServices;

public class CatalogClient : ICatalogClient
{
    private readonly HttpClient _httpClient;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CatalogClient(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
    {
        _httpClient = httpClient;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<Guid> AssignLicenseAsync(Guid softwareId, CancellationToken cancellationToken = default)
    {
        var request = new HttpRequestMessage(
            HttpMethod.Post, $"api/software/{softwareId}/licenses/assign");

        CopyAuthorizationHeader(request);

        HttpResponseMessage response;
        try
        {
            response = await _httpClient.SendAsync(request, cancellationToken);
        }
        catch (Exception ex) when (ex is HttpRequestException or TaskCanceledException)
        {
            throw new ExternalServiceException("Catalog no está disponible.");
        }

        if (response.StatusCode == HttpStatusCode.OK)
        {
            var body = await response.Content.ReadAsStringAsync(cancellationToken);
            using var doc = JsonDocument.Parse(body);
            return doc.RootElement.GetProperty("licenseId").GetGuid();
        }

        if (response.StatusCode == HttpStatusCode.BadRequest)
        {
            var error = await ReadErrorAsync(response, cancellationToken);
            throw new BussinesRuleException(error);
        }

        throw new ExternalServiceException("Catalog rechazó la solicitud de asignar la licencia.");
    }

    public async Task ReleaseLicenseAsync(Guid licenseId, CancellationToken cancellationToken = default)
    {
        var request = new HttpRequestMessage(
            HttpMethod.Post, $"api/license/{licenseId}/release");

        CopyAuthorizationHeader(request);

        HttpResponseMessage response;
        try
        {
            response = await _httpClient.SendAsync(request, cancellationToken);
        }
        catch (Exception ex) when (ex is HttpRequestException or TaskCanceledException)
        {
            throw new ExternalServiceException("Catalog no está disponible.");
        }

        if (response.StatusCode == HttpStatusCode.OK)
            return;

        if (response.StatusCode == HttpStatusCode.BadRequest)
        {
            var error = await ReadErrorAsync(response, cancellationToken);
            throw new BussinesRuleException(error);
        }

        throw new ExternalServiceException("Catalog rechazó la solicitud de liberar la licencia.");
    }

    private void CopyAuthorizationHeader(HttpRequestMessage request)
    {
        var auth = _httpContextAccessor.HttpContext?.Request.Headers["Authorization"].ToString();
        if (!string.IsNullOrEmpty(auth))
            request.Headers.TryAddWithoutValidation(HeaderNames.Authorization, auth);
    }

    private static async Task<string> ReadErrorAsync(HttpResponseMessage response, CancellationToken cancellationToken)
    {
        var body = await response.Content.ReadAsStringAsync(cancellationToken);
        using var doc = JsonDocument.Parse(body);
        return doc.RootElement.TryGetProperty("error", out var error)
            ? error.GetString() ?? "Catalog rechazó la solicitud."
            : "Catalog rechazó la solicitud.";
    }
}
