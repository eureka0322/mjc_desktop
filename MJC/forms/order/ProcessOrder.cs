﻿using MJC.common.components;
using MJC.common;
using MJC.model;
using System.ComponentModel;
using System.Data;
using MJC.forms.sku;
using MJC.qbo;
using System;
using System.Reflection;
using Microsoft.CodeAnalysis;
using System.Security.Cryptography.Xml;
using System.Linq;
using MJC.config;

namespace MJC.forms.order
{
    public partial class ProcessOrder : GlobalLayout
    {
        private HotkeyButton hkAdds = new HotkeyButton("Ins", "Adds", Keys.Insert);
        private HotkeyButton hkDeletes = new HotkeyButton("Del", "Deletes", Keys.Delete);
        private HotkeyButton hkEdit = new HotkeyButton("Enter", "Edits", Keys.F6);
        private HotkeyButton hkSaveOrder = new HotkeyButton("F1", "Save Order", Keys.F1);
        private HotkeyButton hkAddMessage = new HotkeyButton("F2", "Add message", Keys.F2);
        private HotkeyButton hkCustomerProfiler = new HotkeyButton("F4", "Customer Profiler", Keys.F4);
        private HotkeyButton hkSKUInfo = new HotkeyButton("F5", "SKU Info", Keys.F5);
        private HotkeyButton hkSortLines = new HotkeyButton("Alt+S", "Sort lines", Keys.S, "alt");
        private HotkeyButton hkCloseOrder = new HotkeyButton("ESC", "Close order", Keys.Escape);
        private HotkeyButton hkShippingInformation = new HotkeyButton("F6", "Shipping Information", Keys.F6);
        private FCheckBox ShipOrder = new FCheckBox("Ship Order");

        private FComboBox Customer_ComBo = new FComboBox("Customer#:", 150);
        private FlabelConstant CustomerName = new FlabelConstant("Name:", 150);
        private FlabelConstant Terms = new FlabelConstant("Terms:", 150);
        //private FlabelConstant Zone = new FlabelConstant("Zone", 150);
        private FInputBox PoNumber = new FInputBox("PO#:", 100);

        private FlabelConstant Requested = new FlabelConstant("Requested:");
        private FlabelConstant Filled = new FlabelConstant("Filled:");
        private FlabelConstant QtyOnHold = new FlabelConstant("Qty on Hand:");
        private FlabelConstant QtyAllocated = new FlabelConstant("Qty Allocated:");
        private FlabelConstant QtyAvailable = new FlabelConstant("Qty Available:");
        private FlabelConstant Subtotal = new FlabelConstant("Subtotal:");
        private FlabelConstant TaxPercent = new FlabelConstant("7.250% Tax:");
        private FlabelConstant TotalSale = new FlabelConstant("Total Sale:");

        private DataGridView POGridRefer;
        private int POGridSelectedIndex = 0;
        private int customerId = 0;
        private int skuId = 0;
        private int flag = 0;
        private int addedRowIndex = -1;
        private bool isAddNewOrderItem = false;
        private int selectedOrderId = 0;
        private bool isNewOrder = true;
        private int orderId = 0;
        private int oldCustomerIndex = 0;
        private bool changeDetected = true;
        private string searchKey;
        private decimal billAsLabor = 0;
        public string sProcessedBy { get; set; }
        public string sInvoiceDesc { get; set; }
        public DateTime sDateShiped { get; set; }
        public string sInvoiceNumber { get; set; }
        public bool sEnableShipping { get; set; }
        public string sVia { get; set; }
        public bool sFob { get; set; }
        public string sSalesman { get; set; }
        public string sShipTo { get; set; }
        public int sShipID { get; set; }

        private List<OrderItem> OrderItemData = new List<OrderItem>();
        private List<SKUOrderItem> SubSkuList = new List<SKUOrderItem>();

        private string Message;

        public ProcessOrder(int customerId = 0, int orderId = 0, bool isAddNewOrderItem = false) : base("Process an Order", "Fill out the customer order")
        {
            InitializeComponent();
            _initBasicSize();

            this.isAddNewOrderItem = isAddNewOrderItem;
            this.customerId = customerId;
            this.selectedOrderId = orderId;
            this.orderId = selectedOrderId;

            oldCustomerIndex = customerId;

            if (this.selectedOrderId != 0)
            {
                isNewOrder = false;
            }

            if (this.orderId == 0)
            {
                this.orderId = Session.OrderModelObj.GetNextOrderId();
                sInvoiceNumber = $"INV-{this.orderId}";
            }
            else
            {
                sInvoiceNumber = $"INV-{orderId}";
            }

            dynamic customer = Session.CustomersModelObj.GetCustomerDataById(customerId);
            // this.TotalSkuList = Session.SKUModelObj.SkuOrderItems;

            // Default customer to the first priceTierId
            if (customer?.priceTierId == null)
            {
                customer.priceTierId = 1;
            }

            int priceTierId;
            if (customer != null && int.TryParse(customer?.priceTierId?.ToString() ?? "0", out priceTierId))
            {
                this.SubSkuList = Session.SKUModelObj.SkuOrderItems.Where(sku => sku.PriceTierId == customer.priceTierId).ToList();
            }
            else
            {
                this.SubSkuList = Session.SKUModelObj.SkuOrderItems;
            }

            // HotkeyButton[] hkButtons = new HotkeyButton[9] { hkAdds, hkDeletes, hkEdit, hkSaveOrder, hkAddMessage, hkCustomerProfiler, hkSKUInfo, hkSortLines, hkCloseOrder };
            HotkeyButton[] hkButtons = new HotkeyButton[] { hkAdds, hkDeletes, hkEdit, hkAddMessage, hkCustomerProfiler, hkSKUInfo, hkSortLines, hkCloseOrder, hkShippingInformation };

            _initializeHKButtons(hkButtons, false);
            AddHotKeyEvents();

            InitCustomerInfo(this.customerId);

            InitOrderItemsList();

            InitGridFooter();
        }

        private void printInvoice(int orderId, int orderStatus)
        {
            reportBuildAndPrint(orderId);
        }

