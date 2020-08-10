using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using OpcEnumLib;
using opcproxy;
using System.Threading;
using System.IO;

namespace RsLinx_OPC_Client
{
    public partial class Form1 : Form
    {
        #region Variables for OPC client

        private Opc.URL url;
        private Opc.Da.Server server;
        private OpcCom.Factory fact = new OpcCom.Factory();
        private Opc.Da.Subscription groupRead;
        private Opc.Da.Subscription groupWrite;
        private Opc.Da.SubscriptionState groupState;
        private Opc.Da.SubscriptionState groupStateWrite;
        private Opc.Da.Item item;
        private List<Opc.Da.Item> itemsList = new List<Opc.Da.Item>();
        public static Form1 myForm;


        ListView.SelectedIndexCollection selColmy;
        public static String itemValueForJson;
        public static String itemIndexForJson;
        public static String itemIndexForJsonUp;
        public static String itemNameForJson;
        public static Boolean tryToExit = false;

        public IConnectionPoint m_pDataCallback;
        object m_pIfaceObj;
        int m_dwCookie;
        uint m_hGroup;

        #endregion

        //initialization of the sample object that contains opc values
        MyOPCObject myOpcObject = new MyOPCObject();

        public Form1()
        {
            InitializeComponent();
            myForm = this;
        }


        private int ShowRegisteredServers()
        {
            OpcServerList pServerList = new OpcServerList();
            // Идентификатор категории ОРС DA 2.0
            Guid clsidcat = new Guid("{63D5F432-CFE4-11D1-B2C8-0060083BA1FB}");
            IOPCEnumGUID pIOPCEnumGuid;
            try
            {
                pServerList.EnumClassesOfCategories(1, ref clsidcat, 0, ref clsidcat, out pIOPCEnumGuid);

                string pszProgID; // буфер для записи ProgID серверов
                string pszUserType; // буфер для записи описания серверов
                string pszVerIndProgID;
                Guid guid = new Guid();
                int nServerCnt = 0;
                uint iRetSvr; // количество серверов, предоставленных запросом 
                              // получение первого доступного идентификатора сервера 
                pIOPCEnumGuid.Next(1, out guid, out iRetSvr);

                while (iRetSvr != 0)
                {
                    nServerCnt++;
                    pServerList.GetClassDetails(ref guid, out pszProgID, out pszUserType, out pszVerIndProgID);
                    ListViewItem lvItem = new ListViewItem();
                    lvItem.Tag = (object)guid;
                    lvItem.Text = pszProgID;
                    m_listOPCServers.Items.Add(lvItem);
                    pIOPCEnumGuid.Next(1, out guid, out iRetSvr);
                }

                return nServerCnt;
            }
            catch (ApplicationException e)
            {
                return -1;
            }

        }

        protected void OnLoadForm(object sended, EventArgs e)
        {
            if (0 == ShowRegisteredServers())
            {
                MessageBox.Show("Нет установленных серверов");
            }
        }

