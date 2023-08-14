// See https://aka.ms/new-console-template for more information
using RabbitMQ.Server.Queue.Manager;

Server_GetSet_Message server_GetSet_MessageClass = Server_GetSet_Message.Obj(); /* singletion prensibi kullanıldı  */

server_GetSet_MessageClass.RunQueue(); /* sistemi çalıştırıyorum... */

Console.ReadLine();




