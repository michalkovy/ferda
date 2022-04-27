using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Text;

namespace Ferda.WebService
{
    public partial class FerdaCaller : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        protected void CallButton_Click(object sender, EventArgs e)
        {
            StringBuilder pmmlBuilder = new StringBuilder();

            System.IO.Stream stream = PmmlUpload.FileContent;
            // Get the length of the file.
            int fileLen = PmmlUpload.PostedFile.ContentLength;

            // Create a byte array to hold the contents of the file.
            Byte[] buffer = new Byte[fileLen];

            stream.Read(buffer, 0, fileLen);

            // Copy the byte array to a string.
            for (int loop1 = 0; loop1 < fileLen; loop1++)
            {
                pmmlBuilder.Append(buffer[loop1].ToString());
            }

            FerdaService service = new FerdaService();
            DataMiningResult result = service.MineDataWithPmmlSetUp(
                new DataMiningRequest(DataProvider.Text,
                    ConnectionString.Text,
                    DataTableName.Text,
                    pmmlBuilder.ToString()
                    ));
            Result.Text = result.ErrorMessage;
        }
    }
}