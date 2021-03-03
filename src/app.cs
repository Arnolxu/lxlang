using System;
using System.IO;
using System.Collections.Generic;

class Program
{
	public static void Main()
	{
		Console.WriteLine("LXL yorumlayıcı");
		Console.Write("Lütfen bir .lxl dosyası girin: ");
		string path = Console.ReadLine();
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
		string lxl_version = "0.1.2";
		string lxl_build = "7";
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
			}
			else{Console.WriteLine("Satır " + nline + " bazı sorunlara sahip. Komut bulunamadı."); System.Environment.Exit(0);}
			/*
			Taslak: 
			
			if(words[0]=="")
			{
			}
			*/
			nline++;
		}
	}
	void LXL_ForeColor(string color)
	{
	
	}
}
