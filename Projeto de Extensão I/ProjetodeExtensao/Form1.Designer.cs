namespace ProjetodeExtensao
{
    partial class FrmLogin
    {
        /// <summary>
        /// Variável de designer necessária.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpar os recursos que estão sendo usados.
        /// </summary>
        /// <param name="disposing">true se for necessário descartar os recursos gerenciados; caso contrário, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código gerado pelo Windows Form Designer

        /// <summary>
        /// Método necessário para suporte ao Designer - não modifique 
        /// o conteúdo deste método com o editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.txtUsuario = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.BtnAcessar = new System.Windows.Forms.Button();
            this.txtSenha = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // txtUsuario
            // 
            this.txtUsuario.BackColor = System.Drawing.Color.Black;
            this.txtUsuario.Font = new System.Drawing.Font("Arial Rounded MT Bold", 11.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtUsuario.ForeColor = System.Drawing.Color.White;
            this.txtUsuario.Location = new System.Drawing.Point(501, 276);
            this.txtUsuario.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtUsuario.Multiline = true;
            this.txtUsuario.Name = "txtUsuario";
            this.txtUsuario.Size = new System.Drawing.Size(287, 36);
            this.txtUsuario.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Arial Rounded MT Bold", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(501, 250);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(93, 20);
            this.label1.TabIndex = 2;
            this.label1.Text = "USUÁRIO:";
            // 
            // BtnAcessar
            // 
            this.BtnAcessar.BackColor = System.Drawing.Color.Purple;
            this.BtnAcessar.FlatAppearance.BorderSize = 0;
            this.BtnAcessar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnAcessar.Font = new System.Drawing.Font("Arial Rounded MT Bold", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnAcessar.ForeColor = System.Drawing.Color.White;
            this.BtnAcessar.Location = new System.Drawing.Point(571, 446);
            this.BtnAcessar.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.BtnAcessar.Name = "BtnAcessar";
            this.BtnAcessar.Size = new System.Drawing.Size(147, 50);
            this.BtnAcessar.TabIndex = 4;
            this.BtnAcessar.Text = "Acessar";
            this.BtnAcessar.UseVisualStyleBackColor = false;
            this.BtnAcessar.Click += new System.EventHandler(this.BtnAcessar_Click);
            // 
            // txtSenha
            // 
            this.txtSenha.BackColor = System.Drawing.Color.Black;
            this.txtSenha.Font = new System.Drawing.Font("Arial Rounded MT Bold", 11.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSenha.ForeColor = System.Drawing.Color.White;
            this.txtSenha.Location = new System.Drawing.Point(501, 369);
            this.txtSenha.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtSenha.Multiline = true;
            this.txtSenha.Name = "txtSenha";
            this.txtSenha.PasswordChar = '*';
            this.txtSenha.Size = new System.Drawing.Size(287, 36);
            this.txtSenha.TabIndex = 6;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Arial Rounded MT Bold", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(575, 176);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(128, 46);
            this.label2.TabIndex = 7;
            this.label2.Text = "Login";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Arial Rounded MT Bold", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(501, 346);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(74, 20);
            this.label4.TabIndex = 8;
            this.label4.Text = "SENHA:";
            // 
            // FrmLogin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(1249, 686);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtSenha);
            this.Controls.Add(this.BtnAcessar);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtUsuario);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.MaximizeBox = false;
            this.Name = "FrmLogin";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Login: Sistema de Gerenciamento para Locação de Chácaras";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtUsuario;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button BtnAcessar;
        private System.Windows.Forms.TextBox txtSenha;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
    }
}

