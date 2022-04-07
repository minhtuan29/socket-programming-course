#include <iostream>
#include <tchar.h>
#include <stdlib.h>
#include <winsock2.h>
#pragma comment (lib,"WS2_32.lib")


class ServerUDPconnection {
private:
	static int const BUFFER_SIZE = 51;

private:
	char* buffer = new char[BUFFER_SIZE];
	SOCKET wsk;
	struct sockaddr_in cliAddr;
	int cliAddrLen;


public:

	ServerUDPconnection(const char* SERVER_ADDR, int PORT)
	{
		WSADATA wSADATA;
		WSAStartup(MAKEWORD(2, 2), &wSADATA);
		this->wsk = socket(AF_INET, SOCK_DGRAM, 0);

		struct  sockaddr_in serAddr;
		serAddr.sin_family = AF_INET;
		serAddr.sin_addr.S_un.S_addr = INADDR_ANY;
		serAddr.sin_port = htons(PORT);
		if (bind(this->wsk, (struct sockaddr*)&serAddr, sizeof(serAddr)) < 0)
			exit(1);

		this->cliAddrLen = sizeof(this->cliAddr);
	}

	~ServerUDPconnection()
	{
		delete[] this->buffer;
		closesocket(this->wsk);
		WSACleanup();
	}




	int send_data(int n)
	{
		itoa(n, this->buffer, 10);
		return sendto(this->wsk, this->buffer, BUFFER_SIZE, 0, (struct sockaddr*)&this->cliAddr, this->cliAddrLen);
	}

	int send_data(char* text)
	{
		return sendto(this->wsk, text, BUFFER_SIZE, 0, (struct sockaddr*)&this->cliAddr, this->cliAddrLen);
	}

	char* get_data()
	{
		recvfrom(this->wsk, this->buffer, BUFFER_SIZE, 0, (struct sockaddr*)&this->cliAddr, &this->cliAddrLen);
		return this->buffer;
	}

	int get_data_by_num()
	{
		recvfrom(this->wsk, this->buffer, BUFFER_SIZE, 0, (struct sockaddr*)&this->cliAddr, &this->cliAddrLen);
		return atoi(this->buffer);
	}

};
