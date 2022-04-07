#pragma once
#include <iostream>
#include <tchar.h> 
#include <stdlib.h> 
#include <winsock2.h>
#pragma comment (lib,"WS2_32.lib")



class SerCnt
{
	

	SOCKET wsk;
	char* buf;

public:
	static int const BUF_SIZE = 51;


	SerCnt(int port) {
		
		WSADATA w;
		if (WSAStartup(MAKEWORD(2, 2), &w) != 0)
			exit(-1);

		struct sockaddr_in s;
		s.sin_family = AF_INET; // IPv4
		s.sin_addr.S_un.S_addr = INADDR_ANY;
		s.sin_port = htons(port);

		SOCKET tmpSk = socket(AF_INET, SOCK_STREAM, 0); // TCP
		if (bind(tmpSk, (struct sockaddr*)&s, sizeof(s)) < 0)
			exit(-1);

		listen(tmpSk, 5); // 5 milisecond

		struct sockaddr_in cli;
		int cliLen = sizeof(cli);

		std::cout << "san sang, dang cho` ket noi tu phia client\n";
		SOCKET newSk = accept(tmpSk, (struct sockaddr*)&cli, &cliLen);
		std::cout << "ket noi thanh cong\n";

		closesocket(tmpSk);

		if (newSk < 0) {
			std::cout << "loi ket noi\n";
			exit(1);
		}

		wsk = newSk;
		buf = new char[BUF_SIZE];
	}

	~SerCnt() {
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

	int sendMsg(char msg[]) {
		std::cout << 1;
		return send(wsk, msg, BUF_SIZE, 0);
	}

	int sendNum(int n) {
		itoa(n, buf, 10);
		return send(wsk, buf, BUF_SIZE, 0);
	}

};
