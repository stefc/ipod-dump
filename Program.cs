// See https://aka.ms/new-console-template for more information
using stefc.itunes;

var chunks = ITunesReader.ReadFromFile("../green/iTunesDB");
// var chunks = ITunesReader.ReadFromFile("../green/Copy_iTunesDB");

var dump = new ITunesDump(chunks);

dump.Dump();

// 1:1 copy of 'iTunesDB' ?
// ITunesWriter.WriteToFile("../green/Copy_iTunesDB", chunks);
