using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ClientApp
{
    internal class Client
    {
        private const int PORT_NUMBER = 9999;
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            try
            {
                
                TcpClient client = new TcpClient();//ປະກາດຜູ້ໃຊ້

                // 1. connect ເຊື່ອມຕໍ່ໄປ ຊ່ອງສື່ສານ 9999
                client.Connect("127.0.0.1", PORT_NUMBER);
                Stream stream = client.GetStream();
                Console.WriteLine("*** Kết nối sever thành công.");//ເຊື່ອມຕໍ່ server ສຳເລັດ

                
                var reader = new StreamReader(stream);//ກຽມຮັບສົ່ງຂໍ້ມູນ 27-29
                var writer = new StreamWriter(stream);
                writer.AutoFlush = true;

                while (true)
                {    
                    Console.Write("Nhập số nguyên a (gõ \"EXIT\" để thoát): "); //ຮັບຂໍ້ຄວມຈາກຄີບອດ
                    string strNum1 = Console.ReadLine().Trim();
                    string massage = strNum1;
                    if (strNum1 != "EXIT")//ກວດສອບ ຂໍ້ຄວມຮັບຈາກຄີບອດ ແມ່ນ Exit ຫຼືບໍ່
                    {
                        Console.Write("Nhập số nguyên b: ");
                        string strNum2 = Console.ReadLine().Trim();

                        Console.Write("Nhập phép toán [+,-,*,/]: ");  //ຮັບບວກລົບຄູນຫານ ຈາກຄີບອດ
                        char math = Convert.ToChar(Console.ReadLine());
                        massage = strNum1 + "," + strNum2 + "," + math; //ລວມຂໍ້ມູນເປັນ String ອັນດຽວ 
                    }

                    //2.Sent ສົ່ງຂໍ້ຄວາມໄປ server
                    writer.WriteLine(massage);

                    // 3. receive ຮັບຂໍ້ຄວາມຈາກ sever
                    string str = reader.ReadLine();

                    if (str.ToUpper() == "BYE") //ຖ້າ sver ບອກ BYE ໃຫ້ຢຸດໂປແກຣມ
                        break;
                    Console.WriteLine(str); //ສະແດງຂໍ້ມູນເທິງຈໍ
                    Console.WriteLine("_____________________________________________\n");
                }
                // 4. close ປິດການເຊື່ອມຕໍ່
                stream.Close();
                client.Close();
            }

            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex);
            }

            Console.Read();
        }
    }
}
