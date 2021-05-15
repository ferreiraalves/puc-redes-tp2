using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace Servidor
{

	class Program
	{
		public static string comando_sair = "Encerra Servidor<EOF>";

		public static void StartServer()
		{
			// Get Host IP Address that is used to establish a connection
			// In this case, we get one IP address of localhost that is IP : 127.0.0.1
			// If a host has multiple addresses, you will get a list of addresses
			IPHostEntry host = Dns.GetHostEntry("localhost");
			IPAddress ipAddress = host.AddressList[0];
			IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 11000);

			try
			{

				// Create a Socket that will use Tcp protocol
				Socket listener = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
				// A Socket must be associated with an endpoint using the Bind method
				listener.Bind(localEndPoint);
				// Specify how many requests a Socket can listen before it gives Server busy response.
				// We will listen 10 requests at a time
				listener.Listen(10);

				Console.WriteLine("Aguardando Conexão ...");
				Socket handler = listener.Accept();

				// Variaveis para guardar os dados vindos do cliente
				string data = null;
				byte[] bytes = null;
				bool sai = false;

				while (!sai)
				{
					// vai montando a mensagem de texto até encontrar texto EOF (fim de arquivo)
					while (true)
					{
						bytes = new byte[1024];
						int bytesRec = handler.Receive(bytes);
						data += Encoding.ASCII.GetString(bytes, 0, bytesRec);
						if (data.IndexOf("<EOF>") > -1)
						{
							break;
						}
					}
					Console.WriteLine("Texto recebido : {0}", data);
					byte[] msg = Encoding.ASCII.GetBytes(data);
					handler.Send(msg); // ecoa a mensagem recebida para o cliente

					// verifica se o cliente pediu para encerrar o Servidor
					if (String.Equals(data, comando_sair))
					{
						Console.WriteLine("Encerrando o Servidor...\n\n");
						sai = true;
					}
					data = null; // apaga texto recebido
					bytes = null;
				} // while

				handler.Shutdown(SocketShutdown.Both);
				handler.Close();
			}
			catch (Exception e)
			{
				Console.WriteLine(e.ToString());
			}

			Console.WriteLine("\n Press any key to continue...");
			Console.ReadKey();
		}

		static void Main(string[] args)
		{
			Console.WriteLine("Testando o Servidor V2.\n\n");
			StartServer();
		}
	}
}