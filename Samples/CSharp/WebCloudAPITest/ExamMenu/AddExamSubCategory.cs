﻿using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WebCloud.Api.DS;
using WebCloudAPITest.TestData;


namespace WebCloudAPITest
{
    public partial class AddExamSubCategory : Form
    {
        private ApiTestDataUtil objTestDataUtil;
     


        /// <summary>
        /// 
        /// </summary>
        /// <param name="MaincategoryList"></param>
        /// <param name="obj"></param>
        public AddExamSubCategory(List<object> examInfoList, ApiTestDataUtil obj)
        {
            InitializeComponent();
            objTestDataUtil = obj;          
          
           //Populate treeview
            Populate(trvMainCategory.Nodes, examInfoList);
        }

       
        /// <summary>
        /// 
        /// </summary>
        /// <param name="nodes"></param>
        /// <param name="categorylists"></param>
        protected void Populate(TreeNodeCollection nodes, IList<object> examInfoList)
        {
            if (examInfoList != null)
            {                
                TreeNode parentNode = null;

               //Retrieve Exam Main Category from response objects.
                Newtonsoft.Json.Linq.JArray jsonResponse1 = Newtonsoft.Json.Linq.JArray.Parse(JsonConvert.SerializeObject(examInfoList[0]));
               
                foreach (var mainItem in jsonResponse1)
                {
                    parentNode = new TreeNode();
                    parentNode.Text = (string)mainItem["MainCategoryName"];
                    parentNode.Tag = (string)mainItem["MainCategoryID"];
                    parentNode.ToolTipText = (string)mainItem["MainCategoryDesc"];

                    //Retrieve Exam Sub Category from response objects.
                    Newtonsoft.Json.Linq.JArray jsonResponse2 = Newtonsoft.Json.Linq.JArray.Parse(JsonConvert.SerializeObject(examInfoList[1]));
                   
                    foreach (var subItem in jsonResponse2)
                    {
                        TreeNode childNode = new TreeNode();
                        childNode.Text = (string)subItem["SubCategoryName"];
                        childNode.Tag = (string)subItem["MainCategoryID"];
                        childNode.ToolTipText = (string)subItem["SubCategoryDesc"];

                        if (childNode.Tag.ToString() == parentNode.Tag.ToString())
                        {
                            parentNode.Nodes.Add(childNode);
                            
                        }
                    }

                    nodes.Add(parentNode);
                }

                // set the first  nodes to the Checked state
                trvMainCategory.Nodes[0].Checked = true;
                
            }
        
        }

       
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private  void btnAdd_Click(object sender, EventArgs e)
        {
            SaveExamSubCategory();
           
        }

        /// <summary>
        /// 
        /// </summary>
        private async void SaveExamSubCategory()
        {
            ExamSubCategory examSubCategory = new ExamSubCategory();
            int mainCategoryID = 0;

            //Find tag (id) of node which  is selected.
            mainCategoryID = FindCheckedNodeId(trvMainCategory.Nodes);

            examSubCategory.MainCategoryID = mainCategoryID;
            examSubCategory.SubCategoryName = txtSubCategoryName.Text.ToString();
            examSubCategory.SubCategoryDesc = txtSubDescription.Text.ToString();

            await objTestDataUtil.InvokeAddExamSubCategoriesAPI(examSubCategory);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void trvMainCategory_AfterCheck(object sender, TreeViewEventArgs e)
        {
            // only do it if the node became checked:
            if (e.Node.Checked)
            {
                // for all the nodes in the tree...
                foreach (TreeNode cur_node in e.Node.TreeView.Nodes)
                {
                  
                    if (cur_node != e.Node)
                    {
                        // ... uncheck them
                        cur_node.Checked = false;
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="theNodes"></param>
        /// <returns></returns>
        private int FindCheckedNodeId(System.Windows.Forms.TreeNodeCollection theNodes)
        {
            int nodeId = 0;

            if (theNodes != null)
            {
                foreach (System.Windows.Forms.TreeNode aNode in theNodes)
                {
                    if (aNode.Checked)
                    {
                        nodeId=Convert.ToInt32( aNode.Tag);
                        return nodeId;
                    }
                   
                                   

                }
            }

            return nodeId;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
