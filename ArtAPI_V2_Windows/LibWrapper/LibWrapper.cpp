#include "pch.h"

#include <string>
#include <atlstr.h>		//CString
#include <iostream>

#include "LibWrapper.h"
#include "../CryptoBlowfish/CBlowfish.h"
#include "../CryptoBlowfish/CryptoBlowfish.h"


namespace	LibWrapper {

	// Blowfish wrapper
	Blowfish8B::Blowfish8B(System::String ^ key)
	{
		mb_key		= key;
		mBlowfish	= new CBlowfish();
		mBlowfish->Set_Passwd((char *)toStandardString(mb_key).c_str());
		//mBlowfish->Set_Passwd("FEDCBA9876543210");
	}

	Blowfish8B::~Blowfish8B() {
	}

	std::string Blowfish8B::toStandardString(System::String^ string) {
		using System::Runtime::InteropServices::Marshal;
		System::IntPtr pointer = Marshal::StringToHGlobalAnsi(string);
		char* charPointer = reinterpret_cast<char*>(pointer.ToPointer());
		std::string returnString(charPointer, string->Length);
		Marshal::FreeHGlobal(pointer);
		return returnString;
	}

	void	Blowfish8B::SetKey(String ^ key) {
		mb_key = key;
		if (mBlowfish == NULL) {
			mBlowfish = new CBlowfish();
		}
		mBlowfish->Set_Passwd((char *)toStandardString(mb_key).c_str());
		//mBlowfish->Set_Passwd("FEDCBA9876543210");
	}

	String ^ Blowfish8B::Encrypt(String ^ context) {

		if (mBlowfish == NULL) {
			SetKey(mb_key);
		}

		std::string asdf = toStandardString(context);
		//char * temp	= (char *)calloc(asdf.length+1,sizeof(char));
		//memcpy(temp, (char *)asdf.c_str(), asdf.length);
		//char * temp
		char * retv = mBlowfish->Encrypt((char *)asdf.c_str(), asdf.length());

		String ^	de_plain = gcnew String(retv);
		free(retv);
		return	de_plain;
	}

	String ^ Blowfish8B::Decrypt(String ^ context) {
		if (mBlowfish == NULL) {
			SetKey(mb_key);
		}
		std::string asdf = toStandardString(context);
		int		en_len = 0;
		//48Ts032kUVEzA8cIgS + thNMYSGOKCy8OyKTs88zx2IjFtI19ANdKkA ==
		//char * temp = "48Ts032kUVEzA8cIgS+thNMYSGOKCy8OyKTs88zx2IjFtI19ANdKkA==";
		char * retv = mBlowfish->Decrypt((char *)asdf.c_str(), asdf.length(), &en_len);
		//char * retv = mBlowfish->Decrypt(temp, strlen(temp), &en_len);
		//System::String dddd = Text::Encoding::UTF8.GetString(retv);
		//std::string asdf	= "C1BE27B5168D33CDAF5CC792543A071099224DEC03027C43A188971C7422D72796E16D4D65F1472B";

		/*
		cout << "Encrypted: " << asdf << endl;
		std::string retv = mBlowfish->Decrypt_CBC(asdf);
		cout << "Decrypted: " << retv << endl;
		//cout << key << endl;

		CString orig = "aaaaa";
		System::String ^systemstring = gcnew String(orig);

		return	systemstring + mb_key + context;
		*/
		String ^	en_plain = gcnew String(retv);
		free(retv);
		return	en_plain;
		//return	retv;
	}


	// ---------------------------------------------------------
	// 한글 지원
	// Blowfish wrapper
	// ---------------------------------------------------------
	Blowfish::Blowfish(System::String ^ key)
	{
		mb_key = key;
		mBlowfish = new BLOWFISH(toStandardString(mb_key));
	}

	Blowfish::~Blowfish() {
	}

	std::string Blowfish::toStandardString(System::String^ string) {
		using System::Runtime::InteropServices::Marshal;
		System::IntPtr pointer = Marshal::StringToHGlobalAnsi(string);
		char* charPointer = reinterpret_cast<char*>(pointer.ToPointer());
		std::string returnString(charPointer, string->Length);
		Marshal::FreeHGlobal(pointer);
		return returnString;
	}

	System::String ^ Blowfish::Decrypt(System::String ^ context) {

		if (mBlowfish == NULL) {
			std::string b_key = toStandardString(mb_key);
			mBlowfish = new BLOWFISH(b_key);
		}

		//CString orig = "aaaaa";
		//System::String ^systemstring = gcnew String(orig);

		std::string asdf = toStandardString(context);
		std::string retv = mBlowfish->Decrypt_CBC(asdf);

		/*
		return	systemstring + mb_key + context;
		*/
		return	gcnew String(retv.c_str());
	}

	// 
	System::String ^ Blowfish::Encrypt(System::String ^ context) {

		if (mBlowfish == NULL) {
			std::string b_key = toStandardString(mb_key);
			mBlowfish = new BLOWFISH(b_key);
		}
		std::string asdf = toStandardString(context);
		asdf = mBlowfish->Encrypt_CBC(asdf);

		//std::string asdf	= "C1BE27B5168D33CDAF5CC792543A071099224DEC03027C43A188971C7422D72796E16D4D65F1472B";

		/*
		cout << "Encrypted: " << asdf << endl;
		std::string retv = mBlowfish->Decrypt_CBC(asdf);
		cout << "Decrypted: " << retv << endl;
		//cout << key << endl;

		CString orig = "aaaaa";
		System::String ^systemstring = gcnew String(orig);

		return	systemstring + mb_key + context;
		*/
		return	gcnew String(asdf.c_str());
	}
}