        private void DisplayChildren(TreeNode ParentNode, string szItemID, IOPCBrowse pBrowse)
        {
            uint uiProp = 0;
            int bMoreElements;
            string ContinuationPoint = "";
            uint dwCount;
            IntPtr pBrowseElements = IntPtr.Zero;
            pBrowse.Browse(szItemID, ref ContinuationPoint, 0, tagOPCBROWSEFILTER.OPC_BROWSE_FILTER_ITEMS, "", "", 1, 1,
                            0, ref uiProp, out bMoreElements, out dwCount, out pBrowseElements);
            tagOPCBROWSEELEMENT[] OPCItemElements = new tagOPCBROWSEELEMENT[dwCount];
            int sz = Marshal.SizeOf(typeof(tagOPCBROWSEELEMENT));
            TreeNode tvNode;
            for (int i = 0; i < dwCount; i++)
            {

                OPCItemElements[i] = (tagOPCBROWSEELEMENT)Marshal.PtrToStructure(IntPtr.Add(pBrowseElements, i * sz), typeof(tagOPCBROWSEELEMENT));
                if (null == ParentNode)
                    tvNode = m_treeOPCServerBrowse.Nodes.Add(OPCItemElements[i].szName);
                else
                    tvNode = ParentNode.Nodes.Add(OPCItemElements[i].szName);
                tvNode.Tag = OPCItemElements[i].szItemID;
            }

            pBrowse.Browse(szItemID, ref ContinuationPoint, 0, tagOPCBROWSEFILTER.OPC_BROWSE_FILTER_BRANCHES, "", "", 1, 1,
                            0, ref uiProp, out bMoreElements, out dwCount, out pBrowseElements);

            tagOPCBROWSEELEMENT[] OPCBranchElements = new tagOPCBROWSEELEMENT[dwCount];

            for (int i = 0; i < dwCount; i++)
            {
                OPCBranchElements[i] = (tagOPCBROWSEELEMENT)Marshal.PtrToStructure(IntPtr.Add(pBrowseElements, i * sz), typeof(tagOPCBROWSEELEMENT));

                if (null == ParentNode)
                    tvNode = m_treeOPCServerBrowse.Nodes.Add(OPCBranchElements[i].szName);
                else
                    tvNode = ParentNode.Nodes.Add(OPCBranchElements[i].szName);

                DisplayChildren(tvNode, OPCBranchElements[i].szName, pBrowse);
            }
        }

        private void DisplayChildren(TreeNode ParentNode, IOPCBrowseServerAddressSpace pParent)
        {
            opcproxy.IEnumString pEnum;
            uint cnt;
            string strName;
            string szItemID;

            pParent.BrowseOPCItemIDs(tagOPCBROWSETYPE.OPC_LEAF, "", (ushort)VarEnum.VT_EMPTY, 0, out pEnum);
            pEnum.RemoteNext(1, out strName, out cnt);

            while (cnt != 0)
            {
                TreeNode tvNode;
                if (ParentNode == null)
                    tvNode = m_treeOPCServerBrowse.Nodes.Add(strName);
                else
                    tvNode = ParentNode.Nodes.Add(strName);
                pParent.GetItemID(strName, out szItemID); // получает полный идентификатор тега
                tvNode.Tag = (object)szItemID;
                pEnum.RemoteNext(1, out strName, out cnt);
            }
            pParent.BrowseOPCItemIDs(tagOPCBROWSETYPE.OPC_BRANCH, "", 1, 0, out pEnum);
            pEnum.RemoteNext(1, out strName, out cnt);
            while (cnt != 0)
            {
                TreeNode tvNode = new TreeNode(strName);
                if (ParentNode == null)
                    tvNode = m_treeOPCServerBrowse.Nodes.Add(strName);
                else
                    tvNode = ParentNode.Nodes.Add(strName);
                pParent.ChangeBrowsePosition(tagOPCBROWSEDIRECTION.OPC_BROWSE_DOWN, strName);
                DisplayChildren(tvNode, pParent);
                pParent.ChangeBrowsePosition(tagOPCBROWSEDIRECTION.OPC_BROWSE_UP, strName);
                pEnum.RemoteNext(1, out strName, out cnt);
            }
        }

        private void OnServerChange()
        {
            if (m_dwCookie != 0)
            {
                m_pDataCallback.Unadvise(m_dwCookie);
                m_dwCookie = 0;
            }
            if (m_hGroup != 0)
            {
                IOPCServer pServer = (IOPCServer)m_pIfaceObj;
                pServer.RemoveGroup(m_hGroup, 1);
                m_hGroup = 0;
            }

        }