        private void reportBuildAndPrint(int tmpMasterOrderID = 1648)  //1648 is default for dev testing only, remove on prod
        {
            int masterOrderID = tmpMasterOrderID;

            string totaltotal = TotalSale.GetConstant().Text;
            string taxtotal = TaxPercent.GetConstant().Text;
            string subtotal = Subtotal.GetConstant().Text;
            string coretotal = "0.00";
            string labortotal = this.billAsLabor.ToString("0.00");
            string procesedBy = sProcessedBy;

            /// Pull from populateinformationfield subroutine in best manner.  Helper integration imo.
            /// [] To Couple
            string status = "PICKING SLIP";  //setting default
                                             //Testing of Report RDL(C Style)
            Stream reportDefinition; // your RDLC from file or resource
            System.Data.DataTable dataSource; // your datasource for the report
            System.Data.DataTable dataSource2; // datasource for order totals

            dataSource = query("Select * from vwrptOrder Where MasterOrderID=" + masterOrderID.ToString());
            if (dataSource.Rows.Count == 0)
            {
                return;
            }
            if (dataSource.Rows[0]["statusID"].ToString() == "1") { status = "PICKING SLIP"; }
            if (dataSource.Rows[0]["statusID"].ToString() == "2") { status = "QUOTE"; }
            if (dataSource.Rows[0]["statusID"].ToString() == "3") { status = "INVOICE"; }
            if (dataSource.Rows[0]["statusID"].ToString() == "4") { status = "HISTORICAL INVOICE"; }

            dataSource2 = query("SELECT        id, '" + subtotal.ToString() + "' AS subtotal, '" + taxtotal.ToString() + "' AS taxtotal, '" + coretotal.ToString() + "' AS coretotal, '" + labortotal.ToString() + "' AS labortotal, '" + totaltotal.ToString() + "' AS totaltotal, '" + sProcessedBy.ToString() + "' AS processedBy, '" + status + "' AS Expr1 FROM Orders");

            reportDefinition = System.IO.File.Open(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\reports\\rptInvoice.rdl", FileMode.Open);
            Microsoft.Reporting.NETCore.LocalReport report = new Microsoft.Reporting.NETCore.LocalReport();
            report.LoadReportDefinition(reportDefinition);
            report.DataSources.Add(new Microsoft.Reporting.NETCore.ReportDataSource("DataSet1", dataSource));
            report.DataSources.Add(new Microsoft.Reporting.NETCore.ReportDataSource("dataSet2", dataSource2));
            report.SetParameters(new[] { new Microsoft.Reporting.NETCore.ReportParameter("MasterOrderID", masterOrderID.ToString()) });
            byte[] pdf = report.Render("PDF");

            var temporaryPath = Path.GetTempPath();
            var temporaryFile = Path.GetTempFileName();

            var path = Path.Combine(temporaryPath, @"\MJC");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            var tempFile = Path.Combine(temporaryPath, @"\MJC", temporaryFile);
            File.WriteAllBytes(tempFile, pdf);

            File.Copy(tempFile, $"C:\\MJCtemp\\{sInvoiceNumber}.pdf", true);

            SendToPrinter(tempFile);
        }

        public DataTable query(string sqlString)
        {
            string connectionString = @"Server=tcp:mndSQL10.everleap.com; Initial Catalog=DB_7153_mjcdev; User ID=DB_7153_mjcdev_user; Password = Title-Actor-Fin-13; Integrated Security = False";
            try
            {
                var da = new System.Data.SqlClient.SqlDataAdapter(sqlString, connectionString);
                da.SelectCommand.CommandType = CommandType.Text;
                System.Data.DataSet ds = new System.Data.DataSet();
                da.Fill(ds, "MyTable");
                DataTable queryResult = ds.Tables["MyTable"];
                return queryResult;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null/* TODO Change to default(_) if this is not a reference type */;
            }
        }

        private void SendToPrinter(string Filepath)
        {
            var document = PdfiumViewer.PdfDocument.Load(Filepath);
#if RELEASE
    var printer = string.IsNullOrEmpty(Session.SettingsModelObj.Settings.targetPrinter);
    if(!string.isNullOrEmpty(printer)) {
        document.CreatePrintDocument().PrinterSettings.PrinterName = printer;
    }
#endif
            document.CreatePrintDocument().Print();
        }


        private void AddHotKeyEvents()
        {
            hkAdds.GetButton().Click += (s, e) => InsertItem(s, e);
            hkSortLines.GetButton().Click += (s, e) =>
            {
                OrderItemData = OrderItemData.OrderBy(x => x.Sku).ToList();
                POGridRefer.DataSource = this.OrderItemData;
            };
            //hkEdit.GetButton().Click += (s, e) =>
            //{
            //    EditItem(s, e);
            //};

            hkCloseOrder.GetButton().Click += async (sender, e) =>
            {
                Processing processingModal = new Processing();
                processingModal.sProcessedBy = sProcessedBy;
                processingModal.sInvoiceNumber = sInvoiceNumber;
                processingModal.setDetails();
                processingModal.ShowDialog();

                sProcessedBy = processingModal.sProcessedBy;
                sInvoiceDesc = processingModal.sInvoiceDesc;
                sDateShiped = processingModal.sDateShiped;
                sInvoiceNumber = processingModal.sInvoiceNumber;

                int proFlag = processingModal.GetFlag();
                if (proFlag == 1)
                {
                    _navigateToPrev(sender, e);
                }
                if (proFlag == 2)
                {
                    double orderTotal = getAmtTotal();
                    CloseOrderActions CloseOrderActionsModal = new CloseOrderActions(this.customerId, this.orderId, orderTotal);
                    CloseOrderActionsModal.ShowDialog();

                    int saveFlag = CloseOrderActionsModal.GetSaveFlage();
                    if (saveFlag == 7)
                    {
                        Session.OrderModelObj.DeleteOrder(orderId);

                        _navigateToPrev(sender, e);
                    }

                    int status = 0;

                    if (POGridRefer.Rows.Count > 0)
                    {
                        int rowIndex = POGridRefer.SelectedRows[0].Index;
                        DataGridViewRow row = POGridRefer.Rows[rowIndex];

                        if (saveFlag != 0 && saveFlag != 7)
                        {
                            if (orderId != 0)
                            {
                                if (saveFlag == 1)
                                {
                                    if (await CreateInvoice())
                                    {
                                        using (PaymentProcessing paymentProcessing = new PaymentProcessing(this.customerId, this.orderId, orderTotal))
                                        {
                                            paymentProcessing.ShowDialog();
                                            paymentProcessing.FormClosed += async (ss, sargs) =>
                                            {
                                                this.Close();
                                            };
                                        }
                                        this.Close();
                                        status = 3;
                                        Session.OrderModelObj.UpdateOrderStatus(status, orderId);
                                        printInvoice(orderId, status);
                                        _navigateToPrev(sender, e);
                                    }
                                }
                                else if (saveFlag == 2)
                                {
                                    if (await CreateInvoice())
                                    {
                                        using (PaymentProcessing paymentProcessing = new PaymentProcessing(this.customerId, this.orderId, orderTotal))
                                        {
                                            paymentProcessing.ShowDialog();
                                            paymentProcessing.FormClosed += async (ss, sargs) =>
                                            {
                                                this.Close();
                                            };
                                        }
                                        this.Close();
                                        status = 3;
                                        Session.OrderModelObj.UpdateOrderStatus(status, orderId);
                                        printInvoice(orderId, status);

                                        _navigateToPrev(sender, e);
                                    }
                                }
                                else if (saveFlag == 3)
                                {
                                    status = 2;
                                    Session.OrderModelObj.UpdateOrderStatus(status, orderId);
                                    printInvoice(orderId, status);

                                    _navigateToPrev(sender, e);
                                }
                                else if (saveFlag == 4)
                                {
                                    if (await CreateInvoice())
                                    {

                                        status = 2;
                                        Session.OrderModelObj.UpdateOrderStatus(status, orderId);
                                        printInvoice(orderId, status);

                                        _navigateToPrev(sender, e);
                                    }
                                }
                                else if (saveFlag == 5)
                                {
                                    status = 1;
                                    Session.OrderModelObj.UpdateOrderStatus(status, orderId);
                                    _navigateToPrev(sender, e);
                                }
                                else if (saveFlag == 6)
                                {
                                    if (await CreateInvoice())
                                    {
                                        status = 1;
                                        Session.OrderModelObj.UpdateOrderStatus(status, orderId);
                                        printInvoice(orderId, status);
                                        _navigateToPrev(sender, e);
                                    }
                                }
                                else if (saveFlag == 7)
                                {
                                    _navigateToPrev(sender, e);
                                }
                                else if (saveFlag == 8)
                                {

                                }
                            }
                            else
                            {
                                //MessageBox.Show("Order info is not saved yet, please save order info first");
                            }
                        }
                    }

                }

            };
            hkDeletes.GetButton().Click += (sender, e) =>
            {
                DialogResult result = MessageBox.Show("Are you sure you want to delete this row?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    var selectedIndex = POGridRefer.SelectedRows[0].Index;

                    int selectedOrderId = 0;
                    if (POGridRefer.SelectedRows.Count > 0)
                    {
                        foreach (DataGridViewRow row in POGridRefer.SelectedRows)
                        {
                            selectedOrderId = (int)row.Cells[0].Value;
                        }

                        // Remove the item from the OrderData
                        this.OrderItemData.RemoveAt(POGridRefer.SelectedRows[0].Index);

                        BindingList<OrderItem> dataList = new BindingList<OrderItem>(this.OrderItemData);
                        POGridRefer.DataSource = dataList.ToList();

                        //                        LoadOrderItemList();

                        selectedIndex--;
                        if (selectedIndex < 0) selectedIndex = 0;

                        // POGridRefer.Rows[selectedIndex].Selected = true;
                    }
                }
            };
            hkCustomerProfiler.GetButton().Click += (sender, e) =>
            {
                CustomerData selectedItem = (CustomerData)Customer_ComBo.GetComboBox().SelectedItem;
                int customerId = selectedItem.Id;

                CustomerProfile customerProfileModal = new CustomerProfile(customerId);
                _navigateToForm(sender, e, customerProfileModal);
                this.Hide();
            };
            hkSKUInfo.GetButton().Click += (sender, e) =>
            {
                if (POGridRefer.RowCount > 0)
                {
                    SKUInformation detailModal = new SKUInformation(true);

                    int rowIndex = POGridRefer.SelectedRows[0].Index;

                    DataGridViewRow row = POGridRefer.Rows[rowIndex];
                    int skuId = (int)row.Cells[2].Value;
                    List<dynamic> skuData = new List<dynamic>();
                    skuData = Session.SKUModelObj.GetSKUData(skuId);
                    detailModal.setDetails(skuData, skuData[0].id);

                    this.Hide();
                    _navigateToForm(sender, e, detailModal);
                }
            };
            hkAddMessage.GetButton().Click += (sender, e) =>
            {
                if (POGridRefer.RowCount > 0)
                {
                    OrderItemMessage detailModal = new OrderItemMessage();

                    int rowIndex = POGridRefer.SelectedRows[0].Index;

                    DataGridViewRow row = POGridRefer.Rows[rowIndex];
                    int orderItemId = (int)row.Cells[0].Value;

                    detailModal.setDetails(orderItemId > 0 ? orderItemId : 1);
                    detailModal.Message.GetTextBox().Text = Message;

                    if (detailModal.ShowDialog() == DialogResult.OK)
                    {
                        Message = detailModal.Message.GetTextBox().Text;
                    }
                }
            };
            hkShippingInformation.GetButton().Click += (sender, e) =>
            {
                int customerId = this.customerId;
                ShippingInformation shippingInfoModal = new ShippingInformation(customerId);
                this.Enabled = false;
                shippingInfoModal.Show();
                shippingInfoModal.FormClosed += (sender, e) => {
                    this.sVia = shippingInfoModal.sVia;
                    this.sFob = shippingInfoModal.sFob;
                    this.sEnableShipping = shippingInfoModal.sEnableShipping;
                    this.sSalesman = shippingInfoModal.sSalesman;
                    this.sShipTo = shippingInfoModal.sShipTo;
                    this.sShipID = shippingInfoModal.sShipID;
                    this.Enabled = true;
                };
            };
        }

        private void InitCustomerInfo(int customerId = 0)
        {
            List<dynamic> FormComponents = new List<dynamic>();

            FormComponents.Add(Customer_ComBo);
            FormComponents.Add(CustomerName);
            FormComponents.Add(Terms);
            FormComponents.Add(ShipOrder);
            //FormComponents.Add(Zone);
            FormComponents.Add(PoNumber);

            _addFormInputs(FormComponents, 30, 110, 650, 42, 180);

            var refreshData = Session.CustomersModelObj.LoadCustomerData("", false);

            Customer_ComBo.GetComboBox().DataSource = Session.CustomersModelObj.CustomerDataList;
            Customer_ComBo.GetComboBox().MaxDropDownItems = 10;
            Customer_ComBo.GetComboBox().DisplayMember = "Num";
            Customer_ComBo.GetComboBox().ValueMember = "Id";
            Customer_ComBo.GetComboBox().SelectedValueChanged += new EventHandler(ProcessOrder_SelectedValueChanged);
            if (customerId == 0)
            {
                Customer_ComBo.GetComboBox().SelectedIndex = 0;
                Customer_SelectedIndexChanged(Customer_ComBo.GetComboBox(), EventArgs.Empty);
            }
            else
            {
                Customer_ComBo.GetComboBox().SelectedValue = customerId;
            }

            oldCustomerIndex = customerId;

            // PoNumber.GetLabel().Focus();
            // POGridRefer.Select();
        }

        private void LoadSelectedCustomerData()
        {
            if (Customer_ComBo.GetComboBox().SelectedItem != null)
            {

                CustomerData selectedItem = (CustomerData)Customer_ComBo.GetComboBox().SelectedItem;


                int customerId = selectedItem.Id;
                this.customerId = customerId;

                // TODO: GetCustomerData returns very slowly.

                dynamic customer = Session.CustomersModelObj.GetCustomerDataById(customerId);
                var customerData = Session.CustomersModelObj.GetCustomerData(customerId);
                if (customerData != null)
                {
                    if (customerData.customerName != "") CustomerName.SetContext(customerData.customerName);
                    else CustomerName.SetContext("N/A");

                    if (customerData.terms != "") Terms.SetContext(customerData.terms);
                    else Terms.SetContext("N/A");

                    if (customerData.poRequired)
                    {
                        PoNumber.GetLabel().Show();
                        PoNumber.GetTextBox().Show();
                    }
                    else
                    {
                        PoNumber.GetLabel().Hide();
                        PoNumber.GetTextBox().Hide();
                    }
                }
            }
            LoadOrderItemList();
        }

        private void ProcessOrder_SelectedValueChanged(object? sender, EventArgs e)
        {
            var index = Customer_ComBo.GetComboBox().SelectedIndex;
            if (index == oldCustomerIndex) return;

            LoadSelectedCustomerData();

            //if (!changeDetected || (MessageBox.Show("The current order will be lost. Are you sure you want to change the customer without saving the current changes?", "Change?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes))
            //{

            //}
            //else
            //{
            //    Customer_ComBo.GetComboBox().SelectedIndex = oldCustomerIndex;
            //}

            oldCustomerIndex = Customer_ComBo.GetComboBox().SelectedIndex;
        }

        private void Customer_SelectedIndexChanged(object sender, EventArgs e)
        {


        }


        private void InitOrderItemsList()
        {
            GridViewOrigin OrderEntryLookupGrid = new GridViewOrigin();
            POGridRefer = OrderEntryLookupGrid.GetGrid();
            POGridRefer.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(157, 196, 235);
            POGridRefer.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(31, 63, 96);
            POGridRefer.ColumnHeadersDefaultCellStyle.Padding = new Padding(12);
            POGridRefer.Location = new Point(0, 200);
            POGridRefer.Width = this.Width;
            POGridRefer.Height = 490;
            POGridRefer.AllowUserToAddRows = false;
            POGridRefer.ReadOnly = false;
            POGridRefer.KeyDown += DataGridView_KeyDown;
            POGridRefer.VirtualMode = true;
            POGridRefer.EditMode = DataGridViewEditMode.EditOnKeystroke;
            POGridRefer.EditingControlShowing += POGridRefer_EditingControlShowing;
            POGridRefer.DataError += POGridRefer_DataError;
            this.Controls.Add(POGridRefer);

            LoadOrderItemList();
        }

        private void POGridRefer_DataError(object? sender, DataGridViewDataErrorEventArgs e)
        {
            Messages.ShowError("There was a problem setting the cell data.");
        }

        private void InitGridFooter()
        {
            List<dynamic> GridFooterComponents = new List<dynamic>();

            GridFooterComponents.Add(Requested);
            GridFooterComponents.Add(Filled);

            _addFormInputs(GridFooterComponents, 30, 680, 650, 42, 780);

            List<dynamic> GridFooterComponents1 = new List<dynamic>();

            GridFooterComponents1.Add(QtyOnHold);
            GridFooterComponents1.Add(QtyAllocated);
            GridFooterComponents1.Add(QtyAvailable);
            GridFooterComponents1.Add(Subtotal);
            GridFooterComponents1.Add(TaxPercent);
            GridFooterComponents1.Add(TotalSale);

            _addFormInputs(GridFooterComponents1, 680, 680, 650, 42, 780);


            LoadGridFooterInfo();
        }

        public void LoadGridFooterInfo()
        {
            this.skuId = SubSkuList[0].Id;
            PopulateInformationField();
        }

        private void PopulateInformationField()
        {
            if (POGridRefer.SelectedRows.Count == 0) return;

            // Make sure we have the default SKU #2 for tax code
            SalesTaxCodeModel salesTaxCodeModel = new SalesTaxCodeModel();
            salesTaxCodeModel.LoadSalesTaxCodeData("");
            var taxCodeId = Session.SettingsModelObj.Settings.taxCodeId.GetValueOrDefault(2);
            var salesTaxCode = salesTaxCodeModel.GetSalesTaxCodeData(taxCodeId);
            var taxRate = salesTaxCode.rate;

            DataGridViewRow selectedRow = POGridRefer.SelectedRows[0];
            var unitPrice = selectedRow.Cells["UnitPrice"].Value as double?;
            var quantity = selectedRow.Cells["Quantity"].Value as int?;
            var requested = quantity;
            var filled = requested;

            if (this.skuId != 0)
            {
                int skuId = this.skuId;
                var qtyInfo = Session.SKUModelObj.LoadSkuQty(skuId);

                SKUOrderItem sku = this.SubSkuList.FirstOrDefault(x => x.Id == skuId);

                var items = Session.PriceTiersModelObj.GetPriceTierItems();
                var priceTierItem = items[sku.PriceTierId];

                QtyOnHold.SetContext(qtyInfo.qty.ToString());
                QtyAllocated.SetContext(qtyInfo.qtyAllocated.ToString());
                QtyAvailable.SetContext(qtyInfo.qtyAvailable.ToString());

                if (requested > qtyInfo.qtyAvailable)
                {
                    filled = qtyInfo.qtyAvailable;
                }
            }

            var total = 0.00;
            var tax = 0.00;

            double _billAsLabor = 0.0;

            foreach (var item in OrderItemData)
            {
                var _lineTotal = (item?.UnitPrice * item?.Quantity) ?? 0.00;
                var _taxAmount = _lineTotal * (taxRate / 100);

                if (item.BillAsLabor == true)
                {
                    _billAsLabor += _lineTotal;
                    this.billAsLabor = Convert.ToDecimal(_billAsLabor);
                }
                else
                {
                    if (item?.Tax.GetValueOrDefault() ?? false)
                    {
                        tax += _taxAmount;
                    }
                }
                var _totalAmount = tax + _lineTotal + _billAsLabor;
                total += _lineTotal;
            }

            //var lineTotal = unitPrice * quantity;
            //var taxAmount = lineTotal * (taxRate / 100);
            var totalAmount = tax + total;

            selectedRow.Cells["lineTotal"].Value = unitPrice * quantity;

            TaxPercent.GetLabel().Text = $"{taxRate}% Tax:";

            Subtotal.SetContext(total.ToString("#,##0.00"));
            TaxPercent.SetContext(tax.ToString("0.00"));
            TotalSale.SetContext(totalAmount.ToString("$#,##0.00"));

            Filled.SetContext(filled.ToString());
            Requested.SetContext(requested.ToString());
        }

        public void LoadOrderItemList(string sort = "")
        {
            if (POGridRefer == null) return;

            if (this.OrderItemData == null)
            {
                this.OrderItemData = new List<OrderItem>(); // this.selectedOrderId
            }

            if (!isAddNewOrderItem)
            {
                OrderItemData = Session.OrderItemModelObj.GetOrderItemsListByCustomerId(this.customerId, 0, sort);
            }


            POGridRefer.SuspendLayout();
            POGridRefer.VirtualMode = true;
            POGridRefer.Columns.Clear();
            POGridRefer.DataSource = this.OrderItemData;
            POGridRefer.ScrollBars = ScrollBars.Vertical;

            POGridRefer.Columns[0].HeaderText = "OrderItemId";
            POGridRefer.Columns[0].DataPropertyName = "id";
            POGridRefer.Columns[0].Visible = false;
            POGridRefer.Columns[1].HeaderText = "OrderId";
            POGridRefer.Columns[1].DataPropertyName = "orderId";
            POGridRefer.Columns[1].Visible = false;
            POGridRefer.Columns[2].HeaderText = "SkuId";
            POGridRefer.Columns[2].DataPropertyName = "skuId";
            POGridRefer.Columns[2].Visible = false;
            POGridRefer.Columns[3].HeaderText = "QboItemId";
            POGridRefer.Columns[4].DataPropertyName = "qboItemId";
            POGridRefer.Columns[3].Visible = false;
            POGridRefer.Columns[4].HeaderText = "QboSkuId";
            POGridRefer.Columns[4].DataPropertyName = "qboSkuId";
            POGridRefer.Columns[4].Visible = false;
            POGridRefer.Columns[5].HeaderText = "SKU#";
            POGridRefer.Columns[5].DataPropertyName = "sku";
            POGridRefer.Columns[5].Visible = false;

            POGridRefer.Columns[6].HeaderText = "Qty";
            POGridRefer.Columns[6].Width = 100;

            POGridRefer.Columns[7].HeaderText = "Description";
            POGridRefer.Columns[7].DataPropertyName = "description";
            POGridRefer.Columns[7].Width = 400;

            POGridRefer.Columns[8].HeaderText = "Tax";
            POGridRefer.Columns[8].DataPropertyName = "tax";
            POGridRefer.Columns[8].DefaultCellStyle.Format = "0.00##";
            POGridRefer.Columns[8].Visible = false;

            POGridRefer.Columns[9].HeaderText = "Disc.";
            POGridRefer.Columns[9].Name = "PriceTierCode";
            POGridRefer.Columns[9].DataPropertyName = "priceTierCode";
            POGridRefer.Columns[9].Width = 200;

            POGridRefer.Columns[10].HeaderText = "Unit Price";
            POGridRefer.Columns[10].DataPropertyName = "unitPrice";
            POGridRefer.Columns[10].DefaultCellStyle.Format = "0.00##";
            POGridRefer.Columns[10].Width = 200;

            POGridRefer.Columns[11].HeaderText = "Line Total";
            POGridRefer.Columns[11].DataPropertyName = "lineTotal";
            POGridRefer.Columns[11].DefaultCellStyle.Format = "0.00##";
            POGridRefer.Columns[11].Width = 200;

            POGridRefer.Columns[12].HeaderText = "SC";
            POGridRefer.Columns[12].Name = "salesCode";
            POGridRefer.Columns[12].DataPropertyName = "sc";
            POGridRefer.Columns[12].Width = 200;

            POGridRefer.Columns[13].HeaderText = "Price Tier";
            POGridRefer.Columns[13].Name = "PriceTier";
            POGridRefer.Columns[13].DataPropertyName = "priceTier";
            POGridRefer.Columns[13].Width = 200;
            POGridRefer.Columns[13].Visible = false;

            POGridRefer.Columns[14].Visible = false;

            // TODO: SKUDataList is causing the form to load very slowly. Need to find a way to load this data faster.
            //DataGridViewComboBoxColumn comboBoxColumn = new DataGridViewComboBoxColumn();
            //comboBoxColumn.DataSource = Session.SKUModelObj.SKUDataList.ToList().Take(10).ToList();
            //comboBoxColumn.HeaderText = "SKU#";
            //comboBoxColumn.Width = 300;
            //comboBoxColumn.Name = "skuNumber";
            //comboBoxColumn.MaxDropDownItems = 10;
            //comboBoxColumn.DataPropertyName = "skuId";
            //comboBoxColumn.ValueMember = "Id";
            //comboBoxColumn.DisplayMember = "Name";
            //// comboBoxColumn.AutoComplete = true;
            //comboBoxColumn.DisplayIndex = 6;
            //comboBoxColumn.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;
            //// POGridRefer.Columns.Add(comboBoxColumn)

            DataGridViewTextBoxColumn skuColumn = new DataGridViewTextBoxColumn();
            skuColumn.HeaderText = "SKU#";
            skuColumn.DisplayIndex = 6;
            skuColumn.Width = 300;
            skuColumn.DataPropertyName = "Sku";
            POGridRefer.Columns.Add(skuColumn);

            // Tax ComboBox Column
            DataGridViewComboBoxColumn taxComboBoxColumn = new DataGridViewComboBoxColumn();
            List<SKUTax> skuTaxes = new List<SKUTax>();
            skuTaxes.Add(new SKUTax { Value = true, DisplayName = "Yes" });
            skuTaxes.Add(new SKUTax { Value = false, DisplayName = "No" });
            taxComboBoxColumn.DataSource = skuTaxes;
            taxComboBoxColumn.HeaderText = "Tax";
            taxComboBoxColumn.Width = 150;
            taxComboBoxColumn.Name = "skuTax";
            taxComboBoxColumn.DataPropertyName = "tax";
            taxComboBoxColumn.ValueMember = "Value";
            taxComboBoxColumn.DisplayMember = "DisplayName";
            taxComboBoxColumn.DisplayIndex = 9;
            taxComboBoxColumn.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;

            POGridRefer.Columns.Add(taxComboBoxColumn);

            POGridRefer.CellValueChanged += PoGridRefer_CellValueChanged;
            POGridRefer.CellEndEdit += POGridRefer_CellEndEdit;
            POGridRefer.SelectionChanged += POGridRefer_SelectionChanged;

            if (this.OrderItemData.Count == 0)
            {
                InsertItem(null, null);
            }
        }


        private double getAmtTotal()
        {
            SalesTaxCodeModel salesTaxCodeModel = new SalesTaxCodeModel();
            salesTaxCodeModel.LoadSalesTaxCodeData("");
            // Make sure we have the default SKU #2 for tax code
            var taxCodeId = Session.SettingsModelObj.Settings.taxCodeId.GetValueOrDefault(2);
            var salesTaxCode = salesTaxCodeModel.GetSalesTaxCodeData(taxCodeId);
            var taxRate = salesTaxCode.rate;

            var total = 0.00;
            var tax = 0.00;

            double _billAsLabor = 0.0;

            foreach (var item in OrderItemData)
            {
                var _lineTotal = (item?.UnitPrice * item?.Quantity) ?? 0.00;
                var _taxAmount = _lineTotal * (taxRate / 100);

                if (item.BillAsLabor == true)
                {
                    _billAsLabor += _lineTotal;
                    this.billAsLabor = Convert.ToDecimal(_billAsLabor);
                }
                else
                {
                    if (item?.Tax.GetValueOrDefault() ?? false)
                    {
                        tax += _taxAmount;
                    }
                }
                var _totalAmount = tax + _lineTotal + _billAsLabor;
                total += _lineTotal;
            }

            return total + tax;
        }

        private void POGridRefer_SelectionChanged(object? sender, EventArgs e)
        {
            if (POGridRefer.SelectedRows.Count == 0) return;

            var selectedRow = POGridRefer.SelectedRows[0];
            int selectedValue = int.Parse(selectedRow.Cells["skuId"].Value.ToString());

            var skuId = Session.SKUModelObj.SKUDataList.FirstOrDefault(item => item.Id == selectedValue).Id;
            this.skuId = skuId;

            PopulateInformationField();
        }

        private void POGridRefer_CellEndEdit(object? sender, DataGridViewCellEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("POGridRefer_CellEndEdit");
            //if (e.ColumnIndex == 15)
            //{
            //DataGridViewComboBoxCell comboBoxCell = (DataGridViewComboBoxCell)POGridRefer.Rows[e.RowIndex].Cells[e.ColumnIndex];
            //    var x = comboBoxCell.Value;
            //    Console.WriteLine(x);
            //}
            var x = POGridRefer.Rows[e.RowIndex].Cells[e.ColumnIndex];

        }

        private void PoGridRefer_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("PoGridRefer_CellValueChanged");
            if (POGridRefer.SelectedRows.Count == 0) return;
            if (e.RowIndex == -1) return;
            if (e.ColumnIndex == 15)
            {
                var sku = (string)POGridRefer.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;
                if (string.IsNullOrEmpty(sku)) return;

                var skus = Session.SKUModelObj.SKUDataList.Where(item => item.Name == sku.ToLower());
                if (skus.Count() == 0)
                {
                    // Special Order
                    // Non Inventory Item
                    // Message
                }
                else if (skus.Count() > 1)
                {
                    var skuInfo = skus.FirstOrDefault();
                    SKUOrderItem skuData = this.SubSkuList.Where(item => item.Id == skuInfo.Id).ToList()[0];
                    var salesCostCodeData = Session.SalesCostCodesModelObj.SalesCostCodeDataList.FirstOrDefault(x => x.id == skuData.CostCode.Value);

                    DataGridViewRow selectedRow = POGridRefer.SelectedRows[0];
                    selectedRow.Cells["sku"].Value = skuData.Name;
                    selectedRow.Cells["qboSkuId"].Value = skuData.QboSkuId;
                    selectedRow.Cells["description"].Value = skuData.Description;
                    //selectedRow.Cells["priceTier"].Value = sku.PriceTierId;
                    selectedRow.Cells["unitPrice"].Value = skuData.Price;
                    selectedRow.Cells["lineTotal"].Value = skuData.Price * skuData.Qty;
                    selectedRow.Cells["salesCode"].Value = salesCostCodeData.scCode;
                    selectedRow.Cells["quantity"].Value = 1;

                    this.OrderItemData[e.RowIndex].SkuId = skuInfo.Id;
                    this.skuId = skuInfo.Id;

                    PopulateInformationField();
                }
                else
                {
                    var skuInfo = skus.FirstOrDefault();
                    SKUOrderItem skuData = this.SubSkuList.Where(item => item.Id == skuInfo.Id).ToList()[0];
                    var salesCostCodeData = Session.SalesCostCodesModelObj.SalesCostCodeDataList.FirstOrDefault(x => x.id == skuData.CostCode.Value);

                    DataGridViewRow selectedRow = POGridRefer.SelectedRows[0];
                    selectedRow.Cells["sku"].Value = skuData.Name;
                    selectedRow.Cells["qboSkuId"].Value = skuData.QboSkuId;
                    selectedRow.Cells["description"].Value = skuData.Description;
                    //selectedRow.Cells["priceTier"].Value = sku.PriceTierId;
                    selectedRow.Cells["unitPrice"].Value = skuData.Price;
                    selectedRow.Cells["lineTotal"].Value = skuData.Price * skuData.Qty;
                    selectedRow.Cells["salesCode"].Value = salesCostCodeData.scCode;
                    selectedRow.Cells["quantity"].Value = 1;

                    this.OrderItemData[e.RowIndex].SkuId = skuInfo.Id;
                    this.skuId = skuInfo.Id;

                    PopulateInformationField();
                }
            }
            else
            // Quantity changed
            if (e.ColumnIndex == 6)
            {
                PopulateInformationField();
            }
            else if (e.ColumnIndex == 10)
            {
                PopulateInformationField();
            }
            else
            // SKU Changed
            if (e.ColumnIndex == 15)
            {
                DataGridViewComboBoxCell comboBoxCell = (DataGridViewComboBoxCell)POGridRefer.Rows[e.RowIndex].Cells[e.ColumnIndex];
                int selectedValue = int.Parse(comboBoxCell.Value?.ToString());
                DataGridViewRow selectedRow = POGridRefer.SelectedRows[0];

                var skuId = Session.SKUModelObj.SKUDataList.FirstOrDefault(item => item.Id == selectedValue).Id;

                SKUOrderItem sku = this.SubSkuList.Where(item => item.Id == skuId).ToList()[0];
                var salesCostCodeData = Session.SalesCostCodesModelObj.SalesCostCodeDataList.FirstOrDefault(x => x.id == sku.CostCode.Value);

                selectedRow.Cells["sku"].Value = sku.Name;
                selectedRow.Cells["qboSkuId"].Value = sku.QboSkuId;
                selectedRow.Cells["description"].Value = sku.Description;
                //selectedRow.Cells["priceTier"].Value = sku.PriceTierId;
                selectedRow.Cells["unitPrice"].Value = sku.Price;
                selectedRow.Cells["lineTotal"].Value = sku.Price * sku.Qty;
                selectedRow.Cells["salesCode"].Value = salesCostCodeData.scCode;
                selectedRow.Cells["quantity"].Value = 1;

                this.skuId = skuId;

                PopulateInformationField();
            }

            changeDetected = true;
        }

