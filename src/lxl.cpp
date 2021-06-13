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
    string lxl_version = "0.2";
    clear();
    cout << "LXL interpreter " << lxl_version << "\n";
    int atlama;
    if(argc>=3){
        cout << "Please do not enter more than one argument. (" << argc << ")";
        return 0;
    }
    if(argc!=2){
        cout << "Please enter a file: ";
        cin >> lxlfile;
        atlama = 2;
    }
    else
    {
        lxlfile = argv[1];
        atlama = 1;
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
                cout << "Line " << nline << " has some problems.";
                return 0;
            }
            if(words[2]!="=")
            {
                cout << "Line " << nline << " has some problems.\nYes, I know what to do but it should be like this:\nstring var = val";
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
                cout << "Line " << nline << " has some problems.";
                return 0;
            }
            if(words[2]!="=")
            {
                cout << "Line " << nline << " has some problems.\nYes, I know what to do but it should be like this:\nstring var = val";
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
                cout << "Line " << nline << " has some problems.\nCharacter variables can only be one character, you should use integer for numbers, string for strings.";
            }
            else
                characters[words[1]] = str;
        } else
        if(words[0]=="integer"){
            if(!(words.size() >= 4))
            {
                cout << "Line " << nline << " has some problems.";
                return 0;
            }
            if(words[2]!="=")
            {
                cout << "Line " << nline << " has some problems.\nYes, I know what to do but it should be like this:\nstring var = val";
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
                cout << "Line " << nline << " has some problems.\nInteger variables can only be numbers, you should use character for characters, string for strings.";
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
                cout << "Satır " << nline << " bazı sorunlara sahip.\nwritev değişkeni bulamadı.";
            }
        } else
        if(words[0]=="nline"){
            cout << "\n";
        } else
        if(words[0]=="input"){
            if(!(words.size() >= 2))
            {
                cout << "Line " << nline << " has some problems.";
                return 0;
            }
            cin >> strings[words[1]];
        }

        /*
        Draft:
 else
        if(words[0]==""){
            if(!(words.size() >= 2))
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
