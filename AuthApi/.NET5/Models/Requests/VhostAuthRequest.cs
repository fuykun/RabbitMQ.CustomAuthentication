namespace AuthApi.Models.Requests
{
	public class VhostAuthRequest
	{
		public string Username { get; set; }

		public string Vhost { get; set; }

		public string Ip { get; set; }
	}
}