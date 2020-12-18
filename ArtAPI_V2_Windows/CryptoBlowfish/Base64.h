//
// Created by 82103 on 2020-05-07.
//
#pragma once

#ifndef ANYLOCK_BASE64_H
#define ANYLOCK_BASE64_H

int     base64_encode(char *text, int numBytes, char **encodedText);
int     base64_decode(char *text, unsigned char *dst, int numBytes);


#endif //ANYLOCK_BASE64_H
