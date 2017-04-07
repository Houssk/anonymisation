using System;
using System.Windows;
using System.Data.SqlClient;
using System.Data;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.IO.Compression;
using System.Drawing;
using itk.simple;
using gdcm;



namespace AnoDCM
{
    class Traitement
    {

        private int endianness;
        private int pos, pos_endian, pos_endian2;
        private int posbis, taille;
        int[] tabhexa;
        char[] tabdecimal;
        string str = "";


        public Traitement()
        {

        }

        //-------------------------------------------------------------------------------------------------------
           public MySqlConnection ConnectBDD()
         {
           MySql.Data.MySqlClient.MySqlConnection conn = new MySql.Data.MySqlClient.MySqlConnection();

               
             // Méthode qui va initialiser le builder :
             SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();


             // renseigner les paramètres de la connexion :
             builder.DataSource = "127.0.0.1";     // adresse du serveur
             builder.InitialCatalog = "globald";   // nom bdd
             builder.UserID = "root";              // id de l'utilisateur
             builder.Password = "";                // mot de passe


             // enfin passer la ConnectionString à l'objet SqlConnection
             string connString = builder.ConnectionString;
             MySqlConnection cn = new MySqlConnection(connString);
             try
             {
                 cn.Open();
                 Console.WriteLine("Connexion Active!"); // Affiche sur la console que la connexion est active
                 
             }
             catch (MySqlException ex)
             {
                 Console.WriteLine("Echec de connexion" + ex.Message); // Affiche sur la console que la connexion n'est pas active

             }


             return cn;
         }

           //-------------------------------------------------------------------------------------------------------

        public string RécupérerNomPatient(string filename, int endianness, int pos)//Permet de récupérer le nom du patient --> On en a besoin pour faire le PV de contrôle des images 
        {
            
            int oct = 0;
            int oct_prec = 0, oct_pos = 0;
            int pos_epaisseur = 0;


            BinaryReader br = null;


            FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.ReadWrite);
            fs.Position = fs.Seek(0, SeekOrigin.Begin);
            br = new BinaryReader(fs);

