using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace Ferda.WebService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IFerdaService
    {

        [OperationContract]
        [WebInvoke]
        DataMiningResult MineDataWithPmmlSetUp(DataMiningRequest request);

        /*
        [OperationContract]
        //[WebGet]
        CompositeType GetDataUsingDataContract(CompositeType composite);*/

        // TODO: Add your service operations here
    }

    [DataContract]
    public class DataMiningRequest
    {
        public DataMiningRequest(string dataProvider, string connectionString, string dataTableName, string pmml, string[] primaryKey)
        {
            DataProvider = dataProvider;
            ConnectionString = connectionString;
            DataTableName = dataTableName;
            Pmml = pmml;
			PrimaryKey = primaryKey;
		}

        [DataMember]
        public string DataProvider
        {
            get;
            set;
        }

        [DataMember]
        public string ConnectionString
        {
            get;
            set;
        }

        [DataMember]
        public string DataTableName
        {
            get;
            set;
        }

        [DataMember]
        public string Pmml
        {
            get;
            set;
        }
		
		[DataMember]
        public string[] PrimaryKey
        {
            get;
            set;
        }
    }
    
    [DataContract]
    public class DataMiningResult
    {
        public DataMiningResult(bool success, string errorMessage, string pmml)
        {
            Success = success;
            ErrorMessage = errorMessage;
            Pmml = pmml;
        }

        [DataMember]
        public bool Success
        {
            get;
            set;
        }

        [DataMember]
        public string ErrorMessage
        {
            get;
            set;
        }

        [DataMember]
        public string Pmml
        {
            get;
            set;
        }
    }
}
