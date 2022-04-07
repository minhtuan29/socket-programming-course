#pragma once
#pragma once
#include <iostream>
#include <tchar.h>  
#include <stdlib.h> 
#include <winsock2.h>
#include <cstring>
#pragma comment (lib,"WS2_32.lib")




class CliCnt
{
public:
	static int const BUF_SIZE = 1024;

private: 
	SOCKET wsk;
	char* buf;

public:

	CliCnt(const char ip[], int port) {
		WSADATA w;
		WSAStartup(MAKEWORD(2, 2), &w);

		wsk = socket(AF_INET, SOCK_STREAM, 0);
		if (wsk < 0) {
			std::cout << "khong tao duoc wsocket";
			exit(1);
		}

		struct sockaddr_in  s;
		s.sin_family = AF_INET;
		s.sin_addr.S_un.S_addr = inet_addr(ip);
		s.sin_port = htons(port);
		if (connect(wsk, (struct sockaddr*)&s, sizeof(s)) < 0) {
			std::cout << "loi ket noi";
			exit(1);
		}
		std::cout << "ket noi thanh cong\n";

		buf = new char[BUF_SIZE];
	}


	~CliCnt() {
		delete[] buf;
		closesocket(wsk);
		WSACleanup();
	}


	int getNum() {
		recv(wsk, buf, BUF_SIZE, 0);
		return atoi(buf);
	}


	char* getMsg() {
		recv(wsk, buf, BUF_SIZE, 0);
		return buf;
	}

	void sendMsg(char msg[]) {
		send(wsk, msg, BUF_SIZE, 0);
	}


	void sendNum(int n) {
		itoa(n, buf, 10);
		send(wsk, buf, BUF_SIZE, 0);
	}
};
