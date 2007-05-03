using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Reflection;
using System.Xml;
using NHibernate;
using NHibernate.Mapping.Attributes;
using Configuration=NHibernate.Cfg.Configuration;

namespace Csla.NHibernate
{
    /// <summary>
    /// Class to provide standard factory configuration functions around the NHibernate framework.
    /// </summary>
    /// <remarks>
	/// This class is named so it looks similar to the <c>NHibernate.Cfg</c> namespace.
    /// </remarks>
    public class Cfg
    {
        #region fields

        // Declared as static (for singleton pattern)
		private static Dictionary<string, DatabaseConfiguration> _lookupTable;

        #endregion

        #region constructor

        /// <summary>Direct construction not allowed. Use the factory method.</summary>
		private Cfg()
        {
        }

        #endregion

        #region properties

        private static Dictionary<string, DatabaseConfiguration> LookupTable
        {
            get
            {
                if (ReferenceEquals(_lookupTable, null))
                    _lookupTable = new Dictionary<string, DatabaseConfiguration>();
                return _lookupTable;
            }
        }

        private DatabaseConfiguration this[string index]
        {
            get
            {
                CheckKeyIsValid(index);
                CheckKeyInTable(index);
                return LookupTable[index];
            }
        }

        #endregion

        #region non-public helpers

        private static void CheckKeyIsValid(string key)
        {
			if (String.IsNullOrEmpty(key))
				throw new ArgumentException(String.Format("Key '{0}' cannot be null or empty.", key));
        }

        private static void CheckKeyInTable(string key)
        {
            if (!LookupTable.ContainsKey(key))
                AddNewKey(key);
        }

        /// <summary>
        /// Adds a new key, if it exists in the application configuration file.
        /// </summary>
        /// <param name="key">String identifier for the session factory required.</param>
        /// <exception cref="ConfigurationErrorsException">If the specified session factory key is invalid.</exception>
        private static void AddNewKey(string key)
        {
            // Get the name of the hibernate.cfg.xml file for the required key
			string factoryCfg = ConfigurationManager.AppSettings[key];

            if (String.IsNullOrEmpty(factoryCfg))
                throw new ConfigurationErrorsException(String.Format("Key '{0}' cannot be null or empty",key));

            // Path it to point to the correct working directory
            factoryCfg = File.GetNFSConfigFile(factoryCfg);

            // Now build the required NHibernate database configuration and add it to the lookup table
			DatabaseConfiguration nhibernateConfig = DatabaseConfiguration.NewDatabaseConfiguration(key);
            nhibernateConfig.ConnectionString = BuildConnectionString(factoryCfg);
            nhibernateConfig.SessionFactory = BuildSessionFactory(factoryCfg);

            LookupTable.Add(key, nhibernateConfig);
        }

        private static string BuildConnectionString(string factoryCfg)
        {
            string connectionString = null;

            // Get the connection string from the configuration file
            using (XmlReader xmlReader = XmlReader.Create(factoryCfg))
            {
                xmlReader.MoveToContent();

                // Find the first "property" node
                if (xmlReader.ReadToFollowing("property"))
                {
                	bool readRequired;
                    do
                    {
                    	string nameAttribute;
                    	nameAttribute = xmlReader.GetAttribute("name");

                        // If we've found the one we need, set the connection string (no more reads needed)
                        if (nameAttribute == "connection.connection_string")
                        {
                            connectionString = xmlReader.ReadString();
                            readRequired = false;
                        }
                        else
                        {
                            // Get the next "property" node (if one exists)
                            readRequired = xmlReader.ReadToNextSibling("property");
                        }
                    } while (readRequired);
                }
            }

            return connectionString;
        }

        private static ISessionFactory BuildSessionFactory(string factoryCfg)
        {
            MemoryStream stream = new MemoryStream(); // where the xml will be written
            HbmSerializer.Default.Validate = true; // Enable validation (optional)

            // Set any specific values for the <hibernate-mapping> element here

            // Set the 'default-access' attribute
            // This matches our naming convention for member fields
            HbmSerializer.Default.HbmDefaultAccess = "field.camelcase-underscore";

            // Set the 'auto-import' attribute off
            // This forces NHibernate to use fully qualified class names (i.e. full namespace prefixes)
            HbmSerializer.Default.HbmAutoImport = false;

            // Here, we serialize all decorated classes
            GetNhibernateClasses(stream);

            stream.Position = 0; // Rewind

            // TODO: Implement a better method (i.e. configurable) of getting the resulting file out
#if DEBUG
            try
            {
                stream.WriteTo(new FileStream(@"ProjectTracker.hbm.xml", FileMode.Create));
            }
            catch (UnauthorizedAccessException e)
            {
                //user trying to access the path is probably coming from IIS and it does not have permission to create the file
                //to catch it and carry on
                e.ToString(); //prevent r# warning
            }
#endif

			Configuration configuration = new Configuration();;
			configuration.Configure(factoryCfg);
			configuration.AddInputStream(stream); // Use the stream here
            stream.Close();

            // Now use the configuration to build the Session Factory
			return configuration.BuildSessionFactory();
        }

