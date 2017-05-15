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
using gdcm;



namespace AnoDCM
{
    class Traitement
    {

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
             builder.UserID = "root";              // id de l'utilisateur                                           VERSION SERVEUR
             builder.Password = "On30rth0M3d!cal";                // mot de passe

             /*builder.DataSource = "127.0.0.1";     // adresse du serveur
             builder.InitialCatalog = "globald";   // nom bdd
             builder.UserID = "root";              // id de l'utilisateur                                           VERSION TEST
             builder.Password = "";                // mot de passe*/

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

         public bool Anonymize(string chemin, string nom)
         {
             string[] filename ;
             string PPN = nom;
             string cheminDoss= chemin;           
             string patname = "PPN "+PPN;

             filename = System.IO.Directory.GetFiles(cheminDoss,"*.*", SearchOption.AllDirectories);
             
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

         public void ValidationAnonA(int PPN, MySqlConnection conn, bool valid)
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
    
             MySqlCommand maCommande = new MySqlCommand("UPDATE  operation SET anonymisationA = @val WHERE ppn_operation="+PPN,connect);
             maCommande.Parameters.Add("@val", MySqlDbType.Int32).Value = val;
             MySqlDataReader reader;
             
             reader = maCommande.ExecuteReader();

         }
         public void ValidationAnonM(int PPN, MySqlConnection conn, bool valid)
         {
             bool ok = valid;
             //ok = false;
             int val = 0;

             if (ok == true)
             {
                 val = 1;
             }
             else val = 0;

             MySqlConnection connect;
             connect = conn;

             MySqlCommand maCommande = new MySqlCommand("UPDATE  operation SET anonymisationM = @val WHERE ppn_operation=" + PPN, connect);
             maCommande.Parameters.Add("@val", MySqlDbType.Int32).Value = val;
             MySqlDataReader reader;

             reader = maCommande.ExecuteReader();

         }
    }
}




