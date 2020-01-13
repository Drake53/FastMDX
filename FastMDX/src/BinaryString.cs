﻿using System;
using System.Text;

namespace FastMDX {
    static class BinaryString {
        internal unsafe static string Decode(byte* bytes, uint len) {
            for(int i = 0; i < len; i++)
                if(bytes[i] == 0)
                    return Encoding.ASCII.GetString(bytes, i);
            
            return Encoding.ASCII.GetString(bytes, (int)len);
        }

        internal unsafe static void Encode(string str, byte* bytes, uint len) {
            var lp = (long*)bytes;
            var lc = len / sizeof(long);

            for(uint i = 0; i < lc; i++)
                lp[i] = 0L;

            for(uint i = lc * sizeof(long); i < len; i++)
                bytes[i] = 0;

            if(str?.Length > 0) {
                var count = Math.Min(str.Length, (int)len);
                fixed(char* c = str)
                    Encoding.ASCII.GetBytes(c, count, bytes, count);
            }
        }
    }
}
