using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace Cau1_Client_SentNumberToSever
{
    internal class Program
    {
        private const int PORT_NUMBER = 9999;
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8; 
            try
            {
                IPAddress address = IPAddress.Parse("127.0.0.1"); //ລະບຸຊື່ເຄຶ່ອງຜູ້ໃຊ້ປາຍທາງ
                TcpListener listener = new TcpListener(address, PORT_NUMBER); //ສື່ສານ ຊ່ອງທີ 9999

                // 1. listen ຖ້າຜູ້ໃຊ້ເຂົ້າເຊື່ອມຕໍ່
                listener.Start();
                Console.WriteLine("*** Server started on " + listener.LocalEndpoint);
                Console.WriteLine("*** Đang đợi client kết nối.");

                 //ຜູ້ໃຊ້ເຊື່ອມຕໍ່ແລ້ວ
                Socket socket = listener.AcceptSocket();
                Console.WriteLine("*** Clien kết nối:" + socket.RemoteEndPoint);

                 //ກຽມຮັບສົ່ງຂໍ້ມູນ
                var stream = new NetworkStream(socket);
                var reader = new StreamReader(stream);
                var writer = new StreamWriter(stream);
                writer.AutoFlush = true;

                while (true)
                {
                    string[] arrayStr;
                    // 2. receive ຮັບຂໍ້ມູນ
                    string str = reader.ReadLine();
                    if (str.ToUpper() == "EXIT") //ຖ້າຜູ້ໃຊ້ສົ່ງ EXIT ມາ, ໃຫ້ຢຸດໂປແກຣມທັນທີ
                    {
                        // 3. send ສົ່ງຂໍ້ມູນ
                        writer.WriteLine("bye");
                        break;
                    }
                    else //ຖ້າບໍ່ແມ່ນ EXIT
                    {
                        arrayStr = str.Split(','); // ແຍກຂໍ້ຄວາມດ້ວຍ , ແລ້ວເກັບໄວ້ໃນ arrayStr

                        //ແປງຂໍ້ຄວາມເປັນໂຕເລກແລ້ວໄລ່
                        double a = double.Parse(arrayStr[0]);
                        double b = double.Parse(arrayStr[1]);
                        double result = 0;
                        

                        switch (arrayStr[2]) {
                            case "+": result = a + b; break;
                            case "-": result = a - b; break;
                            case "*": result = a * b; break;
                            case "/": result = a / b; break;
                            default: result = 0; break;
                        }
                        // 3. send ສົ່ງຂໍ້ມູນ
                        writer.WriteLine("Server: " + a+" " + arrayStr[2]+" "+b+" = "+result);

                    }
                    
                }
                // 4. close ປິດການເຊື່ອມຕໍ່
                stream.Close();
                socket.Close();
                listener.Stop();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex);
            }
            Console.Read();
        }
    }
}
