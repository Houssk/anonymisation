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
        bool verification1 = false;
        public void Anonymize(string chemin, string nom, int debut, int fin, string[] filename)
        {

            string PPN = nom;
            string cheminDoss = chemin;
            string patname = "PPN " + PPN;

            // SearchOption.AllDirectories permet de gérer tous les répertoires et les sous repertoires pour accéder au dossier contenant les images

            int nbfilename = System.IO.Directory.GetFiles(cheminDoss, "*.*", SearchOption.AllDirectories).Length;

            Tag pattag = new Tag(0x0010, 0x0010);
            bool ret = true;
            bool val = false;



            Console.WriteLine(filename.Length + " debut boucle v5");

            for (int i = debut; i < fin; i++)
            {
                int compteurWrite = 0;
                Anonymizer ano = new Anonymizer();
                Reader reader = new Reader();


                // ret = reader.Read();
                reader.SetFileName(filename[i]);
                reader.Read();
                /*  if (reader.Read()) {
                    comptReadFileT++;
                }
                else
                {
                    comptReadFileF++;
                }*/

                ano.SetFile(reader.GetFile());
                ano.Replace(pattag, patname);
                ano.RemovePrivateTags();


                Writer writer = new Writer();
                writer.SetFileName(filename[i]);
                writer.SetFile(ano.GetFile());

                ret = writer.Write();
                while (!ret)
                {
                    // compteurWrite++;
                    writer = new Writer();
                    writer.SetFileName(filename[i]);
                    writer.SetFile(ano.GetFile());
                    ret = writer.Write();
                    compteurWrite++;
                    if (compteurWrite > 100000)
                    {
                        ret = true;
                        verification1 = true;
                    }
                    //ret = false;

                }
                // return ret;
                Console.WriteLine(compteurWrite);
            }

            val = ret;
            // return val;
        }

        //-------------------------------------------------------------------------------------------------------

        public void ValidationAnonA(int PPN, MySqlConnection conn, bool valid)
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

            MySqlCommand maCommande = new MySqlCommand("UPDATE  operation SET  anonymisationA= @val WHERE ppn_operation=" + PPN, connect); //anonymisationA
            maCommande.Parameters.Add("@val", MySqlDbType.Int32).Value = val;
            MySqlDataReader reader;

            reader = maCommande.ExecuteReader();
            connect.Close();

        }
        public void verification(string nomZip, MySqlConnection conn,int PPN)
        {
            if (verification1 == true)
            {
                if (nomZip == "mandibule")
                {

                    int val = 1;
                    MySqlConnection connect;
                    connect = conn;
                    conn.Open();
                    MySqlCommand maCommande = new MySqlCommand("UPDATE  operation SET  anonymisationOkM= @val WHERE ppn_operation=" + PPN, connect); //anonymisationM
                    maCommande.Parameters.Add("@val", MySqlDbType.Int32).Value = val;
                    MySqlDataReader reader;

                    reader = maCommande.ExecuteReader();
                }
                else if (nomZip == "angioscanner")
                {
                    int val = 1;
                    MySqlConnection connect;
                    connect = conn;
                    conn.Open();

                    MySqlCommand maCommande = new MySqlCommand("UPDATE  operation SET  anonymisationOkA= @val WHERE ppn_operation=" + PPN, connect); //anonymisationM
                    maCommande.Parameters.Add("@val", MySqlDbType.Int32).Value = val;
                    MySqlDataReader reader;

                    reader = maCommande.ExecuteReader();
                }
             //   connect.Close();
            }
            
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

            MySqlCommand maCommande = new MySqlCommand("UPDATE  operation SET  anonymisationM= @val WHERE ppn_operation=" + PPN, connect); //anonymisationM
            maCommande.Parameters.Add("@val", MySqlDbType.Int32).Value = val;
            MySqlDataReader reader;

            reader = maCommande.ExecuteReader();
            connect.Close();

        }



        // Nouvelle fonction pour la gestion du nombre de fichiers contenus dans un dossier
        public bool GestionNbFichier(string chemin, string nom)
        {
            bool val = true;
            string[] filename = new string[0];
            filename = System.IO.Directory.GetFiles(chemin, "*.*", SearchOption.AllDirectories);
            int nbfilename = System.IO.Directory.GetFiles(chemin, "*.*", SearchOption.AllDirectories).Length;

            int nb = nbfilename / 500;
            int modulo = nbfilename % 500;
            if (nb == 0)
            {
                nb = 1;
            }

            else
            {
                if (modulo != 0)
                {
                    nb++;
                }
            }
            // Console.WriteLine("nb" + nb);
            //Console.WriteLine("modulo:" +modulo);

            for (int i = 0; i < (nb * 500); i = i + 500)
            {
                if (nb == 1)
                {
                    Anonymize(chemin, nom, 0, nbfilename, filename);
                }

                else
                {
                    if ((nb * 500) > (i + 500))
                    {
                        Anonymize(chemin, nom, i, i + 500, filename);
                    }
                    else
                    {
                        Anonymize(chemin, nom, i, nbfilename, filename);
                    }
                }
            }

            return val;
        }

    }
}