        private void ConnectAndBrowseServer(Guid guid)
        {
            try
            {
                Type typeOfServer = Type.GetTypeFromCLSID(guid);
                m_pIfaceObj = Activator.CreateInstance(typeOfServer);
                if (m_pIfaceObj is IOPCBrowse)
                {
                    IOPCBrowse pBrowse = (IOPCBrowse)m_pIfaceObj;
                    DisplayChildren(null, "", pBrowse);
                }

                else
                    if (m_pIfaceObj is IOPCBrowseServerAddressSpace)
                {
                    IOPCBrowseServerAddressSpace pBrowse = (IOPCBrowseServerAddressSpace)m_pIfaceObj;
                    DisplayChildren(null, pBrowse);
                }

            }
            catch (ApplicationException ex)
            {
                MessageBox.Show(ex.Message, "Ошибка");
            }

        }


        private void OkButton_Click(object sender, EventArgs e)
        {
            // 1st: Create a server object and connect to the RSLinx OPC Server
            url = new Opc.URL("opcda://localhost/" + m_listOPCServers.SelectedItems[0].Text.ToString());

            server = new Opc.Da.Server(fact, null);

            //2nd: Connect to the created server
            server.Connect(url, new Opc.ConnectData(new System.Net.NetworkCredential()));

            //3rd Create a group if items            
            groupState = new Opc.Da.SubscriptionState();
            groupState.Name = "Group";
            groupState.UpdateRate = 1000;// this is the time between every reads from OPC server
            groupState.Active = true;//this must be true if you the group has to read value
            groupRead = (Opc.Da.Subscription)server.CreateSubscription(groupState);
            groupRead.DataChanged += new Opc.Da.DataChangedEventHandler(group_DataChanged);//callback when the data are readed


            if (m_treeOPCServerBrowse.SelectedNode == null) return;
            string itemID = (string)m_treeOPCServerBrowse.SelectedNode.Tag;


            // add items to the group    (in Rockwell names are identified like [Name of PLC in the server]Block of word:number of word,number of consecutive readed words)        
            Opc.Da.Item item = new Opc.Da.Item();
            item.ItemName = itemID;
            itemsList.Add(item);

            m_valueView.Invoke(new EventHandler(delegate {
                ListViewItem lvItem = new ListViewItem();
                ListViewItem.ListViewSubItem[] lvSubItem = new ListViewItem.ListViewSubItem[1];
                lvItem.Text = item.ItemName;
                lvItem.Name = item.ItemName;
                lvItem.SubItems.Add("null");
                m_valueView.Items.Add(lvItem);
            }));

            itemIndexForJson = m_valueView.Items[item.ItemName].Index.ToString();

            if (!tryToExit)
            {
                (new System.Threading.Thread(delegate ()
                {
                    JsonSender.RunAsync(itemIndexForJson, item.ItemName, "null").GetAwaiter().GetResult();
                })).Start();
                Thread.Sleep(1000);
            }



            //item = new Opc.Da.Item();
            //item.ItemName = "Random.Boolean";
            // itemsList.Add(item);

            // item = new Opc.Da.Item();
            // item.ItemName = ".myValue";
            // itemsList.Add(item);



            groupRead.AddItems(itemsList.ToArray());


            // Create a write group            
            groupStateWrite = new Opc.Da.SubscriptionState();
            groupStateWrite.Name = "Group Write";
            groupStateWrite.Active = false;//not needed to read if you want to write only
            groupWrite = (Opc.Da.Subscription)server.CreateSubscription(groupStateWrite);

           // (new System.Threading.Thread(delegate () {
           //     JsonSender.infinityScan().GetAwaiter().GetResult();
           // })).Start();
        }


