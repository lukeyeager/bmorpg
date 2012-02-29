namespace BMORPG_Client
{
    partial class Home
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.IPAddressBox = new System.Windows.Forms.TextBox();
            this.IpAddress = new System.Windows.Forms.Label();
            this.ConnectButton = new System.Windows.Forms.Button();
            this.Username = new System.Windows.Forms.Label();
            this.UsernameBox = new System.Windows.Forms.TextBox();
            this.Password = new System.Windows.Forms.Label();
            this.PasswordBox = new System.Windows.Forms.TextBox();
            this.LoginButton = new System.Windows.Forms.Button();
            this.LoginStatusBox = new System.Windows.Forms.TextBox();
            this.RestartButton = new System.Windows.Forms.Button();
            this.ConnectionStatusBox = new System.Windows.Forms.TextBox();
            this.SvnCheckBox = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // IPAddressBox
            // 
            this.IPAddressBox.Location = new System.Drawing.Point(173, 57);
            this.IPAddressBox.Name = "IPAddressBox";
            this.IPAddressBox.Size = new System.Drawing.Size(100, 20);
            this.IPAddressBox.TabIndex = 0;
            // 
            // IpAddress
            // 
            this.IpAddress.AutoSize = true;
            this.IpAddress.Location = new System.Drawing.Point(87, 57);
            this.IpAddress.Name = "IpAddress";
            this.IpAddress.Size = new System.Drawing.Size(54, 13);
            this.IpAddress.TabIndex = 1;
            this.IpAddress.Text = "IpAddress";
            // 
            // ConnectButton
            // 
            this.ConnectButton.Location = new System.Drawing.Point(286, 57);
            this.ConnectButton.Name = "ConnectButton";
            this.ConnectButton.Size = new System.Drawing.Size(75, 23);
            this.ConnectButton.TabIndex = 2;
            this.ConnectButton.Text = "Connect";
            this.ConnectButton.UseVisualStyleBackColor = true;
            this.ConnectButton.Click += new System.EventHandler(this.Connect_Click);
            // 
            // Username
            // 
            this.Username.AutoSize = true;
            this.Username.Location = new System.Drawing.Point(87, 163);
            this.Username.Name = "Username";
            this.Username.Size = new System.Drawing.Size(55, 13);
            this.Username.TabIndex = 3;
            this.Username.Text = "Username";
            // 
            // UsernameBox
            // 
            this.UsernameBox.Location = new System.Drawing.Point(173, 160);
            this.UsernameBox.Name = "UsernameBox";
            this.UsernameBox.Size = new System.Drawing.Size(100, 20);
            this.UsernameBox.TabIndex = 4;
            // 
            // Password
            // 
            this.Password.AutoSize = true;
            this.Password.Location = new System.Drawing.Point(87, 196);
            this.Password.Name = "Password";
            this.Password.Size = new System.Drawing.Size(53, 13);
            this.Password.TabIndex = 5;
            this.Password.Text = "Password";
            // 
            // PasswordBox
            // 
            this.PasswordBox.Location = new System.Drawing.Point(173, 193);
            this.PasswordBox.Name = "PasswordBox";
            this.PasswordBox.Size = new System.Drawing.Size(100, 20);
            this.PasswordBox.TabIndex = 6;
            // 
            // LoginButton
            // 
            this.LoginButton.Location = new System.Drawing.Point(286, 160);
            this.LoginButton.Name = "LoginButton";
            this.LoginButton.Size = new System.Drawing.Size(75, 23);
            this.LoginButton.TabIndex = 7;
            this.LoginButton.Text = "Login";
            this.LoginButton.UseVisualStyleBackColor = true;
            this.LoginButton.Click += new System.EventHandler(this.Login_Click);
            // 
            // LoginStatusBox
            // 
            this.LoginStatusBox.Location = new System.Drawing.Point(90, 219);
            this.LoginStatusBox.Name = "LoginStatusBox";
            this.LoginStatusBox.ReadOnly = true;
            this.LoginStatusBox.Size = new System.Drawing.Size(183, 20);
            this.LoginStatusBox.TabIndex = 8;
            // 
            // RestartButton
            // 
            this.RestartButton.Location = new System.Drawing.Point(286, 84);
            this.RestartButton.Name = "RestartButton";
            this.RestartButton.Size = new System.Drawing.Size(124, 23);
            this.RestartButton.TabIndex = 9;
            this.RestartButton.Text = "Restart Server";
            this.RestartButton.UseVisualStyleBackColor = true;
            this.RestartButton.Click += new System.EventHandler(this.Restart_Click);
            // 
            // ConnectionStatusBox
            // 
            this.ConnectionStatusBox.Location = new System.Drawing.Point(90, 87);
            this.ConnectionStatusBox.Name = "ConnectionStatusBox";
            this.ConnectionStatusBox.ReadOnly = true;
            this.ConnectionStatusBox.Size = new System.Drawing.Size(183, 20);
            this.ConnectionStatusBox.TabIndex = 10;
            this.ConnectionStatusBox.Text = "Not Connected";
            // 
            // SvnCheckBox
            // 
            this.SvnCheckBox.AutoSize = true;
            this.SvnCheckBox.Location = new System.Drawing.Point(417, 89);
            this.SvnCheckBox.Name = "SvnCheckBox";
            this.SvnCheckBox.Size = new System.Drawing.Size(48, 17);
            this.SvnCheckBox.TabIndex = 11;
            this.SvnCheckBox.Text = "SVN";
            this.SvnCheckBox.UseVisualStyleBackColor = true;
            // 
            // Home
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(491, 314);
            this.Controls.Add(this.SvnCheckBox);
            this.Controls.Add(this.ConnectionStatusBox);
            this.Controls.Add(this.RestartButton);
            this.Controls.Add(this.LoginStatusBox);
            this.Controls.Add(this.LoginButton);
            this.Controls.Add(this.PasswordBox);
            this.Controls.Add(this.Password);
            this.Controls.Add(this.UsernameBox);
            this.Controls.Add(this.Username);
            this.Controls.Add(this.ConnectButton);
            this.Controls.Add(this.IpAddress);
            this.Controls.Add(this.IPAddressBox);
            this.Name = "Home";
            this.Text = "Connection Window";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox IPAddressBox;
        private System.Windows.Forms.Label IpAddress;
        private System.Windows.Forms.Button ConnectButton;
        private System.Windows.Forms.Label Username;
        private System.Windows.Forms.TextBox UsernameBox;
        private System.Windows.Forms.Label Password;
        private System.Windows.Forms.TextBox PasswordBox;
        private System.Windows.Forms.Button LoginButton;
        private System.Windows.Forms.TextBox LoginStatusBox;
        private System.Windows.Forms.Button RestartButton;
        private System.Windows.Forms.TextBox ConnectionStatusBox;
        private System.Windows.Forms.CheckBox SvnCheckBox;
    }
}