            while (fs.Position < pos)
            {
                oct_prec = oct;
                oct = br.ReadInt16();
                if ((oct_prec == 16) && (oct == 16))
                {
                    pos_epaisseur = (int)fs.Position;
                    pos_epaisseur = pos_epaisseur + 4;
                }
                if (fs.Position == pos_epaisseur)
                {
                    taille = oct;
                    posbis = (int)fs.Position;

                    tabhexa = new int[taille];
                    tabdecimal = new char[taille];
                    for (int i = 0; i < taille; i++)
                    {
                        oct_pos = br.ReadByte();
                        tabhexa[i] = oct_pos;
                        if (oct_pos == 65)
                            tabdecimal[i] = 'A';
                        else if (oct_pos == 66)
                            tabdecimal[i] = 'B';
                        else if (oct_pos == 67)
                            tabdecimal[i] = 'C';
                        else if (oct_pos == 68)
                            tabdecimal[i] = 'D';
                        else if (oct_pos == 69)
                            tabdecimal[i] = 'E';
                        else if (oct_pos == 70)
                            tabdecimal[i] = 'F';
                        else if (oct_pos == 71)
                            tabdecimal[i] = 'G';
                        else if (oct_pos == 72)
                            tabdecimal[i] = 'H';
                        else if (oct_pos == 73)
                            tabdecimal[i] = 'I';
                        else if (oct_pos == 74)
                            tabdecimal[i] = 'J';
                        else if (oct_pos == 75)
                            tabdecimal[i] = 'K';
                        else if (oct_pos == 76)
                            tabdecimal[i] = 'L';
                        else if (oct_pos == 77)
                            tabdecimal[i] = 'M';
                        else if (oct_pos == 78)
                            tabdecimal[i] = 'N';
                        else if (oct_pos == 79)
                            tabdecimal[i] = 'O';
                        else if (oct_pos == 80)
                            tabdecimal[i] = 'P';
                        else if (oct_pos == 81)
                            tabdecimal[i] = 'Q';
                        else if (oct_pos == 82)
                            tabdecimal[i] = 'R';
                        else if (oct_pos == 83)
                            tabdecimal[i] = 'S';
                        else if (oct_pos == 84)
                            tabdecimal[i] = 'T';
                        else if (oct_pos == 85)
                            tabdecimal[i] = 'U';
                        else if (oct_pos == 86)
                            tabdecimal[i] = 'V';
                        else if (oct_pos == 87)
                            tabdecimal[i] = 'W';
                        else if (oct_pos == 88)
                            tabdecimal[i] = 'X';
                        else if (oct_pos == 89)
                            tabdecimal[i] = 'Y';
                        else if (oct_pos == 90)
                            tabdecimal[i] = 'Z';

                            //------------------------------------------MINUSCULE
                        else if (oct_pos == 97)
                            tabdecimal[i] = 'a';
                        else if (oct_pos == 98)
                            tabdecimal[i] = 'b';
                        else if (oct_pos == 99)
                            tabdecimal[i] = 'c';
                        else if (oct_pos == 100)
                            tabdecimal[i] = 'd';
                        else if (oct_pos == 101)
                            tabdecimal[i] = 'e';
                        else if (oct_pos == 102)
                            tabdecimal[i] = 'f';
                        else if (oct_pos == 103)
                            tabdecimal[i] = 'g';
                        else if (oct_pos == 104)
                            tabdecimal[i] = 'h';
                        else if (oct_pos == 105)
                            tabdecimal[i] = 'i';
                        else if (oct_pos == 106)
                            tabdecimal[i] = 'j';
                        else if (oct_pos == 107)
                            tabdecimal[i] = 'k';
                        else if (oct_pos == 108)
                            tabdecimal[i] = 'l';
                        else if (oct_pos == 109)
                            tabdecimal[i] = 'm';
                        else if (oct_pos == 110)
                            tabdecimal[i] = 'n';
                        else if (oct_pos == 111)
                            tabdecimal[i] = 'o';
                        else if (oct_pos == 112)
                            tabdecimal[i] = 'p';
                        else if (oct_pos == 113)
                            tabdecimal[i] = 'q';
                        else if (oct_pos == 114)
                            tabdecimal[i] = 'r';
                        else if (oct_pos == 115)
                            tabdecimal[i] = 's';
                        else if (oct_pos == 116)
                            tabdecimal[i] = 't';
                        else if (oct_pos == 117)
                            tabdecimal[i] = 'u';
                        else if (oct_pos == 118)
                            tabdecimal[i] = 'v';
                        else if (oct_pos == 119)
                            tabdecimal[i] = 'w';
                        else if (oct_pos == 120)
                            tabdecimal[i] = 'x';
                        else if (oct_pos == 121)
                            tabdecimal[i] = 'y';
                        else if (oct_pos == 122)
                            tabdecimal[i] = 'z';
                        else if (oct_pos == 94)
                            tabdecimal[i] = ' ';
                        else
                            tabdecimal[i] = ' ';



                    }

                    str = new string(tabdecimal);

                    //value = Convert.ToDecimal(str);
                    br.Close();
                    fs.Close();
                    break;

                }


            }
            return str;

        }

        //-------------------------------------------------------------------------------------------------------
        public int DeterminerEncodage(string filename) //Permet de déterminer le mode d'encodage du fichier pour connaitre le sens de lecture
        {
            int oct = 0;
            int oct_prec;
            int oct_suiv;
            BinaryReader br = null;// Création d'un object BinaryReader-->Permet de lire un fichier en binaire

            FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read);// Permet d'ouvrir le fichier à partir de son adresse et de le lire
            br = new BinaryReader(fs);// --> Ondécide de lire fs octet par octet

