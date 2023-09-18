// See https://aka.ms/new-console-template for more information
using RabbitMQ.Client.QueueConsole.Classes;

Console.WriteLine("Hello, World!");

RabbitMQClientManager rabbitMQClientManagerClass = RabbitMQClientManager.Obj();

Console.WriteLine("--------- Client Page ---------------\n");

Console.Write("Kaç Adet Client : ");
int ClientCount = Convert.ToInt32(Console.ReadLine());

rabbitMQClientManagerClass.ClientCountCreatedConnection(ClientCount);

Console.Write("\nGeri Dönüş Alınsın mı ( 0 -> Evet , 1 -> Hayır) : ");
int ReplyCheck = Convert.ToInt32(Console.ReadLine());


Console.Write("\nKaç Adet Messaj Gönderilecek : ");
int MessageCount = Convert.ToInt32(Console.ReadLine());

Task task1 = new(() =>
{
    rabbitMQClientManagerClass.SendMessage(MessageCount);
});
task1.Start();

if (ReplyCheck == 0)
{
    Task task2 = new(() =>
    {
        rabbitMQClientManagerClass.ReplyMessage();
    });
    task2.Start();
}




Console.ReadLine();