        void group_DataChanged(object subscriptionHandle, object requestHandle, Opc.Da.ItemValueResult[] values)
        {

            //m_valueView.Invoke(new EventHandler(delegate {
            // m_valueView.Items.Clear();
            //}));

            foreach (Opc.Da.ItemValueResult itemValue in values)
            {
                
                m_valueView.Invoke(new EventHandler(delegate {

                    itemIndexForJsonUp = m_valueView.Items[itemValue.ItemName].Index.ToString();
                    itemValueForJson = Convert.ToSingle(itemValue.Value).ToString();
                    itemNameForJson = itemValue.ItemName;

                    m_valueView.Items[itemValue.ItemName].SubItems[1].Text = Form1.itemValueForJson;

                    }));
                if (!tryToExit)
                {
                    (new System.Threading.Thread(delegate ()
                    {
                        JsonSender.RunAsync(itemIndexForJsonUp, itemNameForJson, itemValueForJson).GetAwaiter().GetResult();
                    })).Start();
                }

                /*switch (itemValue.ItemName)
                {
                    case itemIDmy:
                        //motorSpeed = Convert.ToInt32(itemValue.Value);
                        // label1.Invoke(new EventHandler(delegate { label1.Text = Convert.ToInt32(itemValue.Value).ToString(); }));

                        m_valueView.Invoke(new EventHandler(delegate {
                        ListViewItem lvItem = new ListViewItem();
                        ListViewItem.ListViewSubItem[] lvSubItem = new ListViewItem.ListViewSubItem[3];
                        lvItem.Text = itemID;

                        lvItem.SubItems.Add(ToStringConverter.GetVTString(pResults.vtCanonicalDataType));
                        lvItem.SubItems.Add(pItemState.vDataValue.ToString());
                        lvItem.SubItems.Add(ToStringConverter.GetFTSting(pItemState.ftTimeStamp));
                        lvItem.SubItems.Add(ToStringConverter.GetQualityString(pItemState.wQuality));
                        }));

                        break;

                    case "Random.Boolean":
                        //motorActive = Convert.ToBoolean(itemValue.Value);
                       // label2.Invoke(new EventHandler(delegate { label2.Text = Convert.ToBoolean(itemValue.Value).ToString(); }));
                        break;

                    case ".myValue":
                       // label3.Invoke(new EventHandler(delegate { label3.Text = Convert.ToSingle(itemValue.Value).ToString(); }));
                        JsonSender.value = Convert.ToSingle(itemValue.Value).ToString();

                        // (new System.Threading.Thread(delegate () {
                        //     JsonSender.RunAsync().GetAwaiter().GetResult();
                        // })).Start();

                        break;

                }*/
            }
        }





        private void OPCListClick(object sender, EventArgs e)
        {

            ListView.SelectedIndexCollection selCol = m_listOPCServers.SelectedIndices;
            selColmy = selCol;
            if (selCol.Count == 0)
                return;
            m_treeOPCServerBrowse.Nodes.Clear();
            Guid guid = (Guid)m_listOPCServers.SelectedItems[0].Tag;
            OnServerChange();
            ConnectAndBrowseServer(guid);
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            //MessageBox.Show(m_listOPCServers.SelectedItems[0].Text.ToString());

            //if (m_treeOPCServerBrowse.SelectedNode == null) return;
            //string itemID = (string)m_treeOPCServerBrowse.SelectedNode.Tag;
            //MessageBox.Show(itemID);
            tryToExit = true;
            (new System.Threading.Thread(delegate () {
                JsonSender.RunAsync("null", "null", "null").GetAwaiter().GetResult();
            })).Start();
            
            export2File(historyList);
            MessageBox.Show("теперь нажмите крестик");
        }

        private void export2File(ListView lv)
        {
            string filename = "";
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Title = "сохранение файла выдачи команд управления";
            sfd.Filter = "CommandLogs (.txt)| *.txt";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                filename = sfd.FileName.ToString();
                if (filename != "")
                {
                        using (StreamWriter sw = new StreamWriter(filename))
                    {
                        foreach (ListViewItem lvI in lv.Items)
                        {
                            foreach (ListViewItem.ListViewSubItem lvISub in lvI.SubItems)
                                sw.WriteLine("{0}\t", lvISub.Text);
                        }
                     }
                };
            }
        }