        private async Task<bool> CreateInvoice()
        {
            List<OrderItem> orderItems = this.OrderItemData;
            CustomerData customer = (CustomerData)this.Customer_ComBo.GetComboBox().SelectedItem;

            // TODO: Change this to the Processing form values
            string invoiceNumber = sInvoiceNumber;

            if (orderItems.Count > 0)
            {
                this.selectedOrderId = orderItems[orderItems.Count - 1].OrderId;
                foreach (var order in orderItems)
                {
                    // For all special order items and non inventory items, let's use the NVI SKU
                    if (order.SkuId == null || order.SkuId == 0)
                    {
                        var sku = Session.SKUModelObj.SKUDataList.FirstOrDefault(x => x.Name == "NVI");

                        var skuName = order.Sku;
                        order.SkuId = sku.Id;
                    }

                    // This is a message line, we don't want a visible SKU
                    if (string.IsNullOrEmpty(order.Sku) && !string.IsNullOrEmpty(order.Description))
                    {
                        order.Sku = "";
                    }

                    // Add Memo
                    order.message = Message;
                }


                if (this.selectedOrderId == 0)
                {
                    orderItems = orderItems.Where(item => item.OrderId == 0).ToList();
                    try
                    {
                        bool res = await Session.qboApiService.CreateInvoice(customer, invoiceNumber, orderItems, sProcessedBy, sShipID, sInvoiceDesc, PoNumber.GetTextBox().Text, sVia, Terms.GetConstant().Text);

                        if (res)
                        {
                            this.isAddNewOrderItem = false;

                            // TODO: Where are order items saved?
                            //LoadOrderItemList();

                            selectedOrderId = orderId;

                        }
                        else
                        {
                            ShowError("The invoice could not be created.");
                            return false;
                        }
                    }
                    catch (Exception e)
                    {
                        if (e.Message.Contains("TOKEN"))
                        {
                            ShowError("The invoice could not be created: QuickBooks tokens.json was not found.");
                        }
                        else if (e.Message.Contains("Unauthorized"))
                        {
                            ShowError("The invoice could not be created: QuickBooks needs to be reauthorized.");
                        }
                        else if (e.Message.Contains("Invalid Reference Id"))
                        {
                            ShowError("The invoice could not be created: There was an invalid item on the invoice.");
                        }
                        else if (e.Message.Contains("Duplicate Document Number"))
                        {
                            ShowError("The invoice could not be created: The invoice number has already been used. Please change your invoice number and try again.");
                        }
                        else if (e.Message.Contains("ValidationX"))
                        {
                            ShowError("The invoice could not be created: QuickBooks needs to be reauthorized.");
                        }
                        else
                        {
                            ShowError("The invoice could not be created.");
                        }
                        return false;
                    }
                }
                else
                {
                    orderItems = orderItems.Where(item => item.OrderId == selectedOrderId).ToList();
                    dynamic selectedOrder = Session.OrderModelObj.GetOrderById(this.selectedOrderId);

                    sProcessedBy = selectedOrder.processedBy;
                    sInvoiceDesc = selectedOrder.invoiceDesc;
                    sDateShiped = selectedOrder.dateShiped;
                    sInvoiceNumber = selectedOrder.invoiceNumber;

                    sEnableShipping = true;
                    sVia = selectedOrder.shipVia;
                    sFob = selectedOrder.fob;
                    sSalesman = selectedOrder.salesman;
                    sShipTo = selectedOrder.shipTo;

                    bool res = await Session.qboApiService.UpdateInvoice(customer, orderItems, selectedOrder);
                    if (res)
                    {
                        LoadOrderItemList();

                        selectedOrderId = orderId;

                        ShowInformation("The invoice has been updated successfully");
                    }
                    else
                    {
                        ShowError("Invoice was not created #2.");
                        return false;
                    }
                }
            }
            else
            {
                ShowError("Order Item does not exist.");
                return false;
            }

            return true;
        }

