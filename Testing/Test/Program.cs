// See https://aka.ms/new-console-template for more information
using System;
using System.Text;

Byte[] xy = Encoding.Latin1.GetBytes("Hellö Wörld");

var enctype = FileEncodings.ISO_8859_1;

String encString = Enum.GetName(typeof(FileEncodings), enctype).Replace("_", "-");

Encoding e2 = Encoding.GetEncoding(encString);

Console.WriteLine(e2.GetString(xy));


enum FileEncodings
{
    UTF_8,
    UTF_32,
    ISO_8859_1,
    ASCII,
    LATIN_1
};


    