        /*
        private void btnConnect_Click(object sender, EventArgs e)
        {
            // 1st: Create a server object and connect to the RSLinx OPC Server
            url = new Opc.URL("opcda://localhost/Matrikon.OPC.Simulation.1");

            server = new Opc.Da.Server(fact, null);

            //2nd: Connect to the created server
            server.Connect(url, new Opc.ConnectData(new System.Net.NetworkCredential()));

            //3rd Create a group if items            
            groupState = new Opc.Da.SubscriptionState();
            groupState.Name = "Group";
            groupState.UpdateRate = 1000;// this isthe time between every reads from OPC server
            groupState.Active = true;//this must be true if you the group has to read value
            groupRead = (Opc.Da.Subscription)server.CreateSubscription(groupState);
            groupRead.DataChanged += new Opc.Da.DataChangedEventHandler(group_DataChanged);//callback when the data are readed

            // add items to the group    (in Rockwell names are identified like [Name of PLC in the server]Block of word:number of word,number of consecutive readed words)        
                Opc.Da.Item item = new Opc.Da.Item();
                item.ItemName = "Random.Int2";
                itemsList.Add(item);

                item = new Opc.Da.Item();
                item.ItemName = "Random.Boolean";
                itemsList.Add(item);

                item = new Opc.Da.Item();
                item.ItemName = ".myValue";
                itemsList.Add(item);

            

            groupRead.AddItems(itemsList.ToArray());


            // Create a write group            
            groupStateWrite = new Opc.Da.SubscriptionState();
            groupStateWrite.Name = "Group Write";
            groupStateWrite.Active = false;//not needed to read if you want to write only
            groupWrite = (Opc.Da.Subscription)server.CreateSubscription(groupStateWrite);

            (new System.Threading.Thread(delegate () {
                JsonSender.infinityScan().GetAwaiter().GetResult();
            })).Start();
        }


        void group_DataChanged(object subscriptionHandle, object requestHandle, Opc.Da.ItemValueResult[] values)
        {
            foreach (Opc.Da.ItemValueResult itemValue in values)
            {
                switch (itemValue.ItemName)
                {
                    case "Random.Int2":
                        //motorSpeed = Convert.ToInt32(itemValue.Value);
                        label1.Invoke(new EventHandler(delegate { label1.Text = Convert.ToInt32(itemValue.Value).ToString(); }));
                        break;

                    case "Random.Boolean":
                        //motorActive = Convert.ToBoolean(itemValue.Value);
                        label2.Invoke(new EventHandler(delegate { label2.Text = Convert.ToBoolean(itemValue.Value).ToString(); }));
                        break;

                    case ".myValue":
                        label3.Invoke(new EventHandler(delegate { label3.Text = Convert.ToSingle(itemValue.Value).ToString(); }));
                        JsonSender.value= Convert.ToSingle(itemValue.Value).ToString();
                        
                       // (new System.Threading.Thread(delegate () {
                       //     JsonSender.RunAsync().GetAwaiter().GetResult();
                       // })).Start();

                        break;
                      
                }
            }
        }





        //it's good to remember that if you update a variable every 1 second, this will be a good timer to use also for writings
        /*void group_DataChanged(object subscriptionHandle, object requestHandle, Opc.Da.ItemValueResult[] values)
        {
            for (int i = 0; i < values.Length; i++)
            {
                short[] receivedData = (short[])values[i].Value;
                if (values[i].ItemName == "Random.Int2")
                {
                    myOpcObject.DataN7 = receivedData;
                    //remember that it's in another thread (so if you want to update the UI you should use anonyms methods)
                    lblN7Read.Invoke(new EventHandler(delegate { lblN7Read.Text = myOpcObject.DataN7[0].ToString(); }));
                    label1.Invoke(new EventHandler(delegate { label1.Text = myOpcObject.DataN7[1].ToString(); }));
                }
                else if (values[i].ItemName == "Random.Int4")
                {
                    myOpcObject.DataN11 = receivedData;
                    label2.Invoke(new EventHandler(delegate { label2.Text = myOpcObject.DataN11[3].ToString(); }));
                }
                else if (values[i].ItemName == "[UNTITLED_1]B3:0,L2")
                {
                    myOpcObject.BitsB3 = receivedData;
                }
                else if (values[i].ItemName == "[UNTITLED_1]B10:0")
                {
                    myOpcObject.BitsB10 = receivedData;
                }
            }
        }*/

