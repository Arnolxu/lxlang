using System;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

class Program
{
	public static void Main(string[] args)
	{
		Console.Clear();
		Console.WriteLine("LXL yorumlayıcı");
		string path;
		int atlama;
		if(args.Length>=2)
		{
			Console.WriteLine("Lütfen birden fazla argüman girmeyin.");
			System.Environment.Exit(0);
		}
		if(args.Length!=1)
		{
			Console.Write("Lütfen bir .lxl dosyası girin: ");
			path = Console.ReadLine();
			atlama = 2;
		}
		else
		{
			path = args[0];
			atlama = 1;
		}
		if(!File.Exists(@path)){
			Console.WriteLine("Geçersiz Yol.");
			System.Environment.Exit(0);
		}
		if(!(Path.GetExtension(@path) == ".lxl")){
			Console.WriteLine("Geçersiz dosya uzantısı.");
			System.Environment.Exit(0);
		}
		string[] source = File.ReadAllLines(@path);
		int nline = 1;
		Dictionary<string, string> strings = new Dictionary<string, string>();
		Dictionary<string, string> chars = new Dictionary<string, string>();
		Dictionary<string, string> ints = new Dictionary<string, string>();
		string lxl_version = "0.1.4";
		string lxl_build = "1";
		foreach(string line in source)
		{
			string[] words = line.Split(' ');
			if(line.Substring(0, 1)=="#")
				continue;
			if(words[0]=="STRING")
			{
				if(!(words.Length >= 4))
				{
					Console.WriteLine("Satır " + nline + " bazı sorunlara sahip.");
					System.Environment.Exit(0);
				}
				if(words[2]!="=")
				{
					Console.WriteLine("Satır " + nline + " bazı sorunlara sahip.\nEvet, ne yapacağımı biliyorum ama orasının şöyle bir şey olması gerek:\nSTRING degisken = deger\nProsedür gereği ;)");
					System.Environment.Exit(0);
				}
				if(words[3]=="NULL")
				{
					strings.Remove(words[1]);
				} else {
					strings.Add(words[1], words[3]);
				}
			}else
			if(words[0]=="CHAR")
			{
				if(!(words.Length >= 4))
				{
					Console.WriteLine("Satır " + nline + " bazı sorunlara sahip.");
					System.Environment.Exit(0);
				}
				if(words[2]!="=")
				{
					Console.WriteLine("Satır " + nline + " bazı sorunlara sahip.\nEvet, ne yapacağımı biliyorum ama orasının şöyle bir şey olması gerek:\nCHAR degisken = deger\nProsedür gereği ;)");
					System.Environment.Exit(0);
				}
				if(words[3]=="NULL")
				{
					chars.Remove(words[1]);
				} else {
					chars.Add(words[1], words[3]);
				}
				if(words[3].Length >= 2)
				{
					Console.WriteLine("Satır " + nline + " bazı sorunlara sahip.\nCHAR değişkenleri sadece tek bir karaktere sahip olabilir, sayılar için INT, yazılar için ise STRING kullanmalısın.");
					System.Environment.Exit(0);
				}
			}else
			if(words[0]=="INT")
			{
				if(!(words.Length >= 4))
				{
					Console.WriteLine("Satır " + nline + " bazı sorunlara sahip.");
					System.Environment.Exit(0);
				}
				if(words[2]!="=")
				{
					Console.WriteLine("Satır " + nline + " bazı sorunlara sahip.\nEvet, ne yapacağımı biliyorum ama orasının şöyle bir şey olması gerek:\nINT degisken = deger\nProsedür gereği ;)");
					System.Environment.Exit(0);
				}
				if(words[3]=="NULL")
				{
					ints.Remove(words[1]);
				} else {
					ints.Add(words[1], words[3]);
				}
				if(!Int32.TryParse(words[3], out _))
				{
					Console.WriteLine("Satır " + nline + " bazı sorunlara sahip.\nINT değişkenleri sadece sayı olabilir, karakterler için CHAR, yazılar için ise STRING kullanmalısın.");
					System.Environment.Exit(0);
				}
			}else
			if(words[0]=="PRINT")
			{
				for(int i = 1; i < words.Length; i++)
				{
					Console.Write(words[i]);
					if(i!=words.Length-1)
					{
						Console.Write(" ");
					}
				}
			}else
			if(words[0]=="PRINTVAR")
			{
				if(words[1]=="S")
				{
					Console.Write(strings[words[2]]);
				} else if(words[1]=="C")
				{
					Console.Write(chars[words[2]]);
				} else if(words[1]=="I")
				{
					Console.Write(ints[words[2]]);
				} else {
					Console.WriteLine("Satır " + nline + " bazı sorunlara sahip.\nPRINTVAR komutu değişkenleri yazmak içindir, ve yazarken değişken türünü de belirtmelisin. Şunun gibi;\n PRINTVAR S string\n");
				}
			}else
			if(words[0]=="READ")
			{
				if(!(words.Length == 2))
				{
					if(!(words.Length == 3))
					{
						Console.WriteLine("Satır " + nline + " bazı sorunlara sahip.\nOkunan veriyi sadece bir değişkene atayabilirsin, tabii bunu daha sonra diğerlerine de dağıtabilirsin. Ve bunu da şu şekilde yapmalısın:\n\nREAD a\n\nBu arada, \"a\", bizim değişkenimiz oluyor.\nAyrıca, girdinin yeşil olmasını istemiyorsan, -NCLR parametresini de kullanmalısın.");
						System.Environment.Exit(0);
					}
				}
				if(!(words.Length == 3 && words[2]=="-NCLR"))
					Console.ForegroundColor = ConsoleColor.DarkGreen;
				strings.Add(words[1], Console.ReadLine());
				if(!(words.Length == 3 && words[2]=="-NCLR"))
					Console.ForegroundColor = ConsoleColor.White;
			}else
			if(words[0]=="NEWLINE")
			{
				Console.Write("\n");
			}else
			if(words[0]=="INFO")
			{
				Console.WriteLine("LXL INFO\nSürüm: " + lxl_version + "\nDerleme Numarası: " + lxl_build);
			}else
			if(words[0]=="CURSOR")
			{
				int c1;
				int c2;
				if(words.Length!=3)
				{
					Console.WriteLine("Satır " + nline + " bazı sorunlara sahip.\nCURSOR komutunu kullanırken, 2 parametreye ihtiyacın var. İmlecin yerleştirileceği Y ve X pozisyonları.\n\nCURSOR 0 0\ngibi.");
					System.Environment.Exit(0);
				}
				Int32.TryParse(words[1], out c1);
				Int32.TryParse(words[2], out c2);
				Console.SetCursorPosition(c1, c2 + atlama);
			}else
			if(words[0]=="COLOR")
			{
				string[] colors = {"0", "1", "2", "3", "4", "5", "6", "7", "8", "9"};
				if(words.Length!=2)
				{
					Console.WriteLine("Satır " + nline + " bazı sorunlara sahip.\nCOLOR komutunda 1 adet parametre olmalı.");
					System.Environment.Exit(0);
				}
				if(!colors.Contains(words[1]))
				{
					Console.WriteLine("Satır " + nline + " bazı sorunlara sahip.\nCOLOR komutunun parametresi 0-9 arasında bir rakam olmalı.");
					System.Environment.Exit(0);
				}
				if(words[1]=="0")
					Console.ForegroundColor = ConsoleColor.White;
				if(words[1]=="1")
					Console.ForegroundColor = ConsoleColor.Cyan;
				if(words[1]=="2")
					Console.ForegroundColor = ConsoleColor.Green;
				if(words[1]=="3")
					Console.ForegroundColor = ConsoleColor.DarkBlue;
				if(words[1]=="4")
					Console.ForegroundColor = ConsoleColor.Red;
				if(words[1]=="5")
					Console.ForegroundColor = ConsoleColor.Magenta;
				if(words[1]=="6")
					Console.ForegroundColor = ConsoleColor.DarkRed;
				if(words[1]=="7")
					Console.ForegroundColor = ConsoleColor.Gray;
				if(words[1]=="8")
					Console.ForegroundColor = ConsoleColor.DarkYellow;
				if(words[1]=="9")
					Console.ForegroundColor = ConsoleColor.Yellow;
			}
			else{Console.WriteLine("Satır " + nline + " bazı sorunlara sahip. Komut bulunamadı."); System.Environment.Exit(0);}
			/*
			Taslak: 
			
else
			if(words[0]=="")
			{
				if(words.Length!=2)
				{
					Console.WriteLine("Satır " + nline + " bazı sorunlara sahip.");
					System.Environment.Exit(0);
				}
			}
			*/
			nline++;
		}
	}
}
