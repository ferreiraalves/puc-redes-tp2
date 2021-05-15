using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace Cliente
{
    class Program
    {

        public static void StartClient()
        {
            byte[] bytes = new byte[1024];

            try
            {
                // Connect to a Remote server  
                // Get Host IP Address that is used to establish a connection  
                // In this case, we get one IP address of localhost that is IP : 127.0.0.1  
                // If a host has multiple addresses, you will get a list of addresses  
                IPHostEntry host = Dns.GetHostEntry("localhost");
                IPAddress ipAddress = host.AddressList[0];
                IPEndPoint remoteEP = new IPEndPoint(ipAddress, 11000);


                // Connect the socket to the remote endpoint. Catch any errors.    
                try
                {

                    // Create a TCP/IP  socket.    
                    Socket sender = new Socket(ipAddress.AddressFamily,
                        SocketType.Stream, ProtocolType.Tcp);

                    // Connect to Remote EndPoint  
                    sender.Connect(remoteEP);


                    Console.WriteLine("Socket conectado a  {0}", sender.RemoteEndPoint.ToString());


                    while (true)
                    {
                        Console.WriteLine("Menu de opcões:");
                        Console.WriteLine("1 - Função 1");
                        Console.WriteLine("2 - Função 2");
                        Console.WriteLine("3 - Função 3");
                        Console.WriteLine("4 - Encerrar Servidor\n\n");

                        Console.WriteLine("Escolha a opção:");

                        string option = Console.ReadLine();
                        string messageData = null;
                        switch (option)
                        {
                            case "1":
                                messageData = "Comando 1.";
                                break;
                            case "2":
                                messageData = "Comando 2.";
                                break;
                            case "3":
                                messageData = "Comando 3.";
                                break;
                            case "4":
                                messageData = "Encerra Servidor";
                                break;
                            default:
                                Console.WriteLine("Default message");
                                break;
                        }



                        // Encode the data string into a byte array.    
                        byte[] msg = Encoding.ASCII.GetBytes(messageData + "<EOF>");

                        // Send the data through the socket.    
                        int bytesSent = sender.Send(msg);

                        // Receive the response from the remote device.    
                        int bytesRec = sender.Receive(bytes);
                        Console.WriteLine("Resposta do Servidor = {0}",
                            Encoding.ASCII.GetString(bytes, 0, bytesRec));

                        if (option != "4")
                        {
                            Console.WriteLine("Deseja Continuar? (S/N)");
                            string exit = Console.ReadLine();
                            if (exit == "N")
                            {
                                break;
                            }
                            else
                            {
                                Console.Clear();
                            }
                        }
                        else
                        {
                            Console.WriteLine("Servidor Desligado");
                            Console.WriteLine("Desligando cliente...");
                            break;
                        }

                    }

                    // Release the socket.
                    Console.WriteLine("client shutdown connection");
                    sender.Shutdown(SocketShutdown.Both);
                    sender.Close();

                }
                catch (ArgumentNullException ane)
                {
                    Console.WriteLine("ArgumentNullException : {0}", ane.ToString());
                }
                catch (SocketException se)
                {
                    Console.WriteLine("SocketException : {0}", se.ToString());
                }
                catch (Exception e)
                {
                    Console.WriteLine("Unexpected exception : {0}", e.ToString());
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
    }  

     static void Main(string[] args)
     {
            Console.WriteLine("Redes de Computadores Barreiro.\n\n");
            Console.WriteLine("Testando o Clinte.\n\n");
            StartClient();
            Console.ReadLine();        }
    }

}