        private static void GetNhibernateClasses(MemoryStream stream)
        {
            XmlTextWriter _xmlTextWriter = null;

            // Get the list of all assemblies
            string[] assemblyList;
			//assemblyList = File.GetNFSAssemblyListInBaseDirectory(true);
			assemblyList = File.GetFileListInBaseDirectory("ProjectTracker*.dll", true);


            // Target all classes marked up for NHibernate with the [ClassAttribute]
            Type targetType = typeof (ClassAttribute);

            //  Iterate thru the assemblies to extract all classes marked up for NHibernate
            foreach (string assembly in assemblyList)
            {
				// Get a list of all the marked up classes (as types)
				List<Type> typeList = GetClassesWithCustomAttributes(targetType, assembly);

                // Iterate all types adding them to the serialization
                foreach (Type classType in typeList)
                {
                    _xmlTextWriter = HbmSerializer.Default.Serialize(stream, classType, _xmlTextWriter, false);
                }
            }

			// If the reference is null it means nothing was found with markup for NHibernate
			if (ReferenceEquals(_xmlTextWriter, null))
				throw new NullReferenceException("No classes found marked up for NHibernate");

            _xmlTextWriter.WriteEndElement();
            _xmlTextWriter.WriteEndDocument();
            _xmlTextWriter.Flush();
        }

    	/// <summary>
    	/// Get a list of classes in an assembly that are marked with a particular custom attribute.
    	/// </summary>
    	/// <param name="targetType">The target custom attribute to search for.</param>
    	/// <param name="assemblyString">The name of the assembly.</param>
    	/// <returns>A <see cref="List{T}"/> of <see cref="Type"/> objects.</returns>
		private static List<Type> GetClassesWithCustomAttributes(Type targetType, string assemblyString)
    	{
			// This will hold the types found
			List<Type> typeList = new List<Type>();
			
			// Load the assembly for reflection purposes only
    		Assembly assembly = Assembly.LoadFrom(assemblyString);

    		// Now get the classes in the assembly
			try
			{
			foreach (Type type in assembly.GetTypes())
    		{
				if (type.IsClass)
				{
					object[] objArray = type.GetCustomAttributes(targetType, true);
					if (objArray.Length > 0)
						typeList.Add(type);
				}
    		}				
			}
			catch (ReflectionTypeLoadException ex)
    		{
    			ex.ToString();
    		}


    		return typeList;
    	}

    	#endregion

        #region public static methods

        /// <summary>
        /// Gets an NHibernate database connection string for the specified key.
        /// </summary>
        /// <param name="key">String identifier for the database required.</param>
        /// <returns>An ADO.NET connection string.</returns>
        /// <exception cref="ArgumentNullException">If the key is null.</exception>
        /// <exception cref="ArgumentException">If the key is empty.</exception>
        /// <exception cref="System.Configuration.ConfigurationErrorsException">If the key is a valid string, but it does not appear in the application configuration file.</exception>
        /// <exception cref="System.IO.FileNotFoundException">If the required NHibernate configuration file does not exist for the specified key.</exception>
        public static string GetConnectionString(string key)
        {
			Cfg cfg = new Cfg();
            DatabaseConfiguration databaseConfiguration = cfg[key];
            return databaseConfiguration.ConnectionString;
        }

        /// <summary>
        /// Gets an NHibernate session factory for the specified key.
        /// </summary>
        /// <param name="key">String identifier for the database required.</param>
        /// <returns>An instance object that implements the NHibernate ISessionFactory interface.</returns>
        /// <exception cref="ArgumentNullException">If the key is null.</exception>
        /// <exception cref="ArgumentException">If the key is empty.</exception>
        /// <exception cref="System.Configuration.ConfigurationErrorsException">If the key is a valid string, but it does not appear in the application configuration file.</exception>
        /// <exception cref="System.IO.FileNotFoundException">If the required NHibernate configuration file does not exist for the specified key.</exception>
        public static ISessionFactory GetSessionFactory(string key)
        {
			Cfg cfg = new Cfg();
			DatabaseConfiguration databaseConfiguration = cfg[key];
            return databaseConfiguration.SessionFactory;
        }

        #endregion
    }
}