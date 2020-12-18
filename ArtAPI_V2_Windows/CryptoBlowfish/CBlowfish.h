//
// Created by 82103 on 2020-05-07.
//
#pragma once

#ifndef ANYLOCK_CBLOWFISH_H
#define ANYLOCK_CBLOWFISH_H

#include <stdlib.h>

#define NUM_SUBKEYS  18
#define NUM_S_BOXES  4
#define NUM_ENTRIES  256

#define MAX_STRING   256
#define MAX_PASSWD   56     // 448bits

// #define BIG_ENDIAN
// #define LITTLE_ENDIAN
#ifdef __ARMEB__
#define _BYTE_ORDER _BIG_ENDIAN
struct WordByte
{
	unsigned int zero:8;
	unsigned int one:8;
	unsigned int two:8;
	unsigned int three:8;
};
#else
#define _BYTE_ORDER _LITTLE_ENDIAN
struct WordByte
{
	unsigned int three:8;
	unsigned int two:8;
	unsigned int one:8;
	unsigned int zero:8;
};
#endif

#ifdef BIG_ENDIAN
#endif

#ifdef LITTLE_ENDIAN
#endif

union Word {
	unsigned int word;
	WordByte byte;
};

struct DWord {
	Word    word0;
	Word    word1;
};

//typedef struct  unsigned char   byte;
typedef  unsigned char     byte;

class	CBlowfish
{
private:
    unsigned int PA[NUM_SUBKEYS];
    unsigned int SB[NUM_S_BOXES][NUM_ENTRIES];

    void    Gen_Subkeys(char *);
    inline void BF_En(Word *,Word *);
    inline void BF_De(Word *,Word *);

public:
    CBlowfish();
    ~CBlowfish();

    void    Reset();
    bool    Set_Passwd(char * = NULL);
    char *  Encrypt(char *,int);
    char *  Decrypt(char *,int, int *);

    int     findPaddingEnd(byte* data, int length);
    byte *  padData(byte* data, int length, int* paddedLength, bool decrypt, bool IvSpace = false);
};

#endif  //ANYLOCK_CBLOWFISH_H
