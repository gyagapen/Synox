using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using Synox.Helpers;
using System.Data;
using System.Data.SqlClient;

namespace Synox.Services.ServiceSMS.Donnees
{
    internal class SqlServer
    {
        private String _connectionString;
        public SqlConnection Connexion { get; private set; }
        public SqlCommand Commande { get; private set; }

        /// <summary>
        /// Objet de connexion à la base de données
        /// </summary>
        /// <param name="connectionString"></param>
        public SqlServer(String connectionString)
        {
            _connectionString = connectionString;
        }
        #region Ouverture/Fermture Connexion
        /// <summary>
        /// Ouverture de la connexion à la base de données
        /// </summary>
        /// <returns></returns>
        public bool Open()
        {
            try
            {
                if (Connexion == null)
                    Connexion = new SqlConnection(_connectionString);

                if (Connexion.State != ConnectionState.Open)
                {
                    Connexion = new SqlConnection(_connectionString);
                    Connexion.Open();
                }
                return true;
            }
            catch(Exception e)
            {
                LogHelper.Trace("SqlServer.Open : " + e.Message, LogHelper.EnumCategorie.Erreur);
                return false;
            }
        }
        /// <summary>
        /// Fermeture de la connexion et libération de l'objet de connexion
        /// </summary>
        public void Close()
        {
            if (Connexion != null)
            {
                Connexion.Close();
                Connexion.Dispose();
                Connexion = null;
            }
        }
        #endregion

        public SqlDataReader GetReader(string storedProcedureName, Hashtable parametres=null)
        {
            SqlDataReader reader = null;
            Commande = new SqlCommand();
            if (!Open())
                return null;
            Commande.Connection = Connexion;
            Commande.CommandText = storedProcedureName;
            Commande.CommandType = CommandType.StoredProcedure;

            if (parametres != null)
            {
                foreach (DictionaryEntry DE in parametres)
                {
                    if (DE.Value != null)
                        Commande.Parameters.AddWithValue(DE.Key.ToString(), DE.Value.ToString());
                }
            }


            reader = Commande.ExecuteReader();

            return reader;
        }

        internal int ExecuteQuery(string requete, CommandType commandType = CommandType.Text)
        {
            int numberRowsAffected = 0;
            Commande = new SqlCommand();
            if (!Open())
                return -1;
            Commande.Connection = Connexion;
            Commande.CommandText = requete;
            Commande.CommandType = commandType;

            numberRowsAffected = Commande.ExecuteNonQuery();
            Commande.Dispose();
            Commande = null;

            return numberRowsAffected;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="storedProcedureName"></param>
        /// <param name="parametres"></param>
        /// <returns></returns>
        internal int ExecuteSp(string storedProcedureName, Hashtable parametres=null, CommandType commandType= CommandType.StoredProcedure)
        {
            int numberRowsAffected = 0;
            Commande = new SqlCommand();
            if (!Open())
                return -1;
            Commande.Connection = Connexion;
            Commande.CommandText = storedProcedureName;
            Commande.CommandType = commandType;

            Commande.Parameters.Clear();
            if (parametres != null)
            {
                foreach (DictionaryEntry DE in parametres)
                {
                    if (DE.Value != null)
                        Commande.Parameters.AddWithValue(DE.Key.ToString(), DE.Value);
                }
            }
            numberRowsAffected = Commande.ExecuteNonQuery();
            Commande.Dispose();
            Commande = null;

            return numberRowsAffected;
        }


    }
}
