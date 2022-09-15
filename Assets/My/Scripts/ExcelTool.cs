using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;
using System.IO;
using System.Text;
using Excel;
using UnityEditor;
using Data;

public class ExcelTool : MonoBehaviour
{
     
	public class ExcelConfig
	{
		/// <summary>
		/// 存放excel表文件夹的的路径，本例xecel表放在了"Assets/Excels/"当中
		/// </summary>
		public static readonly string excelsFolderPath = Application.dataPath + "/Excels/";

		/// <summary>
		/// 存放Excel转化CS文件的文件夹路径
		/// </summary>
		public static readonly string assetPath = "Assets/Resources/DataAssets/";
	}

    public class Excel_Tool
    {
        static DataRowCollection ReadExcel(string filePath, ref int columnNum, ref int rowNum)
        {
            FileStream stream = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);

            DataSet result = excelReader.AsDataSet();
            //Tables[0] 下标0表示excel文件中第一张表的数据
            columnNum = result.Tables[0].Columns.Count;
            rowNum = result.Tables[0].Rows.Count;
            return result.Tables[0].Rows;
        }
        /// <summary>
        /// 读取表数据，生成对应的数组
        /// </summary>
        /// <param name="filePath">excel文件全路径</param>
        /// <returns>Item数组</returns>
        public static Item[] CreateItemArrayWithExcel(string filePath)
        {
            //获得表数据
            int columnNum = 0, rowNum = 0;
            DataRowCollection collect = ReadExcel(filePath, ref columnNum, ref rowNum);

            //根据excel的定义，第二行开始才是数据
            Item[] array = new Item[rowNum - 1];
            for (int i = 1; i < rowNum; i++)
            {
                Item item = new Item();
                //解析每列的数据
                item.itemId = uint.Parse(collect[i][0].ToString());
                item.itemName = collect[i][1].ToString();
                item.itemPrice = uint.Parse(collect[i][2].ToString());
                array[i - 1] = item;
            }
            return array;
        }

        /// <summary>
        /// 读取excel文件内容
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <param name="columnNum">行数</param>
        /// <param name="rowNum">列数</param>
        /// <returns></returns>
        
    }
    public class ExcelBuild : Editor
    {

        [MenuItem("CustomEditor/CreateItemAsset")]
        public static void CreateItemAsset()
        {
            ItemManager manager = ScriptableObject.CreateInstance<ItemManager>();
            //赋值
            manager.dataArray = Excel_Tool.CreateItemArrayWithExcel(ExcelConfig.excelsFolderPath + "Item.xlsx");

            //确保文件夹存在
            if (!Directory.Exists(ExcelConfig.assetPath))
            {
                Directory.CreateDirectory(ExcelConfig.assetPath);
            }

            //asset文件的路径 要以"Assets/..."开始，否则CreateAsset会报错
            string assetPath = string.Format("{0}{1}.asset", ExcelConfig.assetPath, "Item");
            //生成一个Asset文件
            AssetDatabase.CreateAsset(manager, assetPath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }

}
