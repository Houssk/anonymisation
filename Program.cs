using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.IO;
using System.IO.Compression;
using MySql.Data.MySqlClient;
using System.Security.Permissions;
using gdcm;



namespace AnoDCM
{
    class Program
    {
        public static void Main( string[] args )
        {
            // Déclaration et initialisation des valeurs d'entrées 
            string PPN_plateforme = args[0];
            string PathBegin = args[1];
            string zip = args[2];
            string PathFich = PathBegin + @"\"+ zip;
            int PPN = int.Parse(PPN_plateforme);// Permet de convertir le string en int pour renommer le header du DICOM
            string zipPath = PathFich +".zip" ;

            // Création du dossier où on va décompresser les images du dossier .zip
            System.IO.Directory.CreateDirectory(PathBegin + @"\anonymisation\anonymisation");
         
            string extractPath = PathBegin + @"\anonymisation";
           
            string extract = extractPath + @"\anonymisation";
            // Méthode qui permet la décompression du dossier
            ZipFile.ExtractToDirectory(zipPath, extract);
            System.IO.Directory.SetCurrentDirectory(extractPath);
            // Déclartion du booléen qui va valider l'anonymisation
            bool validation = false;

            // Donne la permission de lire et d'écrire
            FileIOPermission f = new FileIOPermission(PermissionState.None);
            f.AllLocalFiles = FileIOPermissionAccess.Read;
            FileIOPermission fw = new FileIOPermission(PermissionState.None);
            fw.AllFiles = FileIOPermissionAccess.Write;
            FileIOPermission fw2 = new FileIOPermission(PermissionState.None);
            fw2.AllLocalFiles = FileIOPermissionAccess.Write;
            try
            {
                f.Demand();
                fw.Demand();
                fw2.Demand();
            }
            catch
            {

            }

            
            // Déclaration et Initialisation de la classe traitement
            Traitement Anonymisation = new Traitement();

            // Méthode qui va lancer la connxion à la base de données avec la fonction ConnectBDD de la classe Traitement
            MySqlConnection connect = new MySqlConnection();
            connect = Anonymisation.ConnectBDD();


            // Méthode qui va lancer l'anonymisation des DICOM avec la fonction Anonymize de la classe Traitement. Cette focntion retourne un booléen pour valider le bon fonctionnement de celle ci.
           // validation=Anonymisation.Anonymize(extract, PPN_plateforme,0,500);

            validation = Anonymisation.GestionNbFichier(extract, PPN_plateforme);
            
            // Méthode qui va valider l'anonymisation des DICOM et qui va aller écrire dans la base de données la valeur correspondante (1: A fonctionné 0: N'a pas fonctionné)
           if(zip == "mandibule")
           {
               Anonymisation.ValidationAnonM(PPN, connect, validation);
           }
            
            if(zip == "angioscanner")
            {
                Anonymisation.ValidationAnonA(PPN, connect, validation);
            }
   
            // Méthode qui permet de créer un dossier compressé des images DICOM anonymisées
             //ZipFile.CreateFromDirectory(@"\anonymisation", zip+".zip");
           // string ano = extractPath + @"\anonymisation";
             ZipFile.CreateFromDirectory("anonymisation", zip + ".zip");
             System.IO.Directory.Delete(extract,true);
            
        }
    
    }
}

