using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using System.Net;
using NetCore.Application.Country.Create;
using NetCore.Application.Country.DTOs;
using NetCore.Application.Country.QueryCountries;

namespace NetCore.Api.Controllers;

[Route("~/api/v1/countries")]
public class CountryController : AuthorizedBaseController
{
	private readonly IMediator _mediator;

	public CountryController(IMediator mediator)
	{
		_mediator = mediator;
	}

	[HttpPost]
	[ProducesResponseType((int)HttpStatusCode.Created)]
	public async Task<ActionResult> Create([FromBody] CreateCountriesCommand request)
	{
		var ids = await _mediator.Send(request);
		return Ok(ids);
	}

	/// <summary>
	/// Return OData query from client
	/// </summary>
	/// <returns></returns>
	[HttpGet]
	[EnableQuery(AllowedFunctions = AllowedFunctions.AllFunctions)]

	public async Task<ActionResult<IQueryable<QueryCountryDto>>> GetCountries()
	{
		var response = await _mediator.Send(new QueryCountries());
		return Ok(response);
	}
}