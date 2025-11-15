using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using SkiFieldTracker.Application.Common;
using SkiFieldTracker.Application.SkiFields.Models;
using SkiFieldTracker.Application.SkiFields.UseCases;

namespace SkiFieldTracker.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SkiFieldsController : ControllerBase
{
    private static readonly JsonSerializerOptions QuerySerializerOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    private readonly ICreateSkiFieldUseCase _createUseCase;
    private readonly IUpdateSkiFieldUseCase _updateUseCase;
    private readonly IDeleteSkiFieldUseCase _deleteUseCase;
    private readonly IQuerySkiFieldsUseCase _queryUseCase;

    public SkiFieldsController(
        ICreateSkiFieldUseCase createUseCase,
        IUpdateSkiFieldUseCase updateUseCase,
        IDeleteSkiFieldUseCase deleteUseCase,
        IQuerySkiFieldsUseCase queryUseCase)
    {
        _createUseCase = createUseCase;
        _updateUseCase = updateUseCase;
        _deleteUseCase = deleteUseCase;
        _queryUseCase = queryUseCase;
    }

    [HttpPost("query")]
    [ProducesResponseType(typeof(PaginatedResult<SkiFieldResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> QueryAsync(
        [FromQuery(Name = "query")] string? queryParam,
        [FromBody] FindManyRequest? bodyRequest,
        CancellationToken cancellationToken)
    {
        var request = MergeQueryRequest(queryParam, bodyRequest);
        if (request is null)
        {
            return ValidationProblem("Unable to parse query payload. Ensure it is valid JSON matching FindManyRequest schema.");
        }

        var result = await _queryUseCase.ExecuteAsync(request, cancellationToken);
        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(typeof(SkiFieldResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> CreateAsync(
        [FromBody] CreateSkiFieldRequest request,
        CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        try
        {
            var result = await _createUseCase.ExecuteAsync(request, cancellationToken);
            return Created($"/api/skifields/{result.Id}", result);
        }
        catch (SkiFieldNameAlreadyExistsException ex)
        {
            return Conflict(CreateProblem(StatusCodes.Status409Conflict, ex.Message));
        }
    }

    [HttpPut("{uid}")]
    [ProducesResponseType(typeof(SkiFieldResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> UpdateAsync(
        string uid,
        [FromBody] UpdateSkiFieldRequest request,
        CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        try
        {
            var result = await _updateUseCase.ExecuteAsync(uid, request, cancellationToken);
            return Ok(result);
        }
        catch (SkiFieldNotFoundException ex)
        {
            return NotFound(CreateProblem(StatusCodes.Status404NotFound, ex.Message));
        }
        catch (SkiFieldNameAlreadyExistsException ex)
        {
            return Conflict(CreateProblem(StatusCodes.Status409Conflict, ex.Message));
        }
    }

    [HttpDelete("{uid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteAsync(string uid, CancellationToken cancellationToken)
    {
        try
        {
            await _deleteUseCase.ExecuteAsync(uid, cancellationToken);
            return NoContent();
        }
        catch (SkiFieldNotFoundException ex)
        {
            return NotFound(CreateProblem(StatusCodes.Status404NotFound, ex.Message));
        }
    }

    private static FindManyRequest? MergeQueryRequest(string? queryParam, FindManyRequest? bodyRequest)
    {
        var request = bodyRequest ?? new FindManyRequest();

        if (string.IsNullOrWhiteSpace(queryParam))
        {
            return request;
        }

        try
        {
            var decoded = Uri.UnescapeDataString(queryParam);
            var queryRequest = JsonSerializer.Deserialize<FindManyRequest>(decoded, QuerySerializerOptions);
            return queryRequest ?? request;
        }
        catch (Exception)
        {
            return null;
        }
    }

    private ProblemDetails CreateProblem(int statusCode, string detail) =>
        new()
        {
            Title = ReasonPhrases.GetReasonPhrase(statusCode),
            Status = statusCode,
            Detail = detail
        };
}