        private void InsertItem(object sender, EventArgs e)
        {
            SKUOrderItem sku = this.SubSkuList[0];
            var items = Session.PriceTiersModelObj.PriceTierDataList;
            var priceTierItem = items[sku.PriceTierId];
            int costCodeId = sku.CostCode.Value;

            var salesCostCodeData = Session.SalesCostCodesModelObj.GetSalesCostCodeData(costCodeId);

            List<dynamic> skuData = new List<dynamic>();
            skuData = Session.SKUModelObj.GetSKUData(sku.Id);
            bool taxable = (bool)skuData[0].taxable;

            this.OrderItemData.Add(new OrderItem
            {
                //SkuId = sku.Id,
                //Sku = sku.Name,
                //QboSkuId = sku.QboSkuId,
                //Description = sku.Description,
                //PriceTier = sku.PriceTierId,
                //PriceTierCode = sku.PriceTier,
                //UnitPrice = sku.Price,
                //LineTotal = sku.Price * sku.Qty,
                //SC = salesCostCodeData.scCode,
                //Quantity = sku.Qty > 0 ? sku.Qty : 1,
                //Tax = taxable,
                //BillAsLabor = true
            });

            BindingList<OrderItem> dataList = new BindingList<OrderItem>(this.OrderItemData);
            POGridRefer.DataSource = dataList.ToList();

            if (POGridRefer.Rows.Count > 0)
            {
                POGridRefer.CurrentCell = POGridRefer.Rows[POGridRefer.Rows.Count - 1].Cells[15];
                POGridRefer.Select();
                // POGridRefer.BeginEdit(true); // This is not needed because we are using EditOnKeyDown
            }
        }

