# RabbitMQ_Test_Project
RabbitMQ ile Server ve Client arasındaki iletişimi kuyruklama sisteme aktarım.

# Server
Console ekranında sistem okuam işlemi yapıyor.
  
  -> Clientten servere sadece 1 Queue kuryuğuna mesjalar kayı edilir .
  -> Client kendi bilgilerini gönderiri ve onun için oluşmasını istediği kuyruğunu oluşturmak ister eğer geri bir cevap işstemiyorsa kuyruk oluşmaz.
  -> Sistem dinamik bir yapıda olup istenilen kadar kuyruk oluşturulabilir , mesaj eklenebilir  ve silinebilir.
 

# Client 
Resimde görüldüğü 3 alandan oluşturulmaktdır.

1.Alan = >
  -> Sistem üzerinde client oluşturmak istediğimiz yazıp mesaj gönder tuşuna basıyoruz.
  -> İlgili mesaj sadece tek bir kuyruğa iletilmiş oluyor.
2.Alan = >
  -> Sistemde kendine ait bir 'QueueKey' varsa text kısmına yazıp o kuyruğa gelen mesajları dinliyoruz 
  -> örneğin 10 client için mesaj gönderdim ve sadece 4. client dinlemek istediğimde her QueueMessage kayıt edilen mesajı dinleme başlıyor.
3.Alan => 
  -> Sistemde serverin istediğim kadar clientQueue oluşturduktan sonra yapılandırmış clientQueueName tamammını bulup epsindeki mesajları almaya yarar
  -> Serverde her client özgü Queue oluştuğu için her client kendi mesajını okuma kapasitesine sahip oluyor.
Tüm Kuyrukları Silme
  -> Özel bir formatta oluşmasını istediğim formatta Queue'ler oluştuğu için serverin oluşturduğu tüm Queleri silebiliyorum..
Resim Client :
![resim1](https://github.com/isa-koseoglu/RabbitMQ_Test_Project/assets/93054123/f90c378d-90a6-4c41-a59f-207f1144e87c)
