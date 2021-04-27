using System;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

class Program
{
	public static void Main(string[] args)
	{
		Program prg = new Program();
		string lxl_version = "0.1.5";
		Console.Clear();
		Console.WriteLine("LXL yorumlayıcı " + lxl_version);
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
		foreach(string line in source)
		{
			string[] words = line.Split(' ');
			if(line==""||line==" ")
				continue;
			if(line.Substring(0, 1)=="#")
				continue;
			if(words[0]=="string")
			{
				if(!(words.Length >= 4))
				{
					Console.WriteLine("Satır " + nline + " bazı sorunlara sahip.");
					System.Environment.Exit(0);
				}
				if(words[2]!="=")
				{
					Console.WriteLine("Satır " + nline + " bazı sorunlara sahip.\nEvet, ne yapacağımı biliyorum ama orasının şöyle bir şey olması gerek:\nstring degisken = deger\nProsedür gereği ;)");
					System.Environment.Exit(0);
				}
				if(words[3]=="none")
				{
					prg.addOrUpdate(strings, words[1], null);
				} else {
					prg.addOrUpdate(strings, words[1], words[3]);
				}
			}else
			if(words[0]=="character")
			{
				if(!(words.Length >= 4))
				{
					Console.WriteLine("Satır " + nline + " bazı sorunlara sahip.");
					System.Environment.Exit(0);
				}
				if(words[2]!="=")
				{
					Console.WriteLine("Satır " + nline + " bazı sorunlara sahip.\nEvet, ne yapacağımı biliyorum ama orasının şöyle bir şey olması gerek:\ncharacter degisken = deger\nProsedür gereği ;)");
					System.Environment.Exit(0);
				}
				if(words[3]=="none")
				{
					prg.addOrUpdate(chars, words[1], null);
				} else {
					prg.addOrUpdate(chars, words[1], words[3]);
				}
				if(words[3].Length >= 2)
				{
					Console.WriteLine("Satır " + nline + " bazı sorunlara sahip.\nCHAR değişkenleri sadece tek bir karaktere sahip olabilir, sayılar için INT, yazılar için ise STRING kullanmalısın.");
					System.Environment.Exit(0);
				}
			}else
			if(words[0]=="integer")
			{
				if(!(words.Length >= 4))
				{
					Console.WriteLine("Satır " + nline + " bazı sorunlara sahip.");
					System.Environment.Exit(0);
				}
				if(words[2]!="=")
				{
					Console.WriteLine("Satır " + nline + " bazı sorunlara sahip.\nEvet, ne yapacağımı biliyorum ama orasının şöyle bir şey olması gerek:\ninteger degisken = deger\nProsedür gereği ;)");
					System.Environment.Exit(0);
				}
				if(words[3]=="none")
				{
					prg.addOrUpdate(ints, words[1], null);
				} else {
					prg.addOrUpdate(ints, words[1], words[3]);
				}
				if(!Int32.TryParse(words[3], out _))
				{
					Console.WriteLine("Satır " + nline + " bazı sorunlara sahip.\nInteger değişkenleri sadece sayı olabilir, karakterler için character, yazılar için ise string kullanmalısın.");
					System.Environment.Exit(0);
				}
			}else
			if(words[0]=="write")
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
			if(words[0]=="writev")
			{
				if(strings.ContainsKey(words[1]))
				{
					Console.Write(strings[words[1]]);
				} else if(chars.ContainsKey(words[1]))
				{
					Console.Write(chars[words[1]]);
				} else if(ints.ContainsKey(words[1]))
				{
					Console.Write(ints[words[1]]);
				} else {
					Console.WriteLine("Satır " + nline + " bazı sorunlara sahip.\nwritev değişkeni bulamadı.");
				}
			}else
			if(words[0]=="input")
			{
				if(!(words.Length == 2))
				{
					if(!(words.Length == 3))
					{
						Console.WriteLine("Satır " + nline + " bazı sorunlara sahip.\nOkunan veriyi sadece bir değişkene atayabilirsin, tabii bunu daha sonra diğerlerine de dağıtabilirsin. Ve bunu da şu şekilde yapmalısın:\n\ninput a\n\nBu arada, \"a\", bizim değişkenimiz oluyor.\nAyrıca, girdinin yeşil olmasını istemiyorsan, --no-color parametresini de kullanmalısın.");
						System.Environment.Exit(0);
					}
				}
				if(!(words.Length == 3 && (words[2]=="--no-color")))
					Console.ForegroundColor = ConsoleColor.DarkGreen;
				strings.Add(words[1], Console.ReadLine());
				if(!(words.Length == 3 && (words[2]=="--no-color")))
					Console.ForegroundColor = ConsoleColor.White;
			}else
			if(words[0]=="nline")
			{
				Console.Write("\n");
			}else
			if(words[0]=="cursor")
			{
				int c1;
				int c2;
				if(words.Length!=3)
				{
					Console.WriteLine("Satır " + nline + " bazı sorunlara sahip.\nCursor komutunu kullanırken, 2 parametreye ihtiyacın var. İmlecin yerleştirileceği Y ve X pozisyonları.\n\ncursor 0 0\ngibi.");
					System.Environment.Exit(0);
				}
				Int32.TryParse(words[1], out c1);
				Int32.TryParse(words[2], out c2);
				Console.SetCursorPosition(c1, c2 + atlama);
			}else
			if(words[0]=="color")
			{
				string[] colors = {"0", "1", "2", "3", "4", "5", "6", "7", "8", "9"};
				if(words.Length!=2)
				{
					Console.WriteLine("Satır " + nline + " bazı sorunlara sahip.\ncolor komutunda 1 adet parametre olmalı.");
					System.Environment.Exit(0);
				}
				if(!colors.Contains(words[1]))
				{
					Console.WriteLine("Satır " + nline + " bazı sorunlara sahip.\ncolor komutunun parametresi 0-9 arasında bir rakam olmalı.");
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
			}else
			if(words[0]=="add")
			{
				if(words.Length!=3)
				{
					Console.WriteLine("Satır " + nline + " bazı sorunlara sahip.");
					System.Environment.Exit(0);
				}
				if(!ints.ContainsKey(words[1]) || (!Int32.TryParse(words[2], out _) && !ints.ContainsKey(words[2])))
				{
					Console.WriteLine("Satır " + nline + " bazı sorunlara sahip. Birinden biri sayı değil.");
					System.Environment.Exit(0);
				}
				int a;
				int b;
				Int32.TryParse(ints[words[1]], out a);
				if(!Int32.TryParse(words[2], out b))
					Int32.TryParse(ints[words[2]], out b);
				ints[words[1]] = (a + b).ToString();
			}else
			if(words[0]=="dec")
			{
				if(words.Length!=3)
				{
					Console.WriteLine("Satır " + nline + " bazı sorunlara sahip.");
					System.Environment.Exit(0);
				}
				if(!ints.ContainsKey(words[1]) || (!Int32.TryParse(words[2], out _) && !ints.ContainsKey(words[2])))
				{
					Console.WriteLine("Satır " + nline + " bazı sorunlara sahip. Birinden biri sayı değil.");
					System.Environment.Exit(0);
				}
				int a;
				int b;
				Int32.TryParse(ints[words[1]], out a);
				if(!Int32.TryParse(words[2], out b))
					Int32.TryParse(ints[words[2]], out b);
				ints[words[1]] = (a - b).ToString();
			}else
			if(words[0]=="sep")
			{
				if(words.Length!=3)
				{
					Console.WriteLine("Satır " + nline + " bazı sorunlara sahip.");
					System.Environment.Exit(0);
				}
				if(!ints.ContainsKey(words[1]) || (!Int32.TryParse(words[2], out _) && !ints.ContainsKey(words[2])))
				{
					Console.WriteLine("Satır " + nline + " bazı sorunlara sahip. Birinden biri sayı değil.");
					System.Environment.Exit(0);
				}
				int a;
				int b;
				Int32.TryParse(ints[words[1]], out a);
				if(!Int32.TryParse(words[2], out b))
					Int32.TryParse(ints[words[2]], out b);
				ints[words[1]] = (a / b).ToString();
			}else
			if(words[0]=="mlt")
			{
				if(words.Length!=3)
				{
					Console.WriteLine("Satır " + nline + " bazı sorunlara sahip.");
					System.Environment.Exit(0);
				}
				if(!ints.ContainsKey(words[1]) || (!Int32.TryParse(words[2], out _) && !ints.ContainsKey(words[2])))
				{
					Console.WriteLine("Satır " + nline + " bazı sorunlara sahip. Birinden biri sayı değil.");
					System.Environment.Exit(0);
				}
				int a;
				int b;
				Int32.TryParse(ints[words[1]], out a);
				if(!Int32.TryParse(words[2], out b))
					Int32.TryParse(ints[words[2]], out b);
				ints[words[1]] = (a * b).ToString();
			}else
			if(words[0]=="mod")
			{
				if(words.Length!=3)
				{
					Console.WriteLine("Satır " + nline + " bazı sorunlara sahip.");
					System.Environment.Exit(0);
				}
				if(!ints.ContainsKey(words[1]) || (!Int32.TryParse(words[2], out _) && !ints.ContainsKey(words[2])))
				{
					Console.WriteLine("Satır " + nline + " bazı sorunlara sahip. Birinden biri sayı değil.");
					System.Environment.Exit(0);
				}
				int a;
				int b;
				Int32.TryParse(ints[words[1]], out a);
				if(!Int32.TryParse(words[2], out b))
					Int32.TryParse(ints[words[2]], out b);
				ints[words[1]] = (a % b).ToString();
			}else
			if(words[0]=="end")
			{
				Console.Write("\n");
				Console.ForegroundColor = ConsoleColor.White;
				Environment.Exit(0);
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
	private void addOrUpdate(Dictionary<string, string> dic, string key, string newValue)
	{
    	string val;
    	if (dic.TryGetValue(key, out val))
    	{
        	// değer var.
        	dic[key] = newValue;
    	}
    	else
    	{
        	// ah, değer yok.
        	dic.Add(key, newValue);
    	}
	}
}