        private void AddItem(int skuId = 0)
        {
            if (skuId == 0) return;

            var skuData = Session.SKUModelObj.GetSKUData(skuId);
            POGridRefer.SelectedRows[0].Cells[15].Value = skuData[0].sku;
        }

        private void EditItem(object sender, EventArgs e)
        {
            //int rowIndex = POGridRefer.SelectedRows[0];
            //int rowIndex = POGridRefer.CurrentCell.RowIndex;
            //int columnIndex = POGridRefer.CurrentCell.ColumnIndex;
            //DataGridViewRow row = POGridRefer.Rows[rowIndex];

            //DataGridViewTextBoxCell cell = (DataGridViewTextBoxCell)row.Cells[columnIndex];

            //POGridRefer.CurrentCell = POGridRefer.Rows[rowIndex].Cells[columnIndex];
            //POGridRefer.BeginEdit(true);
        }

        private void DataGridView_KeyDown(object sender, KeyEventArgs e)
        {
            DataGridView dataGridView = (DataGridView)sender;
            if (e.KeyCode == Keys.Enter)
            {
                int rowIndex = POGridRefer.SelectedRows[0].Index;
                int columnIndex = POGridRefer.CurrentCell.ColumnIndex;

                if (dataGridView.CurrentCell != null && dataGridView.CurrentCell.IsInEditMode)  
                { 
                }
                else if (dataGridView.CurrentCell != null && dataGridView.CurrentCell.IsInEditMode == false)
                {
                    if (columnIndex == 15)
                    {
                        var lookupOptionsForm = new LookupOptions();
                        this.Enabled = false;
                        lookupOptionsForm.FormClosed += (s, e) =>
                        {
                            this.Enabled = true;
                            LookUpAction(lookupOptionsForm.lookupOptions);
                        };
                        lookupOptionsForm.Show();
                        e.Handled = true;
                    }
                }
            }
        }


        private void LookUpAction(int actionID)
        {
            switch (actionID)
            {
                case 0:
                    break;
                case 1:
                    SKUList SKUListForm = new SKUList(false, true);
                    SKUListForm._prevForm = this;
                    SKUListForm.VisibleChanged += (s, e) => {
                        if (!SKUListForm.Visible)
                        {
                            int selectedSKUId = SKUListForm.selectedSKUId;
                            AddItem(selectedSKUId);
                        }
                    };
                    SKUListForm.Show();
                    this.Hide();
                    break;
            }
        }

        private void POGridRefer_EditingControlShowing(object? sender, DataGridViewEditingControlShowingEventArgs e)
        {
            var comboBox = e.Control as DataGridViewComboBoxEditingControl;
            if (comboBox != null)
            {
                comboBox.DropDownStyle = ComboBoxStyle.DropDown;
                comboBox.AutoCompleteMode = AutoCompleteMode.Append;
                comboBox.IntegralHeight = false;
                comboBox.MaxDropDownItems = 10;
            }

        }
    }
}