        /// <summary>
        ///  Rockwell identifies their words with N7:0, N7:1, N11:5 and so on
        /// They identifies their bits with B3:0/0 B10:1/14 and so on
        /// </summary>
        /// <param name="block">it means "N7"</param>
        /// <param name="wordNumber">it means the number after ":"</param>
        /// <param name="value">it's the value that we want to write in the word (it's a short, 16 bits)</param>
        /*private void WriteWord(string block, short wordNumber, short value)
        {
            //Create the item to write (if the group doesn't have it, we need to insert it)
            Opc.Da.Item[] itemToAdd = new Opc.Da.Item[1];
            itemToAdd[0] = new Opc.Da.Item();
            itemToAdd[0].ItemName = "[UNTITLED_1]" + block + ":" + wordNumber;

            //create the item that contains the value to write
            Opc.Da.ItemValue[] writeValues = new Opc.Da.ItemValue[1];
            writeValues[0] = new Opc.Da.ItemValue(itemToAdd[0]);

            //make a scan of group to see if it already contains the item
            bool itemFound = false;
            foreach (Opc.Da.Item item in groupWrite.Items)
            {
                if (item.ItemName == itemToAdd[0].ItemName)
                {
                    // if it find the item i set the new value
                    writeValues[0].ServerHandle = item.ServerHandle;
                    itemFound = true;
                }
            }
            if (!itemFound)
            {
                //if it doesn't find it, we add it
                groupWrite.AddItems(itemToAdd);
                writeValues[0].ServerHandle = groupWrite.Items[groupWrite.Items.Length - 1].ServerHandle;
            }
            //set the value to write
            writeValues[0].Value = value;
            //write
            groupWrite.Write(writeValues);
        }*/
        public void WriteWord(string itemName, float value)
         {
             //Create the item to write (if the group doesn't have it, we need to insert it)
             Opc.Da.Item[] itemToAdd = new Opc.Da.Item[1];
             itemToAdd[0] = new Opc.Da.Item();
             itemToAdd[0].ItemName = itemName;

             //create the item that contains the value to write
             Opc.Da.ItemValue[] writeValues = new Opc.Da.ItemValue[1];
             writeValues[0] = new Opc.Da.ItemValue(itemToAdd[0]);

             //make a scan of group to see if it already contains the item
             bool itemFound = false;
             foreach (Opc.Da.Item item in groupWrite.Items)
             {
                 if (item.ItemName == itemToAdd[0].ItemName)
                 {
                     // if it find the item i set the new value
                     writeValues[0].ServerHandle = item.ServerHandle;
                     itemFound = true;
                 }
             }
             if (!itemFound)
             {
                 //if it doesn't find it, we add it
                 groupWrite.AddItems(itemToAdd);
                 writeValues[0].ServerHandle = groupWrite.Items[groupWrite.Items.Length - 1].ServerHandle;
             }
             //set the value to write
             writeValues[0].Value = value;
             //write
             groupWrite.Write(writeValues);
         }







         //private void btnWriteN7_Click(object sender, EventArgs e)
        // {
           //  try
           //  {
                 //writes N7:0
            //     WriteWord(".myValue", Convert.ToSingle(txtN7Value.Text));
           // }
           //  catch (Exception exc) { MessageBox.Show(exc.Message, "Error, you are not connected maybe ?", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        // }

        private void ValueButton_Click(object sender, EventArgs e)
        {
            (new System.Threading.Thread(delegate () {
                JsonSender.updateFromCellPhone("9999").GetAwaiter().GetResult();
            })).Start();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            
        }
    }
}
