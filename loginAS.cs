namespace BSMART
{
    public partial class loginAS : Form
    {
        public loginAS()
        {
            InitializeComponent();
        }



        private void LoginRes_Load(object sender, EventArgs e)
        {

        }



        private void btnMayor_Click(object sender, EventArgs e)
        {
            BsmartFormNavigator.Open(this, new loginMayor());

        }

        private void btnRes_Click(object sender, EventArgs e)
        {
            BsmartFormNavigator.Open(this, new loginRes());
        }

        private void btnBCap_Click_1(object sender, EventArgs e)
        {
            BsmartFormNavigator.Open(this, new loginBCap());
        }

        private void btnLGUStaff_Click(object sender, EventArgs e)
        {
            BsmartFormNavigator.Open(this, new loginLGU());
        }
    }
}
