using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;

namespace PogCounter.Helper
{
    class MetadataReader
    {

        public DataTable ConvertCsvToDataTable(string filePath)
        {
            //reading all the lines(rows) from the file.
            string[] rows = File.ReadAllLines(filePath);

            DataTable dtData = new DataTable();
            string[] rowValues = null;
            DataRow dr = dtData.NewRow();

            //Creating columns
            if (rows.Length > 0)
            {
                var columns = rows[0].Split(',');
                var metadataColumn = new string[4];
                metadataColumn[0] = columns[0]; //textfile name
                metadataColumn[1] = columns[1]; // runtime
                metadataColumn[2] = columns[2]; //link
                var streamTitle = "";
                for (int i = 3; i < columns.Length; i++) 
                {
                    streamTitle += columns[i];
                }
                metadataColumn[3] = streamTitle; //stream title
                foreach (string columnName in metadataColumn)
                    dtData.Columns.Add(columnName);
            }

            //Creating row for each line.(except the first line, which contain column names)
            for (int row = 1; row < rows.Length; row++)
            {
                rowValues = rows[row].Split(',');
                var metadataColumn = new string[4];
                metadataColumn[0] = rowValues[0]; //textfile name
                metadataColumn[1] = rowValues[1]; // runtime
                metadataColumn[2] = rowValues[2]; //link
                var streamTitle = "";
                for (int i = 3; i < rowValues.Length; i++)
                {
                    streamTitle += rowValues[i];
                }
                metadataColumn[3] = streamTitle; //stream title
                dr = dtData.NewRow();
                dr.ItemArray = metadataColumn;
                dtData.Rows.Add(dr);
            }

            return dtData;
        }

        public void ShowData(DataTable dtData)
        {
            if (dtData != null && dtData.Rows.Count > 0)
            {
                foreach (DataColumn dc in dtData.Columns)
                {
                    Console.Write($"**{dc.ColumnName}** | ");
                }
                Console.WriteLine();
                foreach (DataColumn dc in dtData.Columns) 
                {
                    Console.Write(":---: |");
                }

                foreach (DataRow dr in dtData.Rows)
                {
                    foreach (var item in dr.ItemArray)
                    {
                        Console.Write(item.ToString() + " | ");
                    }
                    Console.WriteLine();
                }
            }
        }
    }
}
