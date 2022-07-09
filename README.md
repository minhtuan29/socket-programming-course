# socket-programming
Áp dụng cho tất cả các ngôn ngữ lập trình từ Python Js C C++ C# Java....  

Các phương thức quản lý một socket :  
- bước 1. Khởi tạo socket : new Socket : cung cấp gồm { loại IP, loại protocol : ví dụ IPv4, IPv6, TCP protocol, HTTP protocol, UDP Protocol}
```
new Connection(  new IP (127.x.y.z, IPv4)
		, 443
		, TCP_HTTPS
	      ) 

```
- bước 2.  
Đối với server :  
Liên kết Socket với {IP và Port} bằng phương thức Bind : mySocket.bind( IP, Port),  
Cho Socket listen() để kết nối, socket sẽ chờ phải hồi liên tục cách đều N milisecond và mở ra một luồng mới : Socket.listen(N)  
Cho Socket accecpt() kết nối các các socket trên hàng chờ. Nếu không có socket nào trên hàng chờ đồng thời nó đang đang ở trạng thái blockable, 
nó sẽ chặn lại các cuộc gọi từ client cho đến khi có yêu cầu kết nối từ client.
Đối với Client :  
dùng phương thức connect(IP, PORT) để kết nối tới server  

![Capture](https://user-images.githubusercontent.com/86332370/162412078-66ab5301-ffbd-445a-8996-946bf48d82d3.PNG)
 

- bước 3. Server nghe thấy lời gọi connect() của client, nó tiến hành accept()
- bước 4. cả 2 trao đổi thông tin qua hàm nhận và hàm gửi  
  
  
![12312312323](https://user-images.githubusercontent.com/86332370/178084626-29a0d6e3-5817-485e-b9a1-0db40b1b8fb6.PNG)
![345345345](https://user-images.githubusercontent.com/86332370/178084631-0fc596f5-90d1-4522-a75c-3ad7c31b28c8.PNG)