            while (fs.Position < fs.Length) // Tant que le position de l'octet lu est inférieur au dernier octet du fichier on continue la lecture
            {
                //On va lire sur 16 bits l'octet en cours et le précédent.  L'idée est de comparer l'octet lu avec le précédent ( va correspondre au tag)
                //Le tag correspondant au mode d'encodage est le (0002,0010) or en décimal 0002=2 et 0010=16
                oct_prec = oct;
                oct = br.ReadInt16();
                if ((oct_prec == 2) && (oct == 16)) //Lorsque l'on a la condition qui correspond au tag (0002,0010)
                {
                    pos_endian = (int)fs.Position;
                    pos_endian2 = (int)fs.Position;
                    pos_endian = pos_endian + 22;//On met en mémoire la position courante +22 qui correspond à l'octet qui va nous permettre de déterminer le mode d'encodage
                    pos_endian2 = pos_endian2 + 23;//On met en mémoire la position courante +22 qui correspond à l'octet qui va nous permettre de déterminer le mode d'encodage
                }

                if (fs.Position == pos_endian)
                {
                    oct = fs.ReadByte();
                    oct_suiv = oct;

                    if (oct == 46) //équivaut à . en ASCII --> Coder en Little Endian Implicit
                        endianness = 0;
                    else if (oct == 49) // équivaut à 1 en ASCII -->Coder en Little Endian Explicit
                        endianness = 1;
                    else if (oct == 50)
                        endianness = 2; // val de l'octet équivaut à 2  en ASCII -->Coder en Big Endian Explicit
                    else if (oct == 52)
                        endianness = 3; // val de l'octet équivaut à 4 . 80 en ASCII -->Coder en JPEG-LS
                    break;
                }
            }

            fs.Close();// fermeture du fichier courant : TRES IMPORTANT 
            return endianness;
        }
        //-------------------------------------------------------------------------------------------------------
        public int DeterminerFinEntete(string filename, int endianness) // Permet de déterminer le dernier octet avant les pixels de l'image : Permet de gagner du temps lorsque l'on parcourt les images
        {
            int oct = 0;
            int oct_prec;
            BinaryReader br = null;

            if ((endianness == 0) || (endianness == 1))
            {
                FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read);
                br = new BinaryReader(fs);
                while (fs.Position < fs.Length)
                {
                    oct_prec = oct;
                    oct = br.ReadInt16();
                    if ((oct_prec == 32736) && (oct == 16)) // correspond au taf (7FE0,0010) qui correspond au Pixel Data
                    {
                        pos = (int)fs.Position;
                        break;
                    }
                }
                fs.Close();
            }

            if (endianness == 2)
            {
                FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read);

                br = new BinaryReader(fs);
                while (fs.Position < fs.Length)
                {
                    oct_prec = oct;
                    oct = br.ReadInt16();
                    if ((oct_prec == 16) && (oct == 32736))
                    {
                        pos = (int)fs.Position;

                        break;
                    }
                }
                fs.Close();
            }

            return pos;
        }

        //-------------------------------------------------------------------------------------------------------
         public bool Anonymize(string chemin, string nom)
         {
             string[] filename ;
             string PPN = nom;
             string cheminDoss= chemin;           
             string patname = "PPN "+PPN;

             filename = System.IO.Directory.GetFiles(cheminDoss);
             
             Tag pattag = new Tag(0x0010, 0x0010);
             bool ret = true;
             bool val = false;
             for (int i = 0; i < filename.Length; i++)
             {
                 Anonymizer ano = new Anonymizer();
                 Reader reader = new Reader();
                 
                 ret = reader.Read();
                 reader.SetFileName(filename[i]);
                 reader.Read();

                 ano.SetFile(reader.GetFile());

                 ano.Replace(pattag, patname);
                 ano.RemovePrivateTags();
             

                 Writer writer = new Writer();
                 writer.SetFileName(filename[i]);
                 writer.SetFile(ano.GetFile());

                 ret = writer.Write();
                 if (!ret)
                 {
                     ret = false;
                 }
                // return ret;
             }
             val = ret;
             return val;
         }

         //-------------------------------------------------------------------------------------------------------

         public void ValidationAnon(int PPN, MySqlConnection conn, bool valid)
         {
             bool ok = valid;
             //ok = false;
             int val=0;

             if (ok == true)
             {
                 val = 1;
             }
             else val =0;

             MySqlConnection connect;
             connect = conn;
    
             MySqlCommand maCommande = new MySqlCommand("UPDATE  operation SET anonym = @val WHERE ppn_operation="+PPN,connect);
             maCommande.Parameters.Add("@val", MySqlDbType.Int32).Value = val;
             MySqlDataReader reader;
             
             reader = maCommande.ExecuteReader();

         }
    }
}




