# socket-programming
Áp dụng cho tất cả các ngôn ngữ lập trình từ Python Js C C++ C# Java....  

Các phương thức quản lý một socket :  
- bước 1. Khởi tạo socket : new Socket : cung cấp gồm { loại IP, loại protocol : ví dụ IPv4, IPv6, TCP protocol, HTTP protocol, UDP Protocol}
- bước 2.  
Đối với server :  
Liên kết Socket với {IP và Port} bằng phương thức Bind : mySocket.bind( IP, Port),  
Cho Socket listen() để kết nối, socket sẽ chờ phải hồi liên tục cách đều N milisecond và mở ra một luồng mới : Socket.listen(N)  
Cho Socket accecpt() kết nối các các socket trên hàng chờ. Nếu không có socket nào trên hàng chờ đồng thời nó đang đang ở trạng thái blockable, 
nó sẽ chặn lại các cuộc gọi từ client cho đến khi có yêu cầu kết nối từ client.
Đối với Client :  
dùng phương thức connect(IP, PORT) để kết nối tới server  

![Capture](https://user-images.githubusercontent.com/86332370/162334708-1442c39e-a94e-4464-9810-5d89989882a0.PNG)  

- bước 3. Server nghe thấy lời gọi connect() của client, nó tiến hành accept()
- bước 4. cả 2 trao đổi thông tin qua hàm nhận và hàm gửi
