#include <iostream>
#include <string>
#include <filesystem>
#include <fstream>
#include <map>
#include <regex>
using namespace std;
using std::filesystem::exists;

void clear(){
    #if defined _WIN32
        system("cls");
    #elif defined (__LINUX__) || defined(__gnu_linux__) || defined(__linux__)
        system("clear");
    #elif defined (__APPLE__)
        system("clear");
    #endif
}

bool isNum(string str){
    for(int i=0;i<str.size();i++)
        if(isdigit(str[i])==false)
            return false;
    return true;
}

int main(int argc, char** argv) {
    string lxlfile;
    string lxl_version = "0.2.3";
    clear();
    cout << "LXL interpreter " << lxl_version << "\n";
    int jump;
    if(argc>=3){
        cout << "Please do not enter more than one argument. (" << argc << ")";
        return 0;
    }
    if(argc!=2){
        cout << "Please enter a file: ";
        cin >> lxlfile;
        jump = 3;
    }
    else
    {
        lxlfile = argv[1];
        jump = 2;
    }
    filesystem::path path = lxlfile;
    if(!exists(lxlfile))
    {
        cout << "File doesn't exist.\n";
        return 0;
    }
    if(path.extension() != ".lxl")
    {
        cout << "File extension isn't `.lxl`.\n";
        return 0;
    }
    string word;
    ifstream lxlcont(lxlfile);
    map<string, string> strings;
    map<string, string> characters;
    map<string, string> integers;
    int nline = 1;
    string lin;
    while(getline(lxlcont, lin)){
        regex clregex("#(?=([^\"]*\"[^\"]*\")*[^\"]*$).*");
        string line = std::regex_replace(lin, clregex, "");
        regex tregex("^[ \t]+");
        line = std::regex_replace(line, tregex, "");
        stringstream stream(line);
        vector<string> words;
        while(getline(stream, word, ' ')){
            words.push_back(word);
        }
        if(line==""||line==" ")
            continue;
        if(words[0]=="write"){
            int i;
            string str = words[1];
            for(i = 2; i < words.size(); i++) {
                str = str + " " + words[i];
            }
            if(line[line.size()-1]==' ')
                str = str + " ";
            cout << str;
        } else
        if(words[0]=="string"){
            if(!(words.size() >= 4))
            {
                cout << "Line " << nline << " has some problems.\n";
                return 0;
            }
            if(words[2]!="=")
            {
                cout << "Line " << nline << " has some problems.\nYes, I know what to do but it should be like this:\nstring var = val\n";
                return 0;
            }
            int i;
            string str = words[3];
            for (i = 4; i < words.size(); i++) {
                str = str + " " + words[i];
            }
            if(line[line.size()-1]==' ')
                str = str + " ";
            if(str=="none")
                strings.erase(words[1]);
            else
                strings[words[1]] = str;
        } else
        if(words[0]=="character"){
            if(!(words.size() >= 4))
            {
                cout << "Line " << nline << " has some problems.\n";
                return 0;
            }
            if(words[2]!="=")
            {
                cout << "Line " << nline << " has some problems.\nYes, I know what to do but it should be like this:\ncharacter var = v\n";
                return 0;
            }
            int i;
            string str = words[3];
            for (i = 4; i < words.size(); i++) {
                str = str + " " + words[i];
            }
            if(str=="none")
                characters.erase(words[1]);
            else if(str.size()>=2){
                cout << "Line " << nline << " has some problems.\nCharacter variables can only be one character, you should use integer for numbers, string for strings.\n";
            }
            else
                characters[words[1]] = str;
        } else
        if(words[0]=="integer"){
            if(!(words.size() >= 4))
            {
                cout << "Line " << nline << " has some problems.\n";
                return 0;
            }
            if(words[2]!="=")
            {
                cout << "Line " << nline << " has some problems.\nYes, I know what to do but it should be like this:\ninteger var = 1";
                return 0;
            }
            int i;
            string str = words[3];
            for (i = 4; i < words.size(); i++) {
                str = str + " " + words[i];
            }
            if(str=="none")
                integers.erase(words[1]);
            else if(isNum(str)){
                cout << "Line " << nline << " has some problems.\nInteger variables can only be numbers, you should use character for characters, string for strings.\n";
            }
            else
                integers[words[1]] = str;
        } else
        if(words[0]=="writev"){
            if(strings.count(words[1])!=0)
            {
                cout << strings[words[1]];
            } else if(characters.count(words[1])!=0)
            {
                cout << characters[words[1]];
            } else if(integers.count(words[1])!=0)
            {
                cout << integers[words[1]];
            } else {
                cout << "Line " << nline << " has some problems.\nwritev couldn't find variable.\n";
            }
        } else
        if(words[0]=="nline"){
            cout << "\n";
        } else
        if(words[0]=="input"){
            if(!(words.size() == 2 || words.size() == 3))
            {
                cout << "Line " << nline << " has some problems.";
                return 0;
            }
            if(!(words.size() == 3 && words[2]=="-nc"))
                cout << "\u001b[32m";
            cin >> strings[words[1]];
            if(!(words.size() == 3 && words[2]=="-nc"))
                cout << "\u001b[0m";
        } else
        if(words[0]=="cursor"){
            if(words.size() != 3)
            {
                cout << "Line " << nline << " has some problems.\nWhile using cursor command, you need 2 parameters. Y and X positions.\nLike this:\nCURSOR 0 0\n";
                return 0;
            }
            if(!(isNum(words[1]) && isNum(words[2]))){
                cout << "Line " << nline << " has some problems.\nY and X positions in cursor command should be integers.\n";
                return 0;
            }
            int c1 = stoi(words[2]) + jump;
            int c2 = stoi(words[1]) + jump;
            cout << "\033["<< c1 << ";" << c2 << "f";
        } else
        if(words[0]=="color"){
            map<string, string> colors;
            colors.insert(pair<string, string>("0", "37"));
            colors.insert(pair<string, string>("1", "36"));
            colors.insert(pair<string, string>("2", "32"));
            colors.insert(pair<string, string>("3", "34"));
            colors.insert(pair<string, string>("4", "91"));
            colors.insert(pair<string, string>("5", "35"));
            colors.insert(pair<string, string>("6", "31"));
            colors.insert(pair<string, string>("7", "90"));
            colors.insert(pair<string, string>("8", "33"));
            colors.insert(pair<string, string>("9", "93"));
            colors.insert(pair<string, string>("a", "97"));
            colors.insert(pair<string, string>("b", "94"));
            colors.insert(pair<string, string>("c", "95"));
            colors.insert(pair<string, string>("d", "96"));
            colors.insert(pair<string, string>("e", "97"));
            colors.insert(pair<string, string>("f", "30"));
            if(words.size() != 2)
            {
                cout << "Line " << nline << " has some problems. Color command needs 1 parameter.\n";
                return 0;
            }
            if(colors.count(words[1])==0)
            {
                cout << "Line " << nline << " has some problems. The parameter of color command should be a hex number between 0-f.\n";
                return 0;
            }
            cout << "\u001b[" << colors[words[1]] << "m";
        }

        /*
        Draft:
 else
        if(words[0]==""){
            if(words.size() != 2)
            {
                cout << "Line " << nline << " has some problems.";
                return 0;
            }
        }
        */
        nline++;
    }
    cout << "\u001b[0m\n";
    return 0;
}
