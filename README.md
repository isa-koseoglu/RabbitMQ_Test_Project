# RabbitMQ_Test_Project<br>
RabbitMQ ile Server ve Client arasındaki iletişimi kuyruklama sisteme aktarım.<br>

# Server<br>
Console ekranında sistem okuam işlemi yapıyor.<br>
  <br>
  -> Clientten servere sadece 1 Queue kuryuğuna mesjalar kayı edilir .<br>
  -> Client kendi bilgilerini gönderiri ve onun için oluşmasını istediği kuyruğunu oluşturmak ister eğer geri bir cevap işstemiyorsa kuyruk oluşmaz.<br>
  -> Sistem dinamik bir yapıda olup istenilen kadar kuyruk oluşturulabilir , mesaj eklenebilir  ve silinebilir.<br>
 <br>

# Client 
Resimde görüldüğü 3 alandan oluşturulmaktdır.

1.Alan = ><br>
  -> Sistem üzerinde client oluşturmak istediğimiz yazıp mesaj gönder tuşuna basıyoruz.<br>
  -> İlgili mesaj sadece tek bir kuyruğa iletilmiş oluyor.<br>
2.Alan = ><br>
  -> Sistemde kendine ait bir 'QueueKey' varsa text kısmına yazıp o kuyruğa gelen mesajları dinliyoruz <br>
  -> örneğin 10 client için mesaj gönderdim ve sadece 4. client dinlemek istediğimde her QueueMessage kayıt edilen mesajı dinleme başlıyor.<br>
3.Alan => <br>
  -> Sistemde serverin istediğim kadar clientQueue oluşturduktan sonra yapılandırmış clientQueueName tamammını bulup epsindeki mesajları almaya yarar<br>
  -> Serverde her client özgü Queue oluştuğu için her client kendi mesajını okuma kapasitesine sahip oluyor.<br>
Tüm Kuyrukları Silme<br>
  -> Özel bir formatta oluşmasını istediğim formatta Queue'ler oluştuğu için serverin oluşturduğu tüm Queleri silebiliyorum..<br>
# Client Image :<br>
![resim1](https://github.com/isa-koseoglu/RabbitMQ_Test_Project/assets/93054123/f90c378d-90a6-4c41-a59f-207f1144e87c)
