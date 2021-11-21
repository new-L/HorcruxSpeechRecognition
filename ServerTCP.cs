using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Speech.Recognition;

namespace Speech_Recognition
{
    class ServerTCP
    {
        //Задаем локальные значения для подключения в пределах одного компьютера
        const string ip = "127.0.0.1";
        const int port = 8085;

        private Socket tcpSocket;
        //Обработка конкретного клиента
        Socket listener;

        public void ServerCreate()
        {
            //EndPoint? Конечная точка, то, куда будет происходить подключение
            IPEndPoint tcpEndPoint = new IPEndPoint(IPAddress.Parse(ip), port);

            //Создали сокет, находящийся в ожидании приема сообщений
            tcpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            tcpSocket.Bind(tcpEndPoint);
            tcpSocket.Listen(5);
        }

        public async void ReceiveMessage(string tag)
        {
            await Task.Run(() => { 
                while (true)
                {
                    listener = tcpSocket.Accept();
                
                    //Создаем буффер, куда сможем принимать сообщения. DataStore
                    byte[] buffer = new byte[256];
                    //Количество реально полученных байт
                    var size = 0;
                    //Builder, который собирает полученные данные. Позволяет удобно форматировать строки
                    StringBuilder data = new StringBuilder();

                    do
                    {
                        size = listener.Receive(buffer);//получаем 256 байт из сообщения, но т.к. их может быть другое количество, поэтому в size записываем реальное количество полученных байт
                        data.Append(Encoding.UTF8.GetString(buffer, 0, size));//Добавляем данные. Из большого объема данных, берем по 256 байт
                    }
                    while (listener.Available > 0);//действительно ли мы получили сообщение
                    Console.WriteLine(data.ToString() + "Data Length: " + data.Length);

                    if (data.Length > 0)
                    {
                        Console.WriteLine("Sended: " + tag);
                        SendMessage(tag);
                    }
                    data.Clear();
                    size = 0;
                }
            });
        }


        public void SendMessage(string message)
        {
           // listener = tcpSocket.Accept();
            if (listener != null)
            {
                //Даем обратный ответ
                listener.Send(Encoding.UTF8.GetBytes(message));
                ListenerClose();
            }
        }

        private void ListenerClose()
        {
            listener.Shutdown(SocketShutdown.Both);//Двустороннее закрытие
            listener.Close();
        }

    }
}
