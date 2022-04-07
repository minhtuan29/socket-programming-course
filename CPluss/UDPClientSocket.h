#include <iostream>
#include <tchar.h>
#include <stdlib.h>
#include <winsock2.h>
#pragma comment (lib,"WS2_32.lib")


class ClientUDPconnection {
	static int const BUFFER_SIZE = 51;

	SOCKET wsk;
	struct sockaddr_in serAddr;
	int serAddrLen;
	char* buffer = new char[BUFFER_SIZE];

public:
	ClientUDPconnection(const char* SERVER_ADDR, int PORT)
	{
		WSADATA wSADATA;
		WSAStartup(MAKEWORD(2, 2), &wSADATA);

		this->wsk = socket(AF_INET, SOCK_DGRAM, 0); // IPv4 and UDP CONNECT

		this->serAddr.sin_family = AF_INET;
		this->serAddr.sin_addr.S_un.S_addr = inet_addr(SERVER_ADDR);
		this->serAddr.sin_port = htons(PORT);

		this->serAddrLen = sizeof(this->serAddr);
	}

	~ClientUDPconnection()
	{
		delete[] this->buffer;
		closesocket(this->wsk);
		WSACleanup();
	}


	void send_data(int n)
	{
		itoa(n, this->buffer, 10);
		sendto(this->wsk, this->buffer, BUFFER_SIZE, 0, (struct sockaddr*)&this->serAddr, this->serAddrLen);
	}

	int send_data(char* text)
	{
		return sendto(this->wsk, text, BUFFER_SIZE, 0, (struct sockaddr*)&this->serAddr, 100);
	}

	char* get_data()
	{
		recvfrom(this->wsk, this->buffer, BUFFER_SIZE, 0, (struct sockaddr*)&this->serAddr, &this->serAddrLen);
		return this->buffer;
	}

	int get_data_by_num()
	{
		recvfrom(this->wsk, this->buffer, BUFFER_SIZE, 0, (struct sockaddr*)&this->serAddr, &this->serAddrLen);
		return atoi(this->buffer);
	}

};
