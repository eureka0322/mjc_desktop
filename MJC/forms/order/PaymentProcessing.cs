using MJC.common.components;
using MJC.common;
using MJC.model;

namespace MJC.forms.order
{
    public partial class PaymentProcessing : BasicModal
    {
        public FInputBox Cash = new FInputBox("Cash");
        public FInputBox Check = new FInputBox("Check#");
        public FInputBox FreightCollect = new FInputBox("Freight");
        public FInputBox OnAccount = new FInputBox("OnAccount");
        public FInputBox CargeCard = new FInputBox("CargeCard");
        public FInputBox Dsicount = new FInputBox("Discount");
        public FInputBox CreditsApplied = new FInputBox("CreditsApplied");
        public FInputBox Change = new FInputBox("Change");
        public FInputBox AmountRemaining = new FInputBox("Amount Remaining");

        private HotkeyButton CARemaining = new HotkeyButton("F2", "Calculate Amount Remaining", Keys.F2);
        private Button CARemaining_button;
        private HotkeyButton CustomerInfo = new HotkeyButton("F3", "CustomerInfo", Keys.F3);
        private Button CustomerInfo_button;
        private HotkeyButton CreditCards = new HotkeyButton("F6", "Credit Cards", Keys.F6);
        private Button CreditCards_button;
        private HotkeyButton CompletePayment = new HotkeyButton("F9", "Complete Payment", Keys.F9);
        private Button CompletePayment_button;

        private int orderItemId = 0;

        public PaymentProcessing() : base("Payment Processing")
        {
            InitializeComponent();

            this.Size = new Size(600, 800);
            this.StartPosition = FormStartPosition.CenterScreen;

            InitMBOKButton();
            InitInputBox();

            Cash.GetTextBox().TabIndex = 0;
            Cash.GetTextBox().Focus();
            Cash.GetTextBox().Select();
        }

        private void InitMBOKButton()
        {
            CARemaining.SetPosition(new Point(30, 530));
            CARemaining.GetButton().BackColor = Color.FromArgb(236, 242, 249); ;
            this.Controls.Add(CARemaining.GetButton());
            this.Controls.Add(CARemaining.GetLabel());
            CARemaining.GetButton().TabStop = false;

            CustomerInfo.SetPosition(new Point(30, 580));
            CustomerInfo.GetButton().BackColor = Color.FromArgb(236, 242, 249); ;
            this.Controls.Add(CustomerInfo.GetButton());
            this.Controls.Add(CustomerInfo.GetLabel());
            CustomerInfo.GetButton().TabStop = false;

            CreditCards.SetPosition(new Point(30, 630));
            CreditCards.GetButton().BackColor = Color.FromArgb(236, 242, 249); ;
            this.Controls.Add(CreditCards.GetButton());
            this.Controls.Add(CreditCards.GetLabel());
            CreditCards.GetButton().TabStop = false;

            CompletePayment.SetPosition(new Point(30, 680));
            CompletePayment.GetButton().BackColor = Color.FromArgb(236, 242, 249); ;
            this.Controls.Add(CompletePayment.GetButton());
            this.Controls.Add(CompletePayment.GetLabel());
            CompletePayment.GetButton().TabStop = false;

            CARemaining.GetButton().Click += (sender, e) =>
            {

            };


            this.KeyDown += (s, e) =>
            {
                if (e.KeyCode == Keys.F2)
                {
                    CARemaining.GetButton().PerformClick();
                }
                else if (e.KeyCode == Keys.F3)
                {
                    CustomerInfo.GetButton().PerformClick();
                }
                else if (e.KeyCode == Keys.F6)
                {
                    CreditCards.GetButton().PerformClick();
                }
                else if (e.KeyCode == Keys.F9)
                {
                    CompletePayment.GetButton().PerformClick();
                }
            };
        }

        private void InitInputBox()
        {
            Cash.SetPosition(new Point(30, 30));
            this.Controls.Add(Cash.GetLabel());
            this.Controls.Add(Cash.GetTextBox());
            Cash.GetTextBox().TabIndex = 0;

            Check.SetPosition(new Point(30, 80));
            this.Controls.Add(Check.GetLabel());
            this.Controls.Add(Check.GetTextBox());
            Check.GetTextBox().TabIndex = 1;

            FreightCollect.SetPosition(new Point(30, 130));
            this.Controls.Add(FreightCollect.GetLabel());
            this.Controls.Add(FreightCollect.GetTextBox());
            FreightCollect.GetTextBox().TabIndex = 2;

            OnAccount.SetPosition(new Point(30, 180));
            this.Controls.Add(OnAccount.GetLabel());
            this.Controls.Add(OnAccount.GetTextBox());
            OnAccount.GetTextBox().TabIndex = 3;

            CargeCard.SetPosition(new Point(30, 230));
            this.Controls.Add(CargeCard.GetLabel());
            this.Controls.Add(CargeCard.GetTextBox());
            CargeCard.GetTextBox().TabIndex = 4;

            Dsicount.SetPosition(new Point(30, 280));
            this.Controls.Add(Dsicount.GetLabel());
            this.Controls.Add(Dsicount.GetTextBox());
            Dsicount.GetTextBox().TabIndex = 5;

            CreditsApplied.SetPosition(new Point(30, 330));
            this.Controls.Add(CreditsApplied.GetLabel());
            this.Controls.Add(CreditsApplied.GetTextBox());
            CreditsApplied.GetTextBox().TabIndex = 6;

            Change.SetPosition(new Point(30, 380));
            this.Controls.Add(Change.GetLabel());
            this.Controls.Add(Change.GetTextBox());
            Change.GetTextBox().TabIndex = 7;

            AmountRemaining.SetPosition(new Point(30, 430));
            this.Controls.Add(AmountRemaining.GetLabel());
            this.Controls.Add(AmountRemaining.GetTextBox());
            AmountRemaining.GetTextBox().TabIndex = 8;

        }

        public void setDetails(int _id)
        {
            orderItemId = _id;
            var orderItem = Session.OrderItemModelObj.GetOrderItemMessageById(_id);
            if (orderItem != null)
            {
                this.Cash.GetTextBox().Text = orderItem.message;
                this.Cash.GetTextBox().Select();
                this.Cash.GetTextBox().Focus();
            }
        }

        private void ok_button_Click(object sender, EventArgs e)
        {
            string message = this.Cash.GetTextBox().Text;
            if (string.IsNullOrWhiteSpace(message))
            {
                MessageBox.Show("Please enter a message.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Cash.GetTextBox().Select();
                return;
            }

            string modeText = orderItemId == 0 ? "creating" : "updating";

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

    }
}
