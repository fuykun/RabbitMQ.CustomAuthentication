using System;
using System.Text;
using RabbitMQ.Client;

namespace RabbitMqTestApp
{
	class Program
	{
		private const string HostName = "localhost";
		private const string UserName = "admin";
		private const string Password = "admin";
		static void Main(string[] args)
		{
			try
			{
				Console.WriteLine($"Hostname: {HostName} | Username: {UserName} | Password: {Password}");
				Send();
				Console.WriteLine($"RabbitMQ mesaj gönderme işlemi başarılı.");
				Console.ReadLine();
			}
			catch (Exception e)
			{
				Console.WriteLine($"RabbitMQ mesaj gönderme işlemi başarısız sonuçlandı.");
				throw;
			}
		}
		private static void Send()
		{
			var factory = new ConnectionFactory() { HostName = HostName, UserName = UserName, Password = Password };
			using (var connection = factory.CreateConnection())
				using (var channel = connection.CreateModel())
				{
					channel.QueueDeclare(queue: "hello", durable: false, exclusive: false, autoDelete: false, arguments: null);
					var message = "Hello World!";
					var body = Encoding.UTF8.GetBytes(message);
					channel.BasicPublish(exchange: "", routingKey: "hello", basicProperties: null, body: body);
					Console.WriteLine(" [x] Sent {0}", message);
				}
		}
	}
}