#pragma once

//headers needed for the CSPRNG
#ifdef _WIN32
#include <Windows.h>
#include <Wincrypt.h>
#else
#include <fstream> //for reading from /dev/urandom on *nix systems
#endif

using namespace std;
using namespace System;

class	CBlowfish;
class	BLOWFISH;

namespace	LibWrapper {

	// * 아두이노 작업 : 한글 인코딩을 못해서 영어만 되도록 함.
	// C++, C# 일 경우는 Blowfish를 그대로 사용한다....
	public ref class Blowfish8B
	{
	protected:
		
		CBlowfish		*mBlowfish	= NULL;
		String			^mb_key;

		std::string		toStandardString(String^ string);

	public:
		Blowfish8B(System::String ^ key);
		~Blowfish8B();

		void		SetKey(String ^ key);
		String ^	Encrypt(String ^ context);
		String ^	Decrypt(String ^ context);
	};

	// 키값은 짝수여야 한다. 왠지는 모르지만.....
	public ref class Blowfish
	{
	protected:
		BLOWFISH		*mBlowfish = NULL;
		System::String ^mb_key;

		std::string		toStandardString(System::String^ string);

	public:
		Blowfish(System::String ^ key);
		~Blowfish();

		System::String ^ Blowfish::Encrypt(System::String ^ context);
		System::String ^ Blowfish::Decrypt(System::String ^ context);
	};

}
