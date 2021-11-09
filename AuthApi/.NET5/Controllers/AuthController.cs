using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthApi.Models.Requests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AuthApi.Controllers
{
	[Route("[controller]")]
	[ApiController]
	public class AuthController : ControllerBase
	{
		private readonly ILogger<AuthController> _logger;

		public AuthController(ILogger<AuthController> logger)
		{
			_logger = logger;
		}

		[HttpGet]
		public ActionResult<string> Get()
		{
			_logger.LogInformation("get");
			return "UP!";
		}

		[Route("user")]
		[HttpPost]
		public async Task<IActionResult> CheckUser([FromForm] UserAuthRequest request)
		{
			var tags = new[] { "administrator", "management", "monitoring" };

			if (request.Username == "admin" && request.Password == "admin")
			{
				return AuthResult.Allow(tags);
			}
			else
			{
				return AuthResult.Deny();
			}
		}

		[Route("vhost")]
		[HttpPost]
		public IActionResult CheckVhost([FromForm] VhostAuthRequest request)
		{
			var userlog = $"user : {request.Username}, ip : {request.Ip}";
			_logger.LogInformation(userlog);
			return AuthResult.Allow();
		}

		[Route("resource")]
		[HttpPost]
		public IActionResult CheckResource([FromForm] ResourceAuthRequest request)
		{
			var userlog = $"user : {request.UserName}, vhost : {request.Vhost}, resource : {request.Resource}, name : {request.Name}, permission : {request.Permission}";
			_logger.LogInformation(userlog);
			return AuthResult.Allow();
		}

		[Route("topic")]
		[HttpPost]
		public IActionResult CheckTopic([FromForm] TopicAuthRequest request)
		{
			var userlog =
				$"user : {request.UserName}, vhost : {request.Vhost}, resource : {request.Resource}, name : {request.Name}, routing key: {request.RoutingKey}, permission : {request.Permission}";
			_logger.LogInformation(userlog);
			return AuthResult.Allow();
		}

		private class AuthResult
		{
			public static IActionResult Allow()
			{
				return new OkObjectResult("allow");
			}

			public static IActionResult Allow(params string[] tags)
			{
				return new OkObjectResult($"allow {string.Join(" ", tags)}");
			}

			public static IActionResult Deny()
			{
				return new OkObjectResult("deny");
			}
		}
	}
}