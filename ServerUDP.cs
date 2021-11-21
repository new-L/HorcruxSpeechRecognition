using System;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Speech_Recognition
{
    class ServerUDP
    {
        List<string> tags = new List<string>();
        //Задаем локальные значения для подключения в пределах одного компьютера
        const string ip = "127.0.0.1";
        const int port = 8086;

        private Socket udpSocket;
        //EndPoint? Конечная точка, то, куда будет происходить подключение
        IPEndPoint udpEndPoint;
        public void ServerCreate()
        {
            udpEndPoint = new IPEndPoint(IPAddress.Parse(ip), port);
            //Создали сокет, находящийся в ожидании приема сообщений
            udpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            udpSocket.Bind(udpEndPoint);
        }

        public async void ReceiveMeesage(string tag)
        {
            tags.Clear();
            tags.Add(tag);
            Console.WriteLine("--------------------------------------------------");
            Console.Write("THE LAST ITEM IS: " + tags[tags.Count - 1]);
            Console.WriteLine("\n--------------------------------------------------");
            await Task.Run(() =>
            {
                //udpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                while (true)
                {
                    
                    //Создаем буффер, куда сможем принимать сообщения. DataStore
                    byte[] buffer = new byte[256];
                    //Количество реально полученных байт
                    var size = 0;
                    //Builder, который собирает полученные данные. Позволяет удобно форматировать строки
                    StringBuilder data = new StringBuilder();
                    //Экземпляр адреса эндпоинта. Сохраняется адрес клиента
                    EndPoint senderEndPoint = new IPEndPoint(IPAddress.Any, 0);
                    do
                    {
                        size = udpSocket.ReceiveFrom(buffer, ref senderEndPoint);
                        data.Append(Encoding.UTF8.GetString(buffer));
                    } while (udpSocket.Available > 0);

                    if (tags.Count > 0 && data.Length > 0)
                    {
                        udpSocket.SendTo(Encoding.UTF8.GetBytes(tags[tags.Count - 1]), senderEndPoint);
                        Console.WriteLine("Server Spell Tag: " + tags[tags.Count - 1]);
                    }


                    data.Clear();
                    tags.Clear();
                    //udpSocket.Shutdown(SocketShutdown.Both);
                    //udpSocket.Close();
                    break;
                }
            });
            
        }
    }
}
